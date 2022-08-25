using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpDataOriented.UnitTests;

public class Fixture
{
    public static object Person { get; } = new
    {
        Name = new
        {
            Given = new[] { "Glen", "Ruben" },
            Family = "Rodriguez"
        },
        DoB = new DateTime(2011, 10, 3),
        Addresses = new[] {
            new {
                Lines = new[] { "184 #42301, esq 423" },
                PostalCode = "18100",
                Period = new { Start = new DateTime(2019, 04, 17) } } }
    };
}
