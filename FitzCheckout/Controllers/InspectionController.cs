using FitzCheckout.BizObjects;
using FitzCheckout.DAL;
using FitzCheckout.Models;
using FitzCheckout.PDF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FitzCheckout.Controllers
{
    public class InspectionController : Controller
    {
        private readonly IChecklistRecord _checklistRecord;
        private readonly IChecklistItemRecord _checklistItemRecord;

        private readonly IChecklistVM _checklistVM;

        private readonly IInspectionFormVM _inspectionFormVM;
        private readonly IUsedVehicle _usedVehicle;
        private readonly IVehicleSearch _vehicleSearch;

        private readonly IUserDashboard _userDashboard;
        private readonly IPdfRenderer _pdfRenderer;
        private readonly IChecklistHistory _checklistHistory;

        private User currentUser;

        //private const int USERID = 5;


        public InspectionController(IChecklistRecord checklistRecord, IChecklistItemRecord checklistItemRecord, IChecklistVM checklistVM, IInspectionFormVM inspectionFormVM,
            IUsedVehicle usedVehicle, IVehicleSearch vehicleSearch, IUserDashboard userDashboard, IPdfRenderer pdfRenderer, IChecklistHistory checklistHistory)
        {
            _checklistRecord = checklistRecord;
            _checklistItemRecord = checklistItemRecord;
            _checklistVM = checklistVM;
            _inspectionFormVM = inspectionFormVM;
            _usedVehicle = usedVehicle;
            _vehicleSearch = vehicleSearch;
            _userDashboard = userDashboard;
            _pdfRenderer = pdfRenderer;
            _checklistHistory = checklistHistory;

        }


        public ActionResult Index()
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;

            currentUser = (User)HttpContext.Session["currentUser"];

            var test = _checklistHistory.GetMostRecentTechnician(183);

            ViewData["NoSearchResults"] = "false";
            var dashboard = _userDashboard.GetEmptyDashboard();
            dashboard.Checklists = _checklistRecord.GeChecklistRecordsByUserID((Int32)currentUser.ID, ChecklistStatus.Pending);
            dashboard.SearchResults = new List<DAL.SearchResult>();

            return View(dashboard);
        }

        [HttpPost]
        public ActionResult Index(string submit, string vin, string stockNumber)
        {

            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;
            vin = vin.Trim();
            stockNumber = stockNumber.Trim();
            var values = ",,,,," + stockNumber + "," + vin + ",";

            var newDashboard = _userDashboard.GetEmptyDashboard();
            newDashboard.Checklists = _checklistRecord.GeChecklistRecordsByUserID((Int32)currentUser.ID, ChecklistStatus.Pending);
            ChecklistVM newChecklistVM = new Models.ChecklistVM();

            ViewData["NoSearchResults"] = "true";
            ViewData["DetailsSidebarType"] = "Inspection";
            List<SearchResult> results = _vehicleSearch.Search(values, SearchType.And, (Int32)currentUser.ID);
            if (results.Count == 0)
            {
                newDashboard.SearchResults = new List<SearchResult>();
                return View(newDashboard);
            }
            else if (results.Count == 1)
            {
                return Redirect("Inspection/GetItem?ID=" + results[0].ID + "&type=" + results[0].recordType);
            }
            else
            {
                newDashboard.SearchResults = results;
                ViewData["NoSearchResults"] = "false";
                return View(newDashboard);

            }
        }

        public ActionResult SelectFuel(string id)
        {
            ChecklistVM newChecklistVM = new Models.ChecklistVM();
            int intID = Int32.Parse(id);

            newChecklistVM = _checklistVM.GetChecklistVMByChecklistRecordID(intID);
            return View(newChecklistVM);
        }

        [HttpPost]

        public ActionResult SelectFuel(ChecklistVM parChecklist, string FuelType)
        {

            // insert the value in the table
            // stored proc: [ChecklistRecordUpdateFuel]

           parChecklist.FuelType = FuelType;

            return RedirectToAction("GetItem", new { @id = parChecklist.ID });

        }
        public ActionResult InspectionForm()
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;

            var userChecklist = new UserChecklist();
            var res = userChecklist.GetUserChecklists((Int32)currentUser.ID);

            InspectionFormVM inspectionFormVM = _inspectionFormVM.GetInspectionForm(2);
            ViewBag.UserType = "Technician";
            return View(inspectionFormVM);
        }

        [HttpPost]
        public ActionResult InspectionForm(string submit, InspectionFormVM inspectionFormVM)
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;

            return View(inspectionFormVM);
        }

        [HttpPost]
        public ActionResult Details(string metadataValues, string FuelType)
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;


            // MetaDataValue1 = DealerName;
            // MetaDataValue2 = Miles.ToString();
            // MetaDataValue3 = Yr;
            // MetaDataValue4 = Make;
            // MetaDataValue5 = Carline;
            // MetaDataValue6 = Stk;
            // MetaDataValue7 = Vin;
            // MetaDataValue8 = PermissionCode;
            // MetaDataValue6 = Stk;
            // MetaDataValue7 = Vin;

            ChecklistVM newChecklistVM = new Models.ChecklistVM();
            if (String.IsNullOrEmpty(metadataValues))
            {
                FuelType = "('ALL')";
                ModelState.Clear();
                newChecklistVM = _checklistVM.GetEmptyChecklistVMByChecklistID(2, FuelType);
                ViewBag.UserType = "Technician";
                return PartialView("_Details", newChecklistVM);

            }
            else
            {
                List<SearchResult> results = _vehicleSearch.Search(metadataValues, SearchType.And);
                if (results.Count == 0)
                {
                    return new HttpStatusCodeResult(410, "Unable to find vehicle.");
                }
                else if (results.Count == 1)
                {
                    if (results[0].recordType == RecordType.ChecklistRecord || results[0].recordType == RecordType.Submitted)
                    {
                        return RedirectToAction("GetItem", new { ID = results[0].ID, RecordType.ChecklistRecord });
                    }
                    else
                    {
                        var url = "GetItem?ID=" + results[0].ID + "&type=" + RecordType.ChecklistRecord;
                        return Redirect(url);
                    }
                }
                else
                {
                    return PartialView("MatchingVehicles", results);

                }

            }
        }

        [HttpPost]
        public ActionResult Save(string submit, ChecklistVM checklistVM)
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;

            if (submit.ToUpper() == "SEARCH")
            {
                ModelState.Clear();
                InspectionFormVM newInspectionFormVM = _inspectionFormVM.GetInspectionForm(2);
                return RedirectToAction("Index");
                //return View("InspectionForm", newInspectionFormVM);
                //return View("InspectionForm");
            }

            if (submit.ToUpper() == "SUBMITTED")
            {
                int checklistRecordId = Save(checklistVM, ChecklistStatus.TechComplete);
                var filename = _pdfRenderer.CreatePdf(checklistRecordId);

                if (!String.IsNullOrEmpty(filename))
                {

                    var file = System.IO.Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["PdfLocation"]),
                                        filename);
                    //var file = System.IO.Path.Combine(ConfigurationManager.AppSettings["PdfLocation"],
                    //                    filename);
                    return Json(new
                    {
                        pdfUrl = Url.Action("DisplayPdf", "Inspection", new { @filename = filename }),
                        newUrl = Url.Action("Index", "Inspection")
                    });
                }
                else
                {
                    return Json(new
                    {
                        newUrl = Url.Action("Index", "Inspection")
                    });
                }
            }
            else
            {
                Save(checklistVM);

            }

            return Json(new
            {
                newUrl = Url.Action("Index", "Inspection")
            }
            );
        }

        public ActionResult GetItem(string ID, string type)
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;

            int intID = 0;
            string FuelFound = "";

            ChecklistVM newChecklistVM = new Models.ChecklistVM();
            if (!String.IsNullOrEmpty(ID))
            {
                intID = Int32.Parse(ID);

                if (type == RecordType.ChecklistRecord.ToString() || type == RecordType.Submitted.ToString())
                {

                    newChecklistVM = _checklistVM.GetChecklistVMByChecklistRecordID(intID);
                    FuelFound = newChecklistVM.FuelType;

                }
                else
                {
                    UsedVehicle usedVehicle = _usedVehicle.GetVehicleByID(intID);
                    FuelFound = _usedVehicle.GetFuel(usedVehicle.Vin);

                    newChecklistVM = _checklistVM.GetEmptyChecklistVMByChecklistID(2, FuelFound);

                    newChecklistVM.ID = 2;
                    newChecklistVM.MetaDataValue1 = usedVehicle.DealerName;
                    newChecklistVM.MetaDataValue2 = usedVehicle.Miles.ToString();
                    newChecklistVM.MetaDataValue3 = usedVehicle.Yr;
                    newChecklistVM.MetaDataValue4 = usedVehicle.Make;
                    newChecklistVM.MetaDataValue5 = usedVehicle.Carline;
                    newChecklistVM.MetaDataValue6 = usedVehicle.Stk;
                    newChecklistVM.MetaDataValue7 = usedVehicle.Vin;
                    newChecklistVM.MetaDataValue8 = usedVehicle.PermissionCode;
                    newChecklistVM.FuelType = FuelFound;
                }
                //return PartialView("Details", newChecklistVM);

                if (FuelFound == "('MISSING')" || FuelFound == "" || FuelFound == null)
                {
                    // return RedirectToAction("SelectFuel", new { @parChecklist = newChecklistVM, @FuelType = FuelFound});
                     return RedirectToAction("SelectFuel", new { @id = newChecklistVM.ID});

                }
            }

            ModelState.Clear();
            ModelState.Remove("sections");
            ViewBag.UserType = "Technician";
            return View(newChecklistVM);

        }

        public ActionResult DisplayPDF(string filename)
        {
            var file = System.IO.Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["PdfLocation"]),
                    filename);

            //var path = "J:/inetpub/wwwroot/production/FITZWAY/pictures/UCPDFS/";
            //file = path + filename;

            //var file = System.IO.Path.Combine(ConfigurationManager.AppSettings["PdfLocation"],
            //        filename);
            byte[] FileBytes = System.IO.File.ReadAllBytes(file);
            Response.AppendHeader("Content-Disposition", "inline; filename= " + filename);
            return File(FileBytes, "application/pdf");

        }


        private int Save(ChecklistVM checklistVM, ChecklistStatus currentStatus = ChecklistStatus.Pending)
        {
            int checklistRecordID = 0;
            string action = "Save";
            if (currentStatus == ChecklistStatus.Pending && checklistVM.RecordID == 0)
            {
                action = "Create";
            }
            else if (currentStatus == ChecklistStatus.Complete || currentStatus == ChecklistStatus.FitzWaySelect || currentStatus == ChecklistStatus.FitzWayHPE || currentStatus == ChecklistStatus.FitzWayPremium || currentStatus == ChecklistStatus.FitzWayValue)
            {
                action = "Complete";
            }

            checklistRecordID = _checklistRecord.Save(
                new ChecklistRecord
                {
                    ID = checklistVM.RecordID,
                    ChecklistID = checklistVM.ID,
                    UserID = (Int32)currentUser.ID,
                    MetaDataValue1 = checklistVM.MetaDataValue1,
                    MetaDataValue2 = checklistVM.MetaDataValue2,
                    MetaDataValue3 = checklistVM.MetaDataValue3,
                    MetaDataValue4 = checklistVM.MetaDataValue4,
                    MetaDataValue5 = checklistVM.MetaDataValue5,
                    MetaDataValue6 = checklistVM.MetaDataValue6,
                    MetaDataValue7 = checklistVM.MetaDataValue7,
                    MetaDataValue8 = checklistVM.MetaDataValue8,
                    DateCreated = checklistVM.DateCreated,
                    DateUpdated = DateTime.Now,
                    Status = currentStatus,
                    Action = action
                });

            foreach (ChecklistSectionVM sectionVM in checklistVM.sections)
            {
                if (sectionVM.ChecklistItemRecords != null)
                {
                    foreach (ChecklistItemRecordVM itemRecordVM in sectionVM.ChecklistItemRecords)
                    {


                        _checklistItemRecord.Save(new ChecklistItemRecord
                        {
                            ID = itemRecordVM.ChecklistItemRecordID,
                            ChecklistItemID = itemRecordVM.ItemId,
                            ChecklistSectionID = itemRecordVM.SectionID,
                            ChecklistRecordID = checklistRecordID,
                            IsChecked = itemRecordVM.IsChecked,
                            ITDropDownText1 = itemRecordVM.ITDropDownText1,
                            ITDropDownText2 = itemRecordVM.ITDropDownText2,
                            OptionType1 = itemRecordVM.OptionType1,
                            OptionType2 = itemRecordVM.OptionType2,
                            OptionType3 = itemRecordVM.OptionType3,
                            OptionType4 = itemRecordVM.OptionType4,
                            IsOption1Selected = itemRecordVM.IsOption1Selected,
                            IsOption2Selected = itemRecordVM.IsOption2Selected,
                            IsOption3Selected = itemRecordVM.IsOption3Selected,
                            IsOption4Selected = itemRecordVM.IsOption4Selected,
                            Option1Text = itemRecordVM.Option1Text,
                            Option2Text = itemRecordVM.Option2Text,
                            Option3Text = itemRecordVM.Option3Text,
                            Option4Text = itemRecordVM.Option4Text
                        });
                    }
                }
            }

            return checklistRecordID;
        }

        private bool IsAuthorized()
        {
            //if (Request.Cookies["user"] == null)
            //    return false;

            if (HttpContext.Session["currentUser"] == null)
                return false;

            currentUser = (User)HttpContext.Session["currentUser"];

            if (currentUser.UserRole != UserRole.Technician)
                return false;

            return true;

        }


    }
}