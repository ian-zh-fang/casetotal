

namespace zh.fang.data.oracle.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Linq;

    [TestClass()]
    public class RepositoryTests
    {
        [TestMethod()]
        public void RepositoryTest()
        {
            var rep = new oracle.Repository("U_CT");
            var dels = new short[] { 0, 1 };
            System.Linq.Expressions.Expression<System.Func<fang.data.entity.CaseClasses, bool>> express
                = t => dels.Any(x => t.IsDel == x);

            var items = rep.RemoveAny<entity.CaseClasses>(express).ToList();
            var rst = rep.Commit();
            Assert.IsTrue(rst > 0);
            Assert.IsTrue(0 <= items.Count);

            items = rep.Query<entity.CaseClasses>(express).ToList();
            Assert.IsTrue(items.Count == 0);

            var code = GuidToString16();
            var entity = new entity.CaseClasses { Code = code, Id = code, IsDel = 0, Name = GuidToString16(), ThdVal = -1 };
            var data = rep.Add<zh.fang.data.entity.CaseClasses>(entity);
            rst = rep.Commit();
            Assert.IsTrue(rst > 0);
            
            entity.IsDel = 1;
            data = rep.Update<fang.data.entity.CaseClasses>(entity);
            rst = rep.Commit();
            Assert.IsTrue(rst > 0);

            items = rep.Query<fang.data.entity.CaseClasses>(express).ToList();
            Assert.IsTrue(items.Any(t => t.IsDel == 1));
            Assert.IsTrue(1 == items.Count);

            items = rep.RemoveAny<fang.data.entity.CaseClasses>(express).ToList();
            rst = rep.Commit();
            Assert.IsTrue(rst > 0);
            Assert.IsTrue(1 == items.Count);

            items = rep.Query<fang.data.entity.CaseClasses>(express).ToList();
            Assert.IsTrue(0 == items.Count);
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