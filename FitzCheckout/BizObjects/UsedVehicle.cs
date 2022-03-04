using FitzCheckout.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace FitzCheckout.BizObjects
{
    public interface IUsedVehicle
    {
        List<UsedVehicle> Search(string searchValues, SearchType searchType);
        List<UsedVehicle> Search(string searchValues, SearchType searchType, List<string> excludeStockNumbers, List<string> exlcudeVin);

        UsedVehicle GetVehicleByID(int ID);
    }
    public class UsedVehicle : IUsedVehicle
    {
        public Int16 UsedID { get; set; }
        public string Drloc { get; set; }
        public int Miles { get; set; }
        public string Yr { get; set; }
        public string Make { get; set; }
        public string Carline { get; set; }
        public string Stk { get; set; }
        public string Vin { get; set; }
        public string DealerName { get; set; }
        public string PermissionCode { get; set; }

        public List<UsedVehicle> Search(string searchValues, SearchType searchType)
        {
            string qs = ConstructBaseQuery(searchValues, searchType);

            string connectionString = ConfigurationManager.ConnectionStrings["Junk"].ConnectionString;
            return SqlMapperUtil.SqlWithParams<UsedVehicle>(qs, new { }, connectionString).ToList();


        }

        public List<UsedVehicle> Search(string searchValues, SearchType searchType, List<string> excludeStockNumbers, List<string> excludeVins)
        {
            string qs = ConstructBaseQuery(searchValues, searchType);


            StringBuilder excludeClause = new StringBuilder();
            StringBuilder stockNumberBuilder = new StringBuilder();
            StringBuilder vinBuilder = new StringBuilder();

            if (excludeStockNumbers != null && excludeStockNumbers.Count > 0)
            {

                excludeClause.Append("AND stk NOT IN (");
                foreach (string stockNumber in excludeStockNumbers)
                {
                    if (stockNumberBuilder.Length > 0)
                    {
                        stockNumberBuilder.Append(",");
                    }
                    stockNumberBuilder.Append("'" + stockNumber + "'");
                }
                excludeClause.Append(stockNumberBuilder.ToString());
                excludeClause.Append(")");

            }

            if (excludeVins != null && excludeVins.Count > 0)
            {
                excludeClause.Append(" AND vin NOT IN (");

                foreach (string vin in excludeVins)
                {
                    if (vinBuilder.Length > 0)
                    {
                        vinBuilder.Append(",");
                    }
                    vinBuilder.Append("'" + vin + "'");
                }
                excludeClause.Append(vinBuilder.ToString());
                excludeClause.Append(")");

            }
            qs = qs + excludeClause.ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["Junk"].ConnectionString;
                return SqlMapperUtil.SqlWithParams<UsedVehicle>(qs, new { }, connectionString).ToList();

        }

        private string ConstructBaseQuery(string searchValues, SearchType searchType)
        {
            string[] metadata = searchValues.Split(',');

            StringBuilder where = new StringBuilder(" WHERE ");
            if (metadata[0] != "undefined" && !String.IsNullOrEmpty(metadata[0]))
                where.Append("DRloc LIKE '%" + metadata[0] + "%' " + searchType + " ");

            if (metadata[1] != "undefined" && !String.IsNullOrEmpty(metadata[1]))
                where.Append("miles LIKE '%" + metadata[1] + "%' " + searchType + " ");

            if (metadata[2] != "undefined" && !String.IsNullOrEmpty(metadata[2]))
                where.Append("yr LIKE '%" + metadata[2] + "%' " + searchType + " ");

            if (metadata[3] != "undefined" && !String.IsNullOrEmpty(metadata[3]))
                where.Append("make LIKE '%" + metadata[3] + "%' " + searchType + " ");

            if (metadata[4] != "undefined" && !String.IsNullOrEmpty(metadata[4]))
                where.Append("carline LIKE '%" + metadata[4] + "%' " + searchType + " ");

            if (metadata[5] != "undefined" && !String.IsNullOrEmpty(metadata[5]))
                where.Append("stk LIKE '%" + metadata[5] + "%' " + searchType + " ");

            if (metadata[6] != "undefined" && !String.IsNullOrEmpty(metadata[6]))
                where.Append("vin LIKE '%" + metadata[6] + "%' " + searchType + " ");

            string whereClause = where.ToString();

            if (whereClause != " WHERE ")
            {
                whereClause = whereClause.Substring(0, whereClause.Length - 4);
            }
            else
            {
                whereClause = "";
            }

            var qs = @"SELECT DISTINCT
                        UsedID
                        , Drloc
                        , miles
                        , yr
                        , make
                        , carline
                        , stk
                        , vin 
		                , CASE 
				                WHEN DRloc = 'LFT' AND v.Mall = 'GA' THEN (SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'LFT' AND Mall = 'GA') 
				                WHEN DRloc = 'LFT' AND v.Mall = 'GM' THEN (SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'LFT' AND Mall = 'GM') 
				                WHEN DRloc = 'FBS' AND v.Mall = 'WN' THEN (SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'FBS' AND Mall = 'WN') 
				                WHEN DRloc = 'FBS' AND v.Mall = 'WF' THEN (SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'FBS' AND Mall = 'WF') 
				                ELSE (SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] WHERE LocCode = V.DRloc)
			                END as PermissionCode
		                , CASE 
				                WHEN DRloc = 'LFT' AND v.Mall = 'GA' THEN (SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'LFT' AND Mall = 'GA') 
				                WHEN DRloc = 'LFT' AND v.Mall = 'GM' THEN (SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'LFT' AND Mall = 'GM') 
				                WHEN DRloc = 'FBS' AND v.Mall = 'WN' THEN (SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'FBS' AND Mall = 'WN') 
				                WHEN DRloc = 'FBS' AND v.Mall = 'WF' THEN (SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'FBS' AND Mall = 'WF') 
				                ELSE (SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = V.DRloc)
			                END as DealerName
                    FROM [JUNK].[dbo].[CSV_vehicleUSED] v
                        LEFT OUTER JOIN [Checklists].[dbo].[locations_lkup] l on v.DRloc = l.loccode " + whereClause;
            return qs;
        }

        public UsedVehicle GetVehicleByID(int ID)
        {
            string qs = @"SELECT DISTINCT
                            UsedID 
                            ,Drloc 
                            ,miles 
                            ,yr
                            ,make
                            ,carline
                            ,stk
                            ,Vin 
 		                , CASE 
				                WHEN DRloc = 'LFT' AND v.Mall = 'GA' THEN (SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'LFT' AND Mall = 'GA') 
				                WHEN DRloc = 'LFT' AND v.Mall = 'GM' THEN (SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'LFT' AND Mall = 'GM') 
				                WHEN DRloc = 'LFO' THEN (SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'LFO') 
				                WHEN DRloc = 'FBS' AND v.Mall = 'WN' THEN (SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'FBS' AND Mall = 'WN') 
				                WHEN DRloc = 'FBS' AND v.Mall = 'WF' THEN (SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'FBS' AND Mall = 'WF') 
				                ELSE (SELECT PermissionCode from [checklists].[dbo].[Locations_lkup] WHERE LocCode = V.DRloc)
			                END as PermissionCode
		                , CASE 
				                WHEN DRloc = 'LFT' AND v.Mall = 'GA' THEN (SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'LFT' AND Mall = 'GA') 
				                WHEN DRloc = 'LFT' AND v.Mall = 'GM' THEN (SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'LFT' AND Mall = 'GM') 
				                WHEN DRloc = 'LFO' THEN (SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'LFO') 
				                WHEN DRloc = 'FBS' AND v.Mall = 'WN' THEN (SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'FBS' AND Mall = 'WN') 
				                WHEN DRloc = 'FBS' AND v.Mall = 'WF' THEN (SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = 'FBS' AND Mall = 'WF') 
				                ELSE (SELECT FullName from [checklists].[dbo].[Locations_lkup] WHERE LocCode = V.DRloc)
			                END as DealerName
                       FROM [JUNK].[dbo].[CSV_vehicleUSED] V
                            LEFT OUTER JOIN [Checklists].[dbo].[locations_lkup] l on v.DRloc = l.loccode
                       WHERE UsedID = @ID";

            return SqlMapperUtil.SqlWithParams<UsedVehicle>(qs, new { ID = ID }).FirstOrDefault();

        }
    }
}