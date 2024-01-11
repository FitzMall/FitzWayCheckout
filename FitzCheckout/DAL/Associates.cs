using FitzCheckout.BizObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.DAL
{
    public interface IAssociate
    {
        List<Associate> GetAssociatesList();
        List<AssociateForDropdown> SelectListItems();

        Associate GetAssociateByID(decimal ID);
    }
    public class Associate : IAssociate
    {
        private string permissions;
        public decimal ID { get; set; }
        public string  UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public UserRole UserRole { get; set; }
        public List<string> Locations { get; set; }

        public List<Associate> GetAssociatesList()
        {
            var associates = new List<Associate>();
            var qs = @"SELECT
                            ID, UserID, FirstName, LastName
                        FROM [FitzDB].[dbo].[Users]
                        ORDER BY LastName, FirstName";
            associates = SqlMapperUtil.SqlWithParams<Associate>(qs, null).ToList();

            if (associates != null && associates.Count > 0)
            {
                foreach (var associate in associates)
                {
                    associate.FullName = associate.LastName + ", " + associate.FirstName;
                }
            }

            return associates;
        }

        public List<AssociateForDropdown> SelectListItems()
        {
            var associates = new List<AssociateForDropdown>();
            var qs = @"SELECT
                            ID, UserID, FirstName, LastName
                        FROM [FitzDB].[dbo].[Users]
                        WHERE LastName IS NOT NULL AND FirstName IS NOT NULL
                        ORDER BY LastName, FirstName";
            var items = SqlMapperUtil.SqlWithParams<Associate>(qs, null).ToList();

            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    var selectlistItem = new AssociateForDropdown();

                    selectlistItem.Name = item.LastName + ", " + item.FirstName + " (" + item.UserID + ")";
                    selectlistItem.ID = item.ID;
                    associates.Add(selectlistItem);
                }
            }

            return associates;
        }

        public Associate GetAssociateByID(decimal ID)
        {
            var qs = @"SELECT
                            u.ID, u.UserID, u.FirstName, u.LastName, 
                            a.UserRole, a.Permissions
                        FROM [FitzDB].[dbo].[Users] u
                            LEFT OUTER JOIN [ChecklistsTEST].[dbo].[AccessList] a ON U.ID = a.UserID
                        WHERE u.ID = @ID";
            var associateInfo =  SqlMapperUtil.SqlWithParams<Associate>(qs, new { @ID = ID }).FirstOrDefault();

            if (!String.IsNullOrEmpty(associateInfo.permissions))
            {
                var locaionArray = associateInfo.permissions.Split(',');
                associateInfo.Locations = new List<string>();
                for (int i = 0; i < locaionArray.Length; i++)
                {
                    associateInfo.Locations.Add(locaionArray[i]);
                }
            }

            return associateInfo;
        }
    }

    public class AssociateForDropdown
    {
        public decimal ID { get; set; }
        public string Name { get; set; }
    }

    public class Location
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string PermissionCode { get; set; }
        public string LocCode { get; set; }
        public string Mall { get; set; }
        public bool HasAccess { get; set; }

        public List<Location> GetAll()
        {
            var qs = @"Select ID, FullName, PermissionCode, LocCode, Mall
                        FROM Locations_Lkup";
            return SqlMapperUtil.SqlWithParams<Location>(qs, null).ToList();
        }

        //public List<LocationPermission> GetByUserID(string userID)
        //{
        //    var accessList = new List<LocationPermission>();
        //    var qs = @"SELECT ID, UserID, Permissions
        //                FROM AccessList
        //                WHERE UserID = @userID";
        //    var results = SqlMapperUtil.SqlWithParams<LocationPermission>(qs, new { @userID = userID });

        //    foreach (var item in results)
        //    {
        //        JObject json = JObject.Parse(item.Permissions);

        //    }
        //    return accessList;
        //}
    }

    public struct PermissionJson
    {
        public int LocationID { get; set; }
        public  UserRole[] Permissions { get; set; }
    }
}