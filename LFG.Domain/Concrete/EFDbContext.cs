using LFG.Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using LFG.Domain.Infrastructure;
using Microsoft.AspNet.Identity;

namespace LFG.Domain.Concrete
{
    public class EFDbContext : IdentityDbContext<AppUser>
    {
        public EFDbContext() : base("name=EFDbContext") { }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }

        public static EFDbContext Create()
        {
            return new EFDbContext();
        }
        static EFDbContext()
        {
            Database.SetInitializer<EFDbContext>(new LFGDbInitializer());
        }
    }

    public class LFGDbInitializer : NullDatabaseInitializer<EFDbContext>
    {
        
    }
}