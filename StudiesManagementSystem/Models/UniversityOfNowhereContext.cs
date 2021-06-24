using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace StudiesManagementSystem.Models
{
    public partial class UniversityOfNowhereContext : DbContext
    {
        public UniversityOfNowhereContext()
        {
        }

        public UniversityOfNowhereContext(DbContextOptions<UniversityOfNowhereContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<FieldOfStudy> FieldOfStudies { get; set; }
        public virtual DbSet<FosStudent> FosStudents { get; set; }
        public virtual DbSet<GetRand> GetRands { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Professor> Professors { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=UniversityOfNowhere;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.ClassId).HasColumnName("Class_ID");

                entity.Property(e => e.ClassName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Class_name");

                entity.Property(e => e.FosId).HasColumnName("FOS_ID");

                entity.Property(e => e.ProfId).HasColumnName("Prof_ID");

                entity.Property(e => e.SemesterId).HasColumnName("Semester_ID");

                entity.HasOne(d => d.Fos)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.FosId)
                    .HasConstraintName("FK__Classes__FOS_ID__37A5467C");

                entity.HasOne(d => d.Prof)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.ProfId)
                    .HasConstraintName("FK__Classes__Prof_ID__36B12243");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK__Classes__Semeste__38996AB5");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("Faculty");

                entity.Property(e => e.FacultyId).HasColumnName("faculty_id");

                entity.Property(e => e.FacultyName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("faculty_name");
            });

            modelBuilder.Entity<FieldOfStudy>(entity =>
            {
                entity.HasKey(e => e.FosId)
                    .HasName("PK__FieldOfS__F7ABFFA1929FE1AC");

                entity.ToTable("FieldOfStudy");

                entity.Property(e => e.FosId).HasColumnName("FOS_id");

                entity.Property(e => e.FacultyId).HasColumnName("Faculty_id");

                entity.Property(e => e.FosName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FOS_name");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.FieldOfStudies)
                    .HasForeignKey(d => d.FacultyId)
                    .HasConstraintName("FK__FieldOfSt__Facul__4222D4EF");
            });

            modelBuilder.Entity<FosStudent>(entity =>
            {
                entity.HasKey(e => new { e.FosId, e.StudentId })
                    .HasName("PK_FosStudent");

                entity.Property(e => e.FosId).HasColumnName("FOS_ID");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.SemesterId).HasColumnName("Semester_ID");

                entity.Property(e => e.StudentStatus).HasColumnName("student_status");

                entity.HasOne(d => d.Fos)
                    .WithMany(p => p.FosStudents)
                    .HasForeignKey(d => d.FosId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FosStuden__FOS_I__628FA481");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.FosStudents)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK__FosStuden__Semes__6383C8BA");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.FosStudents)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FosStuden__stude__619B8048");
            });

            modelBuilder.Entity<GetRand>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Get_RAND");

                entity.Property(e => e.MyRand).HasColumnName("MyRAND");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.ClassId });

                entity.Property(e => e.StudentId).HasColumnName("Student_ID");

                entity.Property(e => e.ClassId).HasColumnName("Class_ID");

                entity.Property(e => e.GradeValue).HasColumnName("Grade_value");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Grades__Class_ID__5629CD9C");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Grades__Student___5535A963");
            });

            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.ProfId)
                    .HasName("PK__Professo__A46610C526268391");

                entity.Property(e => e.ProfId).HasColumnName("Prof_ID");

                entity.Property(e => e.AcademicDegree)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Academic_degree");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("First_name");

                entity.Property(e => e.FosId).HasColumnName("FOS_ID");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Last_name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Phone_number");

                entity.HasOne(d => d.Fos)
                    .WithMany(p => p.Professors)
                    .HasForeignKey(d => d.FosId)
                    .HasConstraintName("FK__Professor__FOS_I__33D4B598");
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.Property(e => e.SemesterId).HasColumnName("semester_id");

                entity.Property(e => e.SemesterName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("semester_name");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Birthdate).HasColumnType("date");

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("First_name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Last_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
