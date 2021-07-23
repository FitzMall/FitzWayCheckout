using FitzCheckout.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.Models
{
    public interface IChecklistItemVM
    {
        ChecklistItemVM GetChecklistItemVMByChecklistItemID(int itemID);
        ChecklistItemVM GetChecklistITemByChecklistItemRecordID(int recordID);
    }
    public class ChecklistItemVM: IChecklistItemVM
    {
        private readonly IChecklistItem _checklistItem;
        private readonly IChecklistItemRecord _checklistItemRecord;

        public int ChecklistRecordID { get; set; }
        public int ChecklistItemRecordID { get; set; }
        public int SectionID { get; set; }
        public int ItemId { get; set; }
        public int OrderNumber { get; set; }
        public bool HasCheckbox { get; set; }
        public bool IsChecked { get; set; }
        public string OptionType1 { get; set; }
        public string OptionType2 { get; set; }
        public string OptionType3 { get; set; }
        public string OptionType4 { get; set; }
        public bool AllowAutoChecking { get; set; }
        public bool IsOption1Checked { get; set; }
        public bool IsOption2Checked { get; set; }
        public bool IsOption3Checked { get; set; }
        public bool IsOption4Checked { get; set; }
        public string Option1Text { get; set; }
        public string Option2Text { get; set; }
        public string Option3Text { get; set; }
        public string Option4Text { get; set; }


        public ChecklistItemVM()
        {

        }

        public ChecklistItemVM(IChecklistItem checklistItem, IChecklistItemRecord checklistItemRecord)
        {
            _checklistItem = checklistItem;
            _checklistItemRecord = checklistItemRecord;
        }

        public ChecklistItemVM GetChecklistItemVMByChecklistItemID(int itemID)
        {
            ChecklistItemVM itemVM = new ChecklistItemVM();
            ChecklistItem item = _checklistItem.GetChecklistItemByID(itemID);
            if (item != null && item.ID > 0)
            {
                itemVM.ItemId = itemID;
                itemVM.SectionID = item.ChecklistSectionID;
                itemVM.OrderNumber = item.OrderNumber;
                itemVM.HasCheckbox = item.HasCheckbox;
                itemVM.OptionType1 = item.OptionType1;
                itemVM.OptionType2 = item.OptionType2;
                itemVM.OptionType3 = item.OptionType3;
                itemVM.OptionType4 = item.OptionType4;
            }

            return itemVM;
        }

        public ChecklistItemVM GetChecklistITemByChecklistItemRecordID(int recordID)
        {
            ChecklistItemVM itemVM = new Models.ChecklistItemVM();
            int itemID = _checklistItemRecord.GetChecklistItemIDByChecklistItemRecordID(recordID);
            itemVM = GetChecklistItemVMByChecklistItemID(itemID);

            ChecklistItemRecord itemRecord = _checklistItemRecord.GetChecklistItemRecordByID(recordID);
            if (itemRecord != null && itemRecord.ID > 0)
            {
                itemVM.ChecklistItemRecordID = recordID;
                itemVM.ChecklistRecordID = itemRecord.ChecklistRecordID;
                itemVM.IsChecked = itemRecord.IsChecked;
                itemVM.IsOption1Checked = itemRecord.IsOption1Selected;
                itemVM.IsOption2Checked = itemRecord.IsOption2Selected;
                itemVM.IsOption3Checked = itemRecord.IsOption3Selected;
                itemVM.IsOption4Checked = itemRecord.IsOption4Selected;
                itemVM.Option1Text = itemRecord.Option1Text;
                itemVM.Option2Text = itemRecord.Option2Text;
                itemVM.Option3Text = itemRecord.Option3Text;
                itemVM.Option4Text = itemRecord.Option4Text;
            }

            return itemVM;
        }
    }
}