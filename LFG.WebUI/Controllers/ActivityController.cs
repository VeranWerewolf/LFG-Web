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

        //UnitTest1 - Can_Send_Pagination_View_Model()
        //public ViewResult List(int page = 1)
        //{
        //    ActivitiesListViewModel model = new ActivitiesListViewModel
        //    {
        //        Activities = repository.Activities
        //            .Where(p => p.ActivityAccessType != 2)
        //            .OrderBy(Activity => Activity.ActivityStartDayTime)
        //            .Skip((page - 1) * pageSize)
        //            .Take(pageSize),
        //        PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = page,
        //            ItemsPerPage = pageSize,
        //            TotalItems = repository.Activities.Count()
        //        }
        //    };
        //    return View(model);

        //return View(repository.Activities
        //    .OrderBy(Activity => Activity.ActivityStartDayTime)
        //    .Skip((page - 1) * pageSize)
        //    .Take(pageSize));

        //return View(repository.Activities);

        //}

        //UnitTest1 - Can_Filter_Activities, Generate_Category_Specific_Game_Count()
        public ViewResult List(string category, int page = 1)
        {
            ActivitiesListViewModel model = new ActivitiesListViewModel
            {
                Activities = repository.Activities
                    .Where(p => !p.ActivityAccess.Equals(ActivityAccessTypes.Private))
                    .Where(p => category == null || p.ActivityTypeCurrent.ToString() == category)
                    .OrderBy(Activity => Activity.ActivityStartDayTime)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    //Добавлялись пустые страницы
                    //TotalItems = repository.Activities.Where(p => p.ActivityAccessType != 2).Count()
                    //TODO: !act.ActivityAccess.Equals(ActivityAccessTypes.Private when category == null
                    TotalItems = category == null  ? repository.Activities.Count() : repository.Activities
                    .Where(act => !act.ActivityAccess.Equals(ActivityAccessTypes.Private))
                    .Where(act => act.ActivityTypeCurrent.ToString() == category)
                    .Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
    }
}