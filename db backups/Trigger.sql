USE LoRa;


/*
UPDATE lr2.Node
SET Temperature_outdoor = Temperature_indoor
WHERE Node_ID = 'lht-wierden';

UPDATE lr2.Node
SET  Temperature_indoor = NULL
WHERE Node_ID = 'lht-wierden';
*/

-- Trigger to handle the swapped temp data in Gronau LHT sensor
go
CREATE TRIGGER lr2.handle_outdoor_lhts
ON lr2.Node
AFTER INSERT 
AS
	BEGIN
	IF EXISTS (SELECT * FROM INSERTED WHERE Node_ID = 'lht-gronau' OR Node_ID = 'lht-wierden')
		BEGIN
		-- Update Temperature_outdoor using values from INSERTED
        UPDATE lr2.Node
        SET Temperature_outdoor = i.Temperature_indoor
        FROM lr2.Node n
        INNER JOIN INSERTED i ON n.Time = i.Time AND n.Node_ID = 'lht-gronau';

		UPDATE lr2.Node
        SET Temperature_outdoor = i.Temperature_indoor
        FROM lr2.Node n
        INNER JOIN INSERTED i ON n.Time = i.Time AND n.Node_ID = 'lht-wierden';
        
        -- Set Temperature_indoor to NULL for the same rows
        UPDATE lr2.Node
        SET Temperature_indoor = NULL
        FROM lr2.Node n
        INNER JOIN INSERTED i ON n.Time = i.Time AND n.Node_ID = 'lht-gronau';

		UPDATE lr2.Node
        SET Temperature_indoor = NULL
        FROM lr2.Node n
        INNER JOIN INSERTED i ON n.Time = i.Time AND n.Node_ID = 'lht-wierden';
		END
	END

--drop Trigger lr2.handle_gronau






-- creating the recent values table
	GO
USE LoRa;

CREATE TABLE lr2.most_recent (
Node_ID varchar(50) NOT NULL,
Time DATETIME NOT NULL,
Pressure float NULL,
Illumination FLOAT NULL,
Humidity FLOAT NULL,
Gateway_Location varchar(50) NULL,
Temperature_indoor FLOAT NULL,
Temperature_outdoor FLOAT NULL,
PRIMARY KEY (Node_ID),
FOREIGN KEY (Node_ID) REFERENCES lr2.Sensor_location(Node_ID)
);


--DROP TABLE lr2.most_recent

-- Trigger to fill the Recent values
go
CREATE TRIGGER lr2.handle_recents
ON lr2.Node
AFTER INSERT
AS
	BEGIN
	IF NOT EXISTS (SELECT lr2.most_recent.Node_ID
					FROM lr2.most_recent
					INNER JOIN INSERTED  ON lr2.most_recent.Node_ID = INSERTED.Node_ID  
					WHERE lr2.most_recent.Node_ID = INSERTED.Node_ID)
	BEGIN
	INSERT INTO lr2.most_recent (Node_ID, Time)
	SELECT Node_ID, Time
    FROM INSERTED;
	END;

	UPDATE lr2.most_recent
     SET Pressure = INSERTED.Pressure,
		 Illumination = INSERTED.Illumination,
		 Time = INSERTED.Time,
		 Humidity = INSERTED.Humidity,
		 Gateway_Location = INSERTED.Gateway_Location,
		 Temperature_indoor = INSERTED.Temperature_indoor,
		 Temperature_outdoor = INSERTED.Temperature_outdoor
        FROM lr2.most_recent
        INNER JOIN INSERTED  ON lr2.most_recent.Node_ID = INSERTED.Node_ID; 
		
	
	IF EXISTS (SELECT * FROM INSERTED WHERE Node_ID = 'lht-gronau' OR Node_ID = 'lht-wierden')
		BEGIN
		-- Update Temperature_outdoor using values from INSERTED
        UPDATE lr2.most_recent
        SET Temperature_outdoor = i.Temperature_indoor
        FROM lr2.most_recent n
        INNER JOIN INSERTED i ON n.Time = i.Time AND n.Node_ID = 'lht-gronau';

		UPDATE lr2.most_recent
        SET Temperature_outdoor = i.Temperature_indoor
        FROM lr2.most_recent n
        INNER JOIN INSERTED i ON n.Time = i.Time AND n.Node_ID = 'lht-wierden';
        
        -- Set Temperature_indoor to NULL for the same rows
        UPDATE lr2.most_recent
        SET Temperature_indoor = NULL
        FROM lr2.most_recent n
        INNER JOIN INSERTED i ON n.Time = i.Time AND n.Node_ID = 'lht-gronau';

		UPDATE lr2.most_recent
        SET Temperature_indoor = NULL
        FROM lr2.most_recent n
        INNER JOIN INSERTED i ON n.Time = i.Time AND n.Node_ID = 'lht-wierden';
		END

	END

	--DROP TRIGGER lr2.handle_recents



