USE [Checklists]
GO
/****** Object:  StoredProcedure [dbo].[ReportUnCheckedOutCars_ByLoc]    Script Date: 3/12/2024 10:06:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









-- =============================================
-- Author:		Burroughs, David
-- Create date: 5/23/2022
-- Description:	list of cars not in checkout process by location
-- =============================================
ALTER PROCEDURE [dbo].[ReportUnCheckedOutCars_ByLoc]
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UnCheckedCars' AND TABLE_SCHEMA = 'dbo')
  DROP TABLE UnCheckedCars;

SELECT UsedID AS ID , CASE  WHEN DRloc = 'LFT' AND v.Mall = 'GA' THEN (SELECT FullName from 
[checklists].[dbo].[Locations_lkup] WHERE LocCode = 'LFT' AND Mall = 'GA')  WHEN DRloc = 'LFT' AND v.Mall = 'GM' THEN 
(SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'LFT' AND Mall = 'GM')
WHEN DRloc = 'FBS' AND v.Mall = 'WN' THEN (SELECT FullName from [checklists].[dbo].[Locations_lkup] 
WHERE LocCode = 'FBS' AND Mall = 'WN')  WHEN DRloc = 'FBS' AND v.Mall = 'WF' THEN 
(SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'FBS' AND Mall = 'WF') 
ELSE (SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = V.DRloc) END as MetaDataValue1, 
STR(miles) AS MetaDataValue2, yr AS MetaDataValue3, make AS MetaDataValue4, carline AS MetaDataValue5, 
stk AS MetaDataValue6, vin AS MetaDataValue7 , CASE  WHEN DRloc = 'LFT' AND v.Mall = 'GA' THEN 
(SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'LFT' AND Mall = 'GA') 
WHEN DRloc = 'LFT' AND v.Mall = 'GM' THEN (SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] 
WHERE LocCode = 'LFT' AND Mall = 'GM')  WHEN DRloc = 'FBS' AND v.Mall = 'WN' THEN (SELECT PermissionCode 
from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'FBS' AND Mall = 'WN')  WHEN DRloc = 'FBS' AND 
v.Mall = 'WF' THEN (SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'FBS' AND Mall = 'WF')
ELSE (SELECT PermissionCode from [Locations_lkup] WHERE LocCode = V.DRloc) END as MetaDataValue8, 1 AS Status, 0 AS UserID,
'UnAssigned' AS FullName, GETDATE() AS DateCreated, GETDATE() AS DateUpdated, UPPER(DRloc) as loc, daysn, days  
INTO UnCheckedCars FROM [JUNK].[dbo].[CSV_vehicleUSED] v WHERE  v.status != 6 AND v.status !=  15
AND v.status !=  3 AND v.status != 20 AND
v.vin NOT IN(SELECT cr.MetaDataValue7 as vin FROM [ChecklistRecord] cr) 

UPDATE UnCheckedCars SET MetaDataValue3 = '20' + LTRIM(MetaDataValue3) WHERE LEN(MetaDataValue3) < 3

END
