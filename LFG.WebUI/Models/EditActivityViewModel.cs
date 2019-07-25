using LFG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LFG.WebUI.Models
{
    public class EditActivityViewModel
    {
        public Activity Activity { get; set; }
        public string CurrentCategory { get; set; }
        public SelectList Categories { get; set; }
    }
}