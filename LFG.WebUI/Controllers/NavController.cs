using LFG.Domain.Abstract;
using LFG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LFG.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IActivityRepository repository;
        public NavController(IActivityRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult TopPanel()
        {
            return PartialView();
        }


        //UnitTest1 - Can_Create_Categories(), Indicates_Selected_Category()
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> result = repository.Activities
                .Select(Activity => Activity.ActivityTypeCurrent.ToString())
                .Distinct()
                .OrderBy(s => s);
            return PartialView(result);
        }
    }
}