using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.Models
{
    
    public class ChecklistSearchResultsVM
    {
        public string MetaDataTitle1 { get; set; }
        public string MetaDataTitle2 { get; set; }
        public string MetaDataTitle3 { get; set; }
        public string MetaDataTitle4 { get; set; }
        public string MetaDataTitle5 { get; set; }
        public string MetaDataTitle6 { get; set; }
        public string MetaDataTitle7 { get; set; }

        public List<ChecklistSearchResult> Results { get; set; }
    }

    public class ChecklistSearchResult
    {
        public int ChecklistRecordID { get; set; }

        public string MetaDataValue1 { get; set; }
        public string MetaDataValue2 { get; set; }
        public string MetaDataValue3 { get; set; }
        public string MetaDataValue4 { get; set; }
        public string MetaDataValue5 { get; set; }
        public string MetaDataValue6 { get; set; }
        public string MetaDataValue7 { get; set; }
    }
}