---- Averages table
CREATE TABLE lr2.hours_avg (
Node_ID varchar(50) NOT NULL,
Location varchar(50) NOT NULL,
the_day DATE NOT NULL,
the_hour TINYINT  NOT NULL, --TINYINT  for int so tiny, for only 0-23 values
AVG_Pressure float NULL,
AVG_Illumination FLOAT NULL,
AVG_Humidity FLOAT NULL,
AVG_Temperature_indoor FLOAT NULL,
AVG_Temperature_outdoor FLOAT NULL,
PRIMARY KEY (Node_ID, the_day, the_hour),
FOREIGN KEY (Node_ID) REFERENCES lr2.Sensor_location(Node_ID)
);

--Drop Table lr2.hours_avg;
-- DELETE  lr2.hours_avg;
/*
DELETE  lr2.hours_avg;
BEGIN
INSERT INTO lr2.hours_avg (Node_ID, Location, the_day, the_hour, AVG_Pressure, AVG_Illumination,
							AVG_Humidity, AVG_Temperature_indoor, AVG_Temperature_outdoor)
  SELECT 
	N.Node_id
	, s.Location AS Location
	, CONVERT(DATE,N.[Time]) AS 'the_day'
	, DATEPART(HOUR,N.[Time]) AS 'the_hour',
                ROUND(AVG(N.Pressure),2) AS AVG_Pressure,
                ROUND(AVG(N.Illumination),2) AS AVG_Illumination,
                ROUND(AVG(N.Humidity),2) AS AVG_Humidity,
                ROUND(AVG(N.Temperature_indoor),2) AS AVG_Temperature_indoor,
                ROUND(AVG(N.Temperature_outdoor),2) AS AVG_Temperature_outdoor
FROM lr2.Node AS N
INNER JOIN lr2.Sensor_location AS s
ON N.Node_id = s.Node_ID
WHERE N.Time < '2024-12-06 10:00:00.000' AND N.Time > '2024-12-05 11:00:00.000'
GROUP BY
	N.Node_id
	, s.Location
	, CONVERT(DATE,N.[Time]) 
	, DATEPART(HOUR,N.[Time])
	ORDER BY the_day, the_hour, s.Location;
	END
*/

--- avg trig
go
CREATE TRIGGER trg_avg_hourly
ON lr2.Node
AFTER INSERT
AS
BEGIN
    -- Declare variables for the current date and time
    DECLARE @current_time DATETIME = GETDATE();
    DECLARE @previous_hour_time DATETIME = DATEADD(HOUR, -1, @current_time); -- Move back one hour
    DECLARE @previous_day DATE = CONVERT(DATE, @previous_hour_time); -- Extract the date of the previous hour
    DECLARE @previous_hour INT = DATEPART(HOUR, @previous_hour_time); -- Extract the hour of the previous hour

    -- Ensure this only executes in the first 10 minutes of the current hour
    IF DATEPART(MINUTE, @current_time) < 10
    BEGIN
        -- Check if an average record for the previous hour already exists
        IF NOT EXISTS (
            SELECT 1
            FROM lr2.hours_avg
            WHERE the_day = @previous_day
              AND the_hour = @previous_hour
        )
        BEGIN
            -- Insert averages into lr2.hours_avg for the previous hour
            INSERT INTO lr2.hours_avg (Node_ID, Location, the_day, the_hour, AVG_Pressure, AVG_Illumination,
                                       AVG_Humidity, AVG_Temperature_indoor, AVG_Temperature_outdoor)
            SELECT 
                N.Node_id,
                S.Location AS Location,
                @previous_day AS the_day,
                @previous_hour AS the_hour,
                ROUND(AVG(N.Pressure),2) AS AVG_Pressure,
                ROUND(AVG(N.Illumination),2) AS AVG_Illumination,
                ROUND(AVG(N.Humidity),2) AS AVG_Humidity,
                ROUND(AVG(N.Temperature_indoor),2) AS AVG_Temperature_indoor,
                ROUND(AVG(N.Temperature_outdoor),2) AS AVG_Temperature_outdoor
            FROM lr2.Node AS N
            INNER JOIN lr2.Sensor_location AS S
                ON N.Node_id = S.Node_ID
            WHERE CONVERT(DATE, N.[Time]) = @previous_day
              AND DATEPART(HOUR, N.[Time]) = @previous_hour
            GROUP BY N.Node_id, S.Location;
        END
    END
END;
--DROP TRIGGER trg_avg_hourly;





