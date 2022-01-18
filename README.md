## Studies Management System
>An application connected to the relational database, allowing managing university students, teachers, classes and more.

### General Information
The application uses O/RM and was created through Database First approach - it connects to a local database that consists of roughly 
1500 students, 170 professors, 1000 classes and 5 faculties. Most data was generated randomly through SQL functions and procedures
as its main purpose is to test application against it.

The model consists of: Student, Professor, Faculty, Field of Study, Class, Grade, Semester and, of course, context which application uses
to connect, update and write to database.

The project includes unit tests for some methods; another methods were attempted through TDD. Although *all* tests need to be rewritten
for better approach (see *Project status and direction for improvements* section).

### Features
- add new students and update existing data
- add a student to all classes based on their field of study and year
- display student info and the classes they're enrolled into
- display all students from class or field of study and semester
- display teachers info and the classes they teach
- display average grade of class or a student
- display highest average grade from faculty
- display all students with an average grade higher than average grade of faculty
- display highest grade from class
- display all grades from class (as a chart)

### Technologies and dependencies
- .NET Core v5.0.10
- Entity Framework Core v5.0.7
- Microsoft SQL Server v15.0
- xunit v2.4.1

___

### Project status and direction for improvements
This project is in progress. As it is a console application for now, the next steps would be:
- using it as a base for an ASP.Net Core MVC application
- implementing login system and GUI
- writing decent unit tests that do not run on an actual database
