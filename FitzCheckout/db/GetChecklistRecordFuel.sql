USE [Checklists]
GO

/****** Object:  StoredProcedure [dbo].[GetChecklistRecordFuel]    Script Date: 1/16/2024 1:09:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		DAVID BURROUGHS
-- Create date: 1/11/2023
-- Description:	Inserts FUEL value INTO ChecklistRecord 
-- =============================================
create PROCEDURE [dbo].[GetChecklistRecordFuel] 
	@VIN varchar(50)

AS
BEGIN

SELECT FuelType 
FROM ChecklistRecord 
	WHERE MetaDataValue7 = @VIN
	
		

END
GO


