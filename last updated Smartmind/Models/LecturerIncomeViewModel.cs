using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMUniversity.Models
{
    public class LecturerIncomeViewModel
    {

        // Lecturer details 
        public int ID { get; set; }
        public string LecturerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        // session Details 

        public int SessionsCount { get; set; }
        public decimal LecturerPricePerSession { get; set; }
        public decimal TotalLecturerSessionPrice { get; set; }
        // Courses Details 

        public int CoursesCount { get; set; }
        public decimal LecturerPricePerCourse { get; set; }
        public decimal TotalLecturerCoursePrice { get; set; }

        // Percentage Details 

        public int PercentageSessionCount { get; set; }
        public decimal PercentageSessionSumPrice { get; set; }
        public decimal LecturerPercentagePerSession{ get; set; }
        public decimal TotalLecturerPercentagePrice { get; set; }

        // FromLecturerSide Details 

        public int FromLecturerSideSessionCount { get; set; }
        public decimal FromLecturerSidePerSession { get; set; }
        public decimal TotalFromLecturerSidePrice { get; set; }


        public decimal TotalLecturerIncome { get; set; }

    }
}