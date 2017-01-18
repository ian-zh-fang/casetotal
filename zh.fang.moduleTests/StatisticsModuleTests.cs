using Microsoft.VisualStudio.TestTools.UnitTesting;
using zh.fang.module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zh.fang.module.Tests
{
    [TestClass()]
    public class StatisticsModuleTests
    {
        [TestMethod()]
        public void ClassesTotalTest()
        {
            var clsModule = new ClassesModule();
            var clsItems = clsModule.FetchAll();
            var clsCount = clsItems.Count();
            Assert.IsTrue(0 <= clsCount);

            var module = new StatisticsModule();
            DateTime? timeToStart = null;
            DateTime? timeToEnd = null;
            var result = module.ClassesTotal(timeToStart, timeToEnd);
            Assert.IsTrue(clsCount == result.Count());

            timeToStart = DateTime.Now;
            result = module.ClassesTotal(timeToStart, timeToEnd);
            Assert.IsTrue(clsCount == result.Count());

            timeToEnd = DateTime.Now;
            result = module.ClassesTotal(timeToStart, timeToEnd);
            Assert.IsTrue(clsCount == result.Count());

            timeToStart = DateTime.Now.AddDays(-1);
            result = module.ClassesTotal(timeToStart, timeToEnd);
            Assert.IsTrue(clsCount == result.Count());
        }

        [TestMethod()]
        public void OrgClassTotalTest()
        {
            var clsModule = new ClassesModule();
            var clsItems = clsModule.FetchAll();
            var clsCount = clsItems.Count();

            var orgModule = new OrgModule();
            var orgItems = orgModule.FetchAll();
            var orgCount = orgItems.Count();

            var totalModule = new StatisticsModule();
            var result = totalModule.OrgClassTotalOnToday();
            Assert.IsTrue(orgCount == result.Count());
            foreach (var total in result)
            {
                Assert.IsTrue(clsCount == total.ClassesTotals.Count());
            }
        }
    }
}