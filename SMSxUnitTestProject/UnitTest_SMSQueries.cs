using System;
using Xunit;
using StudiesManagementSystem;
using System.IO;

namespace SMSxUnitTestProject
{
    public class UnitTest_SMSQueries
    {
        
        [Fact]
        public void Test_ShowStudentOfId_OneFos()
        {
            string expected = "Rollins Gianni birthdate: 1992-03-10 Address:   Email: 1@student.edu.com\r\n" +
                "Mechanical Engineering year 3 semester 1, student status: active";

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Uons.ShowStudentOfId(1);

                var result = sw.ToString().Trim();
                Assert.Equal(expected, result);
            }

            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            Console.SetOut(standardOutput);
        }

        [Fact]
        public void Test_ShowStudentOfId_MoreThanOneFos()
        {
            string expected = "Wiêcek Karolina birthdate: 2002-01-20 Address:   Email: 1504@student.edu.com\r\n" +
                "Civil Engineering year 1 semester 1, student status: active\r\n" +
                "Systems Engineering year 2 semester 2, student status: inactive";

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Uons.ShowStudentOfId(1504);

                var result = sw.ToString().Trim();
                Assert.Equal(expected, result);
            }

            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            Console.SetOut(standardOutput);
        }


        [Fact]
        public void Test_ShowAverageGradeOfClass()
        {
            string grade = string.Format("{0:F2}", 4.07);
            string expected = $"INTRODUCTION TO DESIGN 1, AVERAGE GRADE: {grade}";

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Uons.ShowAverageGradeOfClass(1);

                var result = sw.ToString().Trim();
                Assert.Equal(expected, result);
            }

            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            Console.SetOut(standardOutput);
        }
        
        [Fact]
        public void Test_ShowHighestAverageFaculty()
        {
            string expected = $"HIGHEST AVERAGE GRADE FROM FACULTY OF ARCHITECTURE\r\n" +
                $"Lennon Morris 4,88";

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Uons.ShowHighestAverageFaculty(1);

                var result = sw.ToString().Trim();
                Assert.Equal(expected, result);
            }

            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            Console.SetOut(standardOutput);
        }

        [Fact]
        public void Test_ShowHighestAverageFaculty_Top3()
        {
            string grade = string.Format("{0:F2}", 4.88);
            string expected = $"3 HIGHEST AVERAGE GRADES FROM FACULTY OF ARCHITECTURE\r\n" +
                $"Lennon Morris 4,88\r\n" +
                $"Irene Carey 4,83\r\n" +
                $"Georgia Bryant 4,75";

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Uons.ShowHighestAverageFaculty(1,3);

                var result = sw.ToString().Trim();
                Assert.Equal(expected, result);
            }

            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            Console.SetOut(standardOutput);
        }
        


    }
}

