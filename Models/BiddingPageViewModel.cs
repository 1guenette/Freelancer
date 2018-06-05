using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MG_5_FreelanceJobsite.ViewModel
{
    public partial class BiddingPageViewModel 
    {
        public int BidId { get; set; }
        public double StartingPrice { get; set; }
        public double LowestPrice { get; set; }
        public int UserId { get; set; } 
        public int JobId { get; set; }
        public string Title { get; set; }
        public int EmployerID { get; set; } 
        public string JobDesc { get; set; }
        public sbyte CanBid { get; set; } 
    }
}