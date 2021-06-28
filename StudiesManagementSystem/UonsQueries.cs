using Microsoft.EntityFrameworkCore;
using StudiesManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudiesManagementSystem
{
    public class UonsQueries
    {

        public Student GetStudent(int studentId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.Students.Where(s => s.StudentId == studentId).SingleOrDefault();
            }
        }

        public List<FosStudent> GetFosStudentList(int studentId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.FosStudents.Where(s => s.StudentId == studentId)
                                 .Include(f => f.Fos)
                                 .Include(f => f.Semester)
                                 .ToList();
            }
        }

        public List<FosStudent> GetActiveFosStudentList(int studentId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.FosStudents.Where(s => s.StudentId == studentId && s.StudentStatus==1)
                                 .Include(f => f.Fos)
                                 .Include(f => f.Semester)
                                 .ToList();
            }
        }

        public FosStudent GetActiveFosStudent(int studentId, int fosId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.FosStudents.Where(s => s.StudentId == studentId && s.StudentStatus == 1 && s.FosId==fosId)
                                 .Include(f => f.Fos)
                                 .Include(f => f.Semester)
                                 .SingleOrDefault();
            }
        }

        public string GetClassName(int classId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.Classes.Where(c => c.ClassId == classId).Select(c => c.ClassName).FirstOrDefault();
            }
        }

        public Class GetClassInfo(int classId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.Classes.Where(c => c.ClassId == classId).SingleOrDefault();
            }
        }

        public List<Grade> GetStudentsFromClass(int classId)
        {
            var classInfo = GetClassInfo(classId);
            
            using (var uctx = new UniversityOfNowhereContext())
            {
                var students = uctx.FosStudents.Where(s => s.StudentStatus == 1 && s.FosId == classInfo.FosId && s.SemesterId == classInfo.SemesterId)
                                .Join(uctx.Grades, fosstudent => fosstudent.StudentId, grade => grade.StudentId, (fosstudent, grade) => grade)
                                .Where(c=>c.ClassId==classId)
                                .Include(g => g.Student)
                                .OrderBy(g => g.Student.LastName)
                                .ToList();

                return students;
            }
        }

        public string GetFosName(int fosId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.FieldOfStudies.Where(p => p.FosId == fosId).Select(p => p.FosName).FirstOrDefault();
            }
        }

        public string GetSemesterName(int semesterId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.Semesters.Where(p => p.SemesterId == semesterId).Select(p => p.SemesterName).FirstOrDefault();
            }
        }

        public List<FosStudent> GetStudentsFromFosSemester (int fosId, int semesterId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.FosStudents
                    .Where(f => f.FosId == fosId && f.SemesterId == semesterId && f.StudentStatus==1)
                    .Include(f => f.Student)
                    .OrderBy(f => f.Student.LastName).ToList();
            }
        }

        public List<Grade> GetAllClassesOfStudent (int studentId)
        {
            var studentfosList = GetActiveFosStudentList(studentId);

            var classesList = new List<Grade>();

            using (var uctx = new UniversityOfNowhereContext())
            {
                foreach (var studentfos in studentfosList)
                {
                    var fosclassesList = uctx.Grades
                                             .Where(g => g.StudentId == studentId&&g.Class.FosId==studentfos.FosId&&g.Class.SemesterId==studentfos.SemesterId)
                                             .Include(g => g.Class)
                                             .ThenInclude(c => c.Fos)
                                             .ToList();
                    classesList.AddRange(fosclassesList);
                }
                return classesList;
            }
        }

        public List<Grade> GetAllClassesOfStudent (int studentId, int fosId)
        {
            try
            {
                var studentfos = GetActiveFosStudent(studentId, fosId);

                using (var uctx = new UniversityOfNowhereContext())
                {
                    return uctx.Grades.Where(g => g.StudentId == studentId && g.Class.FosId == studentfos.FosId && g.Class.SemesterId == studentfos.SemesterId)
                                      .Include(g => g.Class)
                                      .ThenInclude(c => c.Fos)
                                      .ToList();
                }
            }
            catch (Exception nre)
            {
                Console.WriteLine("ERROR! Index doesn't exist!");//TODO: THROW EXCEPTION?
                return null;
            }
        }

        public List<Professor> GetAllProfsInOrder()
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.Professors.Include(p => p.Fos).ThenInclude(f => f.Faculty)
                                    .AsEnumerable()
                                    .OrderBy(p => p.Fos.FacultyId).ThenBy(p => p.LastName)
                                    .ToList();
            }
        }

        public List<Professor> GetAllProfsFromFaculty(int facultyId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.Professors
                                    .Include(p => p.Fos)
                                    .Where(p => p.Fos.FacultyId == facultyId)
                                    .OrderBy(p => p.LastName)
                                    .ToList();
            }
        }

        public string GetFacultyName(int facultyId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.Faculties.Where(p => p.FacultyId == facultyId).Select(p => p.FacultyName).FirstOrDefault();
            }
        }

        public List<Class> GetClassesOfProfessor(int profId)
        {
            using (var uctx = new UniversityOfNowhereContext()){
                return uctx.Classes
                                .Where(c => c.ProfId == profId)
                                .Include(c => c.Fos)
                                .Include(c => c.Semester)
                                .ToList();
            }
        }

        public Professor GetProfInfo(int profId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.Professors.Where(p => p.ProfId == profId).FirstOrDefault();
            }
        }

        public double? GetAverageGradeOfClass(int classId)
        {
            var studentsList = GetStudentsFromClass(classId);

            double? averageGrade = studentsList.Average(s => s.GradeValue);

            return averageGrade;
           
        }

        public double? GetAverageGradeOfStudent(int studentId, int fosId)
        {      
            var gradesList = GetAllClassesOfStudent(studentId, fosId);

            if (gradesList != null) { 
            double? averageGrade = gradesList.Average(r => r.GradeValue);

               return averageGrade;
            }
            else
            {  //TODO: SHOULD IT THROW EXCEPTION
               return null;
            }
        }

        public List<KeyValuePair<Student, double?>> GetAverageStudentGradesFromFaculty(int facultyId)
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                var studentsList = uctx.FosStudents
                           .Where(s => s.Fos.FacultyId == facultyId && s.StudentStatus==1)
                           .Include(s => s.Student)
                           .ToList();

                var studentsAverageList = new List<KeyValuePair<Student, double?>>();

                foreach (var student in studentsList)
                {
                    double? averageGrade = GetAverageGradeOfStudent(student.StudentId, student.FosId);
                    studentsAverageList.Add(new KeyValuePair <Student, double?>(student.Student, averageGrade));
                }

                return studentsAverageList;
            }
        }

        public IEnumerable<KeyValuePair<Student,double?>> GetHighestAverageFromFaculty(int facultyId)
        {
            var studentsList = GetAverageStudentGradesFromFaculty(facultyId);
            
            return studentsList.Where(s => s.Value == studentsList.Max(s => s.Value)).Select(s => s).AsEnumerable();
        }

        public List<Grade> GetHighestGradesInClass (int classId)
        {
            var studentsList = GetStudentsFromClass(classId);

            return studentsList.Where(s => s.GradeValue == studentsList.Max(g => g.GradeValue)).ToList();
        }

        public List<FieldOfStudy> GetAllFieldOfStudies()
        {
            using (var uctx = new UniversityOfNowhereContext())
            {
                return uctx.FieldOfStudies
                        .Include(f => f.Faculty).ToList();
            }
        }

    }
}
