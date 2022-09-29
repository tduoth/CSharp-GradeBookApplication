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
 public void Save()

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
public virtual void CalculateStatistics()
             var internationalPoints = 0d;
             var standardPoints = 0d;
             var honorPoints = 0d;
             var duelEnrolledPoints = 0d;
             var dualEnrolledPoints = 0d;

             foreach (var student in Students)
             {
public virtual void CalculateStatistics()
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
public virtual void CalculateStatistics()
                 Console.WriteLine("Average for students excluding honors and duel enrollment is " + (standardPoints / Students.Where(e => e.Type == StudentType.Standard).Count()));
             if (honorPoints != 0)
                 Console.WriteLine("Average for only honors students is " + (honorPoints / Students.Where(e => e.Type == StudentType.Honors).Count()));
             if (duelEnrolledPoints != 0)
                 Console.WriteLine("Average for only duel enrolled students is " + (duelEnrolledPoints / Students.Where(e => e.Type == StudentType.DuelEnrolled).Count()));
             if (dualEnrolledPoints != 0)
                 Console.WriteLine("Average for only duel enrolled students is " + (dualEnrolledPoints / Students.Where(e => e.Type == StudentType.DualEnrolled).Count()));
         }

         public virtual void CalculateStudentStatistics(string name)
