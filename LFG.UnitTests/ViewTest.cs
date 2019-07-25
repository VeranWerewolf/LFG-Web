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
    public class ViewTest
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IActivityRepository> mock = new Mock<IActivityRepository>();
            mock.Setup(m => m.Activities).Returns(new List<Activity>
            {
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра1", IsCommited= true},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра2", IsCommited= true},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра3", IsCommited= true},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра4", IsCommited= true},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра5", IsCommited= true}
            });
            ActivityController controller = new ActivityController(mock.Object,null)
            {
                pageSize = 3
            };

            // Действие (act)
            ActivitiesListViewModel result = (ActivitiesListViewModel)controller.List(null, 2).Model;

            // Утверждение (assert)
            List<Activity> games = result.Activities.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].ActivityName, "Игра4");
            Assert.AreEqual(games[1].ActivityName, "Игра5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, i => "Page" + i);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IActivityRepository> mock = new Mock<IActivityRepository>();
            mock.Setup(m => m.Activities).Returns(new List<Activity>
            {
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра1"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра2"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра3"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра4"},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра5"}
            });
            ActivityController controller = new ActivityController(mock.Object, null)
            {
                pageSize = 3
            };

            // Act
            ActivitiesListViewModel result
                = (ActivitiesListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Activities()
        {
            // Организация (arrange)
            Mock<IActivityRepository> mock = new Mock<IActivityRepository>();
            ActivityType type1 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Один" };
            ActivityType type2 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Два" };

            mock.Setup(m => m.Activities).Returns(new List<Activity>
                {

                    new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра1", ActivityTypeCurrent = type1, IsCommited= true},
                    new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра2", ActivityTypeCurrent = type1, IsCommited= true},
                    new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра3", ActivityTypeCurrent = type2, IsCommited= true},
                    new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра4", ActivityTypeCurrent = type1, IsCommited= true},
                    new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра5", ActivityTypeCurrent = type2, IsCommited= true}
                });
            ActivityController controller = new ActivityController(mock.Object,null)
            {
                pageSize = 3
            };

            // Action
            List<Activity> result = ((ActivitiesListViewModel)controller.List("Два", 1).Model)
                .Activities.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].ActivityName == "Игра3" && result[0].ActivityTypeCurrent == type2);
            Assert.IsTrue(result[1].ActivityName == "Игра5" && result[1].ActivityTypeCurrent == type2);
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            //    Mock<IActivityTypeRepository> mockType = new Mock<IActivityTypeRepository>();
            //    mockType.Setup(m => m.ActivityTypes).Returns(new List<ActivityType> {
            //    new ActivityType { ActivityTypeCurrent = 1, ActivityTypeTitle = "Один" },
            //    new ActivityType { ActivityTypeCurrent = 2, ActivityTypeTitle = "Два" },
            //    new ActivityType { ActivityTypeCurrent = 3, ActivityTypeTitle = "Три" }
            //});
            Mock<IActivityRepository> mock = new Mock<IActivityRepository>();
            ActivityType type1 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Один" };
            ActivityType type2 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Два" };
            ActivityType type3 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Три" };

            mock.Setup(m => m.Activities).Returns(new List<Activity> {
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра1", ActivityTypeCurrent = type1},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра2", ActivityTypeCurrent = type1},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра3", ActivityTypeCurrent = type2},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра4", ActivityTypeCurrent = type3},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра5", ActivityTypeCurrent = type2}
                });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "Два");
            Assert.AreEqual(results[1], "Один");
            Assert.AreEqual(results[2], "Три");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<IActivityRepository> mock = new Mock<IActivityRepository>();
            ActivityType type1 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Один" };
            ActivityType type2 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Два" };

            mock.Setup(m => m.Activities).Returns(new List<Activity> {
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра1", ActivityTypeCurrent = type1},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра2", ActivityTypeCurrent = type2},
                });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Два";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            /// Организация (arrange)
            Mock<IActivityRepository> mock = new Mock<IActivityRepository>();
            ActivityType type1 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Один" };
            ActivityType type2 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Два" };
            ActivityType type3 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Три" };

            mock.Setup(m => m.Activities).Returns(new List<Activity> {
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра1", ActivityTypeCurrent = type1},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра2", ActivityTypeCurrent = type1},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра3", ActivityTypeCurrent = type2},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра4", ActivityTypeCurrent = type3},
                new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра5", ActivityTypeCurrent = type2}
                });
            ActivityController controller = new ActivityController(mock.Object, null) { pageSize = 3 };

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((ActivitiesListViewModel)controller.List("Один").Model).PagingInfo.TotalItems;
            int res2 = ((ActivitiesListViewModel)controller.List("Два").Model).PagingInfo.TotalItems;
            int res3 = ((ActivitiesListViewModel)controller.List("Три").Model).PagingInfo.TotalItems;
            int resAll = ((ActivitiesListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}

