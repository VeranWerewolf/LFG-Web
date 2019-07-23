using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LFG.Domain.Entities;

namespace LFG.WebUI.Models
{
    public class ActivitiesListViewModel
    {
        public IEnumerable<Activity> Activities { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
        public ActivityType ActivityType { get; set; }
    }
}