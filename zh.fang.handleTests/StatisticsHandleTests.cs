using Microsoft.VisualStudio.TestTools.UnitTesting;
using zh.fang.handle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zh.fang.handle.Tests
{
    using common;

    [TestClass()]
    public class StatisticsHandleTests
    {
        [TestMethod()]
        public void ClassesTotalTest()
        {
            using (
                Handle statisticsHandler = new StatisticsHandle(),
                clsHandler = new CaseClassesHandle()
                )
            {
                Assert.IsInstanceOfType(clsHandler.Remove<data.entity.CaseClasses>(t => t.IsDel == 0), typeof(bool));
                Assert.IsInstanceOfType(clsHandler.Remove<data.entity.CaseClassesStatistics>(), typeof(bool));

                var cls = CreateCls("刑事");
                Assert.IsTrue(clsHandler.Add(cls));

                var scls = CreateCls("杀人");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                var total = CreateClsTotal(scls);
                Assert.IsTrue(statisticsHandler.Add(total));

                scls = CreateCls("故意伤害");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                total = CreateClsTotal(scls);
                Assert.IsTrue(statisticsHandler.Add(total));

                scls = CreateCls("抢劫");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                total = CreateClsTotal(scls);
                Assert.IsTrue(statisticsHandler.Add(total));

                scls = CreateCls("抢夺");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                total = CreateClsTotal(scls);
                Assert.IsTrue(statisticsHandler.Add(total));

                scls = CreateCls("其它");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                total = CreateClsTotal(scls);
                Assert.IsTrue(statisticsHandler.Add(total));

                // ====================================================

                cls = CreateCls("治安");
                Assert.IsTrue(clsHandler.Add(cls));

                scls = CreateCls("盗窃");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                total = CreateClsTotal(scls);
                Assert.IsTrue(statisticsHandler.Add(total));

                scls = CreateCls("诈骗");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                total = CreateClsTotal(scls);
                Assert.IsTrue(statisticsHandler.Add(total));

                scls = CreateCls("其它");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                total = CreateClsTotal(scls);
                Assert.IsTrue(statisticsHandler.Add(total));

                // ================================================

                cls = CreateCls("其它");
                Assert.IsTrue(clsHandler.Add(cls));

                scls = CreateCls("交通");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                total = CreateClsTotal(scls);
                Assert.IsTrue(statisticsHandler.Add(total));

                scls = CreateCls("火灾");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                total = CreateClsTotal(scls);
                Assert.IsTrue(statisticsHandler.Add(total));

                scls = CreateCls("群众求助");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                total = CreateClsTotal(scls);
                Assert.IsTrue(statisticsHandler.Add(total));

                scls = CreateCls("其它");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                total = CreateClsTotal(scls);
                Assert.IsTrue(statisticsHandler.Add(total));

                var totalitems = ((StatisticsHandle)statisticsHandler).ClassesTotal(DateTime.Now.AddDays(-1).ToUnixTime(), DateTime.Now.ToUnixTime());
                Assert.IsTrue(0 <= totalitems.Count());
            }
        }

        private data.entity.CaseClassesStatistics CreateClsTotal(data.entity.CaseClasses cls)
        {

            var total = new data.entity.CaseClassesStatistics
            {
                CaseCount = (new Random(Guid.NewGuid().GetHashCode())).Next(1, 255),
                ClassesId = cls.Id,
                Id = GuidToString16(),
                OrgId = GuidToString16(),
                TotalDate = DateTime.Now.ToUnixTime()
            };
            return total;
        }

        private data.entity.CaseClasses CreateCls(string name)
        {
            var code = GuidToString16();
            var cls = new data.entity.CaseClasses { Code = code, Id = code, IsDel = 0, Name = name, ParentId = null, ThdVal = -1 };
            return cls;
        }
        
        private string GuidToString16()
        {
            long i = 1;
            foreach (byte b in System.Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - System.DateTime.Now.Ticks);

        }
    }
}