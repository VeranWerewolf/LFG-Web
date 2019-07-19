using System.Collections.Generic;
using LFG.Domain.Entities;
using LFG.Domain.Abstract;

namespace LFG.Domain.Concrete
{
    public class EFActivityRepository : IActivityRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Activity> Activities
        {
            get { return context.Activities; }
        }

        public void SaveActivity(Activity activity)
        {
            if (activity.ActivityId == null) {
            context.Activities.Add(activity);}
            else
            {
                Activity dbEntry = context.Activities.Find(activity.ActivityId);
                if (dbEntry != null)
                {
                    dbEntry = activity;
                }
            }
            context.SaveChanges();
        }
    }
}