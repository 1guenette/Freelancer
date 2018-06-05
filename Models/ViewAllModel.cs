using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MG_5_FreelanceJobsite.Models
{
    public class ViewAllModel
    {

        public String descriptions { get; set; }
        public String locations { get; set; }
        public double Prices { get; set; }
        public String JobTitles { get; set; }
        public String partialDesc { get; set; }
        public DateTime bidEndTime { get; set; }
        public int id { get; set; }


    }
}