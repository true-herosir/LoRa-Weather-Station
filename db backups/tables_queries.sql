USE LoRa


SELECT *
  FROM [LoRa].[lr2].[Node]
  --WHERE Node_ID = 'mkr-saxion'
  ORDER BY Time DESC




  SELECT *
  FROM [LoRa].[lr2].[most_recent]
  ORDER BY Time DESC

   SELECT *
  FROM [LoRa].[lr2].[hours_avg]
  --WHERE Node_ID = 'lht-tester'
  ORDER BY the_day DESC, the_hour DESC, Location;

  
  SELECT *
  FROM [LoRa].[lr2].[max_min]
 --WHERE Node_ID = 'lht-tester'
  ORDER BY the_day DESC, Location;

   SELECT *
  FROM [LoRa].[lr2].[Node_location]

/*
ALTER TABLE [LoRa].[lr2].[Node] DISABLE TRIGGER handle_recents
ALTER TABLE [LoRa].[lr2].[Node] DISABLE TRIGGER trg_avg_hourly
ALTER TABLE [LoRa].[lr2].[Node] DISABLE TRIGGER handle_outdoor_lhts

ALTER TABLE [LoRa].[lr2].[Node] ENABLE TRIGGER handle_recents
ALTER TABLE [LoRa].[lr2].[Node] ENABLE TRIGGER trg_avg_hourly
ALTER TABLE [LoRa].[lr2].[Node] ENABLE TRIGGER handle_outdoor_lhts
*/

  --DELETE [LoRa].[lr2].[Node]
  --WHERE Node_ID = 'lht-gronau' AND Time < '2024-12-06 10:01:33.000'
