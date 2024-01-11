using FitzCheckout.BizObjects;
using FitzCheckout.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.Models
{
    public interface IChecklistSectionVM
    {
        List<ChecklistSectionVM> GetNewSectionsVMByChecklistID(int checklistID);
        List<ChecklistSectionVM> GetSectionsByChecklistRecordID(int checklistRecordID);
        List<ChecklistSectionVM> GetSections(int checklistID, int checklistRecordID, string FuelType);
    }

    public class ChecklistSectionVM : IChecklistSectionVM
    {
        private readonly IChecklistRecord _checklistRecord;
        private readonly IChecklistSection _checklistSection;
        private readonly IChecklistItemRecordVM _checklistItemRecordVM;
        public int ID { get; set; }
        public int ChecklistID { get; set; }
        public int ChecklistRecordID { get; set; }
        public int OrderNumber { get; set; }
        public string SectionLabel { get; set; }
        public string OptionName1 { get; set; }
        public string OptionName2 { get; set; }
        public string OptionName3 { get; set; }
        public string OptionName4 { get; set; }
        public Byte HTMLHeaderType { get; set; }
        public bool CanCheckAllOption1 { get; set; }
        public bool CanCheckAllOption2 { get; set; }
        public bool CanCheckAllOption3 { get; set; }
        public bool CanCheckAllOption4 { get; set; }
        public List<ChecklistItemRecordVM> ChecklistItemRecords { get; set; }

        public ChecklistSectionVM()
        {

        }
        public ChecklistSectionVM(IChecklistRecord checklistRecord, IChecklistSection checklistSection, IChecklistItemRecordVM chdcklistItemRecordVM)
        {
            _checklistRecord = checklistRecord;
            _checklistSection = checklistSection;
            _checklistItemRecordVM = chdcklistItemRecordVM;
        }

        public List<ChecklistSectionVM> GetNewSectionsVMByChecklistID(int checklistID)
        {
            List<ChecklistSectionVM> sections = new List<Models.ChecklistSectionVM>();

            string qs = @"SELECT *
                    FROM [ChecklistsTEST].[dbo].[ChecklistSection_Hybrid_EV]
                    WHERE [ChecklistID] = @checklistID";
            var result = SqlMapperUtil.SqlWithParams<ChecklistSection>(qs, new { checklistID = checklistID }).ToList();
            foreach (var item in result)
            {
                ChecklistSectionVM sectionVM = new ChecklistSectionVM();
                sectionVM.ID = item.ID;
                sectionVM.ChecklistID = checklistID;
                sectionVM.SectionLabel = item.SectionLabel;
                sectionVM.HTMLHeaderType = item.HTMLHeaderType;
                sectionVM.OptionName1 = item.OptionName1;
                sectionVM.OptionName2 = item.OptionName2;
                sectionVM.OptionName3 = item.OptionName3;
                sectionVM.OptionName4 = item.OptionName4;
                sectionVM.CanCheckAllOption1 = item.CanCheckAllOption1;
                sectionVM.CanCheckAllOption2 = item.CanCheckAllOption2;
                sectionVM.CanCheckAllOption3 = item.CanCheckAllOption3;
                sectionVM.CanCheckAllOption4 = item.CanCheckAllOption4;

                sectionVM.ChecklistItemRecords = _checklistItemRecordVM.GetItemsByIDs(0, sectionVM.ID," ('ALL','ICHYBRID','EV')");

                sections.Add(sectionVM);
            }
            return sections;
        }

        public List<ChecklistSectionVM> GetSectionsByChecklistRecordID(int checklistRecordID)
        {
            List<ChecklistSectionVM> sections = new List<Models.ChecklistSectionVM>();

            int checklistID = _checklistRecord.GetChecklistIDByChecklistRecordID(checklistRecordID);

            sections = GetNewSectionsVMByChecklistID(checklistID);

            return sections;
        }

        public List<ChecklistSectionVM> GetSections(int checklistID, int checklistRecordID, string FuelType)
        {
            List<ChecklistSectionVM> sections = new List<Models.ChecklistSectionVM>();

            string qs = @"SELECT *
                    FROM [ChecklistsTEST].[dbo].[ChecklistSection_Hybrid_EV]
                    WHERE [ChecklistID] = @checklistID
                    AND [Fuel] IN " + FuelType;

            var result = SqlMapperUtil.SqlWithParams<ChecklistSection>(qs, new { checklistID = checklistID }).ToList();
            foreach (var item in result)
            {
                ChecklistSectionVM sectionVM = new ChecklistSectionVM();
                sectionVM.ID = item.ID;
                sectionVM.ChecklistID = checklistID;
                sectionVM.SectionLabel = item.SectionLabel;
                sectionVM.HTMLHeaderType = item.HTMLHeaderType;
                sectionVM.OptionName1 = item.OptionName1;
                sectionVM.OptionName2 = item.OptionName2;
                sectionVM.OptionName3 = item.OptionName3;
                sectionVM.OptionName4 = item.OptionName4;
                sectionVM.CanCheckAllOption1 = item.CanCheckAllOption1;
                sectionVM.CanCheckAllOption2 = item.CanCheckAllOption2;
                sectionVM.CanCheckAllOption3 = item.CanCheckAllOption3;
                sectionVM.CanCheckAllOption4 = item.CanCheckAllOption4;

                sectionVM.ChecklistItemRecords = _checklistItemRecordVM.GetItemsByIDs(checklistRecordID, sectionVM.ID, FuelType);

                sections.Add(sectionVM);

            }
            return sections;

        }
    }
    }