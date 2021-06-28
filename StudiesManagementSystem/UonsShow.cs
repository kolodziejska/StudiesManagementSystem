using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudiesManagementSystem.Models;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace StudiesManagementSystem
{
    public static partial class Uons
    {

        private static UonsQueries _queries = new UonsQueries();
        // QUERIES

        //TODO: SEARCH THROUGH STUDENT/FOS/FACULTY NAME, NOT ID

        /*
         * student queries */
        
        
        public static void ShowStudentOfId(int studentId)
        {          
            var student = _queries.GetStudent(studentId);

            var allfos = _queries.GetFosStudentList(studentId);

            Console.WriteLine(student.ToString());

            //TODO: format string
            foreach (var fos in allfos) 
            {
                Console.WriteLine($"{fos.Fos.FosName} {fos.Semester.SemesterName}, student status: {Uons.StudentStatusToString(fos.StudentStatus)}");
            }
            
        }

        
        public static void ShowStudentsFromClass(int classId)
        {
            var studentsList = _queries.GetStudentsFromClass(classId);
            string className = _queries.GetClassName(classId);
            
            Console.WriteLine(className.ToUpper());

            foreach (var student in studentsList)
            {
               Console.WriteLine($"{student.Student.FirstName} {student.Student.LastName}, grade: {student.GradeValue}");
            }
            
        }

        
        public static void ShowStudentsFromFOSSemester(int fosId, int semesterId)
        {
            var students = _queries.GetStudentsFromFosSemester(fosId, semesterId);

            string fosName = _queries.GetFosName(fosId);
            string semesterInfo = _queries.GetSemesterName(semesterId);

            Console.WriteLine($"{fosName.ToUpper()}, {semesterInfo.ToUpper()}");

            foreach (var student in students)
            {
                if (student.StudentStatus == 1)
                {
                    Console.WriteLine(student.Student.ToString());
                }
            } 
        }

        public static void ShowAllClassesOfStudent(int studentId) //TODO: GROUP BY FOS, ShowAllClassesOfStudent(int studentId, int fosId)
        {
            var classes = _queries.GetAllClassesOfStudent(studentId);

            foreach (var clas in classes)
            {
                    Console.WriteLine($"{clas.ClassId} {clas.Class.ClassName} {clas.GradeValue} {clas.Class.Fos.FosName}");
            }
        }


        /*
         * professor queries */
        
        public static void ShowAllProfs()
        {
           var allProfsGrouped = _queries.GetAllProfsInOrder()
                                         .GroupBy(p => p.Fos.Faculty.FacultyName);
                               
           foreach (var faculty in allProfsGrouped)
           {
               Console.WriteLine(faculty.Key.ToUpper());

               foreach (var prof in faculty)
               {
                   Console.WriteLine($"{prof.FirstName} {prof.LastName}, {prof.AcademicDegree}, {prof.Fos.Faculty.FacultyName}, {prof.Fos.FosName}");
                   Console.WriteLine($"\t email: {prof.Email}, phone number: {prof.PhoneNumber}, address: {prof.Address}, {prof.City}");
               }

               Console.WriteLine();

           }
        
        }
        
        
        public static void ShowProfsFromFaculty(int facultyId)
        {
            var allProfs = _queries.GetAllProfsFromFaculty(facultyId);

            string facultyName = _queries.GetFacultyName(facultyId);

            Console.WriteLine(facultyName.ToUpper());

            foreach (var prof in allProfs)
            {
                Console.WriteLine($"{prof.FirstName} {prof.LastName}, {prof.AcademicDegree}, {prof.Fos.FosName}");
                Console.WriteLine($"\t\t email: {prof.Email}, phone number: {prof.PhoneNumber}, address: {prof.Address}, {prof.City}");
            }
        }
        
        
        public static void ShowClassesFromProf(int profId)
        {
            var Classes = _queries.GetClassesOfProfessor(profId);

            var professorInfo = _queries.GetProfInfo(profId);

            Console.WriteLine("Professor: " + professorInfo.FirstName + " " + professorInfo.LastName + ", " + professorInfo.AcademicDegree);
            Console.WriteLine();

            foreach (var clas in Classes)
            {
                Console.WriteLine(clas.ClassName + " || " + clas.Fos.FosName + " || " + clas.Semester.SemesterName);
            }
        }


        /*
         * grades queries */
        
        public static void ShowAverageGradeOfClass(int classId)
        {
            double? averageGrade = _queries.GetAverageGradeOfClass(classId);

            string className = _queries.GetClassName(classId);

            Console.WriteLine($"{className.ToUpper()}, AVERAGE GRADE: {string.Format("{0:F2}", averageGrade)}");
            
        }
        
        public static void ShowAverageGradeOfStudent(int studentId, int fosId)
        {
            double? averageGrade = _queries.GetAverageGradeOfStudent(studentId, fosId);

            var student = _queries.GetStudent(studentId);

            Console.WriteLine($"{student.FirstName.ToUpper()} {student.LastName.ToUpper()}, AVERAGE GRADE: {string.Format("{0:F2}", averageGrade)}");
        }
        
        public static void ShowHighestAverageFaculty(int facultyId)
        {
            var bestStudents = _queries.GetHighestAverageFromFaculty(facultyId);
            
            string facultyName = _queries.GetFacultyName(facultyId);

            Console.WriteLine($"HIGHEST AVERAGE GRADE FROM {facultyName.ToUpper()}");

            foreach (var student in bestStudents)
            {
                Console.WriteLine($"{student.Key.FirstName} {student.Key.LastName} {string.Format("{0:F2}", student.Value)}");
            }
        }
        
        public static void ShowHighestAverageFaculty(int facultyId, int NumberOfEntries)
        {
            var students = _queries.GetAverageStudentGradesFromFaculty(facultyId)
                           .OrderByDescending(s=>s.Value)
                           .Take(NumberOfEntries);

            string facultyName = _queries.GetFacultyName(facultyId);

            Console.WriteLine($"{NumberOfEntries} HIGHEST AVERAGE GRADES FROM {facultyName.ToUpper()}");

            foreach (var student in students)
            {
                Console.WriteLine($"{student.Key.FirstName} {student.Key.LastName} {string.Format("{0:F2}", student.Value)}");
            }
        }
        
        public static void ShowAboveAverageFaculty(int facultyId, double average) //TODO: UNIT TEST
        {
            var students = _queries.GetAverageStudentGradesFromFaculty(facultyId)
                            .Where(s=>s.Value>average);

            string facultyName = _queries.GetFacultyName(facultyId);

            Console.WriteLine($"AVERAGE GRADES ABOVE {average} FROM {facultyName.ToUpper()}");

            foreach (var student in students)
            {
                Console.WriteLine($"{student.Key.FirstName} {student.Key.LastName} {string.Format("{0:F2}", student.Value)}");
            }
        }
        
        public static void ShowHighestGradeFromClass(int classId) //TODO: UNIT TEST
        {
            var bestStudents = _queries.GetHighestGradesInClass(classId);

            string className = _queries.GetClassName(classId);

            Console.WriteLine($"HIGHEST GRADES IN CLASS {className.ToUpper()}");

            foreach (var student in bestStudents)
            {
                Console.WriteLine($"{student.Student.FirstName} {student.Student.LastName} {student.GradeValue}");
            }
        }
        
        public static void ShowGradesFromClass(int classId) //TODO: UNIT TEST
        {
            var grades = _queries.GetStudentsFromClass(classId)
                                 .GroupBy(s=>s.GradeValue)
                                 .OrderByDescending(g=>g.Key);


            string className = _queries.GetClassName(classId);

            Console.WriteLine($"GRADES FROM CLASS {className.ToUpper()}");

            foreach (var grade in grades)
            {
                if (grade.Key != null)
                { Console.Write(string.Format("{0:F1}", grade.Key)); }
                else { Console.Write("0.0"); }

                foreach (var student in grade)
                {
                    Console.Write("|");
                }

                Console.Write("\n");
            }
        }

        /*
         * other queries */

        public static void ShowAllFacultiesAllFOS()
        {
            var allFos = _queries.GetAllFieldOfStudies()
                                     .GroupBy(s => s.Faculty.FacultyName).ToList();
            
            foreach (var faculty in allFos)
            {
                Console.WriteLine(faculty.Key.ToUpper());

                foreach (var fos in faculty)
                {
                    Console.WriteLine(fos.FosName);
                }

                Console.WriteLine();
            }
            
        }

               
    }
}
