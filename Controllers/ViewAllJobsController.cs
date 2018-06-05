using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;

namespace MG_5_FreelanceJobsite.Controllers
{
    public class ViewAllJobsController : Controller
    {
        // GET: ViewAllJobs
        
        [HttpGet]
        public ActionResult Index(int userID)
        {
            return View(new HttpRequestValidationException("You cant be here homie"));
        }

        [HttpGet]
        public ActionResult ViewAllJobs(int? id)
        {
            if(id == null)
                return View("~/Views/Shared/Error.cshtml");

            using (dbEntities db = new dbEntities())
            {
                List<Job> ListOfJobsByUser = db.Users.Where(o => o.Id == id).Single().Jobs.ToList<Job>();

                List<ViewAllModel> allUserIDmatches = new List<ViewAllModel>();

                foreach (var job in ListOfJobsByUser)
                {
                    String partialDescription = job.JobDec.Substring(0, 4);
                    allUserIDmatches.Add(new ViewAllModel()
                    {
                        descriptions = job.JobDec,
                        locations = job.Location,
                        Prices = job.StartPrice,
                        JobTitles = job.Title,
                        partialDesc = partialDescription,
                        id = job.Id,
                        bidEndTime = job.BidEndDate
                    });
                }
                return View("ViewAll", allUserIDmatches);
            }
        }
        [HttpGet]
        public ActionResult ViewAllCurrentJobs()
        {
            using (dbEntities db = new dbEntities())
            {
                List<Job> ListOfJobsByUser = db.Jobs.ToList();


                List<ViewAllModel> allUserIDmatches = new List<ViewAllModel>();

                foreach (var job in ListOfJobsByUser)
                {
                    String partialDescription = job.JobDec.Substring(0, 2);
                    allUserIDmatches.Add(new ViewAllModel()
                    {
                        descriptions = job.JobDec,
                        locations = job.Location,
                        Prices = job.StartPrice,
                        JobTitles = job.Title,
                        partialDesc = partialDescription,
                        id = job.Id,
                        bidEndTime = job.BidEndDate
                    });
                }
                
                return View("ViewAll", allUserIDmatches);
            }

        }


    }
}