USE [Checklists]
GO

/****** Object:  StoredProcedure [dbo].[GetFuelDescription]    Script Date: 2/20/2024 1:51:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		DAVID BURROUGHS
-- Create date: 2/20/2024
-- Description:	Get Fuel Description by code ex ('ALL', 'ICHYBRID', 'IC')
-- =============================================
CREATE PROCEDURE [dbo].[GetFuelDescription]
	@fuel varchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT [FuelDescription]
  FROM [Checklists].[dbo].[FuelDescriptions]
  WHERE FuelCode = @fuel

  END
GO


