using FitzCheckout.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.BizObjects
{
    public interface IChecklistItem
    {
        ChecklistItem GetChecklistItemByID(int ID);
        List<ChecklistItem> GetChecklistItemsByChecklistSectionID(int ID);
    }
    public class ChecklistItem: IChecklistItem
    {

        #region public properties
        public int ID { get; set; }
        public int ChecklistRecordID { get; set; }
        public int ChecklistSectionID { get; set; }
        public int OrderNumber { get; set; }
        public bool HasCheckbox { get; set; }
        public string ItemText { get; set; }
        public string HasITDropDown1 { get; set; }
        public string HasITDropDown2 { get; set; }
        public string ITDropDownType1 { get; set; }
        public string ITDropDownType2 { get; set; }
        public string OptionType1 { get; set; }
        public string OptionType2 { get; set; }
        public string OptionType3 { get; set; }
        public string OptionType4 { get; set; }
        public string ResponseText { get; set; }
        public bool AllowAutoChecking { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        #endregion

        public ChecklistItem GetChecklistItemByID(int ID)
        {
            ChecklistItem item = new ChecklistItem();

            string qs = @"SELECT ID, ChecklistSectionID, OrderNumber, HasCheckbox, ItemText, HasITDropDown1, HasITDropDown2, ITDropDownType1, ITDropDownType2, OptionType1, OptionType2, OptionType3, OptionType4, AllowAutoChecking
                        FROM [ChecklistsTEST].[dbo].[ChecklistItem_Hybrid_EV] 
                        WHERE ID = @ID
                        ORDER BY ChecklistSectionID, OrderNumber";

            var result = SqlMapperUtil.SqlWithParams<ChecklistItem>(qs, new { ID = ID }).FirstOrDefault();

            if (result != null)
            {
                item.ID = result.ID;
                item.ChecklistSectionID = result.ChecklistSectionID;
                item.OrderNumber = result.OrderNumber;
                item.HasCheckbox = result.HasCheckbox;
                item.ItemText = result.ItemText;
                item.HasITDropDown1 = result.HasITDropDown1;
                item.HasITDropDown2 = result.HasITDropDown2;
                item.ITDropDownType1 = result.ITDropDownType1;
                item.ITDropDownType2 = result.ITDropDownType2;
                item.OptionType1 = result.OptionType1;
                item.OptionType2 = result.OptionType2;
                item.OptionType3 = result.OptionType3;
                item.OptionType4 = result.OptionType4;
            }

            return item;
        }

        public List<ChecklistItem> GetChecklistItemsByChecklistSectionID(int checklistSectionID)
        {
            List<ChecklistItem> items = new List<ChecklistItem>();
            string qs = @"SELECT ID, ChecklistSectionID, OrderNumber, 
                            HasCheckbox, ItemText, HasITDropDown1, 
                            HasITDropDown2, ITDropDownType1, ITDropDownType2, 
                            OptionType1, OptionType2, OptionType3, 
                            OptionType4, AllowAutoChecking 
                        FROM [ChecklistsTEST].[dbo].[ChecklistItem_Hybrid_EV]
                        WHERE [ChecklistSectionID] = @ID
                        ORDER BY [OrderNumber]";
            var results = SqlMapperUtil.SqlWithParams<ChecklistItem>(qs, new { ID = checklistSectionID }).ToList();

            if (results != null && results.Count >= 1)
            {
                foreach (var item in results)
                {
                    ChecklistItem newItem = new ChecklistItem();
                    newItem.ID = item.ID;
                    newItem.ChecklistSectionID = item.ChecklistSectionID;
                    newItem.OrderNumber = item.OrderNumber;
                    newItem.HasCheckbox = item.HasCheckbox;
                    newItem.ItemText = item.ItemText;
                    newItem.HasITDropDown1 = item.HasITDropDown1;
                    newItem.HasITDropDown2 = item.HasITDropDown2;
                    newItem.ITDropDownType1 = item.ITDropDownType1;
                    newItem.ITDropDownType2 = item.ITDropDownType2;
                    newItem.OptionType1 = item.OptionType1;
                    newItem.OptionType2 = item.OptionType2;
                    newItem.OptionType3 = item.OptionType3;
                    newItem.OptionType4 = item.OptionType4;

                    items.Add(newItem);
                }
            }

            return items;
        }
    }
}