using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitzCheckout.DAL;

namespace FitzCheckout.BizObjects
{
    public interface IChecklist
    {
        Checklist GetChecklistByID(int ID);
        Checklist GetChecklistInformation(int checklistRecordID);
        Checklist GetMetaData(int checklistID);
    }

    public class Checklist : IChecklist
    {

        public Checklist()
        {

        }

        public Checklist(IChecklistSection checklistSection, IChecklistRecord checklistRecord)
        {
            _checklistSection = checklistSection;
            _checklistRecord = checklistRecord;
        }

        public Checklist GetChecklistByID(int ID)
        {

            string qs = @"SELECT * FROM [Checklists].[dbo].[Checklist] WHERE [ID] = @ID";
            var result = DAL.SqlMapperUtil.SqlWithParams<Checklist>(qs, new { ID = ID }).FirstOrDefault();
            if (result != null && result.ID > 0)
            {
                return result;
            }
            else
            {
                Checklist checklist = new Checklist();
                checklist.ID = 0;
                return checklist;
            }


        }

        public Checklist GetChecklistInformation(int checklistRecordID)
        {

            int checklistID = _checklistRecord.GetChecklistIDByChecklistRecordID(checklistRecordID);
            Checklist checklist = GetChecklistByID(checklistID);
            return checklist;
        }

        public Checklist GetMetaData(int checklistID)
        {
            if (checklistID <= 0)
            {
                throw new ArgumentException("A valid checklist ID must be provided.");
            }

            List<string> metadataTitles = new List<string>();
            string qs = @"SELECT ID,
                        Name,
                        Description,
                        MetaDataTitle1, 
                        MetaDataTitle2, 
                        MetaDataTitle3, 
                        MetaDataTitle4, 
                        MetaDataTitle5, 
                        MetaDataTitle6,
                        MetaDataTitle7,
                        MetaDataTitle8,
                        IsMeta1Searchable, 
                        IsMeta2Searchable, 
                        IsMeta3Searchable, 
                        IsMeta4Searchable, 
                        IsMeta5Searchable, 
                        IsMeta6Searchable,
                        IsMeta7Searchable,
                        IsMeta8Searchable,
                        IsMeta1Required,
                        IsMeta2Required,
                        IsMeta3Required,
                        IsMeta4Required,
                        IsMeta5Required,
                        IsMeta6Required,
                        IsMeta7Required
                        IsMeta8Required
                    FROM [Checklists].[dbo].[Checklist]
                    WHERE ID = @checklistID";

            
            return SqlMapperUtil.SqlWithParams<Checklist>(qs, new { checklistID = checklistID }).FirstOrDefault();
        }


        #region public properties
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string MetaDataTitle1 { get; set; }
        public string MetaDataTitle2 { get; set; }
        public string MetaDataTitle3 { get; set; }
        public string MetaDataTitle4 { get; set; }
        public string MetaDataTitle5 { get; set; }
        public string MetaDataTitle6 { get; set; }
        public string MetaDataTitle7 { get; set; }
        public string MetaDataTitle8 { get; set; }
        public bool IsMeta1Searchable { get; set; }
        public bool IsMeta2Searchable { get; set; }
        public bool IsMeta3Searchable { get; set; }
        public bool IsMeta4Searchable { get; set; }
        public bool IsMeta5Searchable { get; set; }
        public bool IsMeta6Searchable { get; set; }
        public bool IsMeta7Searchable { get; set; }
        public bool IsMeta8Searchable { get; set; }
        public bool IsMeta1Required { get; set; }
        public bool IsMeta2Required { get; set; }
        public bool IsMeta3Required { get; set; }
        public bool IsMeta4Required { get; set; }
        public bool IsMeta5Required { get; set; }
        public bool IsMeta6Required { get; set; }
        public bool IsMeta7Required { get; set; }
        public bool IsMeta8Required { get; set; }

        public List<ChecklistSection> Sections { get; set; }
        #endregion

        IChecklistSection _checklistSection;
        IChecklistRecord _checklistRecord;
    }

}