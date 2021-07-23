using Ninject.Web.Common.WebHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ninject;
using FitzCheckout.BizObjects;
using FitzCheckout.Models;
using FitzCheckout.DAL;
using FitzCheckout.PDF;

namespace FitzCheckout
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected void Session_Start()
        {
            HttpContext.Current.Session.Add("currentUser", new BizObjects.User());
        }
        protected new void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected override IKernel CreateKernel()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var kernel = new StandardKernel();
            RegisterServices(kernel);
            //RegisterModelBinders(kernel);
            //kernel.Load(Assembly.GetExecutingAssembly());
            //Kernal = kernel;
            return kernel;
        }

        public static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IChecklist>().To<Checklist>();
            kernel.Bind<IChecklistSection>().To<ChecklistSection>();
            kernel.Bind<IChecklistItem>().To<ChecklistItem>();
            kernel.Bind<IChecklistRecord>().To<ChecklistRecord>();
            kernel.Bind<IChecklistItemRecord>().To<ChecklistItemRecord>();
            kernel.Bind<IChecklistVM>().To<ChecklistVM>();
            kernel.Bind<IChecklistItemVM>().To<ChecklistItemVM>();
            kernel.Bind<IChecklistSectionVM>().To<ChecklistSectionVM>();
            kernel.Bind<IChecklistItemRecordVM>().To<ChecklistItemRecordVM>();
            kernel.Bind<IInspectionFormVM>().To<InspectionFormVM>();
            kernel.Bind<IUsedVehicle>().To<UsedVehicle>();
            kernel.Bind<IVehicleSearch>().To<VehicleSearch>();
            kernel.Bind<IChecklistHistory>().To<ChecklistHistory>();
            kernel.Bind<IUserDashboard>().To<UserDashboard>();
            kernel.Bind<ISupervisorViewDisplayVM>().To<SupervisorViewDisplayVM>();
            kernel.Bind<IUser>().To<User>();
            kernel.Bind<IAssociate>().To<Associate>();
            kernel.Bind<IPdfRenderer>().To<PdfRenderer>();
            kernel.Bind<ISupervisorTableData>().To<SupervisorTableData>();
        }
    }
}
