using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MG_5_FreelanceJobsite.Models
{
    public class SingleMessageDisplayViewModel
    { 
        public Messaging Msg { get; set; }
        public string SenderName { get; set; }
        public string ReceipientName { get; set; }
    }
}