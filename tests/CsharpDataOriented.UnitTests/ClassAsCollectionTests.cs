using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpDataOriented.CollectionsModule;

namespace CsharpDataOriented.UnitTests
{
    public class ClassAsCollectionTests
    {
        [Fact]
        public void CanListClassProps()
        {
            var obj = new { 
                Name = new
                {
                    Given = new[] { "Tania", "Mirllan" },
                    Family = "Gonzalez"
                }, 
                DoB = new DateTime(1969, 10, 16)
            };

            var seq = Seq(obj.GetType());

            var objColl = seq(obj);
        }

        [Fact]
        public void CanRecognizeSeqTypes()
        {
            var nameArr = new[] { "Tania", "Mirllan" };
            var nameList = nameArr.ToList();
            var nameDict = nameArr.ToDictionary(n => n);

            Assert.True(LeaveAsIs(nameArr));
            Assert.True(LeaveAsIs(nameList));
            Assert.True(LeaveAsIs(nameDict));
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public DateTime DoB { get; set; }
    }
}
