using FitzCheckout.BizObjects;
using FitzCheckout.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.Models
{
    public interface IChecklistItemRecordVM
    {
        List<ChecklistItemRecordVM> GetItemsByIDs(int checklistRecordID, int sectionID, string FuelType);
    }
    public class ChecklistItemRecordVM: IChecklistItemRecordVM
    {
        private readonly IChecklistRecord _checklistRecord;
        private readonly IChecklistItem _checklistItem;

        public int ChecklistRecordID { get; set; }
        public int ChecklistItemRecordID { get; set; }
        public int SectionID { get; set; }
        public int ItemId { get; set; }
        public string ItemText { get; set; }
        public int SectionOrderNumber { get; set; }
        public bool HasCheckbox { get; set; }
        public bool IsChecked { get; set; }
        public bool HasITDropDown1 { get; set; }
        public bool HasITDropDown2 { get; set; }
        public string ITDropDownType1 { get; set; }
        public string ITDropDownType2 { get; set; }
        public string ITDropDownText1 { get; set; }
        public string ITDropDownText2 { get; set; }
        public string DropDownType { get; set; }
        public string OptionType1 { get; set; }
        public string OptionType2 { get; set; }
        public string OptionType3 { get; set; }
        public string OptionType4 { get; set; }
        public bool AllowAutoChecking { get; set; }
        public bool IsOption1Selected { get; set; }
        public bool IsOption2Selected { get; set; }
        public bool IsOption3Selected { get; set; }
        public bool IsOption4Selected { get; set; }
        public string Option1Text { get; set; }
        public string Option2Text { get; set; }
        public string Option3Text { get; set; }
        public string Option4Text { get; set; }


        public ChecklistItemRecordVM()
        {

        }

        public ChecklistItemRecordVM(IChecklistRecord checklistRecord, IChecklistItem checklistItem)
        {
            _checklistRecord = checklistRecord;
            _checklistItem = checklistItem;
        }

        public List<ChecklistItemRecordVM> GetItemsByIDs(int checklistRecordID, int sectionID, string FuelType)
        {
            if (sectionID == 0)
            {
                throw new ArgumentException("SectionID must be provided.");
            }

            List<ChecklistItemRecordVM> recordItems = new List<ChecklistItemRecordVM>();
            string qs;

            if (checklistRecordID > 0)
            {
                qs = @"SELECT ir.ChecklistRecordID, ir.ID ChecklistItemRecordID, i.ID ItemID, 
                            i.OrderNumber SectionOrderNumber, i.ItemText, 
                            i.HasCheckbox, ir.IsChecked, i.HasITDropDown1, 
                            i.HasITDropDown2, ir.ITDropDownText1, ir.ITDropDownText2, 
                            i.ITDropDownType1, i.ITDropDownType2, i.OptionType1, 
                            i.OptionType2, i.OptionType3, i.OptionType4, 
                            i.AllowAutoChecking, ir.IsOption1Selected, ir.IsOption2Selected, 
                            ir.IsOption3Selected, ir.IsOption4Selected, ir.Option1Text, 
                            ir.Option2Text, ir.Option3Text, ir.Option4Text
                        FROM [Checklists].[dbo].[ChecklistItemRecord] ir, [Checklists].[dbo].[ChecklistItem_Hybrid_EV] i
                        WHERE ir.ChecklistRecordID = @checklistRecordID AND [Fuel] IN ";
                qs += FuelType;
                        qs += " AND ir.ChecklistItemID = i.ID and i.ChecklistSectionID = @sectionID ORDER BY i.OrderNumber";
                recordItems = SqlMapperUtil.SqlWithParams<ChecklistItemRecordVM>(qs, new { checklistRecordID = checklistRecordID, sectionID = sectionID }).ToList();

            }
            else
            {
                qs = @"SELECT 0 ChecklistRecordID, 0 ChecklistItemRecordID, i.ID ItemID
                            , i.OrderNumber SectionOrderNumber, i.ItemText
                            , i.HasCheckbox
                            , i.HasITDropDown1, '' ITDropDownText1, i.ITDropDownType1
                            , i.HasITDropDown2, '' ITDropDownText2, i.ITDropDownType2
                            , i.OptionType1, i.OptionType2
                            , i.OptionType3, i.OptionType4, i.AllowAutoChecking
                            , CAST(0 as bit) IsOption1Selected
                            , CAST(0 as bit) IsOption2Selected 
                            , CAST(0 as bit) IsOption3Selected 
                            , CAST(0 as bit) IsOption4Selected
	                        , '' Option1Text, '' Option2Text, '' Option3Text
                            , '' Option4Text
                        FROM [Checklists].[dbo].[ChecklistItem_Hybrid_EV] i
                        WHERE i.ChecklistSectionID = @sectionID AND[Fuel] IN ";
                qs += FuelType;
                qs += " AND i.ChecklistSectionID = @sectionID ORDER BY i.OrderNumber";

                recordItems = SqlMapperUtil.SqlWithParams<ChecklistItemRecordVM>(qs, new { sectionID = sectionID }).ToList();
            }

            return recordItems;
        }

        public List<ChecklistItemRecordVM> GetItemVMsByChecklistID(int checklistID, int sectionID)
        {

            List<ChecklistItemRecordVM> recordItems = new List<ChecklistItemRecordVM>();


            return recordItems;

        }
    }
}