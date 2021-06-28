using System;
using Xunit;
using StudiesManagementSystem;
using StudiesManagementSystem.Models;
using System.IO;
using System.Globalization;
using FluentAssertions;
using System.Collections.Generic;

namespace SMSxUnitTestProject
{
    public class UnitTest_SMSGet
    {

        [Fact]
        public void Test_GetStudent()
        {
            var queries = new UonsQueries();
            var actualStudent = queries.GetStudent(1);
            var cultureInfo = new CultureInfo("pl-PL");

            var expectedStudent = new Student
            {
                StudentId = 1,
                LastName = "Rollins",
                FirstName = "Gianni",
                Birthdate = DateTime.ParseExact("1992-03-10", "d", cultureInfo),
                Address = null,
                City = null,
                Email = "1@student.edu.com"
            };

            expectedStudent.Should().BeEquivalentTo(actualStudent);

        }

        [Fact]
        public void Test_GetFosStudentList_OneFos()
        {
            var queries = new UonsQueries();
            var actualList = queries.GetFosStudentList(1);

            var expectedList = new List<FosStudent>
            {
                new FosStudent { StudentStatus=1, FosId=12, SemesterId=5, StudentId=1,
                Fos = new FieldOfStudy{FosId=12, FosName="Mechanical Engineering", FacultyId=5, FosStudents=null },
                Semester = new Semester {SemesterId=5, SemesterName="year 3 semester 1", FosStudents=null } }
            };

            expectedList.Should().BeEquivalentTo(actualList, options => options.Excluding(o=>o.Fos.FosStudents).Excluding(o=>o.Semester.FosStudents));
        }

        [Fact]
        public void Test_GetFosStudentList_MoreThanOneFos()
        {
            var queries = new UonsQueries();
            var actualList = queries.GetFosStudentList(1504);

            var expectedList = new List<FosStudent>
            {
                new FosStudent { StudentStatus=1, FosId=3, SemesterId=1, StudentId=1504,
                Fos = new FieldOfStudy{FosId=3, FosName="Civil Engineering", FacultyId=2, FosStudents=null },
                Semester = new Semester {SemesterId=1, SemesterName="year 1 semester 1", FosStudents=null } },
                new FosStudent {StudentStatus=3, FosId=10, SemesterId=4, StudentId=1504,
                Fos = new FieldOfStudy{FosId=10, FosName="Systems Engineering", FacultyId=4, FosStudents=null },
                Semester = new Semester{ SemesterId=4, SemesterName="year 2 semester 2", FosStudents=null} }
            };

            expectedList.Should().BeEquivalentTo(actualList, options => options.Excluding(o => o.Fos.FosStudents).Excluding(o => o.Semester.FosStudents));
        }

        [Fact]
        public void Test_GetClassName()
        {
            string expected = "Introduction to Design 1";
            var queries = new UonsQueries();

            string actual = queries.GetClassName(1);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Test_GetStudentsFromClass()
        {
            var queries = new UonsQueries();
            var actualList = queries.GetStudentsFromClass(1);

            var expectedList = new List<Grade>
            { new Grade {StudentId=764, ClassId=1, GradeValue=3, Student = new Student {LastName="Carey", FirstName="Iliana" } },
              new Grade {StudentId=269, ClassId=1, GradeValue=5, Student = new Student{LastName="Delacruz", FirstName="Paola" } },
              new Grade {StudentId=1035, ClassId=1, GradeValue=3, Student = new Student{LastName="Hamilton", FirstName="Bailey" } },
              new Grade {StudentId=898, ClassId=1, GradeValue=null, Student = new Student{LastName="Knox", FirstName="Ares" } },
              new Grade {StudentId=1502, ClassId=1, GradeValue=null, Student = new Student{LastName="Kowalska", FirstName="Anna" } },
              new Grade {StudentId=364, ClassId=1, GradeValue=4.5, Student = new Student{LastName="Landry", FirstName="Maeve" } },
              new Grade {StudentId=661, ClassId=1, GradeValue=5, Student = new Student{LastName="Matthews", FirstName="Ayla" } },
              new Grade {StudentId=392, ClassId=1, GradeValue=3, Student = new Student{LastName="Mcguire", FirstName="Jaxon" } },
              new Grade {StudentId=637, ClassId=1, GradeValue=null, Student = new Student{LastName="Sharp", FirstName="Ann" } },
              new Grade {StudentId=358, ClassId=1, GradeValue=null, Student = new Student{LastName="Trujillo", FirstName="Kathryn" } },
              new Grade {StudentId=1029, ClassId=1, GradeValue=5, Student = new Student{LastName="Warner", FirstName="Kylo" } }
            };

            actualList.Should().BeEquivalentTo(expectedList, 
                options=>options.Excluding(o=>o.Student.StudentId).Excluding(o=>o.Student.Birthdate)
                .Excluding(o=>o.Student.Email).Excluding(o=>o.Student.Grades)
                .Excluding(o=>o.Student.Address).Excluding(o=>o.Student.City));
        }

       [Fact]
        public void Test_GetAverageGradeOfClass() //TODO: WHEN AVERAGE IS NULL
        {
            var queries = new UonsQueries();
            double expectedGrade = 4.07;
                        
            double? actualGrade = queries.GetAverageGradeOfClass(1);

            double actualGradeRounded = Math.Round((double)actualGrade, 2);
                       
            Assert.Equal(expectedGrade, actualGradeRounded);

        }
        
        [Fact]
        public void Test_GetAverageGradeOfStudent()
        {
            var queries = new UonsQueries();
            double expected = 4.00;
            double res = (double)queries.GetAverageGradeOfStudent(1,12);
            Assert.Equal(expected, res);
        }
        
        [Theory]
        [InlineData(10000,12)]
        [InlineData(1,1)]
        public void Test_GetAverageGradeOfStudent_IndexOutOfBounds(int studentId, int fosId)
        {
            var queries = new UonsQueries();            
            double? res = queries.GetAverageGradeOfStudent(studentId, fosId);
            Assert.Null(res); //TODO: SHOULD THROW EXCEPTION INSTEAD?

        }
        
        [Fact]
        public void Test_GetStudentsAverageFromFaculty()
        {
            var cultureInfo = new CultureInfo("pl-PL");
            var queries = new UonsQueries();
            var randomStudent = new KeyValuePair<Student, double?>(new Student
            {
                StudentId = 4,
                LastName = "Yates",
                FirstName = "Kendra",
                Birthdate = DateTime.ParseExact("2000-08-23", "d", cultureInfo),
                Address = null,
                City = null,
                Email = "4@student.edu.com"
            }, 4.2);

            var StudentsList = queries.GetAverageStudentGradesFromFaculty(1);

            //not sure why the comparision fails
            StudentsList.Should().ContainEquivalentOf(randomStudent);           
        }


        [Fact]
        public void Test_GetHighestAverageFromFaculty()
        {
            var queries = new UonsQueries();
            double expectedAverage = 4.88;

            var bestStudents = queries.GetHighestAverageFromFaculty(1);

            foreach(var student in bestStudents)
            {
                double actualAverage = Math.Round((double)student.Value, 2);
                Assert.Equal(expectedAverage, actualAverage);
            }
        }
    }
}

