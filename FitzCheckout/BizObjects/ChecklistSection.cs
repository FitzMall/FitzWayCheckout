using FitzCheckout.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.BizObjects
{
    public interface IChecklistSection
    {
        ChecklistSection GetChecklistSectionByID(int ID);
        List<ChecklistSection> GetSectionsByChecklistID(int checklistID);
    }

    public class ChecklistSection: IChecklistSection
    {

        private readonly IChecklistItem _checklistItem;
        public ChecklistSection()
        {

        }

        public ChecklistSection(IChecklistItem checklistItem)
        {
            _checklistItem = checklistItem;
        }

        #region public properties
        public int ID { get; set; }
        public int ChecklistID { get; set; }
        public int OrderNumber { get; set; }
        public string SectionLabel { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string OptionName1 { get; set; }
        public string OptionName2 { get; set; }
        public string OptionName3 { get; set; }
        public string OptionName4 { get; set; }
        public bool CanCheckAllOption1 { get; set; }
        public bool CanCheckAllOption2 { get; set; }
        public bool CanCheckAllOption3 { get; set; }
        public bool CanCheckAllOption4 { get; set; }

        public Byte HTMLHeaderType { get; set; }

        public List<ChecklistItem> ChecklistItems;
        #endregion

        public ChecklistSection GetChecklistSectionByID(int ID)
        {
            ChecklistSection section = new ChecklistSection();

            string qs = @"SELECT * FROM [Checklists].[dbo].[ChecklistSection] WHERE [ID] = @ID";
            var result = SqlMapperUtil.SqlWithParams<ChecklistSection>(qs, new { ID = ID }).FirstOrDefault();

            if (result == null)
            {
                section.ID = ID;
            }
            else
            {
                section.ID = ID;
                section.ChecklistID = result.ChecklistID;
                section.OrderNumber = result.OrderNumber;
                section.SectionLabel = result.SectionLabel;
                section.DateCreated = result.DateCreated;
                section.DateUpdated = result.DateUpdated;
                section.OptionName1 = result.OptionName1;
                section.OptionName2 = result.OptionName2;
                section.OptionName3 = result.OptionName3;
                section.OptionName4 = result.OptionName4;
                section.CanCheckAllOption1 = result.CanCheckAllOption1;
                section.CanCheckAllOption2 = result.CanCheckAllOption2;
                section.CanCheckAllOption3 = result.CanCheckAllOption3;
                section.CanCheckAllOption4 = result.CanCheckAllOption4;
                section.HTMLHeaderType = result.HTMLHeaderType;
            }

            //section.ChecklistItems = _checklistItem.GetChecklistItemsByChecklistSectionID(section.ID);

            return section;

        }

        public List<ChecklistSection> GetSectionsByChecklistID(int checklistID)
        {
            List<ChecklistSection> checklistSections = new List<ChecklistSection>();

            var qs = @"SELECT *
                            FROM [Checklists].[dbo].[ChecklistSection]
                            WHERE [ChecklistID] = @ID
                            ORDER BY [OrderNumber]";
            var result = SqlMapperUtil.SqlWithParams<ChecklistSection>(qs, new { ID = checklistID }).ToList();

            if (result != null && result.Count >= 1)
            {
                foreach (var item in result)
                {
                    ChecklistSection newSection = new ChecklistSection();
                    newSection.ID = item.ID;
                    newSection.ChecklistID = item.ChecklistID;
                    newSection.OrderNumber = item.OrderNumber;
                    newSection.SectionLabel = item.SectionLabel;
                    newSection.DateCreated = item.DateCreated;
                    newSection.DateUpdated = item.DateUpdated;
                    newSection.OptionName1 = item.OptionName1;
                    newSection.OptionName2 = item.OptionName2;
                    newSection.OptionName3 = item.OptionName3;
                    newSection.OptionName4 = item.OptionName4;
                    newSection.CanCheckAllOption1 = item.CanCheckAllOption1;
                    newSection.CanCheckAllOption2 = item.CanCheckAllOption2;
                    newSection.CanCheckAllOption3 = item.CanCheckAllOption3;
                    newSection.CanCheckAllOption4 = item.CanCheckAllOption4;
                    newSection.HTMLHeaderType = item.HTMLHeaderType;

                    //newSection.ChecklistItems = _checklistItem.GetChecklistItemsByChecklistSectionID(newSection.ID);

                    checklistSections.Add(newSection);
                }


            }
            return checklistSections;
        }

    }
}