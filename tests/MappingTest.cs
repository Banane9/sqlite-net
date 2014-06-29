using System;
using System.Collections.Generic;

#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SetUp = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestInitializeAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else

using NUnit.Framework;

#endif

namespace SQLite.Tests
{
    [TestFixture]
    public class MappingTest
    {
        [Test]
        public void HasGoodNames()
        {
            var db = new TestDb();

            db.CreateTable<AFunnyTableName>();

            var mapping = db.GetMapping<AFunnyTableName>();

            Assert.AreEqual("AGoodTableName", mapping.TableName);

            Assert.AreEqual("AGoodColumnName", mapping.Columns[0].Name);
            Assert.AreEqual("Id", mapping.Columns[1].Name);
        }

        [Table("AGoodTableName")]
        private class AFunnyTableName
        {
            [Column("AGoodColumnName")]
            public string AFunnyColumnName { get; set; }

            [PrimaryKey]
            public int Id { get; set; }
        }

        #region Issue #86

        [Test]
        public void Issue86()
        {
            var db = new TestDb();
            db.CreateTable<Foo>();

            db.Insert(new Foo { Bar = 42 });
            db.Insert(new Foo { Bar = 69 });

            var found42 = db.Table<Foo>().Where(f => f.Bar == 42).FirstOrDefault();
            Assert.IsNotNull(found42);

            var ordered = new List<Foo>(db.Table<Foo>().OrderByDescending(f => f.Bar));
            Assert.AreEqual(2, ordered.Count);
            Assert.AreEqual(69, ordered[0].Bar);
            Assert.AreEqual(42, ordered[1].Bar);
        }

        [Table("foo")]
        public class Foo
        {
            [Column("baz")]
            public int Bar { get; set; }
        }

        #endregion Issue #86
    }
}