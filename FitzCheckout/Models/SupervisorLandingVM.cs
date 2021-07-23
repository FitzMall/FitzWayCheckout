using FitzCheckout.BizObjects;
using FitzCheckout.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.Models
{
    public class SupervisorLandingVM
    {
        public List<SupervisorTableData> TableData { get; set; }

        public List<ChecklistRecord> Checklists { get; set; }

        public SupervisorLandingVM()
        {
            Checklists = new List<BizObjects.ChecklistRecord>();
            TableData = new List<BizObjects.SupervisorTableData>();
        }

    }


}