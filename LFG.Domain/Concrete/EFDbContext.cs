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

    public class LFGDbInitializer : DropCreateDatabaseIfModelChanges<EFDbContext>
    {
        protected override void Seed(EFDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }
        public void PerformInitialSetup(EFDbContext context)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));

            string roleName = "Administrators";
            string userName = "Admin";
            string password = "12211221";
            string email = "mike.gusev.val@gmail.com";

            if (!roleMgr.RoleExists(roleName))
            {
                roleMgr.Create(new AppRole(roleName));
            }

            AppUser user = userMgr.FindByName(userName);
            if (user == null)
            {
                userMgr.Create(new AppUser { UserName = userName, Email = email },
                    password);
                user = userMgr.FindByName(userName);
            }

            if (!userMgr.IsInRole(user.Id, roleName))
            {
                userMgr.AddToRole(user.Id, roleName);
            }



            // настройки конфигурации контекста указываются здесь
            ActivityType type1 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Outdoor" };
            ActivityType type2 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Indoor" };
            ActivityType type3 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Спорт", ActivityTypeParent = type1};
            ActivityType type4 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Прогулки", ActivityTypeParent = type1};
            ActivityType type5 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Посиделки", ActivityTypeParent = type1};
            ActivityType type6 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Учеба", ActivityTypeParent = type2 };
            ActivityType type7 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Хобби и рукоделие", ActivityTypeParent = type2 };
            ActivityType type8 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Игры", ActivityTypeParent = type1 };
            ActivityType type9 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Игры", ActivityTypeParent = type2 };
            ActivityType type10 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Велосипед", ActivityTypeParent = type4 };
            ActivityType type11 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "DnD", ActivityTypeParent = type9 };
            ActivityType type12 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Вязание", ActivityTypeParent = type7 };

            context.ActivityTypes.Add(type1);
            context.ActivityTypes.Add(type2);
            context.ActivityTypes.Add(type3);
            context.ActivityTypes.Add(type4);
            context.ActivityTypes.Add(type5);
            context.ActivityTypes.Add(type6);
            context.ActivityTypes.Add(type7);
            context.ActivityTypes.Add(type8);
            context.ActivityTypes.Add(type9);
            context.ActivityTypes.Add(type10);
            context.ActivityTypes.Add(type11);
            context.ActivityTypes.Add(type12);
            context.SaveChanges();

            Activity activity1 = new Activity()
            {
                ActivityId = Guid.NewGuid(),
                ActivityName = "Велозабег",
                ActivityDescription = "Велозабег мечты Public",
                ActivityPostDayTime = DateTime.Now,
                ActivityStartDayTime = DateTime.Now.AddDays(20),
                ActivityEndDayTime = DateTime.Now.AddDays(20).AddHours(1),
                ActivityAccess = ActivityAccessTypes.Public,
                ActivityTypeCurrent = type10
            };
            Activity activity2 = new Activity()
            {
                ActivityId = Guid.NewGuid(),
                ActivityName = "Велозабег",
                ActivityDescription = "Велозабег мечты Protected",
                ActivityPostDayTime = DateTime.Now,
                ActivityStartDayTime = DateTime.Now.AddDays(20),
                ActivityEndDayTime = DateTime.Now.AddDays(20).AddHours(1),
                ActivityAccess = ActivityAccessTypes.Protected,
                ActivityTypeCurrent = type10
            };
            Activity activity3 = new Activity()
            {
                ActivityId = Guid.NewGuid(),
                ActivityName = "Велозабег",
                ActivityDescription = "Велозабег мечты Private",
                ActivityPostDayTime = DateTime.Now,
                ActivityStartDayTime = DateTime.Now.AddDays(20),
                ActivityEndDayTime = DateTime.Now.AddDays(20).AddHours(1),
                ActivityAccess = ActivityAccessTypes.Private,
                ActivityTypeCurrent = type10
            };
            Activity activity4 = new Activity()
            {
                ActivityId = Guid.NewGuid(),
                ActivityName = "DND 5.0",
                ActivityDescription = "Dice'n'Roll в классическом сеттинге",
                ActivityPostDayTime = DateTime.Now,
                ActivityStartDayTime = DateTime.Now.AddDays(21),
                ActivityEndDayTime = DateTime.Now.AddDays(21).AddHours(12),
                ActivityAccess = ActivityAccessTypes.Public,
                ActivityTypeCurrent = type11
            };
            Activity activity5 = new Activity()
            {
                ActivityId = Guid.NewGuid(),
                ActivityName = "Вязалка",
                ActivityDescription = "Пыщ пыщ ололо, Оля водит НЛО",
                ActivityPostDayTime = DateTime.Now,
                ActivityStartDayTime = DateTime.Now.AddDays(22),
                ActivityEndDayTime = DateTime.Now.AddDays(22).AddHours(3),
                ActivityAccess = ActivityAccessTypes.Public,
                ActivityTypeCurrent = type12
            };

            context.Activities.AddRange(new List<Activity> { activity1 , activity2, activity3, activity4, activity5 });
            context.SaveChanges();
        }
    }
}