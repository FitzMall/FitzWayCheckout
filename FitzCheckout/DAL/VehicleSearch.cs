using FitzCheckout.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.DAL
{
    public interface IVehicleSearch
    {
        List<SearchResult> Search(string searchValues, SearchType searchType, int userID = -1);
    }
    public class VehicleSearch: IVehicleSearch
    {
        private readonly IChecklistRecord _checklistRecord;
        private readonly IUsedVehicle _usedVehicle;


        public VehicleSearch(IChecklistRecord checklistRecord, IUsedVehicle usedVehicle)
        {
            _checklistRecord = checklistRecord;
            _usedVehicle = usedVehicle;
        }

        public List<SearchResult> Search(string searchValues, SearchType searchType, int userID = -1)
        {
            List<SearchResult> foundVehicles = new List<SearchResult>();

            List<ChecklistStatus> statusList = new List<ChecklistStatus>();
            //statusList.Add(ChecklistStatus.Complete);

            //var existingRecords = _checklistRecord.Search(searchValues, searchType, userID, statusList);
            var existingRecords = _checklistRecord.Search(searchValues, searchType);

            var stockNumbers = from p in existingRecords
                               select p.MetaDataValue6;

            var Vins = from p in existingRecords
                       select p.MetaDataValue7;

            var usedRecords = _usedVehicle.Search(searchValues, searchType, stockNumbers.ToList(), Vins.ToList());

            foreach (var checklistRecord in existingRecords)
            {
                if (checklistRecord.Status == ChecklistStatus.Pending && checklistRecord.UserID != userID)
                {
                    SearchResult result = new DAL.SearchResult();
                    result.ID = checklistRecord.ID;
                    result.recordType = RecordType.ChecklistRecord;
                    result.MetaDataValue1 = checklistRecord.MetaDataValue1;
                    result.MetaDataValue2 = checklistRecord.MetaDataValue2;
                    result.MetaDataValue3 = checklistRecord.MetaDataValue3;
                    result.MetaDataValue4 = checklistRecord.MetaDataValue4;
                    result.MetaDataValue5 = checklistRecord.MetaDataValue5;
                    result.MetaDataValue6 = checklistRecord.MetaDataValue6;
                    result.MetaDataValue7 = checklistRecord.MetaDataValue7;
                    result.MetaDataValue8 = checklistRecord.MetaDataValue8;

                    foundVehicles.Add(result);
                }
            }

            foreach (var usedVehicle in usedRecords)
            {
                SearchResult result = new DAL.SearchResult();
                result.ID = usedVehicle.UsedID;
                result.recordType = RecordType.UsedVehicle;
                result.MetaDataValue1 = usedVehicle.DealerName;
                result.MetaDataValue2 = usedVehicle.Miles.ToString();
                result.MetaDataValue3 = usedVehicle.Yr;
                result.MetaDataValue4 = usedVehicle.Make;
                result.MetaDataValue5 = usedVehicle.Carline;
                result.MetaDataValue6 = usedVehicle.Stk;
                result.MetaDataValue7 = usedVehicle.Vin;
                result.MetaDataValue8 = usedVehicle.PermissionCode;

                foundVehicles.Add(result);
            }

            return foundVehicles.OrderBy(p => p.recordType).ThenBy(p => p.MetaDataValue6).ToList();
        }
    }


    public class SearchResult
    {
        public int ID { get; set; }
        public RecordType recordType  { get; set; }

        public string MetaDataValue1 { get; set; }
        public string MetaDataValue2 { get; set; }
        public string MetaDataValue3 { get; set; }
        public string MetaDataValue4 { get; set; }
        public string MetaDataValue5 { get; set; }
        public string MetaDataValue6 { get; set; }
        public string MetaDataValue7 { get; set; }
        public string MetaDataValue8 { get; set; }

    }

}