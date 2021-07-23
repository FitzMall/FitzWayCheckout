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
            var qs = @"SELECT 
	                    cr.MetadataValue1 Location
	                    , cr.MetaDataValue8 LocationCode
	                    , SUM(CASE WHEN Cr.Status = 1 OR Cr.Status = 7 THEN 1 ELSE 0 END) as OpenTechnicianItems
	                    , SUM( CASE WHEN Cr.Status = 2 THEN 1 ELSE 0 END) as OpenSupervisorItems 
                    FROM ChecklistRecord cr, AccessList a 
                    WHERE a.UserID = @userID 
	                    AND a.Permissions LIKE '%' + cr.MetaDataValue8 + '%' 
                    GROUP BY cr.MetadataValue1, cr.MetaDataValue8";

            return SqlMapperUtil.SqlWithParams<SupervisorTableData>(qs, new { @userID = userID }).ToList();
        }

    }
}