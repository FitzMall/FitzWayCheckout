USE [Checklists]
GO
/****** Object:  StoredProcedure [dbo].[GetCheckoutPermissions]    Script Date: 3/12/2024 10:08:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		DAVID BURROUGHS
-- Create date: 11/20/2023
-- Description:	Get Fitzway Checkout Permissions by IVORY user id
-- =============================================
ALTER PROCEDURE [dbo].[GetCheckoutPermissions]
    @parUSERID varchar(10)
    
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
SELECT a.[Permissions]
  FROM [Checklists].[dbo].[AccessList] a JOIN
  [FITZDB].[dbo].[users] b
  on a.userid = b.ID 
  where b.USERID = @parUSERID

END
