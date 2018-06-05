using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;
namespace MG_5_FreelanceJobsite.Controllers
{
    public class DisplayJobController : Controller
    {
        // GET: DisplayJob
        public ActionResult Index()
        {
            return View();
        }

        //[Route()] //incase need routing
        [HttpGet]
        public ActionResult Display(int id)
        {
            
            dbEntities db = new dbEntities();
            JobSkillsModel jobSkills = new JobSkillsModel();
            jobSkills.myJob = db.Jobs.Where(o => o.Id == id).Single();
            List<JobRequirement> jobReqs = db.JobRequirements.Where(o => o.JobId == jobSkills.myJob.Id).ToList();
            jobSkills.mySkillNames = new List<String>();
            foreach(var req in jobReqs)
            {
                jobSkills.mySkillNames.Add(db.SkillsLogs.Where(o => o.Id == req.SkillId).Single().Name);
            }
            return View("~/Views/Jobs/JobDisplay.cshtml", jobSkills);
            
        }

       
       
    }
}