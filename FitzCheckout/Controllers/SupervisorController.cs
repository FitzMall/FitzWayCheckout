using FitzCheckout.BizObjects;
using FitzCheckout.DAL;
using FitzCheckout.Models;
using FitzCheckout.PDF;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FitzCheckout.Controllers
{
    public class SupervisorController : Controller
    {
        private readonly IChecklistRecord _checklistRecord;
        private readonly IChecklistVM _checklistVM;
        private readonly ISupervisorViewDisplayVM _supervisorViewDisplayVM;
        private readonly IUser _user;
        private readonly IPdfRenderer _pdfRenderer;
        private readonly ISupervisorTableData _supervisorTableData;
        private readonly IChecklistHistory _checklistHistory;
        private readonly IUsedVehicle _usedVehicle;
        private User currentUser;

        public SupervisorController(IChecklistRecord checklistRecord, IChecklistVM checklistVM
            , ISupervisorViewDisplayVM supervisorViewDisplayVM, IUser user, IPdfRenderer pdfRenderer, ISupervisorTableData supervisorTableData, IChecklistHistory checklistHistory, IUsedVehicle usedVehicle)
        {
            _checklistRecord = checklistRecord;
            _checklistVM = checklistVM;
            _supervisorViewDisplayVM = supervisorViewDisplayVM;
            _user = user;
            _pdfRenderer = pdfRenderer;
            _supervisorTableData = supervisorTableData;
            _checklistHistory = checklistHistory;
            _usedVehicle = usedVehicle;
        }

        // GET: Supervisor

        public ActionResult SelectFuel()
        {
            if (!IsAuthorized())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            ViewBag.SearchType = "";
            var supervisorVM = new SupervisorLandingVM();

            if (HttpContext.Session["currentUser"] != null)
            {
                var currentUser = (User)HttpContext.Session["currentUser"];

                if (currentUser.UserRole == UserRole.Admin)
                {
                    ViewBag.IsAdmin = "true";
                }
                else
                {
                    ViewBag.IsAdmin = "false";
                }

                ViewBag.UserID = currentUser.ID;
                supervisorVM.TableData = _supervisorTableData.GetTableData(currentUser.ID);
                return View(supervisorVM);
            }
            else
            {
                return View(supervisorVM);
            }


        }

        [HttpPost]
        public ActionResult SelectFuel(string submit, string vin, string stockNumber)
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            var supervisorVM = new SupervisorLandingVM();
            vin = vin.Trim();
            stockNumber = stockNumber.Trim();

            if (HttpContext.Session["currentUser"] != null)
            {
                var currentUser = (User)HttpContext.Session["currentUser"];

                if (currentUser.UserRole == UserRole.Admin)
                {
                    ViewBag.IsAdmin = "true";
                }
                else
                {
                    ViewBag.IsAdmin = "false";
                }

            }
            else
            {
                ViewBag.IsAdmin = "false";
            }

            ViewBag.UserID = currentUser.ID;


            if (String.IsNullOrEmpty(vin) && String.IsNullOrEmpty(stockNumber))
            {
                return RedirectToAction("Index");
            }


            supervisorVM.TableData = _supervisorTableData.GetTableData(currentUser.ID);

            ViewBag.SearchType = "Search Results:";


            return View(supervisorVM);
        }
        // GET: Supervisor

        public ActionResult Index()
        {
            if (!IsAuthorized())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            ViewBag.SearchType = "";
            var supervisorVM = new SupervisorLandingVM();

            if (HttpContext.Session["currentUser"] != null)
            {
                var currentUser = (User)HttpContext.Session["currentUser"];

                if (currentUser.UserRole == UserRole.Admin)
                {
                    ViewBag.IsAdmin = "true";
                }
                else
                {
                    ViewBag.IsAdmin = "false";
                }

                ViewBag.UserID = currentUser.ID;
                supervisorVM.TableData = _supervisorTableData.GetTableData(currentUser.ID);
                return View(supervisorVM);
            }
            else
            {
                return View(supervisorVM);
            }


        }

        [HttpPost]
        public ActionResult Index(string submit, string vin, string stockNumber)
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            var supervisorVM = new SupervisorLandingVM();
            vin = vin.Trim();
            stockNumber = stockNumber.Trim();

            if (HttpContext.Session["currentUser"] != null)
            {
                var currentUser = (User)HttpContext.Session["currentUser"];

                if (currentUser.UserRole == UserRole.Admin)
                {
                    ViewBag.IsAdmin = "true";
                }
                else
                {
                    ViewBag.IsAdmin = "false";
                }

            }
            else
            {
                ViewBag.IsAdmin = "false";
            }

            ViewBag.UserID = currentUser.ID;


            if (String.IsNullOrEmpty(vin) && String.IsNullOrEmpty(stockNumber))
            {
                return RedirectToAction("Index");
            }

            var values = ",,,,," + stockNumber + "," + vin + ",";
            List<ChecklistStatus> status = new List<ChecklistStatus>()
            {
                ChecklistStatus.Pending,
                ChecklistStatus.TechComplete,
                ChecklistStatus.Handyman,
                ChecklistStatus.Wholesale,
                ChecklistStatus.Reopen,
                ChecklistStatus.RepairForRetail,
                ChecklistStatus.FitzWayValue,
                ChecklistStatus.FitzWayPremium,
                ChecklistStatus.FitzWaySelect,
                ChecklistStatus.FitzWayHPE

            };

            supervisorVM.TableData = _supervisorTableData.GetTableData(currentUser.ID);
            supervisorVM.Checklists = _checklistRecord.SearchWider(values, SearchType.And, -1, status);

            ViewBag.SearchType = "Search Results:";


            return View(supervisorVM);
        }
        public ActionResult OpenItems(string locationCode, string inspectionType)
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            var currentUser = (User)HttpContext.Session["currentUser"];
            var locations = new List<string>();
            if (!String.IsNullOrEmpty(locationCode))
            {
                if (locationCode.ToUpper() == "ALL")
                {
                    locations = _user.GetUserLocationCodes(currentUser.ID);
                }
                else
                {
                    locations.Add(locationCode);
                }
            }
            else
            {
                //show error message
            }

            var statusList = new List<ChecklistStatus>();
            if (!String.IsNullOrEmpty(inspectionType))
            {
                if (inspectionType.ToUpper() == "S")
                {
                    statusList.Add(ChecklistStatus.TechComplete);
                }
                else if (inspectionType.ToUpper() == "T")
                {
                    statusList.Add(ChecklistStatus.Pending);
                    statusList.Add(ChecklistStatus.Reopen);
                }
                else
                {
                    //show error message
                }
            }
            HttpContext.Session["LocationCode"] = locationCode;
            HttpContext.Session["inspectionType"] = inspectionType;
            var checklists = _checklistRecord.GetOutsandingRecords(locations, statusList);
            return View(checklists);
        }
        public ActionResult Index_Old()
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;


            ViewBag.SearchType = "Inspections for Your Location(s):";

            List<ChecklistStatus> status = new List<ChecklistStatus>()
            {
                ChecklistStatus.TechComplete,
                ChecklistStatus.Reopen,
                ChecklistStatus.RepairForRetail,
                ChecklistStatus.FitzWayPremium,
                ChecklistStatus.FitzWaySelect,
                ChecklistStatus.FitzWayHPE,
                ChecklistStatus.FitzWayValue
            };


            var checklistRecords = _checklistRecord.GetRecordsByLocation(currentUser.Locations, status);
            return View(checklistRecords);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index_Old(string submit, string vin, string stockNumber)
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;


            if (String.IsNullOrEmpty(vin) && String.IsNullOrEmpty(stockNumber))
            {
                return RedirectToAction("Index");
            }

            var values = ",,,,," + stockNumber + "," + vin + ",";
            List<ChecklistStatus> status = new List<ChecklistStatus>()
            {
                ChecklistStatus.Pending,
                ChecklistStatus.TechComplete,
                ChecklistStatus.Reopen,
                ChecklistStatus.RepairForRetail,
                ChecklistStatus.FitzWayPremium,
                ChecklistStatus.FitzWaySelect,
                ChecklistStatus.FitzWayHPE,
                ChecklistStatus.FitzWayValue
            };

            var checklistRecords = _checklistRecord.Search(values, SearchType.And, -1, status);


            ViewBag.SearchType = "Search Results - Matching Inspections:";


            return View(checklistRecords);
        }

        public ActionResult ViewInspection(string id)
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;

            ViewBag.UserType = "Supervisor";
            var supervisorViewVM = new SupervisorViewDisplayVM();
            string thisFuel = "";

            supervisorViewVM.checklistInfo = _checklistVM.GetChecklistVMByChecklistRecordID(Int32.Parse(id));
            supervisorViewVM.Users = _user.GetUsersByLocation(currentUser.Locations).Where(u => u.UserRole == UserRole.Technician).ToList();

            if (supervisorViewVM.checklistInfo != null && !String.IsNullOrEmpty(supervisorViewVM.checklistInfo.MetaDataValue7))
            {
                var filename = ConfigurationManager.AppSettings["PdfFilenameRoot"] + supervisorViewVM.checklistInfo.MetaDataValue7 + ".pdf";
                int intID = Int32.Parse(id);
                UsedVehicle usedVehicle = _usedVehicle.GetVehicleByID(1);
                thisFuel = usedVehicle.GetFuel(supervisorViewVM.checklistInfo.MetaDataValue7);
                var path = Server.MapPath(ConfigurationManager.AppSettings["PdfLocation"]);
                //path = "J:\\inetpub\\wwwroot\\production\\FITZWAY\\pictures\\UCPDFS\\";

                if (PdfHelper.DoesPdfExist(filename, path))
                {
                    supervisorViewVM.checklistInfo.PdfFileName = filename;
                }
            }
            else
            {

                ChecklistVM newChecklistVM = new Models.ChecklistVM();
                if (!String.IsNullOrEmpty(id))
                {
                    int intID = Int32.Parse(id);

                    if (("0" == RecordType.ChecklistRecord.ToString() || "0" == RecordType.Submitted.ToString()) && (id != "0"))
                    {
                        newChecklistVM = _checklistVM.GetChecklistVMByChecklistRecordID(intID);
                    }
                    else
                    {

                        UsedVehicle usedVehicle = _usedVehicle.GetVehicleByID(intID);
                        thisFuel = usedVehicle.GetFuel(usedVehicle.Vin);
                        newChecklistVM = _checklistVM.GetEmptyChecklistVMByChecklistID(2, thisFuel);
                        newChecklistVM.ID = 2;
                        newChecklistVM.MetaDataValue1 = usedVehicle.DealerName;
                        newChecklistVM.MetaDataValue2 = usedVehicle.Miles.ToString();
                        newChecklistVM.MetaDataValue3 = usedVehicle.Yr;
                        newChecklistVM.MetaDataValue4 = usedVehicle.Make;
                        newChecklistVM.MetaDataValue5 = usedVehicle.Carline;
                        newChecklistVM.MetaDataValue6 = usedVehicle.Stk;
                        newChecklistVM.MetaDataValue7 = usedVehicle.Vin;
                        newChecklistVM.MetaDataValue8 = usedVehicle.PermissionCode;


                        ModelState.Clear();
                        ModelState.Remove("sections");
                        supervisorViewVM.checklistInfo = newChecklistVM;  // move new checklist into supervisor view

                    }
                }

            }
            if (thisFuel == "('MISSING')" | thisFuel == "")
            {

                //return RedirectToAction("SelectFuel");

            }
            return View(supervisorViewVM);
        }

        [HttpPost]
        public ActionResult ViewInspection(string recordID, string status, string technician)
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden); ;

            if (String.IsNullOrEmpty(status))
            {
                if (HttpContext.Session["LocationCode"] != null && HttpContext.Session["InspectionType"] != null)
                {
                    return RedirectToAction("OpenItems", "Supervisor", new { @locationCode = HttpContext.Session["LocationCode"], @inspectionType = HttpContext.Session["InspectionType"] });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            var newChecklistRecord = _checklistRecord.GetChecklistRecordByID(Int32.Parse(recordID));
            newChecklistRecord.Status = (ChecklistStatus)Enum.Parse(typeof(ChecklistStatus), status);


            newChecklistRecord.UserID = (Int32)currentUser.ID;
            newChecklistRecord.Save();

            //For Re-open, assign the inspection to the selected technician
            if (newChecklistRecord.Status == ChecklistStatus.Reopen)
            {
                newChecklistRecord.UserID = Int32.Parse(technician);
                newChecklistRecord.Status = ChecklistStatus.Pending;
                newChecklistRecord.Save();

            }
            else if (newChecklistRecord.Status == ChecklistStatus.Complete || newChecklistRecord.Status == ChecklistStatus.Handyman ||
                newChecklistRecord.Status == ChecklistStatus.Wholesale || newChecklistRecord.Status == ChecklistStatus.RepairForRetail
           )
            {
                //For Repair for Retail, reasign to last technician to work on the inspection
                if (newChecklistRecord.Status == ChecklistStatus.RepairForRetail)
                {
                    newChecklistRecord.UserID = _checklistHistory.GetMostRecentTechnician(newChecklistRecord.ID);
                    newChecklistRecord.Status = ChecklistStatus.Pending;
                    newChecklistRecord.Save();

                }
                var filename = _pdfRenderer.CreatePdf(newChecklistRecord.ID);
                if (!String.IsNullOrEmpty(filename))
                {

                    var file = System.IO.Path.Combine(Server.MapPath(ConfigurationManager.AppSettings["PdfLocation"]),
                                        filename);
                    if (HttpContext.Session["LocationCode"] != null && HttpContext.Session["InspectionType"] != null)
                    {
                        return Json(new
                        {
                            pdfUrl = Url.Action("DisplayPdf", "Supervisor", new { @filename = filename }),
                            newUrl = Url.Action("OpenItems", "Supervisor", new { @locationCode = HttpContext.Session["LocationCode"], @inspectionType = HttpContext.Session["InspectionType"] })
                        });

                    }
                    else
                    {
                        return Json(new
                        {
                            pdfUrl = Url.Action("DisplayPdf", "Supervisor", new { @filename = filename }),
                            newUrl = Url.Action("Index", "Supervisor")
                        });
                    }
                }
            }

            if (HttpContext.Session["LocationCode"] != null && HttpContext.Session["InspectionType"] != null)
            {
                return Json(new
                {
                    newUrl = Url.Action("OpenItems", "Supervisor", new { @locationCode = HttpContext.Session["LocationCode"], @inspectionType = HttpContext.Session["InspectionType"] })
                });

            }
            else
            {
                return Json(new
                {
                    newUrl = Url.Action("Index", "Supervisor")
                });
            }
        }

        public ActionResult DisplayPDF(string filename)
        {
            if (!IsAuthorized())
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            try
            {

                var path = Server.MapPath(ConfigurationManager.AppSettings["PdfLocation"]);
                //path = "J:\\inetpub\\wwwroot\\production\\FITZWAY\\pictures\\UCPDFS\\";


                var file = System.IO.Path.Combine(path,
                        filename);
                //var file = System.IO.Path.Combine(ConfigurationManager.AppSettings["PdfLocation"],
                //        filename);
                byte[] FileBytes = System.IO.File.ReadAllBytes(file);
                Response.AppendHeader("Content-Disposition", "inline; filename= " + filename);
                return File(FileBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

        }

        private bool IsAuthorized()
        {
            //if(Request.Cookies["user"] == null)
            //    return false;

            if (HttpContext.Session["currentUser"] == null)
                return false;

            currentUser = (User)HttpContext.Session["currentUser"];

            return true;

        }

    }
}