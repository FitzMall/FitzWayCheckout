USE [Checklists]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckoutRole]    Script Date: 3/12/2024 10:08:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		DAVID BURROUGHS
-- Create date: 11/20/2023
-- Description:	Get Fitzway Checkout Role by IVORY user id
-- =============================================
ALTER PROCEDURE [dbo].[GetCheckoutRole]
    @parUSERID varchar(10)
    
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
SELECT CAST(a.UserRole AS VARCHAR(3))
  FROM [Checklists].[dbo].[AccessList] a JOIN
  [FITZDB].[dbo].[users] b
  on a.userid = b.ID 
  where b.USERID = @parUSERID

END
