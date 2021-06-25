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
        // QUERIES

        //TODO: SEARCH THROUGH STUDENT/FOS/FACULTY NAME, NOT ID

        /*
         * student queries */
        
        
        public static void ShowStudentOfId(int StudentId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var student = uctx.Students.Where(s => s.StudentId == StudentId).SingleOrDefault();

                var allfos = uctx.FosStudents.Where(s=>s.StudentId==StudentId)   
                                 .Include(f=>f.Fos)
                                 .Include(f=>f.Semester)
                                 .ToList();

                Console.WriteLine(student.ToString());

                //TODO: format string
                foreach (var fos in allfos) {

                    Console.WriteLine($"{fos.Fos.FosName} {fos.Semester.SemesterName}, student status: {Uons.StudentStatusToString(fos.StudentStatus)}");
                }
            }
        }

        
        public static void ShowStudentsFromClass(int ClassId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var students = from grade in uctx.Grades
                               join student in uctx.Students on grade.StudentId equals student.StudentId
                               where grade.ClassId == ClassId
                               orderby student.LastName
                               select new
                               {
                                   studentName = student.FirstName,
                                   studentSurname = student.LastName,
                                   gradeValue = grade.GradeValue
                               };

                string className = uctx.Classes.Where(c=>c.ClassId==ClassId).Select(c=>c.ClassName).FirstOrDefault();

                var studentsList = students.ToList();

                Console.WriteLine(className.ToUpper());

                foreach (var student in studentsList)
                {
                    Console.WriteLine($"{student.studentName} {student.studentSurname}, grade: {student.gradeValue}");
                }
            }
        }

        
        public static void ShowStudentsFromFOSSemester(int fosId, int semesterId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var students = uctx.FosStudents
                    .Where(f => f.FosId == fosId && f.SemesterId == semesterId)
                    .Select(f => new { student = f.Student, studentstatus=f.StudentStatus })
                    .OrderBy(f=>f.student.LastName);
                        
                string fosName = uctx.FieldOfStudies.Where(p => p.FosId == fosId).Select(p => p.FosName).FirstOrDefault();
                string semesterInfo = uctx.Semesters.Where(p => p.SemesterId == semesterId).Select(p => p.SemesterName).FirstOrDefault();

                Console.WriteLine($"{fosName.ToUpper()}, {semesterInfo.ToUpper()}");

                foreach (var student in students)
                {
                    if (student.studentstatus == 1)
                    {
                        Console.WriteLine(student.student.ToString());
                    }
                }
            }
        }

        public static void ShowAllClassesOfStudent(int studentID)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var classes = uctx.Grades.Where(g => g.StudentId == studentID)
                    .Include(g => g.Class)
                    .ThenInclude(c=>c.Fos)
                    .ToList();

                foreach (var clas in classes)
                {
                    Console.WriteLine($"{clas.ClassId} {clas.Class.ClassName} {clas.GradeValue} {clas.Class.Fos.FosName}");
                }

            }
        }


        /*
         * professor queries */
        
        public static void ShowAllProfs()
        {

            using (var uctx = new UniversityOfNowhereContext())
            {
                var allProfs = uctx.Professors.Include(p => p.Fos).ThenInclude(f => f.Faculty)
                                .AsEnumerable()
                                .OrderBy(p => p.Fos.FacultyId).ThenBy(p => p.LastName)
                                .GroupBy(p => p.Fos.Faculty.FacultyName);
                                
                          
                foreach (var faculty in allProfs)
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
        }
        
        
        public static void ShowProfsFromFaculty(int facultyID)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var allProfs = uctx.Professors
                                .Include(p => p.Fos)
                                .Where(p => p.Fos.FacultyId == facultyID)
                                .OrderBy(p => p.LastName)
                                .ToList();
                                
                string facultyName = uctx.Faculties.Where(p=>p.FacultyId==facultyID).Select(p=>p.FacultyName).FirstOrDefault();

                Console.WriteLine(facultyName.ToUpper());

                foreach (var prof in allProfs)
                {
                    Console.WriteLine($"{prof.FirstName} {prof.LastName}, {prof.AcademicDegree}, {prof.Fos.FosName}");
                    Console.WriteLine($"\t\t email: {prof.Email}, phone number: {prof.PhoneNumber}, address: {prof.Address}, {prof.City}");
                }
            }
        }
        
        
        public static void ShowClassesFromProf(int profId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var Classes = uctx.Classes
                                .Where(c => c.ProfId == profId)
                                .Include(c => c.Fos)
                                .Include(c => c.Semester)
                                .ToList();
                
                var professorInfo = uctx.Professors.Where(p => p.ProfId==profId).Select(p => new { p.FirstName, p.LastName, p.AcademicDegree }).FirstOrDefault();

                Console.WriteLine("Professor: " + professorInfo.FirstName + " " + professorInfo.LastName + ", " + professorInfo.AcademicDegree);
                Console.WriteLine();

                foreach (var clas in Classes)
                {
                    Console.WriteLine(clas.ClassName + " || " + clas.Fos.FosName + " || " + clas.Semester.SemesterName);
                }
            }

        }


        /*
         * grades queries */
        
        public static void GetAverageGradeOfClass(int classId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                double? averageGrade = uctx.Grades.Where(r => r.ClassId == classId).Average(r => r.GradeValue);

                string className = uctx.Classes.Where(c => c.ClassId == classId).Select(c => c.ClassName).SingleOrDefault();

                Console.WriteLine($"{className.ToUpper()}, AVERAGE GRADE: {string.Format("{0:F2}", averageGrade)}");
            }
        }
        
        public static double? GetAverageGradeOfStudent(int studentId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                try
                {
                    double? averageGrade = uctx.Grades.Where(r => r.StudentId == studentId).Average(r => r.GradeValue);

                    var studentName = uctx.Students.Where(c => c.StudentId == studentId).Select(c => new { c.FirstName, c.LastName }).SingleOrDefault();

                    Console.WriteLine($"{studentName.FirstName.ToUpper()} {studentName.LastName.ToUpper()}, AVERAGE GRADE: {string.Format("{0:F2}", averageGrade)}");

                    return averageGrade;
                }
                catch (System.NullReferenceException nre)
                {
                    Console.WriteLine("ERROR! Index doesn't exist!");
                    return null;
                }
            }

        }
        
        public static void ShowHighestAverageFaculty(int facultyId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var studentlist = uctx.FosStudents
                                    .Where(s => s.Fos.FacultyId == facultyId)
                                    .Include(s=>s.Student)
                                    .Select(s => new { s, average = s.Student.Grades.Average(g => g.GradeValue) })
                                    .ToList();
                                    

                var bestStudents = studentlist.Where(s => s.average == studentlist.Max(s => s.average)).Select(s => s);
                            

                string facultyName = uctx.Faculties.Where(f => f.FacultyId == facultyId).Select(f=>f.FacultyName).SingleOrDefault();

                Console.WriteLine($"HIGHEST AVERAGE GRADE FROM {facultyName.ToUpper()}");

                foreach (var student in bestStudents)
                {
                    Console.WriteLine($"{student.s.Student.FirstName} {student.s.Student.LastName} {string.Format("{0:F2}", student.average)}");
                }
      
            }
        }
        
        public static void ShowHighestAverageFaculty(int facultyId, int NumberOfEntries)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var students = uctx.FosStudents
                                .Where(s => s.Fos.FacultyId == facultyId)
                                .Select(s => new { student = s.Student, average = s.Student.Grades.Average(g => g.GradeValue) })
                                .OrderByDescending(s => s.average)
                                .Take(NumberOfEntries);

                string facultyName = uctx.Faculties.Where(f => f.FacultyId == facultyId).Select(f => f.FacultyName).Single();

                Console.WriteLine($"{NumberOfEntries} HIGHEST AVERAGE GRADES FROM {facultyName.ToUpper()}");

                foreach (var student in students)
                {
                    Console.WriteLine($"{student.student.FirstName} {student.student.LastName} {string.Format("{0:F2}", student.average)}");
                }

            }
        }
        
        public static void ShowAboveAverageFaculty(int facultyId, double average)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var students = uctx.FosStudents
                            .Where(s => s.Fos.FacultyId == facultyId)
                            .Select(s => new { student=s.Student, average = s.Student.Grades.Average(g => g.GradeValue) })
                            .Where(s=>s.average>average)
                            .OrderByDescending(s => s.average);

                string facultyName = uctx.Faculties.Where(f => f.FacultyId == facultyId).Select(f => f.FacultyName).Single();

                Console.WriteLine($"AVERAGE GRADES ABOVE {average} FROM {facultyName.ToUpper()}");

                foreach (var student in students)
                {
                    Console.WriteLine($"{student.student.FirstName} {student.student.LastName} {string.Format("{0:F2}", student.average)}");
                }

            }
        }
        
        public static void ShowHighestGradeFromClass(int classId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var students = uctx.Grades.Where(s => s.ClassId == classId)
                                .Include(s=>s.Student)
                                .ToList();
                    
                var beststudents = students.Where(s => s.GradeValue == students.Max(g => g.GradeValue));

                string className = uctx.Classes.Where(s => s.ClassId == classId).Select(c => c.ClassName).Single();

                Console.WriteLine($"HIGHEST GRADES IN CLASS {className.ToUpper()}");

                foreach (var student in beststudents)
                {
                    Console.WriteLine($"{student.Student.FirstName} {student.Student.LastName} {student.GradeValue}");
                }
                             
            }
        }
        
        public static void ShowGradesFromClass(int classId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var grades = uctx.Grades.Where(s => s.ClassId == classId)
                    .AsEnumerable()
                    .GroupBy(s=>s.GradeValue)
                    .OrderByDescending(g=>g.Key);


                string className = uctx.Classes.Where(s => s.ClassId == classId).Select(c => c.ClassName).Single();

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
        }

        /*
         * other queries */

        public static void ShowAllFacultiesAllFOS()
        {
            using(var uctx = new UniversityOfNowhereContext())
            {
                var allFos = uctx.FieldOfStudies
                    .Include(f=>f.Faculty)
                    .AsEnumerable().GroupBy(s => s.Faculty.FacultyName).ToList();


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
}
