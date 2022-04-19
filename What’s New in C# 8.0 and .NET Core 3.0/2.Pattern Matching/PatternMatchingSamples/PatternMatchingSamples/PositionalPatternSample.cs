using System;
using System.Collections.Generic;
using System.Text;

namespace PatternMatchingSamples
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Teacher HomeRoomTeacher { get; set; }
        public int GradeLevel { get; set; }

        public Student(string firstName, string lastName,
                       Teacher homeRoomTeacher, int gradeLevel)
        {
            FirstName = firstName;
            LastName = lastName;
            HomeRoomTeacher = homeRoomTeacher;
            GradeLevel = gradeLevel;
        }

        public void Deconstruct(out string firstName,
                                  out string lastName,
                                  out Teacher homeRoomTeacher,
                                  out int gradeLevel)
        {
            firstName = FirstName;
            lastName = LastName;
            homeRoomTeacher = HomeRoomTeacher;
            gradeLevel = GradeLevel;
        }

    }

    public class Teacher
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Subject { get; set; }

        public Teacher(string firstName, string lastName, string subject)
        {
            FirstName = firstName;
            LastName = lastName;
            Subject = subject;
        }

        public void Deconstruct(out string firstName,
                                  out string lastName,
                                  out string subject)
        {
            firstName = FirstName;
            lastName = LastName;
            subject = Subject;
        }

    }

    public static class PositionalPatternSample
    {
        public static bool IsInSeventhGradeMath(Student s)
        {
            return s is (_, _, (_, _, "Math"), 7);
        }

    }
}
