using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace First_MvC_Project.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index() //first page of website
        {
            ViewData["Info"] = "Welcome to the Page.";
            string[] cric = { "Virat", "Rohit", "Rahul", "Surya", "Hardik"};
            ViewBag.ArrayCrics = cric;
            TempData["IndCricketers"] = cric;
            Session["Mssg"] = "My Fav Indian Cricketers";
            return RedirectToAction("/Contact");

            //return View();//respond html page
        }
        public ActionResult Contact()
        {
            
            TempData.Keep();
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }
    }
}