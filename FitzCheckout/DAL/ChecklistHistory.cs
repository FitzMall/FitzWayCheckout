using System.Collections.Generic;
using System.Linq;
using FitzCheckout.BizObjects;
using System;
using System.ComponentModel.DataAnnotations;

namespace FitzCheckout.DAL
{
    public interface IChecklistHistory
    {
        List<ChecklistHistory> GetBasicHistory(int checklistRecordID);
        int GetMostRecentTechnician(int checklistRecordID);
    }
    
    public class ChecklistHistory: IChecklistHistory
    {
        public List<ChecklistHistory> GetBasicHistory(int checklistRecordID)
        {
            List<ChecklistHistory> history = new List<ChecklistHistory>();
            var qs = @"SELECT 
                            ch.ID
                            ,ch.ChecklistID
                            ,u.LastName + ', ' + u.FirstName FullName
                            ,ch.Status
                            ,ch.Action
                            ,ch.DateUpdated
                        FROM [ChecklistsTEST].[dbo].[ChecklistRecordHistory] ch, [FITZDB].[dbo].[users] u 
                        WHERE ch.UserID = u.ID AND  ch.ID = @ID 
                        UNION
                        SELECT 
                            cr.ID
                            ,cr.ChecklistID
                            ,u2.LastName + ', ' + u2.FirstName FullName
                            ,cr.Status
                            ,cr.Action
                            ,cr.DateUpdated
                        FROM [ChecklistsTEST].[dbo].[ChecklistRecord] cr, [FITZDB].[dbo].[users] u2 
                        WHERE cr.UserID = u2.ID AND cr.ID = @ID
                        ORDER BY DateUpdated";
            return SqlMapperUtil.SqlWithParams <ChecklistHistory>(qs, new {@ID = checklistRecordID}).ToList();
        }

        public int GetMostRecentTechnician(int ID)
        {
            return SqlMapperUtil.StoredProcWithParams<int>("GetMostRecentChecklistTechnician", new { @ID = ID }).FirstOrDefault();
        }

        public int RecordID { get; set; }
        public int ChecklistID { get; set; }
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string MetaDataValue1 { get; set; }
        public string MetaDataValue2 { get; set; }
        public string MetaDataValue3 { get; set; }
        public string MetaDataValue4 { get; set; }
        public string MetaDataValue5 { get; set; }
        public string MetaDataValue6 { get; set; }
        public string MetaDataValue7 { get; set; }
        public string MetaDataValue8 { get; set; }
        public ChecklistStatus Status { get; set; }
        public string Action { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}