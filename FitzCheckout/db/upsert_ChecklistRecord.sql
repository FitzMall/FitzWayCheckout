USE [Checklists]
GO
/****** Object:  StoredProcedure [dbo].[upsert_ChecklistRecord]    Script Date: 3/12/2024 10:10:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Jim Stripling>
-- Create date: <4/8/2021>
-- Description:	<Inserts or updates a Checklist Record>
-- =============================================
ALTER PROCEDURE [dbo].[upsert_ChecklistRecord] 
	@ID					int
	,@ChecklistID		int
	,@UserID			int
	,@MetaDataValue1	varchar(50)
	,@MetaDataValue2	varchar(50)
	,@MetaDataValue3	varchar(50)
	,@MetaDataValue4	varchar(50)
	,@MetaDataValue5	varchar(50)
	,@MetaDataValue6	varchar(50)
	,@MetaDataValue7	varchar(50)
	,@MetaDataValue8	varchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRANSACTION;
		UPDATE [Checklists].[dbo].[ChecklistRecord] SET
			ChecklistID = @ChecklistID
			,UserID = @UserID
			,MetaDataValue1 = @MetaDataValue1
			,MetaDataValue2 = @MetaDataValue2
			,MetaDataValue3 = @MetaDataValue3
			,MetaDataValue4 = @MetaDataValue4
			,MetaDataValue5 = @MetaDataValue5
			,MetaDataValue6 = @MetaDataValue6
			,MetaDataValue7 = @MetaDataValue7
			,MetaDataValue8 = @MetaDataValue8
			,DateUpdated = GETDATE()
		WHERE ID = @ID

		IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO [Checklists].[dbo].[ChecklistRecord] 
				(ChecklistID
				,UserID
				,MetaDataValue1
				,MetaDataValue2
				,MetaDataValue3
				,MetaDataValue4
				,MetaDataValue5
				,MetaDataValue6
				,MetaDataValue7
				,MetaDataValue8
				,DateCreated
				,DateUpdated
				)
			VALUES 
				(@ChecklistID
				,@UserID
				,@MetaDataValue1
				,@MetaDataValue2
				,@MetaDataValue3
				,@MetaDataValue4
				,@MetaDataValue5
				,@MetaDataValue6
				,@MetaDataValue7
				,@MetaDataValue8
				,GETDATE()
				,GETDATE()
				)
		SELECT SCOPE_IDENTITY();

		END

	COMMIT TRANSACTION;



END