---- max-min table
CREATE TABLE lr2.max_min (
Node_ID varchar(50) NOT NULL,
Location varchar(50) NOT NULL,
the_day DATE NOT NULL,
max_Pressure float NULL,
min_Pressure float NULL,
max_Illumination FLOAT NULL,
min_Illumination FLOAT NULL,
max_Humidity FLOAT NULL,
min_Humidity FLOAT NULL,
max_Temperature_indoor FLOAT NULL,
min_Temperature_indoor FLOAT NULL,
max_Temperature_outdoor FLOAT NULL,
min_Temperature_outdoor FLOAT NULL,
PRIMARY KEY (Node_ID, the_day),
FOREIGN KEY (Node_ID) REFERENCES lr2.Sensor_location(Node_ID)
);


/*
--delete lr2.max_min
BEGIN
INSERT INTO lr2.max_min (Node_ID, Location, the_day, max_Pressure, min_Pressure, max_Illumination, min_Illumination,
							max_Humidity, min_Humidity, max_Temperature_indoor, min_Temperature_indoor,
							max_Temperature_outdoor, min_Temperature_outdoor)
  SELECT 
	N.Node_id
	, s.Location AS Location
	, CONVERT(DATE,N.[Time]) AS 'the_day'
	, MAX(N.Pressure) AS 'max_Pressure'
	, MIN(N.Pressure) AS 'min_Pressure'
	, MAX(N.Illumination) AS 'max_Illumination'
	, MIN(N.Illumination) AS 'min_Illumination'
	, MAX(N.Humidity) AS 'max_Humidity'
	, MIN(N.Humidity) AS 'min_Humidity'
	, MAX(N.Temperature_indoor) AS 'max_Temperature_indoor'
	, MIN(N.Temperature_indoor) AS 'min_Temperature_indoor'
	, MAX(N.Temperature_outdoor) AS 'max_Temperature_outdoor'
	, MIN(N.Temperature_outdoor) AS 'min_Temperature_outdoor'
FROM lr2.Node AS N
INNER JOIN lr2.Sensor_location AS s
ON N.Node_id = s.Node_ID
WHERE N.Time < '2024-12-06 00:00:00.000'
GROUP BY
	N.Node_id
	, s.Location
	, CONVERT(DATE,N.[Time]) 
	ORDER BY the_day, s.Location;
	END
*/


--- max_min trig
go
CREATE TRIGGER trg_max_min
ON lr2.hours_avg
AFTER INSERT
AS
BEGIN
	 -- Declare variables for the current date and time
    DECLARE @current_time DATETIME = GETDATE();
    DECLARE @previous_day DATETIME = DATEADD(DAY, -1, @current_time); -- Move back one day
    DECLARE @previous_date DATE =  CONVERT(DATE, @previous_day)
    


    IF DATEPART(HOUR, @current_time) =0
    BEGIN
        -- Check if an average record for the previous day already exists
        IF NOT EXISTS (
            SELECT the_day
            FROM lr2.max_min
            WHERE the_day = @previous_date   
        )
        BEGIN
		INSERT INTO lr2.max_min (Node_ID, Location, the_day, max_Pressure, min_Pressure, max_Illumination, min_Illumination,
									max_Humidity, min_Humidity, max_Temperature_indoor, min_Temperature_indoor,
									max_Temperature_outdoor, min_Temperature_outdoor)				
		  SELECT 
			N.Node_id
			, s.Location AS Location
			, CONVERT(DATE,N.[Time]) AS 'the_day'
			, MAX(N.Pressure) AS 'max_Pressure'
			, MIN(N.Pressure) AS 'min_Pressure'
			, MAX(N.Illumination) AS 'max_Illumination'
			, MIN(N.Illumination) AS 'min_Illumination'
			, MAX(N.Humidity) AS 'max_Humidity'
			, MIN(N.Humidity) AS 'min_Humidity'
			, MAX(N.Temperature_indoor) AS 'max_Temperature_indoor'
			, MIN(N.Temperature_indoor) AS 'min_Temperature_indoor'
			, MAX(N.Temperature_outdoor) AS 'max_Temperature_outdoor'
			, MIN(N.Temperature_outdoor) AS 'min_Temperature_outdoor'
		FROM lr2.Node AS N
		INNER JOIN lr2.Sensor_location AS s
		ON N.Node_id = s.Node_ID
		WHERE CONVERT(DATE, N.[Time]) = @previous_date
		--Where N.Time >= '2024-12-03 00:00:00.000' AND N.Time < '2024-12-04 00:00:00.000' 
		GROUP BY
			N.Node_id
			, s.Location
			, CONVERT(DATE,N.[Time]) 
			ORDER BY the_day, s.Location;
		END
	END
END;


SELECT 
    name AS TriggerName,
    parent_id AS ObjectID, -- The object (table or view) the trigger is attached to
    type_desc AS TriggerType,
    is_disabled AS IsDisabled
FROM sys.triggers;