/****** Script for SelectTopNRows command from SSMS  ******/
SELECT 
       COUNT([VehicleID]) AS TOTALCARS
  FROM [VINSolutions_API].[dbo].[Inventory_NEW] WHERE StockNumber NOT LIKE '%-%'

  SELECT 
       COUNT([VehicleID]) AS TOTALCARS
  FROM [VINSolutions_API].[dbo].[Inventory_NEW] WHERE StockNumber  LIKE '%-%'