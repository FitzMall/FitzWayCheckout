USE [Checklists]
GO
/****** Object:  StoredProcedure [dbo].[GetChecklistRecordFuelByID]    Script Date: 1/29/2024 11:29:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		DAVID BURROUGHS
-- Create date: 1/11/2023
-- Description:	Updates FUEL value in ChecklistRecord by ID
-- =============================================
ALTER PROCEDURE [dbo].ChecklistRecordUpdateFuel 
	@ID varchar(50),
	@FuelType varchar(50)
AS
BEGIN

UPDATE ChecklistRecord SET FuelType = @FuelType 
	WHERE ID = @ID
	
		

END
