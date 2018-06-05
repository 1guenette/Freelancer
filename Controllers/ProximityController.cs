using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;
using MG_5_FreelanceJobsite.HelperClasses;


namespace MG_5_FreelanceJobsite.Controllers
{
    public class ProximityController : Controller
    {

        private dbEntities db = new dbEntities();

        // GET: Proximity
        public ActionResult Index(int userId)
        {
            return View();
        }

        [HttpGet]
        public ActionResult ProximityFilter(int id)
        {
            //Get all users coordinates from the database 
            ProcessAddress procCoords = new ProcessAddress();
            
            List<User> Users = db.Users.ToList<User>();

            //Preserves each users coordinates so as to calculate each distance from one user to another
            Dictionary<User, Dictionary<decimal, decimal>> toAddress = new Dictionary<User, Dictionary<decimal, decimal>>();
            Dictionary<decimal, decimal> fromUserAdd = procCoords.latlong(db.Users.Where(o => o.Id == id).Single().Address);
            
            foreach(User user in Users)
            {
                toAddress.Add(user,procCoords.latlong(user.Address));
            }

            Dictionary<User, double> fromToUserDistance = new Dictionary<User, double>();
            List<ProximityFilterModel> proxyModelFilterIteratorFrontEnd = new List<ProximityFilterModel>();
            int countTest = toAddress.Count;

            for (int i = 0; i < toAddress.Count; i++)
            {
                double toUserLat = (double)toAddress.ElementAt(i).Value.ElementAt(0).Key;
                double toUserLon = (double)toAddress.ElementAt(i).Value.ElementAt(0).Value;

                double fromUserLat = (double)fromUserAdd.ElementAt(0).Key;
                double fromUserLon = (double)fromUserAdd.ElementAt(0).Value;

                fromToUserDistance.Add(toAddress.ElementAt(i).Key,procCoords.calcualteDistanceCord(fromUserLat,fromUserLon,toUserLat,toUserLon,'K'));
                proxyModelFilterIteratorFrontEnd.Add(new ProximityFilterModel
                {
                    fromUser = toAddress.ElementAt(i).Key,
                    distance = procCoords.calcualteDistanceCord(fromUserLat, fromUserLon, toUserLat, toUserLon, 'K')
                });
            }

            return View("Proximity", proxyModelFilterIteratorFrontEnd);
        }


    }
}