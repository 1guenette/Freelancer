using System;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;

namespace MG_5_FreelanceJobsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;


            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            DefaultHomePageController dfh = new DefaultHomePageController();

            //return View();
            return View("~/Views/DefaultHomePage/DefaultPageView.cshtml",dfh.ListJobsRandom());
        }

        public ActionResult Login(int id)
        {

            if (id == 0)
                return View("~/Views/Login/Create.cshtml");
            else
                return View("~/Views/Login/Login.cshtml");



        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session.RemoveAll();

            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult About()
        {

            return View("~/Views/Home/About.cshtml");
        }




    }



}
