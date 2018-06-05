using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;
using MG_5_FreelanceJobsite.ViewModel;

namespace MG_5_FreelanceJobsite.Controllers
{
    public class BidController : Controller
    {
        [Route("BidController/CreateBid")]
        [HttpPost]  
        public ActionResult CreateBid([Bind(Include = "BidId, StartingPrice, LowestPrice, UserId, JobId, Title, EmployeerID, JobDesc, CanBid")]BiddingPageViewModel model) 
        {
            BiddingPageViewModel OldData = (BiddingPageViewModel)TempData["BiddingPageViewModel"]; //persisted data
            using (dbEntities db = new dbEntities()) 
            {
                if(ModelState.IsValid && OldData.CanBid==1) 
                {
                    int id2 = db.Bids.Count();
                    Bid bid = new Bid()
                    {
                        Id = id2 + 1,
                        Price = model.StartingPrice,
                        UserId = (int)Session["loginid"],     //UNCOMMENT THIS UPON MERGE AND DELETE ABOVE LINE
                        JobId = OldData.JobId 
                    };

                    db.Bids.Add(bid); 
                    db.SaveChanges();
                    return RedirectToAction("Display","DisplayJob",new { id = OldData.JobId }); //parameter will send to ajay view for specific job needs to come up with it
                }
                
                return View(); //redirect to partial view possibly for error handling 
            }
        }  
        
        public ActionResult GetBid(int id) 
        {
            //checks if thats an existing bid id
            using (dbEntities db = new dbEntities())
            {
                var bid = db.Bids.Where(o => o.Id == id).Single(); //correct approach?
                return View(bid);
            }
        }

        [HttpGet] //insert route as well?
        public ActionResult BidGetInfo(int id)       //sets up bid creation
        {
            using (dbEntities db = new dbEntities())
            {
                double CurrentLowestBid = db.Jobs.Where(o => o.Id == id).Single().StartPrice;
                List<Bid> temp = db.Jobs.Where(o => o.Id == id).Single().Bids.ToList();
                foreach(var b in temp)
                {
                    if(CurrentLowestBid > b.Price)
                    {
                        CurrentLowestBid = b.Price;
                    }
                }
                BiddingPageViewModel SetUp = new BiddingPageViewModel()
                {
                    BidId = -1, //temp value for now
                    StartingPrice = db.Jobs.Where(o => o.Id == id).Single().StartPrice,
                    LowestPrice = CurrentLowestBid,
                    UserId = (int)Session["loginid"], //UNCOMMENT THIS UPON MERGE AND DELETE ABOVE LINE
                    JobId = id,
                    Title = db.Jobs.Where(o => o.Id == id).Single().Title,
                    EmployerID = db.Jobs.Where(o => o.Id == id).Single().EmployerId,
                    JobDesc = db.Jobs.Where(o => o.Id == id).Single().JobDec,
                    CanBid = db.Jobs.Where(o => o.Id == id).Single().CanBid,
                };
                TempData["BiddingPageViewModel"] = SetUp;
                return View("~/Views/Bid/Bid.cshtml", SetUp); 
            }
        }

        [Route("BidController/EditBidPrice/{id}")]
        [HttpPost]
        public ActionResult EditBidPrice(int bidId, int? value)     //basic implementation for edit incase desire similar method later method
        {
            using (dbEntities db = new dbEntities())
            {
                Bid bid = db.Bids.Where(o => o.Id == bidId).Single(); 
                if(value.HasValue && value>0)
                    bid.Price = (int)value;

                db.Bids.Add(bid);
                db.SaveChanges();
            }
            return View();
        }
    }
}