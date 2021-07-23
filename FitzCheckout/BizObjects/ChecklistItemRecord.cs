using FitzCheckout.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.BizObjects
{
    public interface IChecklistItemRecord
    {
        ChecklistItemRecord GetChecklistItemRecordByID(int ID);
        List<ChecklistItemRecord> GetChecklistItemsByChecklistRecordID(int checklistRecordID);

        int GetChecklistItemIDByChecklistItemRecordID(int itemRecordID);
        void Save(ChecklistItemRecord itemRecord);

    }
    public class ChecklistItemRecord: IChecklistItemRecord
    {

        #region Properties
        public int ID { get; set; }
        public int ChecklistItemID { get; set; }
        public int ChecklistSectionID { get; set; }
        public int ChecklistRecordID { get; set; }
        public bool IsChecked { get; set; }

        public string ITDropDownText1 { get; set; }
        public string ITDropDownText2 { get; set; }
        public string OptionType1 { get; set; }
        public string OptionType2 { get; set; }
        public string OptionType3 { get; set; }
        public string OptionType4 { get; set; }
        public bool IsOption1Selected { get; set; }
        public bool IsOption2Selected { get; set; }
        public bool IsOption3Selected { get; set; }
        public bool IsOption4Selected { get; set; }
        public string Option1Text { get; set; }
        public string Option2Text { get; set; }
        public string Option3Text { get; set; }
        public string Option4Text { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        #endregion

        public ChecklistItemRecord GetChecklistItemRecordByID(int ID)
        {
            ChecklistItemRecord checklistItemRecord = new ChecklistItemRecord();
            string qs = @"SELECT ir.ID, ir.ChecklistItemID, ir.ChecklistRecordID, ir.IsChecked, ir.ITDropDownText1, ir.ITDropDownText2, ir.OptionType1, ir.OptionType2, ir.OptionType3, ir.OptionType4, ir.IsOption1Selected , ir.IsOption2Selected, 
                                    ir.IsOption3Selected, ir.IsOption4Selected, ir.Option1Text, ir.Option2Text, ir.Option3Text, ir.Option4Text, i.ChecklistSectionID
                                FROM [Checklists].[dbo].[ChecklistItemRecord] ir 
                                JOIN [Checklists].[dbo].[ChecklistItem] i on ir.ChecklistItemID = i.ID
                                WHERE ir.ID = @ID
                                ORDER BY i.ChecklistSectionID, i.OrderNumber";
            var result = SqlMapperUtil.SqlWithParams<ChecklistItemRecord>(qs, new { ID = ID }).FirstOrDefault();

            if (result != null)
            {
                checklistItemRecord.ID = result.ID;
                checklistItemRecord.ITDropDownText1 = result.ITDropDownText1;
                checklistItemRecord.ITDropDownText2 = result.ITDropDownText2;
                checklistItemRecord.ChecklistItemID = result.ChecklistItemID;
                checklistItemRecord.ChecklistRecordID = result.ChecklistRecordID;
                checklistItemRecord.ChecklistSectionID = result.ChecklistSectionID;
                checklistItemRecord.IsChecked = result.IsChecked;
                checklistItemRecord.OptionType1 = result.OptionType1;
                checklistItemRecord.OptionType2 = result.OptionType2;
                checklistItemRecord.OptionType3 = result.OptionType3;
                checklistItemRecord.OptionType4 = result.OptionType4;
                checklistItemRecord.IsOption1Selected = result.IsOption1Selected;
                checklistItemRecord.IsOption2Selected = result.IsOption2Selected;
                checklistItemRecord.IsOption3Selected = result.IsOption3Selected;
                checklistItemRecord.IsOption4Selected = result.IsOption4Selected;
                checklistItemRecord.Option1Text = result.Option1Text;
                checklistItemRecord.Option2Text = result.Option2Text;
                checklistItemRecord.Option3Text = result.Option3Text;
                checklistItemRecord.Option4Text = result.Option4Text;
            }

            return checklistItemRecord;
        }

        public List<ChecklistItemRecord> GetChecklistItemsByChecklistRecordID(int checklistRecordID)
        {
            throw new NotImplementedException();
        }

        public int GetChecklistItemIDByChecklistItemRecordID(int itemRecordID)
        {
            string qs = @"SELECT ChecklistItemID
								FROM [Checklists].[dbo].[ChecklistItemRecord]
								WHERE ID = @itemRecordID";
            return SqlMapperUtil.SqlWithParams<int>(qs, new { itemRecordID = itemRecordID }).FirstOrDefault();
        }

        public void Save(ChecklistItemRecord itemRecord)
        {
            if (itemRecord.ID == 0)
            {
                SaveRecord(itemRecord);
            }
            else
            {
                UpdateRecord(itemRecord);
            }
        }

        private int UpdateRecord(ChecklistItemRecord itemRecord)
        {

            var result = SqlMapperUtil.StoredProcWithParams<int>("ChecklistItemRecordUpdate", new
            {
                @ID = itemRecord.ID,
                @ChecklistItemID = itemRecord.ChecklistItemID,
                @CheckistSectionID = itemRecord.ChecklistSectionID,
                @ChecklistRecordID = itemRecord.ChecklistRecordID,
                @IsChecked = itemRecord.IsChecked,
                @ITDropDownText1 = itemRecord.ITDropDownText1,
                @ITDropDownText2 = itemRecord.ITDropDownText2,
                @OptionType1 = itemRecord.OptionType1,
                @OptionType2 = itemRecord.OptionType2,
                @OptionType3 = itemRecord.OptionType3,
                @OptionType4 = itemRecord.OptionType4,
                @IsOption1Selected = itemRecord.IsOption1Selected,
                @IsOption2Selected = itemRecord.IsOption2Selected,
                @IsOption3Selected = itemRecord.IsOption3Selected,
                @IsOption4Selected = itemRecord.IsOption4Selected,
                @Option1Text = itemRecord.Option1Text,
                @Option2Text = itemRecord.Option2Text,
                @Option3Text = itemRecord.Option3Text,
                @Option4Text = itemRecord.Option4Text

            });
            return itemRecord.ID;
        }

        private void SaveRecord(ChecklistItemRecord itemRecord)
        {
            string qs = @"INSERT INTO [Checklists].[dbo].[ChecklistItemRecord] (
                            ChecklistItemID
                            ,ChecklistRecordID
                            ,isChecked
                            ,ITDropDownText1
                            ,ITDropDownText2
                            ,IsOption1Selected
                            ,IsOption2Selected
                            ,IsOption3Selected
                            ,IsOption4Selected
                            ,Option1Text
                            ,Option2Text
                            ,Option3Text
                            ,Option4Text
                            ,OptionType1
                            ,OptionType2
                            ,OptionType3
                            ,OptionType4
                            ,DateCreated
                            ,DateUpdated
                        )
                        OUTPUT INSERTED.ID
                        VALUES
                            (
                            @ChecklistItemID
                            ,@ChecklistRecordID
                            ,@isChecked
                            ,@ITDropDownText1
                            ,@ITDropDownText2
                            ,@IsOption1Selected
                            ,@IsOption2Selected
                            ,@IsOption3Selected
                            ,@IsOption4Selected
                            ,@Option1Text
                            ,@Option2Text
                            ,@Option3Text
                            ,@Option4Text
                            ,@OptionType1
                            ,@OptionType2
                            ,@OptionType3
                            ,@OptionType4
                            ,GETDATE()
                            ,GETDATE()
                            )
                        ";
            int result = SqlMapperUtil.SqlWithParams<int>(qs, itemRecord).First();

        }
    }
}