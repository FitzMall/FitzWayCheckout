USE [Checklists]
GO
/****** Object:  StoredProcedure [dbo].[ChecklistRecordUpdate]    Script Date: 2/20/2024 8:44:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jim Stripling
-- Create date: 05/24/2021
-- Description:	Inserts current values from ChecklistRecord into the history table and updates ChecklistRecord with new values.
-- =============================================
ALTER PROCEDURE [dbo].[ChecklistRecordUpdate] 
	@ID int,
    @ChecklistID int,
    @MetaDataValue1 varchar(50),
    @MetaDataValue2 varchar(50),
    @MetaDataValue3 varchar(50),
    @MetaDataValue4 varchar(50),
    @MetaDataValue5 varchar(50),
    @MetaDataValue6 varchar(50),
    @MetaDataValue7 varchar(50),
    @MetaDataValue8 varchar(50),
    @UserID int,
	@FullName varchar(100),
    @Status int,
    @Action varchar(20),
	@DateCreated datetime,
	@DateUpdated datetime,
	@FuelType varchar(50)

AS
BEGIN

	DECLARE     @prevMetaDataValue1 varchar(50),
    @prevMetaDataValue2 varchar(50),
    @prevMetaDataValue3 varchar(50),
    @prevMetaDataValue4 varchar(50),
    @prevMetaDataValue5 varchar(50),
    @prevMetaDataValue6 varchar(50),
    @prevMetaDataValue7 varchar(50),
    @prevMetaDataValue8 varchar(50),
    @prevUserID int,
    @prevStatus int,
    @prevAction varchar(20),
	@prevDateCreated datetime,
	@prevDateUpdated datetime


	SET NOCOUNT ON;

	SELECT @prevMetaDataValue1 = MetaDataValue1
		,@prevMetaDataValue2 = MetaDataValue2
		,@prevMetaDataValue3 = MetaDataValue3
		,@prevMetaDataValue4 = MetaDataValue4
		,@prevMetaDataValue5 = MetaDataValue5
		,@prevMetaDataValue6 = MetaDataValue6
		,@prevMetaDataValue7 = MetaDataValue7
		,@prevMetaDataValue8 = MetaDataValue8
		,@prevUserID = UserID
		,@prevStatus = Status
		,@prevAction = Action
		,@prevDateCreated = DateCreated
		,@prevDateUpdated = DateUpdated
	FROM ChecklistRecord
	WHERE ID = @ID

	UPDATE ChecklistRecord SET
		ChecklistID = @ChecklistID
		,MetaDataValue1 = @MetaDataValue1
		,MetaDataValue2 = @MetaDataValue2
		,MetaDataValue3 = @MetaDataValue3
		,MetaDataValue4 = @MetaDataValue4
		,MetaDataValue5 = @MetaDataValue5
		,MetaDataValue6 = @MetaDataValue6
		,MetaDataValue7 = @MetaDataValue7
		,MetaDataValue8 = @MetaDataValue8
		,UserID = @UserID
		,Status = @Status 
		,Action = @Action 
		,DateUpdated =  GETDATE()
		,FuelType = @FuelType
	WHERE ID = @ID

	INSERT INTO ChecklistRecordHistory (
		ID
		,ChecklistID
		,MetaDataValue1 
		,MetaDataValue2 
		,MetaDataValue3 
		,MetaDataValue4 
		,MetaDataValue5 
		,MetaDataValue6 
		,MetaDataValue7
		,MetaDataValue8
		,UserID
		,Status
		,Action
		,DateCreated
		,DateUpdated
	) VALUES (
		@ID,
		@ChecklistID,
		@prevMetaDataValue1,
		@prevMetaDataValue2,
		@prevMetaDataValue3,
		@prevMetaDataValue4,
		@prevMetaDataValue5,
		@prevMetaDataValue6,
		@prevMetaDataValue7,
		@prevMetaDataValue8,
		@prevUserID,
		@prevStatus,
		@prevAction,
		coalesce(@prevDateCreated,GETDATE()),
		coalesce(@prevDateUpdated,GETDATE())
	)
		
		

END
