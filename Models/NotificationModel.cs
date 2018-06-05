using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MG_5_FreelanceJobsite.Models
{
    public class NotificationModel
    {
        public List<Notification> readNotifications { get; set; }
        public List<Notification> unreadNotifications { get; set; }
        public int userID { get; set; }
    }
}