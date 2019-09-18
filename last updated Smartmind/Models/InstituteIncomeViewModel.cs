using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMUniversity.Models
{
    public class InstituteIncomeViewModel
    {
        // Lecturer details 
        public int ID { get; set; }
        public string InstituteName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        // session Details 

        public int SessionsCount { get; set; }
        public decimal TotalInstituteSessionPrice { get; set; }
        public decimal NetInstituteSessionPrice { get; set; }
        public decimal NetLecturerSessionPrice { get; set; }

        // Courses Details 

        public int CoursesCount { get; set; }
        public decimal TotalInstituteCoursePrice { get; set; }
        public decimal NetInstituteCoursePrice { get; set; }
        public decimal NetLecturerCoursePrice { get; set; }


        // Percentage Details 

        public int PercentageSessionCount { get; set; }
        public decimal TotalInstitutePercentagePrice { get; set; }
        public decimal NetInstitutePercentagePrice { get; set; }
        public decimal NetLecturerPercentagePrice { get; set; }


        public int FromLecturerSideSessionCount { get; set; }
        public decimal TotalInstituteFromLecturerSidePrice { get; set; }
        public decimal NetInstituteFromLecturerSidePrice { get; set; }
        public decimal NetFromLecturerSidePrice { get; set; }


        public decimal TotalInstituteIncome { get; set; }
        public decimal NetInstituteIncome { get; set; }
        public decimal NetLecturerIncome { get; set; }


    }
}