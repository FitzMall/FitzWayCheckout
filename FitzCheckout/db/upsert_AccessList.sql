USE [Checklists]
GO
/****** Object:  StoredProcedure [dbo].[upsert_AccessList]    Script Date: 3/12/2024 10:10:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jim Stripling
-- Create date: 06/14/2021
-- Description:	Inserts current values from AccessList into the history table and updates AccessList with new values.
-- =============================================
ALTER PROCEDURE [dbo].[upsert_AccessList] 
    @UserID decimal,
	@UserRole int,
    @Permissions varchar(2000)

AS
BEGIN

	DECLARE @prevPermissions varchar(2000),
		@PrevDateUpdated datetime,
		@PrevUserRole int,
		@AccessListID int


	SET NOCOUNT ON;


	SELECT @prevPermissions = Permissions,
		@AccessListID = ID,
		@PrevUserRole = UserRole,
		@PrevDateUpdated = DateUpdated
	FROM AccessList
	WHERE UserID = @UserID

	IF @AccessListID IS NULL
		BEGIN
			INSERT INTO AccessList (
				UserID,
				Permissions,
				UserRole,
				DateCreated,
				DateUpdated
			) VALUES (
				@UserID
				,@Permissions
				,@UserRole
				,GETDATE()
				,GETDATE()
			)
		END
	ELSE
		BEGIN
			UPDATE AccessList SET
				Permissions = @Permissions
				,UserRole = @UserRole
				,DateUpdated =  GETDATE()
			WHERE UserID = @UserID

			INSERT INTO AccessListHistory (
				AccessListID
				,UserID
				,UserRole
				,Permissions 
				,DateUpdated 
			) VALUES (
				@AccessListID,
				@UserID,
				@PrevUserRole,
				@PrevPermissions,
				@PrevDateUpdated
			)
		END
END
