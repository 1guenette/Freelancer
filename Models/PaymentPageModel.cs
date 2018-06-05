using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MG_5_FreelanceJobsite.Models;

namespace MG_5_FreelanceJobsite.Models
{
    public class PaymentPageModel
    {
        public Job Job { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }

    }
}