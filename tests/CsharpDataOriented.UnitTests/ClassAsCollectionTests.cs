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
                    Given = new[] { "Glen", "Ruben" },
                    Family = "Rodriguez"
                }, 
                DoB = new DateTime(2011, 10, 3)
            };

            var seq = Seq(obj.GetType());

            var objColl = seq(obj);

            Assert.Equal(2, objColl.Count());
        }

        [Fact]
        public void CanRecognizeSeqTypes()
        {
            var nameArr = new[] { "Glenis", "Mayra" };
            var nameList = nameArr.ToList();
            var nameDict = nameArr.ToDictionary(n => n);

            Assert.True(LeaveAsIs(nameArr));
            Assert.True(LeaveAsIs(nameList));
            Assert.True(LeaveAsIs(nameDict));
        }
    }
}
