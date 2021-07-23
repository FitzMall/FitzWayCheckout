using FitzCheckout.BizObjects;
using FitzCheckout.DAL;
using FitzCheckout.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FitzCheckout.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUser _user;
        private readonly IAssociate _associate;
        private User currentUser;


        public AdminController(IUser user, IAssociate associate)
        {
            _user = user;
            _associate = associate;
        }

        // GET: Admin
        public ActionResult Index()
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;

            var adminVM = new AdminVM();
            adminVM.associates = _associate.SelectListItems();
            ViewBag.DisplayErrorMessage = false;



            return View(adminVM);
        }
        [HttpPost]
        public ActionResult Index(string submit, AdminVM adminVM)
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;

            var newAdminVM = new AdminVM();

            if (submit.ToUpper() == "SAVE")
                //if (adminVM.locations != null)
                {
                    try
                {
                    if (adminVM.SelectedAssociate.UserRole == UserRole.Admin)
                    {
                        foreach (var location in adminVM.locations)
                        {
                            location.HasAccess = true;
                        }
                    }
                }
                catch (NullReferenceException ex)
                {
                    ViewBag.DisplayErrorMessage = true;
                    adminVM.associates = _associate.SelectListItems();
                    return View(adminVM);
                }
                var isUpdated = _user.SavePermissions(adminVM.AssociateID, adminVM.SelectedAssociate.UserRole, adminVM.locations);

                newAdminVM.AssociateID = 0;
            }
            else
            {
                if (adminVM.AssociateID > 0)
                {
                    ViewBag.DisplayErrorMessage = false;
                    newAdminVM.AssociateID = adminVM.AssociateID;
                    newAdminVM.SelectedAssociate = _associate.GetAssociateByID(adminVM.AssociateID);

                    var locations = new Location();
                    newAdminVM.locations = locations.GetAll();
                    if (newAdminVM.SelectedAssociate != null && newAdminVM.SelectedAssociate.Locations != null)
                    foreach( var location in newAdminVM.locations)
                    {
                        if (newAdminVM.SelectedAssociate.Locations.Contains(location.PermissionCode))
                        {
                            location.HasAccess = true;
                        }
                    }
                }
                ModelState.Clear();

            }
            newAdminVM.associates = _associate.SelectListItems();

            return View(newAdminVM);
        }

        private bool IsAuthorized()
        {
            //if (Request.Cookies["user"] == null)
            //    return false;

            if (HttpContext.Session["currentUser"] == null)
                return false;

            currentUser = (User)HttpContext.Session["currentUser"];

            if (currentUser.UserRole != UserRole.Admin)
                return false;

            return true;

        }

    }
}