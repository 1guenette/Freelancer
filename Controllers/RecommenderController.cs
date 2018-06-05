using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;

namespace MG_5_FreelanceJobsite.Controllers
{
    public class RecommenderController : Controller
    {
        // GET: Recommender
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SkillMatchFinder(int id)
        {
            dbEntities db = new dbEntities();

            List<int> jobReqID = new List<int>();

            db.JobRequirements.Where(o => o.JobId == id)
                   .ToList()
                   .ForEach(delegate(JobRequirement reqs)
                   {
                       jobReqID.Add(reqs.SkillId);
                   });

            List<RecommendationResultModel> requestedLog = getSkillMatch(jobReqID);
            
            return View("SkillMatcher", requestedLog);
        }


        private List<RecommendationResultModel> getSkillMatch(List<int> skillsID)
        {

            List<int> shallowCopy = skillsID;
            using(dbEntities db = new dbEntities())
            {
                List<User> UserIDs = db.Users.ToList();
                List<RecommendationResultModel> ranking = new List<RecommendationResultModel>();

                foreach (var user in UserIDs)
                {
                    List<UserSkill> currentUserSkill = db.UserSkills.Where(o => o.UserId == user.Id).ToList();// user.WorkerSkills.ToList<WorkerSkill>();
                    int numberOfRequestedSkillMatches = 0;
                    foreach(var userSkill in currentUserSkill)
                    {
                        //Iterate through the ID's of requested skills
                        foreach(var requestedSkillID in skillsID)
                        {
                            if(requestedSkillID == userSkill.SkillId)
                            {
                                numberOfRequestedSkillMatches = numberOfRequestedSkillMatches + 1;
                            }
                        }
                    }
                    //Add the # of skills matches to a dictionary to be iterated over
                    ranking.Add(new RecommendationResultModel
                    {
                        SkillMatches = numberOfRequestedSkillMatches,
                        UserSkillMatch = user
                    });
                    //Dictionary<object, User> userData = new Dictionary<object, User>();
                    //foreach(User data in db.Users.Where(o => o.Jobs.Count > 0).ToList())
                    //{
                    //    userData.Add(data.IsAdmin, data);
                    //}

                }
                return ranking;
            }
        }



    }
}