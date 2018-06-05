using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MG_5_FreelanceJobsite.Models;

namespace MG_5_FreelanceJobsite.ViewModels
{
    public class GetUsersViewModel
    {
        public List<User> Users { get; set; }
        public List<SkillsLog> Skills { get; set; }

    }
}