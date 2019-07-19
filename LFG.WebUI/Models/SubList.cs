using LFG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFG.WebUI.Models
{
    public class SubList
    {
        public IEnumerable<Activity> Line
        {
            get { return activityLine; }
        }

        private List<Activity> activityLine = new List<Activity>();

        public void SubActivity(Activity activity)
        {
            activityLine.Add(activity);
        }

        public void UnsubActivity(Activity activity)
        {
            activityLine.RemoveAll(l => l.ActivityId == activity.ActivityId);
        }

        public decimal ComputeTotalSubs()
        {
            return activityLine.Count();
        }
        public void Clear()
        {
            activityLine.Clear();
        }
    }
}

