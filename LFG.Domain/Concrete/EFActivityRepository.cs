using System.Collections.Generic;
using LFG.Domain.Entities;
using LFG.Domain.Abstract;
using LFG.Domain.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace LFG.Domain.Concrete
{
    public class EFActivityRepository : IActivityRepository
    {
        EFDbContext context;

        public EFActivityRepository(EFDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Activity> Activities
        {
            get { return context.Activities; }
        }

        public void SaveActivity(Activity activity)
        {
            if (activity.ActivityId == null)
            {
                context.Activities.Add(activity);
            }
            else
            {
                Activity dbEntry = context.Activities.Find(activity.ActivityId);
                if (dbEntry != null)
                {
                    dbEntry.ActivityName = activity.ActivityName;
                    dbEntry.ActivityDescription = activity.ActivityDescription;
                    dbEntry.ActivityStartDayTime = activity.ActivityStartDayTime;
                    dbEntry.ActivityEndDayTime = activity.ActivityEndDayTime;
                    dbEntry.ActivityAccess = activity.ActivityAccess;
                    dbEntry.ActivityTypeCurrent = activity.ActivityTypeCurrent;
                }
            }
            context.SaveChanges();
        }

        public void CreateActivity(Activity activity, AppUser currentUser)
        {
            activity.ActivityCreator = currentUser;
            activity.ActivityId = new System.Guid();
            context.Activities.Add(activity);
            context.SaveChanges();
        }

        public void Commit(Activity activity, AppUser currentUser)
        {
            if (activity.ActivityId != null)
            {
                AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
                AppUser user = userMgr.FindByEmail(currentUser.Email);
                Activity dbEntry = context.Activities.Find(activity.ActivityId);
                if (dbEntry != null)
                {
                    dbEntry.IsCommited = activity.IsCommited;
                    dbEntry.CommitDescription = activity.CommitDescription;
                    dbEntry.CommitCreator = user;
                }
            }
            context.SaveChanges();
        }
    }
}