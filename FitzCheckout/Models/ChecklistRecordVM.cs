using FitzChecklist.BizObjects;
using FitzChecklist.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzChecklist.Models
{
    public interface IChecklistRecordVM
    {
        ChecklistRecordVM GetChecklistRecordVM(int checklistRecordID);
    }
    public class ChecklistRecordVM: IChecklistRecordVM
    {
        private readonly IChecklistRecord _checklistRecord;
        private readonly IChecklist _checklist;

        public int ID { get; set; }
        public string ChecklistName { get; set; }
        public string Description { get; set; }
        public int RecordID { get; set; }

        //Labels for the metadata values
        public string MetaDataTitle1 { get; set; }
        public string MetaDataTitle2 { get; set; }
        public string MetaDataTitle3 { get; set; }
        public string MetaDataTitle4 { get; set; }
        public string MetaDataTitle5 { get; set; }
        public string MetaDataTitle6 { get; set; }

        //the record metadata values
        public string MetaDataValue1 { get; set; }
        public string MetadataValue2 { get; set; }
        public string MetadataValue3 { get; set; }
        public string MetaDataValue4 { get; set; }
        public string MetaDataValue5 { get; set; }
        public string MetaDataValue6 { get; set; }


        public ChecklistRecordVM()
        {

        }

        public ChecklistRecordVM(IChecklist checklist, IChecklistRecord checklistRecord)
        {
            _checklist = checklist;
            _checklistRecord = checklistRecord;
        }
        public ChecklistRecordVM GetChecklistRecordVM(int checklistRecordID)
        {
            ChecklistRecordVM recordVM = new ChecklistRecordVM();

            //get the checklist metadata
            Checklist checklistInfo = GetChecklistInformation(checklistRecordID);
            if (checklistInfo.ID != 0)
            {
                recordVM.ID = checklistInfo.ID;
                recordVM.ChecklistName = checklistInfo.Name;
                recordVM.Description = checklistInfo.Description;
                recordVM.RecordID = checklistRecordID;
                recordVM.MetaDataTitle1 = checklistInfo.MetaDataTitle1;
                recordVM.MetaDataTitle2 = checklistInfo.MetaDataTitle2;
                recordVM.MetaDataTitle3 = checklistInfo.MetaDataTitle3;
                recordVM.MetaDataTitle4 = checklistInfo.MetaDataTitle4;
                recordVM.MetaDataTitle5 = checklistInfo.MetaDataTitle5;
                recordVM.MetaDataTitle6 = checklistInfo.MetaDataTitle6;
                //vvvvvvvv could be a good place for fueltype

            }

            ChecklistRecord checklistRecordInfo = GetChecklistRecordInformation(checklistRecordID);
            if (checklistRecordInfo.ID > 0)
            {
                recordVM.MetaDataValue1 = checklistRecordInfo.MetaDataValue1;
                recordVM.MetadataValue2 = checklistRecordInfo.MetadataValue2;
                recordVM.MetadataValue3 = checklistRecordInfo.MetadataValue3;
                recordVM.MetaDataValue4 = checklistRecordInfo.MetaDataValue4;
                recordVM.MetaDataValue5 = checklistRecordInfo.MetaDataValue5;
                recordVM.MetaDataValue6 = checklistRecordInfo.MetaDataValue6;
                //vvvvvvvv could be a good place for fueltype
            }

            return recordVM;
        }

        private Checklist GetChecklistInformation(int checklistRecordID)
        {

            int checklistID = _checklistRecord.GetChecklistIDByChecklistRecordID(checklistRecordID);
            Checklist checklist = _checklist.GetChecklistByID(checklistID);
            return checklist;
        }

        private ChecklistRecord GetChecklistRecordInformation(int checklistRecordID)
        {
            ChecklistRecord record = _checklistRecord.GetChecklistRecordByID(checklistRecordID);

            return record;

        }
    }
}