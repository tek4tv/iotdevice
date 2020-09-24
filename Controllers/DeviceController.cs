using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tek4TV.Devices.Controllers
{
    public class DeviceController : Controller
    {
        // GET: Deive
        public ActionResult Index()
        {
            if (Session["User"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

            
        }
    }
}