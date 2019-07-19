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
    public class SubListTests
    {
        [TestMethod]
        public void Can_Add_New_Subs()
        {
            ActivityType type1 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Один" };
            ActivityType type2 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Два" };
            // Организация - создание нескольких тестовых игр
            Activity act1 = new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра1", ActivityTypeCurrent = type1 };
            Activity act2 = new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра2", ActivityTypeCurrent = type1 };

            // Организация - создание корзины
            SubList SubList = new SubList();

            // Действие
            SubList.SubActivity(act1);
            SubList.SubActivity(act2);
            List<Activity> results = SubList.Line.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0], act1);
            Assert.AreEqual(results[1], act2);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            ActivityType type1 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Один" };
            ActivityType type2 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Два" };
            // Организация - создание нескольких тестовых игр
            Activity act1 = new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра1", ActivityTypeCurrent = type1 };
            Activity act2 = new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра2", ActivityTypeCurrent = type1 };

            // Организация - создание корзины
            SubList SubList = new SubList();

            // Действие
            SubList.SubActivity(act1);
            SubList.SubActivity(act2);
            SubList.UnsubActivity(act1);
            List<Activity> results = SubList.Line.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 1);
            Assert.AreEqual(results[0], act2);
        }

        [TestMethod]
        public void Can_Remove_all()
        {
            ActivityType type1 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Один" };
            ActivityType type2 = new ActivityType { ActivityTypeId = Guid.NewGuid(), ActivityTypeTitle = "Два" };
            // Организация - создание нескольких тестовых игр
            Activity act1 = new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра1", ActivityTypeCurrent = type1 };
            Activity act2 = new Activity { ActivityId = Guid.NewGuid(), ActivityName = "Игра2", ActivityTypeCurrent = type1 };

            // Организация - создание корзины
            SubList SubList = new SubList();

            // Действие
            SubList.SubActivity(act1);
            SubList.SubActivity(act2);
            SubList.Clear();

            List<Activity> results = SubList.Line.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), SubList.ComputeTotalSubs());
        }
    }
}
