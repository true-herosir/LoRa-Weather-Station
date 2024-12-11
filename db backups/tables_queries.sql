USE LoRa


SELECT *
  FROM [LoRa].[lr2].[Node]
  --WHERE Node_ID = 'lht-saxion' 
  ORDER BY Time DESC



  SELECT *
  FROM [LoRa].[lr2].[most_recent]
  ORDER BY Time DESC

   SELECT *
  FROM [LoRa].[lr2].[hours_avg]
  --WHERE Node_ID = 'weather-thingy-g4-2024'
  ORDER BY the_day DESC, the_hour DESC, Location;

  
  SELECT *
  FROM [LoRa].[lr2].[max_min]
  WHERE Node_ID LIKE 'lht%' 
  ORDER BY the_day DESC, Location;

   SELECT *
  FROM [LoRa].[lr2].[Node_location]

