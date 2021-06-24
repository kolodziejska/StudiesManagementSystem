using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudiesManagementSystem.Models;

namespace StudiesManagementSystem
{
    public static partial class Uons
    {
        //TODO: SEARCH THROUGH STUDENT/FOS/FACULTY NAME, NOT ID
        public static void AddStudent(string firstName, string lastName, string birthdate)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var cultureInfo = new CultureInfo("pl-PL");

                var std = new Student()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Birthdate = DateTime.ParseExact(birthdate, "d", cultureInfo),
                    
                };

                uctx.Add<Student>(std);
                uctx.SaveChanges();

                Console.WriteLine("STUDENT ADDED");
            }
        }
        
        public static void AddStudent(string firstName, string lastName, string birthdate, string address,
            string city)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var cultureInfo = new CultureInfo("pl-PL");

                var std = new Student()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Birthdate = DateTime.ParseExact(birthdate, "d", cultureInfo),
                    Address = address,
                    City = city
                    
                };

                uctx.Add<Student>(std);
                uctx.SaveChanges();

                Console.WriteLine("STUDENT ADDED");
            }
        }

        public static void AddStudentToFos(int studentID, int fosID)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var studentfos = uctx.FosStudents.Where(s => s.StudentId == studentID && s.FosId==fosID).SingleOrDefault();

                if (studentfos != null)
                {
                    Console.WriteLine("STUDENT ALREADY ADDED");
                }
                else
                {
                    var fosstudent = new FosStudent()
                    {
                        FosId = fosID,
                        SemesterId = 1,
                        StudentStatus = 1,
                        StudentId = studentID
                    };

                    uctx.Add<FosStudent>(fosstudent);

                    var student = uctx.Students.Where(s => s.StudentId == studentID).SingleOrDefault();

                    if (student.Email == null)
                    {
                        student.Email = $"{student.StudentId}@student.edu.com";
                    }
                                        
                    uctx.SaveChanges();

                    Console.WriteLine("CHANGES APPLIED");
                }
            }
        }

        public static void AddStudentToClasses(int studentID)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var studentfos = uctx.FosStudents.Where(s => s.StudentId == studentID).ToList();

                foreach (var fos in studentfos)
                {
                    if (fos.StudentStatus == 1)
                    {
                        var fosclasses = uctx.Classes.Where(c => c.FosId == fos.FosId && c.SemesterId==fos.SemesterId).ToList();

                        foreach (var fosclass in fosclasses)
                        {
                            if (uctx.Grades.Where(g => g.StudentId == studentID && g.ClassId == fosclass.ClassId).SingleOrDefault() == null)
                            {
                                var grade = new Grade()
                                {
                                    StudentId = studentID,
                                    ClassId = fosclass.ClassId,
                                    GradeValue = null
                                };

                                uctx.Add<Grade>(grade);
                            }
                            else { Console.WriteLine($"CANNOT ADD STUDENT TO CLASS {fosclass.ClassName.ToUpper()} BECAUSE STUDENT ALREADY ADDED"); }
                        }
                    }
                    else
                    {
                        Console.WriteLine("CANNOT BE ADDED TO CLASSES BECAUSE STUDENT NOT ACTIVE");
                    }

                    uctx.SaveChanges();
                }
            }
        }

        public static void ChangeStudentGrade(int classID, int studentID, double gradeValue)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var grade = uctx.Grades.Where(g => g.ClassId == classID && g.StudentId == studentID).SingleOrDefault();

                if (grade != null)
                {
                    grade.GradeValue = gradeValue;

                    uctx.SaveChanges();
                }
                else { Console.WriteLine("WRONG CLASS ID OR STUDENT ID"); }
            }
        }

        public static void ChangeStudentStatus(int studentID, int FosID, int studentStatus)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var studentfos = uctx.FosStudents.Where(s => s.StudentId == studentID && s.FosId == FosID).SingleOrDefault();

                if (studentfos != null)
                {

                    studentfos.StudentStatus = studentStatus;

                    uctx.SaveChanges();
                }
                else { Console.WriteLine("WRONG FIELD OF STUDY ID OR STUDENT ID"); }
            }
        }

        //TODO: UPDATE SEMESTER

        public static string StudentStatusToString(int Studentstatus)
        {
            switch (Studentstatus)
            {
                case 0: return "candidate"; break;
                case 1: return "active"; break;
                case 2: return "inactive"; break;
                case 3: return "graduate"; break;
                default: return "undefined"; break;
            }
        }

    }
}
