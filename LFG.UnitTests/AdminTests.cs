using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LFG.Domain.Abstract;
using LFG.Domain.Entities;
using LFG.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LFG.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Activities()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IActivityRepository> mock = new Mock<IActivityRepository>();
            mock.Setup(m => m.Activities).Returns(new List<Activity>
            {
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра1"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра2"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра3"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра4"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object, null);

            // Действие
            List<Activity> result = ((IEnumerable<Activity>)controller.ActivitiesAdministration().ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count, 5);
            Assert.AreEqual("Игра1", result[0].ActivityName);
            Assert.AreEqual("Игра2", result[1].ActivityName);
            Assert.AreEqual("Игра3", result[2].ActivityName);
        }

        //[TestMethod]
        //public void Index_Contains_All_Users()
        //{
        //    // Организация - создание имитированного хранилища данных
        //    Mock<IUserRepository> mock = new Mock<IUserRepository>();
        //    mock.Setup(m => m.AppUsers).Returns(new List<AppUser>
        //    {
        //        new AppUser { UserName = "Юзверь1"},
        //        new AppUser { UserName = "Юзверь2"},
        //        new AppUser { UserName = "Юзверь3"},
        //        new AppUser { UserName = "Юзверь4"},
        //        new AppUser { UserName = "Юзверь5"}
        //    });

        //    // Организация - создание контроллера
        //    AdminController controller = new AdminController(mock.Object);

        //    // Действие
        //    List<AppUser> result = ((IEnumerable<AppUser>)controller.UsersAdministration().ViewData.Model).ToList();

        //    // Утверждение
        //    Assert.AreEqual(result.Count, 5);
        //    Assert.AreEqual("Юзверь1", result[0].UserName);
        //    Assert.AreEqual("Юзверь2", result[1].UserName);
        //    Assert.AreEqual("Юзверь5", result[4].UserName);
        //}

        [TestMethod]
        public void Can_Edit_Activity()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IActivityRepository> mock = new Mock<IActivityRepository>();
            mock.Setup(m => m.Activities).Returns(new List<Activity>
            {
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра1"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра2"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра3"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра4"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object, null);
            List<Activity> result = ((IEnumerable<Activity>)controller.ActivitiesAdministration().ViewData.Model).ToList();

            // Действие
            Activity game1 = controller.EditActivity(result[0].ActivityId).ViewData.Model as Activity;
            Activity game2 = controller.EditActivity(result[1].ActivityId).ViewData.Model as Activity;
            Activity game3 = controller.EditActivity(result[2].ActivityId).ViewData.Model as Activity;

            // Assert
            Assert.AreEqual(result[0].ActivityId.ToString(), game1.ActivityId.ToString());
            Assert.AreEqual(result[1].ActivityId.ToString(), game2.ActivityId.ToString());
            Assert.AreEqual(result[2].ActivityId.ToString(), game3.ActivityId.ToString());
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Activity()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IActivityRepository> mock = new Mock<IActivityRepository>();
            mock.Setup(m => m.Activities).Returns(new List<Activity>
            {
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра1"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра2"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра3"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра4"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object, null);

            // Действие
            Activity result = controller.EditActivity(Guid.NewGuid()).ViewData.Model as Activity;

            // Assert
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IActivityRepository> mock = new Mock<IActivityRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object, null);

            // Организация - создание объекта Game
            Activity game = new Activity { ActivityName = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.EditActivity(game);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveActivity(game));

            // Утверждение - проверка типа результата метода
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IActivityRepository> mock = new Mock<IActivityRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object,null);

            // Организация - создание объекта Game
            Activity game = new Activity { ActivityName = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.EditActivity(game);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveActivity(It.IsAny<Activity>()), Times.Never());

            // Утверждение - проверка типа результата метода
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
