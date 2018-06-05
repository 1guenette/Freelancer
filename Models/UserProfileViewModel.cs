using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MG_5_FreelanceJobsite.Models;

namespace MG_5_FreelanceJobsite.ViewModels
{
    public class UserProfileViewModel
    {
        public User UserProfile {get; set;}
        public List<ProfileSkillViewModel> UserSkills { get;set;}
        public List<UserJob> JobPosts { get; set; }
        public List<UserJob> UserBids { get; set; }
    }
}