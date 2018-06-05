using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;


namespace MG_5_FreelanceJobsite.Controllers
{
    public class NotificationsController : Controller
    {
        // GET: Notifications
        public ActionResult Index()
        {
            return View();
        }

        [Route("Notifications/displayNotifications")]  
        public ActionResult displayNotifications()
        {
            NotificationModel notifications = new NotificationModel();
            notifications.unreadNotifications = new List<Notification>();
            notifications.readNotifications = new List<Notification>();

            using (dbEntities db = new dbEntities())
            {
                int currentUserID = (int)Session["loginid"];
                User currentUser = db.Users.Where(o => o.Id == currentUserID).Single();
                notifications.userID = currentUser.Id;
                List<Notification> allNotif = db.Notifications.Where(o => o.UserId == currentUser.Id).ToList();
                foreach(var notif in allNotif)
                {
                    //check if unread
                    if (notif.Read == 0)
                    {
                        notifications.unreadNotifications.Add(notif);
                    }
                    
                    //check if read
                    if (notif.Read == 1)
                    {
                        notifications.readNotifications.Add(notif);
                    }
                }
            }
                return View("~/Views/Notifications/NotificationsView.cshtml", notifications);
        }

        [Route("Notifications/markAsRead")]
        public ActionResult markAsRead()
        {
            NotificationModel notifications = new NotificationModel();
            notifications.unreadNotifications = new List<Notification>();
            notifications.readNotifications = new List<Notification>();

            using (dbEntities db = new dbEntities())
            {
                int currentUserID = (int)Session["loginid"];
                User currentUser = db.Users.Where(o => o.Id == currentUserID).Single();
                notifications.userID = currentUser.Id;
                List<Notification> allNotif = db.Notifications.Where(o => o.UserId == currentUser.Id).ToList();

                foreach(var notif in allNotif)
                {
                    if (notif.Read == 0)
                    {
                        db.Notifications.Where(o => o.Id == notif.Id).Single().Read = 1;
                        db.SaveChanges();
                        
                    }
                    
                }

                
            }

            return View("~/Views/Home/Index.cshtml");
        }
    }
}