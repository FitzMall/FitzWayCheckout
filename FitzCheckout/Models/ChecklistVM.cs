using FitzCheckout.BizObjects;
using FitzCheckout.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitzCheckout.Models
{
    public interface IChecklistVM
    {
        ChecklistVM GetEmptyChecklistVMByChecklistID(int checklistID, string FuelType);
        ChecklistVM GetChecklistVMByChecklistRecordID(int checklistRecordID);
    }
    public class ChecklistVM: IChecklistVM  //, IValidatableObject
    {
        private readonly IChecklistRecord _checklistRecord;
        private readonly IChecklist _checklist;
        private readonly IChecklistSectionVM _checklistSectionVM;
        private readonly IChecklistHistory _checklistHistory;

        public ChecklistVM()
        {

        }

        public ChecklistVM(IChecklist checklist, IChecklistRecord checklistRecord, IChecklistSectionVM checklistSectionVM, IChecklistHistory checklistHistory)
        {
            _checklist = checklist;
            _checklistRecord = checklistRecord;
            _checklistSectionVM = checklistSectionVM;
            _checklistHistory = checklistHistory;
        }

        #region Properties

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
        public string MetaDataTitle7 { get; set; }
        public string MetaDataTitle8 { get; set; }

        //the record metadata values
        public string MetaDataValue1 { get; set; }
        public string MetaDataValue2 { get; set; }
        public string MetaDataValue3 { get; set; }
        public string MetaDataValue4 { get; set; }
        public string MetaDataValue5 { get; set; }
        public string MetaDataValue6 { get; set; }
        public string MetaDataValue7 { get; set; }
        public string MetaDataValue8 { get; set; }

        //Determines whether the metadata value is required
        public bool IsMeta1Required { get; set; }
        public bool IsMeta2Required { get; set; }
        public bool IsMeta3Required { get; set; }
        public bool IsMeta4Required { get; set; }
        public bool IsMeta5Required { get; set; }
        public bool IsMeta6Required { get; set; }
        public bool IsMeta7Required { get; set; }
        public bool IsMeta8Required { get; set; }

        public string FuelType { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public string PdfFileName { get; set; }

        public List<ChecklistSectionVM> sections { get; set; }

        public List<ChecklistHistory> history { get; set; }
        public List<SearchResult> SearchResults { get; set; }

        #endregion

        public ChecklistVM GetEmptyChecklistVMByChecklistID(int checklistID, string FuelType)
        {
            ChecklistVM recordVM = GetBaseChecklistInfo(checklistID);

            recordVM.sections = _checklistSectionVM.GetSections(checklistID, 0, FuelType);
            return recordVM;
        }

        public ChecklistVM GetChecklistVMByChecklistRecordID(int checklistRecordID)
        {
            ChecklistVM recordVM = new ChecklistVM();

            //get the checklist metadata
            int checklistID = _checklistRecord.GetChecklistIDByChecklistRecordID(checklistRecordID);
            if (checklistID > 0)
            {
                recordVM = GetBaseChecklistInfo(checklistID);
            }
            recordVM.RecordID = checklistRecordID;

            ChecklistRecord checklistRecordInfo = _checklistRecord.GetChecklistRecordInformation(checklistRecordID);
            if (checklistRecordInfo.ID > 0)
            {

                // MetaDataValue1 = DealerName;
                // MetaDataValue2 = Miles.ToString();
                // MetaDataValue3 = Yr;
                // MetaDataValue4 = Make;
                // MetaDataValue5 = Carline;
                // MetaDataValue6 = Stk;
                // MetaDataValue7 = Vin;

                recordVM.MetaDataValue1 = checklistRecordInfo.MetaDataValue1;
                recordVM.MetaDataValue2 = checklistRecordInfo.MetaDataValue2;
                recordVM.MetaDataValue3 = checklistRecordInfo.MetaDataValue3;
                recordVM.MetaDataValue4 = checklistRecordInfo.MetaDataValue4;
                recordVM.MetaDataValue5 = checklistRecordInfo.MetaDataValue5;
                recordVM.MetaDataValue6 = checklistRecordInfo.MetaDataValue6;
                recordVM.MetaDataValue7 = checklistRecordInfo.MetaDataValue7;
                recordVM.MetaDataValue8 = checklistRecordInfo.MetaDataValue8;
                recordVM.DateCreated = checklistRecordInfo.DateCreated;
                recordVM.DateUpdated = checklistRecordInfo.DateUpdated;
                recordVM.FuelType = checklistRecordInfo.FuelType.Trim();
            }

            UsedVehicle usedVehicle = new UsedVehicle();
            string FuelType = usedVehicle.GetFuel(checklistRecordInfo.MetaDataValue7);

            recordVM.sections = _checklistSectionVM.GetSections(checklistID, checklistRecordID, FuelType);

            recordVM.history = _checklistHistory.GetBasicHistory(checklistRecordID);
            return recordVM;
        }

        private ChecklistVM GetBaseChecklistInfo(int checklistID)
        {
            ChecklistVM recordVM = new ChecklistVM();
            Checklist checklistInfo = _checklist.GetChecklistByID(checklistID);
            if (checklistInfo.ID != 0)
            {
                recordVM.ID = checklistInfo.ID;
                recordVM.ChecklistName = checklistInfo.Name;
                recordVM.Description = checklistInfo.Description;
                recordVM.MetaDataTitle1 = checklistInfo.MetaDataTitle1;
                recordVM.MetaDataTitle2 = checklistInfo.MetaDataTitle2;
                recordVM.MetaDataTitle3 = checklistInfo.MetaDataTitle3;
                recordVM.MetaDataTitle4 = checklistInfo.MetaDataTitle4;
                recordVM.MetaDataTitle5 = checklistInfo.MetaDataTitle5;
                recordVM.MetaDataTitle6 = checklistInfo.MetaDataTitle6;
                recordVM.MetaDataTitle7 = checklistInfo.MetaDataTitle7;
                recordVM.MetaDataTitle8 = checklistInfo.MetaDataTitle8;

                recordVM.IsMeta1Required = checklistInfo.IsMeta1Required;
                recordVM.IsMeta2Required = checklistInfo.IsMeta2Required;
                recordVM.IsMeta3Required = checklistInfo.IsMeta3Required;
                recordVM.IsMeta4Required = checklistInfo.IsMeta4Required;
                recordVM.IsMeta5Required = checklistInfo.IsMeta5Required;
                recordVM.IsMeta6Required = checklistInfo.IsMeta6Required;
                recordVM.IsMeta7Required = checklistInfo.IsMeta7Required;
                recordVM.IsMeta8Required = checklistInfo.IsMeta8Required;
                recordVM.IsMeta8Required = checklistInfo.IsMeta8Required;

            }

            return recordVM;
        }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    var propertyMetadataValue5 = new[] { "MetaDataValue5" };


        //    if(IsMeta5Required && String.IsNullOrEmpty(MetaDataValue5))
        //        {
        //        yield return new ValidationResult("Stock # is required", propertyMetadataValue5);
        //    }
        //}
    }
}