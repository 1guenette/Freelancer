using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;
using MG_5_FreelanceJobsite.ViewModels;

namespace MG_5_FreelanceJobsite.Controllers
{
    public class UserBioController : Controller
    {
        [Route("UserBio/GetUsers")]                                     //DONE
        public ActionResult GetUsers()
        {
            using (dbEntities db = new dbEntities())
            {

                List<User> userList = db.Users.ToList();
                List<SkillsLog> skillList = db.SkillsLogs.ToList();

                var users = new GetUsersViewModel { Users = userList, Skills = skillList};

                return View(users);
            }
        }

        [Route("UserBio/GetUserById/{id}")]                             //Done
        public ActionResult GetUserById(int id)
        {
            dbEntities db = new dbEntities();

            //get UserProfile skills
            var user = db.Users.Where(o => o.Id == id).Single();
            var skillsID = db.UserSkills.Where(o => o.UserId == id).ToList();

            List<ProfileSkillViewModel> userSkills = new List<ProfileSkillViewModel>();
            foreach (var skillNum in skillsID)
            {
                var skill = db.SkillsLogs.Where(o => o.Id == skillNum.SkillId).Single();
                userSkills.Add(new ProfileSkillViewModel { SkillID = skillNum.SkillId, SkillName = skill.Name });
            }

            var posts = db.Jobs.Where(o => o.EmployerId == id).ToList();

            //Gets all jobPosts for UserProfile
            List<UserJob> jobPosts = new List<UserJob>();
           foreach (var job in posts)
           {
               List<Bid> bidders = db.Bids.Where(o => o.JobId == job.Id).ToList();
               UserJob nextJob = new UserJob {Job = job, Bids =bidders };
               jobPosts.Add(nextJob);
           }


           var bids = db.Bids.Where(o => o.UserId == id).ToList();
           List<UserJob> bidPosts = new List<UserJob>();
           foreach (var bid in bids)
           {
               Job job = db.Jobs.Where(o => o.Id == bid.JobId).Single();
               List<Bid> bidders = db.Bids.Where(o => o.JobId == job.Id).ToList();
               var nextJob = new UserJob { Job = job, Bids = bidders };
               bidPosts.Add(nextJob);
           }
            var result = new UserProfileViewModel { UserProfile = user,  UserSkills = userSkills, JobPosts = jobPosts, UserBids = bidPosts};
               return View(result);
            
        }

        [Route("UserBio/AddSkill/{skillName}")]
        [HttpPost]
        public ActionResult AddSkill(string skillName)      //Test
        {
            using (dbEntities db = new dbEntities())
            {
                var newSkill = db.SkillsLogs.Where(o => o.Name.Equals(skillName.ToLower())).Any(); //Check if skill doesn't exist
                if (!newSkill)//Adds skill to skill log
                {
                    db.SkillsLogs.Add(new SkillsLog { Name = skillName.ToLower() });
                    db.SaveChanges();
                }
                object test = Session["loginid"];

                int skillID = db.SkillsLogs.Where(o => o.Name.Equals(skillName.ToLower())).First().Id; //skill id
                int userID = (int)Session["loginid"];
                var hasSkill = db.UserSkills.Where(o => o.SkillId == skillID && o.UserId == userID).Any(); //Check user already has skill


                if (!hasSkill)//Add Skill
                {
                    int id = Convert.ToInt32(Session["loginid"]);
                    db.UserSkills.Add(new UserSkill { SkillId = skillID, UserId = id });
                    db.SaveChanges();
                }
            }

            return RedirectToAction("GetUserById", "UserBio",new { id = (int)Session["Loginid"] });
            //return View();
        }


        [Route("UserBio/GetUsersBySkill/{skill}")]                              //Test
        public ActionResult GetUsersBySkill(string skillFilter)
        {
            using (dbEntities db = new dbEntities())
            {

                    var skillID = db.SkillsLogs.Where(o => o.Name.Equals(skillFilter)).Single().Id; //Get SkillID
                    var userSkillIdList = db.UserSkills.Where(o => o.SkillId == skillID).ToList(); //Get UserIDs with skill 

                    List<SkillsLog> skillList = db.SkillsLogs.ToList();
                    List<User> userList = new List<User>();

                    foreach (var profile in userSkillIdList)
                    {
                        User x = db.Users.Where(o => o.Id == profile.UserId).Single();
                        userList.Add(x);
                    }

                    var users = new GetUsersViewModel { Users = userList, Skills = skillList };
                    return View("GetUsers", users);
                

            }
        }




    }
}