using FitzCheckout.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.DAL
{
    public interface IUserChecklist
    {
        List<UserChecklist> GetUserChecklists(int userID);
    }
    public class UserChecklist : IUserChecklist
    {
        public List<ChecklistRecord> Checklists { get; set; }
        public List<UserChecklist> GetUserChecklists(int userID)
        {
            List<ChecklistRecord> checklists = new List<BizObjects.ChecklistRecord>();
            var qs = @"SELECT
                        ID
                        ,UserID
                        ,MetaDataValue1
                        ,MetaDataValue2
                        ,MetaDataValue3
                        ,MetaDataValue4
                        ,MetaDataValue5
                        ,MetaDataValue6
                        ,MetaDataValue7
                        ,Status
                        ,Action
                    FROM [Checklists].[dbo].[ChecklistRecord]
                    WHERE UserID = @UserID";
            return SqlMapperUtil.SqlWithParams<UserChecklist>(qs, new { @UserID = userID });
        }

        public int ID { get; set; }
        public int UserID { get; set; }
        public string MetaDataValue1 { get; set; }
        public string MetaDataValue2 { get; set; }
        public string MetaDataValue3 { get; set; }
        public string MetaDataValue4 { get; set; }
        public string MetaDataValue5 { get; set; }
        public string MetaDataValue6 { get; set; }
        public string MetaDataValue7 { get; set; }
        public ChecklistStatus Status { get; set; }
        public string Action { get; set; }


    }
}