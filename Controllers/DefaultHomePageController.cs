using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;

namespace MG_5_FreelanceJobsite.Controllers
{
    public class DefaultHomePageController : Controller
    {
        /*
         * Controller method to retreive 20 active jobs closest to the range of 1 hour time left in their remaining time 
         */
        [Route("DefaultHomePageController/ListJobByTime")]
        public TimeSpecificJobListing ListJobsByTime()
        {
                dbEntities db = new dbEntities();
                TimeSpecificJobListing list = new TimeSpecificJobListing
                {
                    ForPosting = new List<Job>()
                };
                List<Job> total = new List<Job>();
                int counter = 1, i=0;
                int TimeRangeLower = 1, TimeRangeUpper=2;
                total = db.Jobs.ToList();
                foreach(var j in total)
                {
                    if(j.Status.Equals("bidding open")) 
                    {
                        counter++;
                    }
                }
                if (counter < 21) //less than 20 jobs currently open
                {
                    counter = 1;
                    while(counter<total.Count())
                    {
                        if (db.Jobs.Any(o => o.Id == counter))
                        {
                            Job t = db.Jobs.Where(o => o.Id == counter).Single();
                            if (t.Status.Equals("bidding open"))
                            {
                                list.ForPosting.Add(t);
                            }
                        }
                        counter++;
                    }

                }
                else if(counter>0)  //more than 20 jobs open
                {
                    counter = 1;
                    while (i < 20)
                    {
                        if (db.Jobs.Any(o => o.Id == counter))
                        {
                            Job temp = db.Jobs.Where(o => o.Id == counter).Single();
                            if (temp.Status.Equals("bidding open"))
                            {
                                int timeleft = Convert.ToInt32((DateTime.Now - temp.BidEndDate).TotalHours);
                                if (timeleft < TimeRangeUpper && timeleft > TimeRangeLower)
                                {
                                    list.ForPosting.Add(temp);
                                    i++;
                                }
                            }
                            else
                            {
                                if (counter == db.Jobs.Count())
                                {
                                    counter = 1;
                                    TimeRangeUpper += 2;
                                    if (TimeRangeLower == 1)
                                    {
                                        TimeRangeLower -= 1;
                                    }
                                }
                            }
                        }

                    }
                }
                return list; //need default page view
            
        }

        /*
         * Controller method to retreive 20 random, active jobs or as many are in the db if less than 20 total active
         */
        [Route("DefaultHomePageController/ListJobsRandom")]
        public TimeSpecificJobListing ListJobsRandom()
        {
                dbEntities db = new dbEntities();
                TimeSpecificJobListing list = new TimeSpecificJobListing
                {
                    ForPosting = new List<Job>() 
                };
                Random rand = new Random();
                int counter = 0;
                int Id, i=0;
                //retreive 20 random jobs
                while(i<20)
                {
                    Id = rand.Next(1, db.Jobs.Count() + 1); //generate random job id 
                    if (db.Jobs.Any(o => o.Id == Id))
                    {
                        int Duplicate = 0;
                        Job temp = db.Jobs.Where(o => o.Id == Id).Single();  //get job at the random id
                        if (temp.Status.Equals("bidding open"))  //checks to see if the job is currently open/active
                        {
                            if (list.ForPosting.Count() > 0)  //checks to see if at least one job is currently in list.ForPosting
                            {
                                foreach (var q in list.ForPosting)
                                {
                                    if (q.Id == Id)  //checks if the job already in list.ForPosting
                                    {
                                        Duplicate = 1;
                                        break;
                                    }
                                }
                            }
                            if (Duplicate == 0)  //adds the job if not already in the list
                            {
                                list.ForPosting.Add(temp);
                                i++;
                            }
                        }
                        if (counter == db.Jobs.Count())  //
                        {
                            break;  //breaks when less than 20 jobs active currently in db
                        }
                        counter++;
                    }
                }
                return list;  //need to redirect to default html page view
            }
        
    }
}