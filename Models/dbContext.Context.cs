﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MG_5_FreelanceJobsite.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbEntities : DbContext
    {
        public dbEntities()
            : base("name=dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bid> Bids { get; set; }
        public virtual DbSet<JobRequirement> JobRequirements { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Messaging> Messagings { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<SkillsLog> SkillsLogs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSkill> UserSkills { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
    }
}
