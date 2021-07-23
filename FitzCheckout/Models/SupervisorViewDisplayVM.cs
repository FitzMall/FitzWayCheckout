using FitzCheckout.BizObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.Models
{
    public interface ISupervisorViewDisplayVM
    { }
    public class SupervisorViewDisplayVM: ISupervisorViewDisplayVM
    {
        public ChecklistVM checklistInfo { get; set; }
        public ChecklistStatus NewStatus { get; set; }
        public int ReassignedToUserID { get; set; }
        public List<User> Users { get; set; }
    }
}