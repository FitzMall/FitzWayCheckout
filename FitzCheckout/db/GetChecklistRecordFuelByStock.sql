USE [Checklists]
GO

/****** Object:  StoredProcedure [dbo].[GetChecklistRecordFuel]    Script Date: 3/20/2024 10:14:13 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		DAVID BURROUGHS
-- Create date: 3/20/2024
-- Description:	Gets FUEL value by Stock #
-- =============================================
alter PROCEDURE [dbo].[GetChecklistRecordFuelByStock] 
	@Stk varchar(50)

AS
BEGIN

SELECT FuelType 
FROM ChecklistRecord 
	WHERE MetaDataValue6 = @Stk
	
END
GO


