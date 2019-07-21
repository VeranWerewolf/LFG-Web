namespace LFG.Domain.Migrations
{
    using LFG.Domain.Concrete;
    using LFG.Domain.Entities;
    using LFG.Domain.Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EFDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "LFG.Domain.Concrete.EFDbContext";
        }

        protected override void Seed(EFDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            PerformInitialSetup(context);
            base.Seed(context);
        }
        public void PerformInitialSetup(EFDbContext context)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));

            //Создание Ролей для пользователей
            string roleName1 = "Administrators";
            if (!roleMgr.RoleExists(roleName1))
            {
                roleMgr.Create(new AppRole(roleName1));
            }
            string roleName2 = "Moderators";
            if (!roleMgr.RoleExists(roleName2))
            {
                roleMgr.Create(new AppRole(roleName2));
            }
            string roleName3 = "Users";
            if (!roleMgr.RoleExists(roleName3))
            {
                roleMgr.Create(new AppRole(roleName3));
            }

            //Создание пользователей
            string userName = "Admin";
            string password = "12211221";
            string email = "mike.gusev.val@gmail.com";
            AppUser user = userMgr.FindByName(userName);
            if (user == null)
            {
                userMgr.Create(new AppUser { UserName = userName, Email = email },
                    password);
                user = userMgr.FindByName(userName);
            }
            if (!userMgr.IsInRole(user.Id, roleName1))
            {
                userMgr.AddToRole(user.Id, roleName1);
            }

            string userName1 = "MikeUser";
            string password1 = "11111111";
            string email1 = "cheaterlord@mail.ru";
            AppUser user1 = userMgr.FindByName(userName1);
            if (user1 == null)
            {
                userMgr.Create(new AppUser { UserName = userName1, Email = email1 },
                    password1);
                user1 = userMgr.FindByName(userName1);
            }
            if (!userMgr.IsInRole(user1.Id, roleName3))
            {
                userMgr.AddToRole(user1.Id, roleName3);
            }
            string userName2 = "MikeModer";
            string password2 = "11111111";
            string email2 = "admin@golfstream.spb.ru";
            AppUser user2 = userMgr.FindByName(userName2);
            if (user2 == null)
            {
                userMgr.Create(new AppUser { UserName = userName2, Email = email2 },
                    password2);
                user2 = userMgr.FindByName(userName2);
            }
            if (!userMgr.IsInRole(user2.Id, roleName2))
            {
                userMgr.AddToRole(user2.Id, roleName2);
            }


            // Создание категорий
            ActivityType type1 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Outdoor" };
            ActivityType type2 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Indoor" };
            ActivityType type3 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Спорт", ActivityTypeParent = type1 };
            ActivityType type4 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Прогулки", ActivityTypeParent = type1 };
            ActivityType type5 = new ActivityType() { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Посиделки", ActivityTypeParent = type1 };
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

            //Создание мероприятий
            Activity activity1 = new Activity()
            {
                ActivityName = "Велозабег",
                ActivityDescription = "Велозабег мечты Public",
                ActivityStartDayTime = DateTime.Now.AddDays(20),
                ActivityEndDayTime = DateTime.Now.AddDays(20).AddHours(1),
                ActivityAccess = ActivityAccessTypes.Public,
                ActivityTypeCurrent = type10,
                ActivityCreator = user1

            };
            Activity activity2 = new Activity()
            {
                ActivityName = "Велозабег",
                ActivityDescription = "Велозабег мечты Protected",
                ActivityPostDayTime = DateTime.Now,
                ActivityStartDayTime = DateTime.Now.AddDays(20),
                ActivityEndDayTime = DateTime.Now.AddDays(20).AddHours(1),
                ActivityAccess = ActivityAccessTypes.Protected,
                ActivityTypeCurrent = type10,
                ActivityCreator = user1
            };
            Activity activity3 = new Activity()
            {
                ActivityName = "Велозабег",
                ActivityDescription = "Велозабег мечты Private",
                ActivityPostDayTime = DateTime.Now,
                ActivityStartDayTime = DateTime.Now.AddDays(20),
                ActivityEndDayTime = DateTime.Now.AddDays(20).AddHours(1),
                ActivityAccess = ActivityAccessTypes.Private,
                ActivityTypeCurrent = type10,
                ActivityCreator = user1
            };
            Activity activity4 = new Activity()
            {
                ActivityName = "DND 5.0",
                ActivityDescription = "Dice'n'Roll в классическом сеттинге",
                ActivityPostDayTime = DateTime.Now,
                ActivityStartDayTime = DateTime.Now.AddDays(21),
                ActivityEndDayTime = DateTime.Now.AddDays(21).AddHours(12),
                ActivityAccess = ActivityAccessTypes.Public,
                ActivityTypeCurrent = type11,
                ActivityCreator = user
            };
            Activity activity5 = new Activity()
            {
                ActivityName = "Вязалка",
                ActivityDescription = "Пыщ пыщ ололо, Оля водит НЛО",
                ActivityPostDayTime = DateTime.Now,
                ActivityStartDayTime = DateTime.Now.AddDays(22),
                ActivityEndDayTime = DateTime.Now.AddDays(22).AddHours(3),
                ActivityAccess = ActivityAccessTypes.Public,
                ActivityTypeCurrent = type12,
                ActivityCreator = user2
            };

            context.Activities.AddRange(new List<Activity> { activity1, activity2, activity3, activity4, activity5 });
            context.SaveChanges();
        }
    }
}
