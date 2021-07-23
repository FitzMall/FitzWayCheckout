using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FitzCheckout.BizObjects
{
    public enum SearchType
    {
        And,
        Or
    }

    public enum ChecklistStatus
    {
        Pending = 1,
        TechComplete = 2,
        Complete = 3,
        Handyman = 4,
        Wholesale = 5,
        RepairForRetail = 6,
        Reopen = 7
    }

    public enum RecordType
    {
        ChecklistRecord,
        UsedVehicle,
        Submitted
    }

    public enum UserRole
    {
        None = 0,
        Admin,
        Supervisor,
        Technician
    }



    public static class DropDownValues
    {

        public static string[] TireTreadMeasurment =
        {
            "Select One",
        "15/32",
        "14/32",
        "13/32",
        "12/32",
        "11/32",
        "10/32",
        "9/32",
        "8/32",
        "7/32",
        "6/32",
        "5/32",
        "4/32",
        "3/32",
        "2/32",
        "1/32"
        };

        public static string[] BrakeLiningMeasurment =
        {
            "Select One",
        "15/32",
        "14/32",
        "13/32",
        "12/32",
        "11/32",
        "10/32",
        "9/32",
        "8/32",
        "7/32",
        "6/32",
        "5/32",
        "4/32",
        "3/32",
        "2/32",
        "1/32"
        };

        public static string[] SparTireTypes =
        {
            "Select One",
            "N/A",
            "Donut",
            "Mobility Kit",
            "Full Size"
        };

    }
}