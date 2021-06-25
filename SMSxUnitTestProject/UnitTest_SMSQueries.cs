using System;
using Xunit;
using StudiesManagementSystem;
using System.IO;

namespace SMSxUnitTestProject
{
    public class UnitTest_SMSQueries
    {
           
        private string? expected = null;

        [Fact]
        public void Test_GetAverageGradeOfStudent()
        {
            double expected = 4.00;
            double res = (double)Uons.GetAverageGradeOfStudent(1);
            Assert.Equal(expected, res);
        }

        [Fact]
        public void Test_GetAverageGradeOfStudent_IndexOutOfBounds()
        {
            int studentId = 10000;
            double? res = Uons.GetAverageGradeOfStudent(studentId);
            Assert.Null(res);

        }

        
        [Fact]
        public void Test_GetAverageGradeOfClass()
        {
            string grade = string.Format("{0:F2}", 4.07);
            expected = $"INTRODUCTION TO DESIGN 1, AVERAGE GRADE: {grade}";

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Uons.GetAverageGradeOfClass(1);

                var result = sw.ToString().Trim();
                Assert.Equal(expected, result);
            }

            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            Console.SetOut(standardOutput);
        }
    }
}

