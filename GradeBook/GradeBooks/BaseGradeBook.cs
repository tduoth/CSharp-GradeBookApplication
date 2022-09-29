using System;
using System.Linq;
using GradeBook.Enums;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

 namespace GradeBook.GradeBooks
 {
     public class BaseGradeBook
     public abstract class BaseGradeBook
     {
         public string Name { get; set; }
         public List<Student> Students { get; set; }
         public GradeBookType Type { get; set; }
         public bool IsWeighted { get; set; }

         public BaseGradeBook(string name)
         public BaseGradeBook(string name, bool isWeighted)
         {
             Name = name;
             Students = new List<Student>();
             IsWeighted = isWeighted;
         }

         public void AddStudent(Student student)
 @@ -106,18 +109,27 @@ public void Save()

         public virtual double GetGPA(char letterGrade, StudentType studentType)
         {
             var gpa = 0;
             switch (letterGrade)
             {
                 case 'A':
                     return 4;
                     gpa = 4;
                     break;
                 case 'B':
                     return 3;
                     gpa = 3;
                     break;
                 case 'C':
                     return 2;
                     gpa = 2;
                     break;
                 case 'D':
                     return 1;
                     gpa = 1;
                     break;
             }
             return 0;

             if (IsWeighted && (studentType == StudentType.Honors || studentType == StudentType.DualEnrolled))
                 gpa++;

             return gpa;
         }

         public virtual void CalculateStatistics()
 @@ -129,7 +141,7 @@ public virtual void CalculateStatistics()
             var internationalPoints = 0d;
             var standardPoints = 0d;
             var honorPoints = 0d;
             var duelEnrolledPoints = 0d;
             var dualEnrolledPoints = 0d;

             foreach (var student in Students)
             {
 @@ -163,8 +175,8 @@ public virtual void CalculateStatistics()
                     case StudentType.Honors:
                         honorPoints += student.AverageGrade;
                         break;
                     case StudentType.DuelEnrolled:
                         duelEnrolledPoints += student.AverageGrade;
                     case StudentType.DualEnrolled:
                         dualEnrolledPoints += student.AverageGrade;
                         break;
                 }
             }
 @@ -183,8 +195,8 @@ public virtual void CalculateStatistics()
                 Console.WriteLine("Average for students excluding honors and duel enrollment is " + (standardPoints / Students.Where(e => e.Type == StudentType.Standard).Count()));
             if (honorPoints != 0)
                 Console.WriteLine("Average for only honors students is " + (honorPoints / Students.Where(e => e.Type == StudentType.Honors).Count()));
             if (duelEnrolledPoints != 0)
                 Console.WriteLine("Average for only duel enrolled students is " + (duelEnrolledPoints / Students.Where(e => e.Type == StudentType.DuelEnrolled).Count()));
             if (dualEnrolledPoints != 0)
                 Console.WriteLine("Average for only duel enrolled students is " + (dualEnrolledPoints / Students.Where(e => e.Type == StudentType.DualEnrolled).Count()));
         }

         public virtual void CalculateStudentStatistics(string name)
        {
            var student = Students.FirstOrDefault(e => e.Name == name);
            student.LetterGrade = GetLetterGrade(student.AverageGrade);
            student.GPA = GetGPA(student.LetterGrade, student.Type);
            Console.WriteLine("{0} ({1}:{2}) GPA: {3}.", student.Name, student.LetterGrade, student.AverageGrade, student.GPA);
            Console.WriteLine();
            Console.WriteLine("Grades:");
            foreach (var grade in student.Grades)
            {
                Console.WriteLine(grade);
            }
        }
        public virtual char GetLetterGrade(double averageGrade)
        {
            if (averageGrade >= 90)
                return 'A';
            else if (averageGrade >= 80)
                return 'B';
            else if (averageGrade >= 70)
                return 'C';
            else if (averageGrade >= 60)
                return 'D';
            else
                return 'F';
        }
        /// <summary>
        ///     Converts json to the appropriate grade book type.
        ///     Note: This method contains code that is not recommended practice.
        ///     This has been used as a compromise to avoid adding additional complexity to the learner.
        /// </summary>
        /// <returns>The to grade book.</returns>
        /// <param name="json">Json.</param>
        public static dynamic ConvertToGradeBook(string json)
        {
            // Get GradeBookType from the GradeBook.Enums namespace
            var gradebookEnum = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                 from type in assembly.GetTypes()
                                 where type.FullName == "GradeBook.Enums.GradeBookType"
                                 select type).FirstOrDefault();
            var jobject = JsonConvert.DeserializeObject<JObject>(json);
            var gradeBookType = jobject.Property("Type")?.Value?.ToString();
            // Check if StandardGradeBook exists
            if ((from assembly in AppDomain.CurrentDomain.GetAssemblies()
                 from type in assembly.GetTypes()
                 where type.FullName == "GradeBook.GradeBooks.StandardGradeBook"
                 select type).FirstOrDefault() == null)
                gradeBookType = "Base";
            else
            {
                if (string.IsNullOrEmpty(gradeBookType))
                    gradeBookType = "Standard";
                else
                    gradeBookType = Enum.GetName(gradebookEnum, int.Parse(gradeBookType));
            }
            // Get GradeBook from the GradeBook.GradeBooks namespace
            var gradebook = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                             from type in assembly.GetTypes()
                             where type.FullName == "GradeBook.GradeBooks." + gradeBookType + "GradeBook"
                             select type).FirstOrDefault();
            //protection code
            if (gradebook == null)
                gradebook = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                             from type in assembly.GetTypes()
                             where type.FullName == "GradeBook.GradeBooks.StandardGradeBook"
                             select type).FirstOrDefault();
            
            return JsonConvert.DeserializeObject(json, gradebook);
        }
    }
}
