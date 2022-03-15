using FitzCheckout.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.BizObjects
{
    public interface ISupervisorTableData
    {
        List<SupervisorTableData> GetTableData(decimal userID);
    }

    public class SupervisorTableData : ISupervisorTableData
    {
        public string Location { get; set; }
        public string LocationCode { get; set; }
        public int OpenTechnicianItems { get; set; }
        public int OpenSupervisorItems { get; set; }

        public List<SupervisorTableData> GetTableData(decimal userID)
        {
            var qs = @"SELECT ((l.FullName) + SPACE(1) + '(' + (l.LocCode) + ')') AS Location , l.PermissionCode AS LocationCode, 
                (SELECT COUNT(*) as OpenTechnicianItems FROM ChecklistRecord WHERE MetaDataValue8 = l.PermissionCode and (Status = 1 OR Status = 7)) as OpenTechnicianItems ,
                (SELECT COUNT(*) as OpenSupervisorItems FROM ChecklistRecord WHERE MetaDataValue8 = l.PermissionCode and (Status = 2)) as OpenSupervisorItems
                FROM Locations_lkup l, AccessList a  
                WHERE a.UserID = @userID AND CHARINDEX(l.PermissionCode, a.Permissions) > 0 
                ORDER BY l.ID
            ";

            return SqlMapperUtil.SqlWithParams<SupervisorTableData>(qs, new { @userID = userID }).ToList();
        }

    }
}