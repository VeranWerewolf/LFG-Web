using LFG.Domain.Abstract;
using LFG.Domain.Entities;
using LFG.WebUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LFG.WebUI.Controllers
{
    public class AdminController : Controller
    {
        IActivityRepository activityRepository;
        IUserRepository userRepository;
        public AdminController(IActivityRepository repo)
        {
            activityRepository = repo;
        }
        public AdminController(IUserRepository repo)
        {
            userRepository = repo;
        }

        public ViewResult ActivitiesAdministration()
        {
            return View(activityRepository.Activities);
        }
        public ViewResult UsersAdministration()
        {
            return View(userRepository.AppUsers);
        }

        public ViewResult EditActivity(Guid? activityId)
        {
            Activity act = activityRepository.Activities
                .FirstOrDefault(g => g.ActivityId == activityId);
            return View(act);
        }
        [HttpPost]
        public ActionResult Edit(Activity activity)
        {
            if (ModelState.IsValid)
            {
                activityRepository.SaveActivity(activity);
                TempData["message"] = string.Format("Изменения в мероприятии \"{0}\" были сохранены", activity.ActivityName);
                return RedirectToAction("ActivitiesAdministration");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(activity);
            }
        }

        [HttpPost]
        public ActionResult CommitActivity(Guid? activityId)
        {
            IEnumerable<Activity> Activities = activityRepository.Activities
                .Where(g => g.ActivityId == activityId);
            
            if (Activities.Count() == 1)
            {
                foreach (var item in Activities) {
                    item.IsCommited = true;
                    //Как еще передать Коммитящего как экземпляр IdentityUser?
                    IEnumerable<AppUser> Users = userRepository.AppUsers.
                        Where(g => g.Id.ToString() == User.Identity.GetUserId());
                    item.CommitCreator = Users.First();
                }
            } 
            return RedirectToAction("ActivitiesAdministration");
        }
    }
}