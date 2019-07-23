using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LFG.Domain.Abstract;
using LFG.Domain.Entities;
using LFG.Domain.Infrastructure;
using LFG.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace LFG.WebUI.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private IActivityRepository activityRepository;
        public ActivityController(IActivityRepository repo)
        {
            activityRepository = repo;
        }
        public int pageSize = 4;
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        private AppUser CurrentUser
        {
            get
            {
                return UserManager.FindByName(HttpContext.User.Identity.Name);
            }
        }


        //UnitTest1 - Can_Send_Pagination_View_Model(), Can_Filter_Activities, Generate_Category_Specific_Game_Count()
        [AllowAnonymous]
        public ViewResult List(string category, int page = 1)
        {
            ActivitiesListViewModel model = new ActivitiesListViewModel
            {
                Activities = activityRepository.Activities
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
                    activityRepository.Activities
                    .Where(p => !p.ActivityAccess.Equals(ActivityAccessTypes.Private)).Count() :
                    activityRepository.Activities
                    .Where(p => !p.ActivityAccess.Equals(ActivityAccessTypes.Private))
                    .Where(p => p.ActivityTypeCurrent.ToString() == category)
                    .Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
        [AllowAnonymous]
        public ViewResult Page(string id)
        {
            Activity activity = activityRepository.Activities
                .FirstOrDefault(g => g.ActivityId.ToString() == id);
            ActivitiesInfoViewModel model = new ActivitiesInfoViewModel(activity, CurrentUser);
            return View(model);
        }

        public ViewResult CreateActivity()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateActivity(Activity activity)
        {
            if (ModelState.IsValid)
            {
                activityRepository.CreateActivity(activity, CurrentUser);
                TempData["message"] = string.Format("Изменения в мероприятии \"{0}\" были сохранены", activity.ActivityName);
                return RedirectToAction("List");
            }
            else
            {
                // Что-то не так со значениями данных
                TempData["error"] = string.Format("Ошибка при создании мероприятия \"{0}\"! ", activity.ActivityName);
            }
            return View("List");
        }

        [HttpPost]
        public ActionResult Sub(Activity activity)
        {
            //!!!

            return RedirectToAction("UserSubs");
        }

        public ViewResult UserSubs()
        {
            //!!!

            return View();
        }

        public ViewResult ActivitySubs()
        {
            //!!!

            return View();
        }
    }
}