using System;
using System.Collections.Generic;
using System.Text;

namespace PatternMatchingSamples
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Type { get; set; }
        public string Region { get; set; }
        public Employee ReportsTo { get; set; }
    }

    public static class PropertyPatternSample
    {
        public static bool IsUsBasedWithUkManager(object o)
        {
            return o is Employee e && 
                   e is { Region: "US", ReportsTo: { Region: "UK" } };
        }

    }
}
