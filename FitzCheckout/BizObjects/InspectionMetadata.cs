using FitzCheckout.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitzCheckout.Models
{
    
    class InspectionMetadata
    {
        public int ID { get; set; }
        public string Dealership { get; set; }
        public string Mileage { get; set; }
        public string Model{ get; set; }
        public string Year { get; set; }
        public string Vin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime InspectionDate { get; set; }

        public InspectionMetadata GetInspectionMetadata(int recordID)
        {
            string qs = @"SELECT 
                            cr.ID
                            , cr.MetaDataValue1 Dealership
                            , cr.MetaDataValue2 Mileage
                            , cr.MetaDataValue3 Year
                            , cr.MetaDataValue5 Model
                            , cr.MetaDataValue7 Vin
                            , u.FirstName 
                            , u.LastName
                            , cr.DateUpdated InspectionDate
                        FROM [Checklists].[dbo].[ChecklistRecord] cr
                            LEFT OUTER JOIN [FitzDB].[dbo].[Users] u ON cr.UserID = u.ID
                        WHERE cr.ID = @ID";
            return SqlMapperUtil.SqlWithParams<InspectionMetadata>(qs, new { @ID = recordID }).FirstOrDefault();
        }
    }
}
