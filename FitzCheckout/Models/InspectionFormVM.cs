using FitzCheckout.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.Models
{
    public interface IInspectionFormVM
    {
        InspectionFormVM GetInspectionForm(int checklistID);
    }
    public class InspectionFormVM: IInspectionFormVM
    {
        private readonly IChecklist _checklist;

        #region Public Properties
        public int ChecklistRecordID { get; set; }
        public int ChecklistID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaDataTitle1 { get; set; }
        public string MetaDataTitle2 { get; set; }
        public string MetaDataTitle3 { get; set; }
        public string MetaDataTitle4 { get; set; }
        public string MetaDataTitle5 { get; set; }
        public string MetaDataTitle6 { get; set; }
        public string MetaDataTitle7 { get; set; }

        public string MetaDataValue1 { get; set; }
        public string MetaDataValue2 { get; set; }
        public string MetaDataValue3 { get; set; }
        public string MetaDataValue4 { get; set; }
        public string MetaDataValue5 { get; set; }
        public string MetaDataValue6 { get; set; }
        public string MetaDataValue7 { get; set; }

        public bool IsMeta1Searchable { get; set; }
        public bool IsMeta2Searchable { get; set; }
        public bool IsMeta3Searchable { get; set; }
        public bool IsMeta4Searchable { get; set; }
        public bool IsMeta5Searchable { get; set; }
        public bool IsMeta6Searchable { get; set; }
        public bool IsMeta7Searchable { get; set; }

        public bool IsMeta1Required { get; set; }
        public bool IsMeta2Required { get; set; }
        public bool IsMeta3Required { get; set; }
        public bool IsMeta4Required { get; set; }
        public bool IsMeta5Required { get; set; }
        public bool IsMeta6Required { get; set; }
        public bool IsMeta7Required { get; set; }
        #endregion

        public InspectionFormVM()
        {

        }
        public InspectionFormVM(IChecklist checklist)
        {
            _checklist = checklist;
        }
        public InspectionFormVM GetInspectionForm(int checklistID)
        {
            Checklist checklistMetadata = _checklist.GetMetaData(checklistID);

            InspectionFormVM inspectionform = new InspectionFormVM();


            inspectionform.ChecklistID = checklistMetadata.ID;
            inspectionform.Name = checklistMetadata.Name;
            inspectionform.Description = checklistMetadata.Description;
            inspectionform.MetaDataTitle1 = checklistMetadata.MetaDataTitle1;
            inspectionform.MetaDataTitle2 = checklistMetadata.MetaDataTitle2;
            inspectionform.MetaDataTitle3 = checklistMetadata.MetaDataTitle3;
            inspectionform.MetaDataTitle4 = checklistMetadata.MetaDataTitle4;
            inspectionform.MetaDataTitle5 = checklistMetadata.MetaDataTitle5;
            inspectionform.MetaDataTitle6 = checklistMetadata.MetaDataTitle6;
            inspectionform.MetaDataTitle7 = checklistMetadata.MetaDataTitle7;

            inspectionform.IsMeta1Searchable = checklistMetadata.IsMeta1Searchable;
            inspectionform.IsMeta2Searchable = checklistMetadata.IsMeta2Searchable;
            inspectionform.IsMeta3Searchable = checklistMetadata.IsMeta3Searchable;
            inspectionform.IsMeta4Searchable = checklistMetadata.IsMeta4Searchable;
            inspectionform.IsMeta5Searchable = checklistMetadata.IsMeta5Searchable;
            inspectionform.IsMeta6Searchable = checklistMetadata.IsMeta6Searchable;
            inspectionform.IsMeta7Searchable = checklistMetadata.IsMeta7Searchable;

            inspectionform.IsMeta1Required = checklistMetadata.IsMeta1Required;
            inspectionform.IsMeta2Required = checklistMetadata.IsMeta2Required;
            inspectionform.IsMeta3Required = checklistMetadata.IsMeta3Required;
            inspectionform.IsMeta4Required = checklistMetadata.IsMeta4Required;
            inspectionform.IsMeta5Required = checklistMetadata.IsMeta5Required;
            inspectionform.IsMeta6Required = checklistMetadata.IsMeta6Required;
            inspectionform.IsMeta7Required = checklistMetadata.IsMeta7Required;

            return inspectionform;
        }
    }
}