USE [Checklists]
GO
/****** Object:  StoredProcedure [dbo].[ChecklistItemRecordUpdate]    Script Date: 3/12/2024 10:07:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jim Stripling
-- Create date: 07/16/2021
-- Description:	Inserts current values from ChecklistItemRecord into the history table and updates ChecklistItemRecord with new values.
-- =============================================
ALTER PROCEDURE [dbo].[ChecklistItemRecordUpdate] 
	@ID int,
    @ChecklistItemID int,
	@CheckistSectionID int,
    @ChecklistRecordID int,
	@IsChecked bit,
	@ITDropDownText1 varchar(100),
	@ITDropDownText2 varchar(100),
	@OptionType1 varchar(15),
	@OptionType2 varchar(15),
	@OptionType3 varchar(15),
	@OptionType4 varchar(15),
	@IsOption1Selected bit,
	@IsOption2Selected bit,
	@IsOption3Selected bit,
	@IsOption4Selected bit,
	@Option1Text varchar(100),
	@Option2Text varchar(100),
	@Option3Text varchar(100),
	@Option4Text varchar(100)

AS
BEGIN

	DECLARE @prevIsChecked bit,
	@prevIsOption1Seected bit,
	@prevIsOption2Seected bit,
	@prevIsOption3Seected bit,
	@prevIsOption4Seected bit,
	@prevOption1Text varchar(100),
	@prevOption2Text varchar(100),
	@prevOption3Text varchar(100),
	@prevOption4Text varchar(100),
	@prevOptionType1 varchar(15),
	@prevOptionType2 varchar(15),
	@prevOptionType3 varchar(15),
	@prevOptionType4 varchar(15),
	@prevITDropDownText1 varchar(100),
	@prevITDropDownText2 varchar(100),
	@prevDateCreated datetime




	SET NOCOUNT ON;

	SELECT @prevIsChecked = IsChecked,
		@prevIsOption1Seected = IsOption1Selected,
		@prevIsOption2Seected = IsOption2Selected,
		@prevIsOption3Seected = IsOption3Selected,
		@prevIsOption4Seected = IsOption4Selected,
		@prevOption1Text = Option1Text,
		@prevOption2Text = Option2Text,
		@prevOption3Text = Option3Text,
		@prevOption4Text = Option4Text,
		@prevOptionType1 = OptionType1,
		@prevOptionType2 = OptionType2,
		@prevOptionType3 = OptionType3,
		@prevOptionType4 = OptionType4,
		@prevITDropDownText1= ITDropDownText1,
		@prevITDropDownText2= ITDropDownText2,
		@prevDateCreated = DateCreated
	FROM ChecklistItemRecord
	WHERE ID = @ID

	UPDATE ChecklistItemRecord SET
		ChecklistItemID = @ChecklistItemID ,
		ChecklistRecordID  = @ChecklistRecordID,
		ChecklistSectionID = @CheckistSectionID,
		IsChecked  = @IsChecked,
		IsOption1Selected = @IsOption1Selected,
		IsOption2Selected = @IsOption2Selected,
		IsOption3Selected = @IsOption3Selected,
		IsOption4Selected = @IsOption4Selected,
		Option1Text = @Option1Text,
		Option2Text = @Option2Text,
		Option3Text = @Option3Text,
		Option4Text = @Option4Text,
		OptionType1 = @OptionType1,
		OptionType2 = @OptionType2,
		OptionType3 = @OptionType3,
		OptionType4 = @OptionType4,
		ITDropDownText1 = @ITDropDownText1,
		ITDropDownText2 = @ITDropDownText2,
		DateUpdated =  GETDATE()
	WHERE ID = @ID

	INSERT INTO ChecklistItemRecordHistory (
		ID,
		ChecklistItemID,
		ChecklistRecordID,
		ChecklistSectionID,
		IsChecked,
		IsOption1Selected ,
		IsOption2Selected,
		IsOption3Selected,
		IsOption4Selected,
		Option1Text,
		Option2Text,
		Option3Text,
		Option4Text,
		OptionType1,
		OptionType2,
		OptionType3,
		OptionType4,
		ITDropDownText1,
		ITDropDownText2,
		DateCreated,
		DateUpdated
	) VALUES (
		@ID,
		@ChecklistItemID,
		@ChecklistRecordID,
		@CheckistSectionID,
		@prevIsChecked,
		@prevIsOption1Seected,
		@prevIsOption2Seected,
		@prevIsOption3Seected,
		@prevIsOption4Seected,
		@prevOption1Text,
		@prevOption2Text,
		@prevOption3Text,
		@prevOption4Text,
		@prevOptionType1,
		@prevOptionType2,
		@prevOptionType3,
		@prevOptionType4,
		@prevITDropDownText1,
		@prevITDropDownText2,
		@prevDateCreated,
		GETDATE()
	)
		
		

END
