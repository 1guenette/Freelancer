using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;
namespace MG_5_FreelanceJobsite.Controllers
{
    public class PaymentPageController : Controller
    {
        // GET: PaymentPage
        [HttpGet]
        [Route("PaymentPage/GetPaymentPage/{id}")]
        public ActionResult GetPaymentPage(int id)
        {
            dbEntities db = new dbEntities();
            PaymentPageModel payment = new PaymentPageModel{Job = db.Jobs.Where(o => o.Id == id).Single()};
            //PaymentPageModel payment = new PaymentPageModel { Review = "fine" };
            return View("~/Views/PaymentPage/PaymentPageView.cshtml", payment);
        }

        [HttpPost]
        [Route("PaymentPage/MakePayment/{id}/{rating}/{review}")]
        public ActionResult MakePayment(int id, int rating, string review)
        {
            dbEntities db = new dbEntities();
            var job = db.Jobs.Where(o => o.Id == id).Single();
            job.Status = "payed";
            Review r = new Review{Rating = rating, JobId= id, Review1 = review};
            PaymentPageModel payment = new PaymentPageModel { Job = job, Rating = rating, Review = review };
            db.Reviews.Add(r);
            db.SaveChanges();
            return View("~/Views/PaymentPage/PaymentConfirmation.cshtml", payment);
        }
    }
}