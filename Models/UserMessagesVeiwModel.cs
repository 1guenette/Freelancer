using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MG_5_FreelanceJobsite.Models
{
    public class UserMessagesVeiwModel
    {
        public List<Messaging> Inbox { get; set; }
        public List<Messaging> Sent { get; set; }
        public Messaging Msg { get; set; } 
        public string Jobtitle { get; set; }
        public string Employeremail { get; set; }
        public string ErrorLoadingPage { get; set; }
        public int? Employerid { get; set; }
        public int Autofill { get; set; }
    }
}