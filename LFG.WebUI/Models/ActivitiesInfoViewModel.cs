using LFG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LFG.WebUI.Models
{
    public class ActivitiesInfoViewModel
    {
        public ActivitiesInfoViewModel(Activity Incoming, AppUser user)
        {
            this.Activity = Incoming;
            this.CurrentUser = user;
            ActivityType type = Activity.ActivityTypeCurrent;
            if (type != null)
                ListFiller(type);
        }

        public Activity Activity { get; set; }
        public AppUser CurrentUser { get; set; }
        public List<ActivityType> ActivityTypeList = new List<ActivityType>();

        
        private void ListFiller(ActivityType start)
        {
            this.ActivityTypeList.Add(start);
            if (start.ActivityTypeParent != null)
            {
                ListFiller(start.ActivityTypeParent);
            }
        }
    }
}