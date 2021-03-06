using System.Linq;

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
    public class MigrationTest
    {
        [Test]
        public void UpperAndLowerColumnNames()
        {
            using (var db = new TestDb(true) { Trace = true })
            {
                db.CreateTable<LowerId>();
                db.CreateTable<UpperId>();

                var cols = db.GetTableInfo("Test").ToList();
                Assert.That(cols.Count, Is.EqualTo(1));
                Assert.That(cols[0].Name, Is.EqualTo("Id"));
            }
        }

        [Table("Test")]
        private class LowerId
        {
            public int Id { get; set; }
        }

        [Table("Test")]
        private class UpperId
        {
            public int ID { get; set; }
        }
    }
}