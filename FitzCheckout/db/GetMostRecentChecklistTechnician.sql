USE [Checklists]
GO
/****** Object:  StoredProcedure [dbo].[GetMostRecentChecklistTechnician]    Script Date: 3/20/2024 8:45:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<im Stripling
-- Create date: 7/21/21
-- =============================================
ALTER PROCEDURE [dbo].[GetMostRecentChecklistTechnician]
	@ID int 
AS
BEGIN

DECLARE @UserID int,
	@UserRole int

	SELECT @UserID = cr.UserID, @UserRole = a.userrole FROM ChecklistRecord cr, AccessList a WHERE cr.UserID = a.UserID AND  cr.ID = @ID 

	IF @UserRole <> 3
			SELECT TOP 1 @UserID = crh.UserID FROM  ChecklistRecord crh, AccessList a WHERE crh.id = @ID and a.UserID = crh.UserID AND a.UserRole = 3 ORDER BY crh.DateUpdated DESC

	SELECT @UserID

END
