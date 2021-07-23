using FitzCheckout.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace FitzCheckout.BizObjects
{
    public interface IChecklistRecord
    {
        ChecklistRecord GetChecklistRecordByID(int ID);

        ChecklistRecord GetChecklistRecordInformation(int checklistRecordID);
        int GetChecklistIDByChecklistRecordID(int checklistRecordID);
        int Save(ChecklistRecord checklistRecord);
        List<ChecklistRecord> Search(string metadataValues, SearchType searchType, int excludeUserID = -1, List<ChecklistStatus> statusCriteria = null);

        List<ChecklistRecord> GetOutsandingRecords(List<string> locationList, List<ChecklistStatus> statusList);

        List<ChecklistRecord> GeChecklistRecordsByUserID(int UserID, ChecklistStatus status);
        List<ChecklistRecord> GetRecordsByLocation(List<string> locations, List<ChecklistStatus> excludeStatus = null);
    }

    public class ChecklistRecord: IChecklistRecord
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ChecklistID { get; set; }
        public string FullName { get; set; }
        public string MetaDataValue1 { get; set; }
        public string MetaDataValue2 { get; set; }
        public string MetaDataValue3 { get; set; }
        public string MetaDataValue4 { get; set; }
        public string MetaDataValue5 { get; set; }
        [Required]
        public string MetaDataValue6 { get; set; }
        [Required]
        public string MetaDataValue7 { get; set; }
        public string MetaDataValue8 { get; set; }
        public ChecklistStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Action { get; set; }

        public List<ChecklistItem> checklistItemValues { get; set; } 



        public ChecklistRecord GetChecklistRecordByID(int ID)
        {
            ChecklistRecord checklistRecord = new ChecklistRecord();
            string qs = @"SELECT *
                            FROM [Checklists].[dbo].[ChecklistRecord] cr
                            WHERE [ID] = @ID";

            var results = SqlMapperUtil.SqlWithParams<ChecklistRecord>(qs, new { ID = ID }).FirstOrDefault();
            if (results == null)
            {
                checklistItemValues = new List<ChecklistItem>();
            }
            else
            {
                checklistRecord.ID = results.ID;
                checklistRecord.UserID = results.UserID;
                checklistRecord.ChecklistID = results.ChecklistID;
                checklistRecord.MetaDataValue1 = results.MetaDataValue1;
                checklistRecord.MetaDataValue2 = results.MetaDataValue2;
                checklistRecord.MetaDataValue3 = results.MetaDataValue3;
                checklistRecord.MetaDataValue4 = results.MetaDataValue4;
                checklistRecord.MetaDataValue5 = results.MetaDataValue5;
                checklistRecord.MetaDataValue6 = results.MetaDataValue6;
                checklistRecord.MetaDataValue7 = results.MetaDataValue7;
                checklistRecord.MetaDataValue8 = results.MetaDataValue8;
                checklistRecord.Status = (ChecklistStatus)results.Status;
                checklistRecord.DateCreated = results.DateCreated;
                checklistRecord.DateUpdated = results.DateUpdated;


            }

            return checklistRecord;
        }

        public int Save()
        {
            return Save(this);

        }
        public int Save(ChecklistRecord checklistRecord)
        {
            int result;
            if (checklistRecord.ID == 0)
            {
                result = SaveRecord(checklistRecord);
            }
            else
            {
                result = UpdateRecord(checklistRecord);
            }

            return result;
        }

        public int GetChecklistIDByChecklistRecordID(int checklistRecordID)
        {
            string qs = @"SELECT ChecklistID 
                            FROM [Checklists].[dbo].[ChecklistRecord]
                            WHERE [ID] = @ID";
            int result = SqlMapperUtil.SqlWithParams<int>(qs, new { ID = checklistRecordID }).FirstOrDefault();

            return result;
        }

        public ChecklistRecord GetChecklistRecordInformation(int checklistRecordID)
        {
            ChecklistRecord record = GetChecklistRecordByID(checklistRecordID);

            return record;
        }

        private int UpdateRecord(ChecklistRecord record)
        {
            var result = SqlMapperUtil.StoredProcWithParams<int>("ChecklistRecordUpdate", record);
            return record.ID;
        }

        private int SaveRecord(ChecklistRecord record)
        {


            string qs = @"INSERT INTO [Checklists].[dbo].[ChecklistRecord] 
                    (ChecklistID
                    ,UserID
                    ,MetaDataValue1
                    ,MetaDataValue2
                    ,MetaDataValue3
                    ,MetaDataValue4
                    ,MetaDataValue5
                    ,MetaDataValue6
                    ,MetaDataValue7
                    ,MetaDataValue8
                    ,Status
                    ,Action)
                OUTPUT INSERTED.ID
                VALUES
                    (@ChecklistID
                    ,@UserID
                    ,@MetaDataValue1
                    ,@MetaDataValue2
                    ,@MetaDataValue3
                    ,@MetaDataValue4
                    ,@MetaDataValue5
                    ,@MetaDataValue6
                    ,@MetaDataValue7
                    ,@MetaDataValue8
                    ,@Status
                    ,@Action)";
            int result = SqlMapperUtil.SqlWithParams<int>(qs, record).First();
            return result;
        }

        public List<ChecklistRecord> Search(string searchValues, SearchType searchType, int excludeUserID = -1, List<ChecklistStatus> statusCriteria = null)
        {
            List<ChecklistRecord> checklistRecords = new List<ChecklistRecord>();
            string[] metadata = searchValues.Split(',');
            StringBuilder where = new StringBuilder("WHERE ");

            for (int i = 0; i < metadata.Length; i++)
            {
                if (metadata[i] != "undefined" && !String.IsNullOrEmpty(metadata[i]))
                    where.Append("MetaDataValue" + (i + 1) + "  LIKE '%" + metadata[i] + "%' " + searchType + " ");

            }

            string whereClause = where.ToString();

            if (whereClause != "WHERE ")
            {
                whereClause = whereClause.Substring(0, whereClause.Length - 4);

                if (excludeUserID != -1)
                {
                    whereClause = whereClause + " AND UserID <> " + excludeUserID;
                }

                string qs = @"SELECT 
                                cr.ID, cr.MetaDataValue1, cr.MetaDataValue2, 
                                cr.MetaDataValue3, cr.MetaDataValue4, cr.MetaDataValue5, 
                                cr.MetaDataValue6, cr.MetaDataValue7, cr.MetaDataValue8, cr.Status, 
                                cr.UserID, uv.LastName + ', ' + uv.FirstName FullName, cr.DateCreated, cr.DateUpdated 
                            FROM [Checklists].[dbo].[ChecklistRecord] cr
                                LEFT OUTER JOIN [FITZDB].[dbo].[users] uv on cr.UserID = uv.ID " + whereClause;
                checklistRecords = SqlMapperUtil.SqlWithParams<ChecklistRecord>(qs, new { }).ToList();

                if (statusCriteria != null && statusCriteria.Count > 0)
                {
                    return checklistRecords.Where(r => statusCriteria.Contains(r.Status)).ToList();
                }

            }

            return checklistRecords;
        }

        public List<ChecklistRecord> GetOutsandingRecords(List<string> locationList, List<ChecklistStatus> statusList)
        {
            //WHERE MetadataValue8 IN ('l1', 'l2', l3') AND Status in (1, 2, 3)
            var whereBuilder = new StringBuilder("WHERE ");
            if (locationList !=null && locationList.Count > 0)
            {
                whereBuilder.Append("MetadataValue8 IN (");
                for(int i = 0; i < locationList.Count; i++)
                {
                    if (i == locationList.Count - 1)
                    {
                        whereBuilder.Append("'" + locationList[i] + "')");
                    }
                    else
                    {
                        whereBuilder.Append("'" + locationList[i] + "',");
                    }
                }
            }

            
            if (statusList != null && statusList.Count > 0)
            {
                if (whereBuilder.Length > 1)
                {
                    whereBuilder.Append(" AND ");
                }

                whereBuilder.Append("Status IN (");
                for (int i = 0; i < statusList.Count; i++)
                {
                    if (i == statusList.Count - 1)
                    {
                        whereBuilder.Append((int)statusList[i] + ")");
                    }
                    else
                    {
                        whereBuilder.Append((int)statusList[i] + ",");
                    }
                }
            }

            string whereClause = whereBuilder.ToString();
            var qs = @"SELECT
                        cr.ID, cr.MetaDataValue1, cr.MetaDataValue2, 
                        cr.MetaDataValue3, cr.MetaDataValue4, cr.MetaDataValue5, 
                        cr.MetaDataValue6, cr.MetaDataValue7, cr.MetaDataValue8, cr.Status, 
                        cr.UserID, uv.LastName + ', ' + uv.FirstName FullName, cr.DateCreated, cr.DateUpdated 
                    FROM [Checklists].[dbo].[ChecklistRecord] cr
                        LEFT OUTER JOIN [FITZDB].[dbo].[users] uv on cr.UserID = uv.ID " + whereClause;
            return SqlMapperUtil.SqlWithParams<ChecklistRecord>(qs, null);
        }

        public List<ChecklistRecord> GeChecklistRecordsByUserID(int userID, ChecklistStatus status)
        {
            var results = new List<ChecklistRecord>();
            string qs = @"SELECT cr.*
                            FROM [Checklists].[dbo].[ChecklistRecord] cr
                            JOIN [Checklists].[dbo].[ChecklistStatus] cs ON cr.Status = cs.ID
                            WHERE cr.[UserID] = @userID AND cs.Status = @status";

            results = SqlMapperUtil.SqlWithParams<ChecklistRecord>(qs, new { userID = userID, status = status.ToString() }).ToList();

            return results;
        }

        public List<ChecklistRecord> GetRecordsByLocation(List<string> locations, List<ChecklistStatus> includeStatus = null)
        {
            StringBuilder locationList = new StringBuilder();
            foreach (var location in locations)
            {
                locationList.Append("'" + location + "',");
            }


            string qs = @"SELECT cr.*, u.LastName + ', ' + u.FirstName FullName
                            FROM [Checklists].[dbo].[ChecklistRecord] cr, 
                                [FITZDB].[dbo].[users] u
                            WHERE cr.UserID = u.ID AND 
                                MetaDataValue8 in (" + locationList.ToString().Substring(0, locationList.ToString().Length - 1) + ")" +
                            "ORDER BY Status";

            var results = SqlMapperUtil.SqlWithParams<ChecklistRecord>(qs, null).ToList();

            if (results != null && includeStatus != null && includeStatus.Count > 0)
            {
                return results.Where(r => includeStatus.Contains(r.Status)).ToList();
            }

            return results ;
        }

    }
}