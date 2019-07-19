using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using LFG.Domain.Abstract;
using LFG.Domain.Entities;
using LFG.WebUI.Controllers;
using LFG.WebUI.Models;
using LFG.WebUI.HtmlHelpers;

namespace LFG.UnitTests
{
    [TestClass]
    public class UnitTest2
    {
        
        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //// Организация (arrange)
            //Mock<IUserRepository> mock = new Mock<IUserRepository>();
            //mock.Setup(m => m.AppUsers).Returns(new List<AppUser>
            //{
            //    new AppUser { Id = Guid.NewGuid().ToString(), UserName = "Игра1"},
            //    new AppUser { Id = Guid.NewGuid().ToString(), UserName = "Игра2"},
            //    new AppUser { Id = Guid.NewGuid().ToString(), UserName = "Игра3"},
            //    new AppUser { Id = Guid.NewGuid().ToString(), UserName = "Игра4"},
            //    new AppUser { Id = Guid.NewGuid().ToString(), UserName = "Игра5"}
            //});
            //ActivityController controller = new ActivityController(mock.Object)
            //{
            //    pageSize = 3
            //};

            //// Действие (act)
            //ActivitiesListViewModel result = (ActivitiesListViewModel)controller.List(null, 2).Model;

            //// Утверждение (assert)
            //List<Activity> games = result.Activities.ToList();
            //Assert.IsTrue(games.Count == 2);
            //Assert.AreEqual(games[0].ActivityName, "Игра4");
            //Assert.AreEqual(games[1].ActivityName, "Игра5");
        }
    }
}
