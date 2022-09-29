using System;
 namespace GradeBook.GradeBooks
 {//test
     public class StandardGradeBook : BaseGradeBook
     {
         public StandardGradeBook(string name, bool isWeighted) : base(name, isWeighted)
         {
             Type = Enums.GradeBookType.Standard;
         }
     }
 }
