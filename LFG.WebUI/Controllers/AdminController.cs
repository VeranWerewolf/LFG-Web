using LFG.Domain.Abstract;
using LFG.Domain.Entities;
using LFG.Domain.Infrastructure;
using LFG.WebUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace LFG.WebUI.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdminController : Controller
    {
        IActivityRepository activityRepository;
        IActivityTypeRepository activityTypeRepository;
        public AdminController(IActivityRepository repo, IActivityTypeRepository repot)
        {
            activityRepository = repo;
            activityTypeRepository = repot;
        } 

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        private AppUser CurrentUser
        {
            get
            {
                return UserManager.FindByName(HttpContext.User.Identity.Name);
            }
        }


        public ActionResult Index()
        {
            return RedirectToAction("ActivitiesAdministration");
        }
        public ViewResult ActivitiesAdministration()
        {
            return View(activityRepository.Activities);
        }
        public ViewResult EditActivity(Guid? activityId)
        {
            Activity activity = activityRepository.Activities
                .FirstOrDefault(g => g.ActivityId == activityId);
            EditActivityViewModel model = new EditActivityViewModel
            {
                Activity = activity,
                Categories = new SelectList(activityTypeRepository.ActivityTypes, "ActivityTypeId", "ActivityTypeTitle", new ActivityType())
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult EditActivity(EditActivityViewModel model)
        {
            model.Activity.ActivityTypeCurrent = activityTypeRepository.ActivityTypes
                    .FirstOrDefault(p => p.ActivityTypeId.ToString() == model.CurrentCategory);
            if (ModelState.IsValid)
            {
                activityRepository.SaveActivity(model.Activity);
                TempData["message"] = string.Format("Изменения в мероприятии \"{0}\" были сохранены", model.Activity.ActivityName);
                return RedirectToAction("ActivitiesAdministration");
            }
            else
            {
                // Что-то не так со значениями данных
                TempData["error"] = string.Format("Ошибка при создании мероприятия \"{0}\"! ", model.Activity.ActivityName);
            }
            return View(model);
        }
        [Authorize(Roles = "Moderators, Administrators")]
        public ViewResult CommitActivityList()
        {
            return View(activityRepository.Activities.Where(x => x.IsCommited == null));
        }
        [Authorize(Roles = "Moderators, Administrators")]
        public ViewResult CommitActivity(Guid? activityId)
        {
            Activity act = activityRepository.Activities
                .FirstOrDefault(g => g.ActivityId == activityId);
            return View(act);
        }
        [HttpPost]
        [Authorize(Roles = "Moderators, Administrators")]
        public ActionResult CommitActivity(Activity activity)
        {
            activityRepository.Commit(activity, CurrentUser);
            TempData["message"] = string.Format("Изменения в мероприятии \"{0}\" были сохранены", activity.ActivityName);
            if (HttpContext.User.IsInRole("Administrators"))
            {
                return RedirectToAction("ActivitiesAdministration");
            }
            else
            {
                return RedirectToAction("CommitActivityList");
            }
        }


        public ViewResult UsersAdministration()
        {
            return View(UserManager.Users);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("UsersAdministration");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "Пользователь не найден" });
            }
        }

        public ActionResult RolesAdministration()
        {
            return View(RoleManager.Roles);
        }
        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateRole([Required]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result
                    = await RoleManager.CreateAsync(new AppRole(name));

                if (result.Succeeded)
                {
                    return RedirectToAction("RolesAdministration");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }
        [HttpPost]
        public async Task<ActionResult> DeleteRole(string id)
        {
            AppRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("RolesAdministration");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "Роль не найдена" });
            }
        }
        public async Task<ActionResult> EditRole(string id)
        {
            AppRole role = await RoleManager.FindByIdAsync(id);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();

            IEnumerable<AppUser> members
                = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));

            IEnumerable<AppUser> nonMembers = UserManager.Users.Except(members);

            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }
        [HttpPost]
        public async Task<ActionResult> EditRole(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);

                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    result = await UserManager.RemoveFromRoleAsync(userId,
                    model.RoleName);

                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("Index");

            }
            return View("Error", new string[] { "Роль не найдена" });
        }

    }
}