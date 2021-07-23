using FitzCheckout.BizObjects;
using FitzCheckout.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FitzCheckout.BizObjects
{
    public interface IUser
    {
        User GetUserByID(string userid);

        List<string> GetUserLocationCodes(decimal ID);
        List<User> GetUsersByLocation(List<string> locations);
        bool SavePermissions(decimal ID, UserRole Role, List<Location> permissions);
    }
    public class User : IUser
    {
        public decimal ID { get; set; }
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }

        public UserRole UserRole { get; set; }

        private string permissions;
        public List<string> Locations { get; set; }

        //private string permissions;
        //public List<LocationPermission> locationPermissions { get; set; }

        public User GetUserByID(string userid)
        {
            User newUser = new BizObjects.User();

            var qs = @"SELECT
                        u.ID
                        , u.UserID
                        , u.FirstName
                        , u.LastName
                        , a.UserRole
                        , a.Permissions
                    FROM [FitzDB].[dbo].[users] u
                        LEFT OUTER JOIN [Checklists].[dbo].[AccessList] a ON u.ID = a.UserID
                    WHERE u.UserID = @userID";

            newUser = SqlMapperUtil.SqlWithParams<User>(qs, new { @userID = userid }).FirstOrDefault();

            if (!String.IsNullOrEmpty(newUser.LastName))
            {
                if (!String.IsNullOrEmpty(newUser.FirstName))
                {
                    newUser.FullName = newUser.LastName + ", " + newUser.FirstName;
                }
                else
                {
                    newUser.FullName = newUser.LastName;
                }
            }
            else if (!String.IsNullOrEmpty(newUser.FirstName))
            {
                newUser.FullName = newUser.FirstName;
            }

            if (!String.IsNullOrEmpty(newUser.permissions))
            {
                var locationArray = newUser.permissions.Split(',');

                newUser.Locations = new List<string>(); 
                for (int i = 0; i < locationArray.Length; i++)
                {
                    newUser.Locations.Add(locationArray[i]);
                }

            }
            return newUser;
        }

       public List<User> GetUsersByLocation(List<string> locations)
        {
            List<User> users = new List<BizObjects.User>();

            string whereClause = "";

            if (locations != null && locations.Count > 0)
            {
                var permissions = new StringBuilder("WHERE ");
                foreach (var location in locations)
                {
                    if (String.Equals(permissions.ToString(), "WHERE ", StringComparison.CurrentCultureIgnoreCase))
                    {
                        permissions.Append("permissions LIKE '%" + location + "%'");
                    }
                    else
                    {
                        permissions.Append(" OR permissions LIKE '%" + location + "%'");
                    }
                }
                whereClause = permissions.ToString();
            }

            var qs = @"SELECT
                        u.ID
                        , u.FirstName
                        , u.LastName
                        , a.UserRole
                        , a.Permissions
                    FROM [FitzDB].[dbo].[users] u
                        LEFT OUTER JOIN [Checklists].[dbo].[AccessList] a ON u.ID = a.UserID " 
                        + whereClause;
            users = SqlMapperUtil.SqlWithParams<User>(qs, null);

            

            if (users != null && users.Count >0)
            {
                foreach (var user in users)
                {
                    user.FullName = user.LastName + ", " + user.FirstName;
                }
            }


            return users;
        }

        public bool SavePermissions(decimal ID, UserRole Role, List<Location> permissions)
        {
            var locations = new StringBuilder();
            foreach (var item in permissions)
            {
                if (item.HasAccess)
                {
                    locations.Append(item.PermissionCode + ",");
                }
            }

            string locationPermissions = locations.ToString();
            var results = SqlMapperUtil.StoredProcWithParams<int>("upsert_Accesslist", new { UserID = ID, UserRole = Role, Permissions = locationPermissions.Substring(0, locationPermissions.Length - 1)});
            return true;
        }

        public List<string> GetUserLocationCodes(decimal ID)
        {
            var locationCodes = new List<string>();
            var qs = @"SELECT Permissions FROM [Checklists].[dbo].[AccessList] WHERE UserID = @id";
            var permissions = SqlMapperUtil.SqlWithParams<string>(qs, new { @id = ID }).FirstOrDefault();
            if (!String.IsNullOrEmpty(permissions))
            {
                string[] codes = permissions.Split(',');
                for (int i = 0; i < codes.Length; i++)
                {
                    locationCodes.Add(codes[i]);
                }
            }

            return locationCodes;
        }
    }
}