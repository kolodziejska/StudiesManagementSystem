using System;
using Xunit;
using StudiesManagementSystem;
using System.IO;
using StudiesManagementSystem.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SMSxUnitTestProject
{
    public class UnitTest_SMSCUD
    {

        [Fact]
        public void Test_UpdateSemesterFosStudent()
        {   //TODO: MAKE THREE TESTS INSTEAD OF ONE
            // AND USE FAKE DB FOR ALL TESTS!!!!!!!!!!!!!!!!

            int studentId = 1503;
            int fosId = 1;

            using (var uctx = new UniversityOfNowhereContext())
            {
                var fosstudent = uctx.FosStudents.Where(s => s.StudentId == studentId).Single();
                Assert.Equal(1, fosstudent.SemesterId);
                Assert.Equal(fosId, fosstudent.FosId);
            }

            Uons.UpdateSemesterStudent(studentId);

            using (var uctx = new UniversityOfNowhereContext())
            {
                //test if student status changed to 2 on finished semester
                var fosstudent1 = uctx.FosStudents.Where(s => s.StudentId == studentId && s.SemesterId == 1)
                    .SingleOrDefault();
                Assert.Equal(2, fosstudent1.StudentStatus);
          
                //test if student moved to next semester
                var fosstudent2actual = uctx.FosStudents.Where(s => s.StudentId == studentId && s.SemesterId == 2).SingleOrDefault();

                var fosstudent2expected = new FosStudent
                {
                    StudentStatus = 1,
                    FosId = 1,
                    SemesterId = 2,
                    StudentId = studentId

                };

                Assert.Equal(fosstudent2expected.StudentStatus, fosstudent2actual.StudentStatus);
                Assert.Equal(fosstudent2expected.FosId, fosstudent2actual.FosId);
                Assert.Equal(fosstudent2expected.SemesterId, fosstudent2actual.SemesterId);
                Assert.Equal(fosstudent2expected.StudentId, fosstudent2actual.StudentId);


                //test if student was added to new classes - THIS FAILS BUT IDK WHY
                //TODO: TRY TO COMPARE ONLY CLASS ID?

                var gradesactual = uctx.Grades
                                    .Where(g => g.StudentId == studentId && g.Class.FosId == fosId
                                    && g.Class.SemesterId == 2).Select(g => g).ToList();

                List<Grade> gradesexpected = new List<Grade>() {
                new Grade { StudentId=studentId, ClassId=7, GradeValue=null },
                new Grade { StudentId=studentId, ClassId=8, GradeValue=null },
                new Grade { StudentId=studentId, ClassId=9, GradeValue=null },
                new Grade { StudentId=studentId, ClassId=10, GradeValue=null },
                new Grade { StudentId=studentId, ClassId=11, GradeValue=null },
                new Grade { StudentId=studentId, ClassId=12, GradeValue=null }
                };

                Assert.Equal(gradesexpected, gradesactual);

            }
            //TODO:
            //IF STUDENT HAS MORE THAN ONE FOS
            //IF STUDENT STATUS != 1
            //IF SEMESTERID=10 => STUDENTSTATUS=4
        }

    }

}

