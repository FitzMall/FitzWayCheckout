using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitzCheckout.BizObjects;
using System.Collections.Specialized;
using FitzCheckout.PDF;
using System.Net;
using FitzCheckout.DAL;
using System.Configuration;

namespace FitzCheckout.Controllers
{
    public class HomeController : Controller
    {
        private const string USERTYPE = "S";
        private readonly IChecklist _checklist;
        private readonly IChecklistSection _checklistSection;
        private readonly IUser _user;
        private readonly IPdfRenderer _pdfRenderer;

        public HomeController(IChecklist checklist, IChecklistSection checklistSection, User user, IPdfRenderer pdfRenderer)
        {
            _checklist = checklist;
            _checklistSection = checklistSection;
            _user = user;
            _pdfRenderer = pdfRenderer;
        }

        public ActionResult Index()
        {

#if DEBUG
            //-------------------------------------------------------
            //------- jjs Let's create a cookie for testing

            // *** multiple user testing (for variety's sake
            if (Request.Cookies["user"] != null)
            {
                var cookie = Request.Cookies["user"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
                return View();

            }

            HttpCookie testCookie = new HttpCookie("user");
            testCookie["login"] = "burroughsd"; //Admin (originally striplingj)
            //testCookie["login"] = "jburnet1"; //Supervisor
            //testCookie["login"] = "daveb1"; //Technician
            testCookie["name"] = "Ralph";
            testCookie.Expires = DateTime.Now.AddDays(5);
            Response.Cookies.Add(testCookie);


#endif

            if (Request.Cookies["user"] != null)
            {
                var cookieValue = Request.Cookies["user"].Value;
                NameValueCollection qsCollection = HttpUtility.ParseQueryString(cookieValue);
                var userIDFromCookie = qsCollection["login"].ToString();
                var userNameFromCookie = qsCollection["name"].ToString();



                var currentUser = _user.GetUserByID(userIDFromCookie);
                HttpContext.Session["currentUser"] = currentUser;
                HttpContext.Session["userName"] = currentUser.FullName;
                HttpContext.Session["userRole"] = currentUser.UserRole.ToString();
                


                if (currentUser.UserRole == UserRole.Supervisor || currentUser.UserRole == UserRole.Admin)
                {
                    return RedirectToAction("Index", "Supervisor");
                }
                else if (currentUser.UserRole == UserRole.Technician)
                {
                    return RedirectToAction("Index", "Inspection");
                }
                else
                {
                    return RedirectToAction("Index", "Inspection");
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;
            }

        }

        //public ActionResult NotAuthorized()
        //{
        //    return View();
        //}
    }
}