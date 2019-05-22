using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    public class homeController : Controller
    {
        // GET: home
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Display(string ip,int port)
        {
            Singelton.Instance.openServer(ip,port);
                     
            ViewBag.lon = Singelton.Instance.Lon;
            ViewBag.lat = Singelton.Instance.Lat;
            return View();
        }
    }
}