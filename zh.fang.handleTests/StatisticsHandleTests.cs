using Microsoft.VisualStudio.TestTools.UnitTesting;
using zh.fang.handle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zh.fang.handle.Tests
{
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

                var cls = CreateCls("刑事");
                Assert.IsTrue(clsHandler.Add(cls));

                var scls = CreateCls("杀人");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                scls = CreateCls("故意伤害");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                scls = CreateCls("抢劫");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                scls = CreateCls("抢夺");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                scls = CreateCls("其它");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                cls = CreateCls("治安");
                Assert.IsTrue(clsHandler.Add(cls));

                scls = CreateCls("盗窃");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                scls = CreateCls("诈骗");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                scls = CreateCls("其它");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                cls = CreateCls("其它");
                Assert.IsTrue(clsHandler.Add(cls));

                scls = CreateCls("交通");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                scls = CreateCls("火灾");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                scls = CreateCls("群总求组");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                scls = CreateCls("其它");
                scls.ParentId = cls.Id;
                scls.Code = $"{cls.Code}{scls.Code}";
                Assert.IsTrue(clsHandler.Add(scls));

                var total = ((StatisticsHandle)statisticsHandler).ClassesTotal(DateTime.Now.AddDays(-1).Ticks, DateTime.Now.Ticks);
                Assert.IsTrue(0 < total.Count());
            }
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