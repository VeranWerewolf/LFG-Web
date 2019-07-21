using LFG.Domain.Abstract;
using LFG.Domain.Entities;
using LFG.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LFG.WebUI.Controllers
{
    public class SubListController : Controller
    {
        private IActivityRepository repository;
        public SubListController(IActivityRepository repo)
        {
            repository = repo;
        }

        public RedirectToRouteResult SubActivity(Activity activity, string returnUrl)
        {
            //Activity Activity = repository.Activities
            //    .FirstOrDefault(g => g == activity);

            //if (activity != null)
            //{
            //    GetList().AddItem(activity);
            //}
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int gameId, string returnUrl)
        {
            //Game game = repository.Games
            //    .FirstOrDefault(g => g.GameId == gameId);

            //if (game != null)
            //{
            //    GetList().RemoveLine(game);
            //}
            return RedirectToAction("Index", new { returnUrl });
        }

        public SubList GetList()
        {
            SubList subList = (SubList)Session["Cart"];
            //if (subList == null)
            //{
            //    subList = new SubList();
            //    Session["Cart"] = subList;
            //}
            return subList;
        }
    }
}