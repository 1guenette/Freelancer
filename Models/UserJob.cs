using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MG_5_FreelanceJobsite.Models
{
    public class UserJob
    {
        public Job Job { get; set; }
        public List <Bid> Bids { get; set; }
    }
}