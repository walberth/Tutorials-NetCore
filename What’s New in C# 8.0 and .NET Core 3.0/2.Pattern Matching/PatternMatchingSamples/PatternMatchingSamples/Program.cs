using System;

namespace PatternMatchingSamples
{
    class Program
    {
        static void Main(string[] args)
        {
            DoPositionalPatternDemo();
            DoPropertyPatternDemo();
            DoSwitchExpressionDemo();
            DoTupleExpressionDemo();
        }

        static void DoTupleExpressionDemo()
        {
            Console.WriteLine($"{TuplePatternSample.GetColor(Color.Blue, Color.Red)}");
            Console.WriteLine($"{TuplePatternSample.GetColor(Color.Blue, Color.Blue)}");
        }

        static void DoSwitchExpressionDemo()
        {
            var shape = new Rectangle(5, 5);

            Console.WriteLine(SwitchExpressionSample.DisplayShapeInfo(shape));
        }

        static void DoPropertyPatternDemo()
        {
            var ukManager = new Employee
            {
                FirstName = "David",
                LastName = "Smith",
                Region = "UK",
                Type = "Global Director"
            };

            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Region = "US",
                ReportsTo = ukManager,
                Type = "Director"
            };

            var result = PropertyPatternSample.IsUsBasedWithUkManager(employee);

            Console.WriteLine($"{employee.FirstName} is US-based with UK-manager? {result}");
        }

        static void DoPositionalPatternDemo()
        {
            var mathTeacher = new Teacher("Alex", "Smith", "Math");
            var student = new Student("John", "Doe", mathTeacher, 6);

            var result = PositionalPatternSample.IsInSeventhGradeMath(student);

            Console.WriteLine($"{student.FirstName} is in 7th grade math? {result}");
        }
    }
}
