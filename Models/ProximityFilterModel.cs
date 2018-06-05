using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MG_5_FreelanceJobsite.Models
{
    public class ProximityFilterModel
    {
        public User fromUser { get; set; }
        public double distance { get; set; }
    }
}