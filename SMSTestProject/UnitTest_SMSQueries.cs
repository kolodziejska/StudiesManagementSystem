using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StudiesManagementSystem;
using System.IO;

namespace SMSTestProject
{
    [TestClass]
    public class UnitTest_SMSQueries
    {
        private string? expected = null;

        [TestMethod]
        public void Test_GetAverageGradeOfStudent()
        {
            double actual = 4.00;
            double res = (double)Uons.GetAverageGradeOfStudent(1);
            Assert.AreEqual(res, actual, 0.001, "NOT EQUAL");
        }

        [TestMethod]
        public void Test_GetAverageGradeOfStudentIndexOutOfBounds()
        {
            double? res = Uons.GetAverageGradeOfStudent(10000);
            Assert.IsNull(res);
                  
        }

        
        [TestMethod]
        public void Test_GetAverageGradeOfClass()
        {
            string grade = string.Format("{0:F2}", 4.07);
            expected = $"INTRODUCTION TO DESIGN 1, AVERAGE GRADE: {grade}";

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                Uons.GetAverageGradeOfClass(1);                

                var result = sw.ToString().Trim();
                Assert.AreEqual(expected, result);
            }
        }

    }
}
