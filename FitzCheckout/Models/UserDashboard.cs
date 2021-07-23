using FitzCheckout.BizObjects;
using FitzCheckout.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitzCheckout.Models
{
    public interface IUserDashboard
    {
        UserDashboard GetEmptyDashboard();
    }
    public class UserDashboard: IUserDashboard
    {
        public List<ChecklistRecord> Checklists { get; set; }
        public List<SearchResult> SearchResults { get; set; }

        public UserDashboard GetEmptyDashboard()
        {
            return new UserDashboard();
        }
    }
}