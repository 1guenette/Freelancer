using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;

namespace MG_5_FreelanceJobsite.Controllers
{
    public class SelectEmployeeController : Controller
    {
        // GET: SelectEmployee
        public ActionResult BidderListPage(int jobID)
        {
            dbEntities db = new dbEntities();

            Job job = db.Jobs.Where(o => o.Id == jobID).Single();
            return View("~/Views/SelectEmployee/SelectEmployeeView.cshtml", job);
        }

        //[Route()] //incase need routing
        [Route("SelectEmployee/Bidder/{skillName}")]
        [HttpPost]
        public ActionResult SelectBidder(int bidderID, int jobID)
        {
            dbEntities db = new dbEntities();

            Job job = db.Jobs.Where(o => o.Id == jobID).Single();
            List<Bid> bids = db.Bids.Where(o => o.JobId == jobID && o.UserId == bidderID).ToList();

            job.EmployeeId = bidderID;
            job.PaymentPrice = bids.Min(o => o.Price);
            job.Status = "bidding closed";
            db.SaveChanges();

            return View("~/Views/SelectEmployee/ConfirmationPage.cshtml", job);

        }
    }
}