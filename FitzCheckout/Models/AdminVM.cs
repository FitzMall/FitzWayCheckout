using FitzCheckout.BizObjects;
using FitzCheckout.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.Models
{
    public interface IAdminVM
    {

    }

    public class AdminVM: IAdminVM
    {
        public decimal AssociateID { get; set; }
        public UserRole Role { get; set; }
        public List<AssociateForDropdown> associates { get; set; }

        public List<Location> locations { get; set; }

        public Associate SelectedAssociate { get; set; }
    }
}