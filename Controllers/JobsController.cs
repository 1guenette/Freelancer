using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;

namespace MG_5_FreelanceJobsite.Controllers
{
    public class JobsController : Controller
    {
        // GET: Jobs
        public ActionResult Index() 
        {

            return View();
        }
        [HttpGet]
        public ActionResult redirectToJobAddView()
        {
            using (dbEntities db = new dbEntities())
            {
                List<String> skillList = new List<String>();
                foreach(var skill in db.SkillsLogs)
                {
                    skillList.Add(skill.Name);
                }

                ViewBag.skills = skillList;
            }

            return View("~/Views/Jobs/JobView.cshtml");
        }

        [Route("JobsController/putJobInfo")]
        [HttpPost]
        public ActionResult putJobInfo(JobSkillsModel model)
        {
            if (ModelState.IsValid)
            {

                using (dbEntities db = new dbEntities())
                {
                    model.myJob.EmployerId = (int)Session["loginid"];
                    User passIn = db.Users.Where(o => o.Id == model.myJob.EmployerId).Single();

                    var num2 = db.Jobs.LongCount();
                    db.Jobs.Add(new Job
                    {
                        Title = model.myJob.Title,
                        EmployerId = (int)Session["loginid"],
                        JobDec = model.myJob.JobDec,
                        Location = model.myJob.Location,
                        StartPrice = model.myJob.StartPrice,
                        CanBid = model.myJob.CanBid,
                        User = passIn,
                        Status = "Public",
                        StartDate = model.myJob.StartDate.ToLocalTime(),
                        BidEndDate = model.myJob.BidEndDate.ToLocalTime()
                        
                    });

                    db.SaveChanges();


                    int jobCount = (int)db.Jobs.ToList().LongCount()+2;
                    foreach(var skill in model.mySkillNames)
                    {

                        db.Jobs.Where(o => o.Id == jobCount).Single().JobRequirements.Add(new JobRequirement
                        {
                            JobId = jobCount,
                            SkillId = db.SkillsLogs.Where(o => o.Name.Equals(skill)).Single().Id,
                            Id = db.JobRequirements.Count() + 1
                        });

                    }
                    db.SaveChanges();




                    db.SaveChanges();
                    return RedirectToAction("Display", "DisplayJob", new { id = jobCount});
                   

                    

                }
            }            
            return View(new HttpException("Model Invalid"));
        }

        /*
        public ActionResult addSkill(JobSkillsModel model)
        {
            using (dbEntities db = new dbEntities())
            {
                Job job = db.Jobs.Where(o => o.Id == model.myJob.Id).Single();
                var skill = db.SkillsLogs.Where(o => o.Name == model.skillName).Single();

                var jobReq = new JobRequirement
                {
                    SkillId = db.SkillsLogs.Where(o => o.Name == model.skillName).Single().Id,
                    JobId = job.Id
                };
                db.JobRequirements.Add(jobReq);
                db.SaveChanges();
                

                return View("~/Views/Jobs/JobDisplay.cshtml", job);

            }
        }
        */
       
    }
}