using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LFG.Domain.Abstract;
using LFG.Domain.Entities;
using LFG.WebUI.Models;

namespace LFG.WebUI.Controllers
{
    public class ActivityController : Controller
    {
        private IActivityRepository repository;
        public int pageSize = 4;

        public ActivityController(IActivityRepository repo)
        {
            repository = repo;
        }

        //UnitTest1 - Can_Send_Pagination_View_Model(), Can_Filter_Activities, Generate_Category_Specific_Game_Count()
        public ViewResult List(string category, int page = 1)
        {
            ActivitiesListViewModel model = new ActivitiesListViewModel
            {
                Activities = repository.Activities
                    .Where(p => !p.ActivityAccess.Equals(ActivityAccessTypes.Private))
                    .Where(p => p.IsCommited==true)
                    .Where(p => category == null || p.ActivityTypeCurrent.ToString() == category)
                    .OrderBy(Activity => Activity.ActivityStartDayTime)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,

                    TotalItems = category == null  ? 
                    repository.Activities
                    .Where(p => !p.ActivityAccess.Equals(ActivityAccessTypes.Private)).Count() : 
                    repository.Activities
                    .Where(p => !p.ActivityAccess.Equals(ActivityAccessTypes.Private))
                    .Where(p => p.ActivityTypeCurrent.ToString() == category)
                    .Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
    }
}