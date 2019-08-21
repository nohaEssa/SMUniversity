using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class StudentData
    {
        public int StudentID { get; set; }
        public int? UniversityID { get; set; }
        public int? CollegeID { get; set; }
        public int? MajorID { get; set; }
        public int? AreaID { get; set; }
        public int? GovernorateID { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Balance { get; set; }
        public string UniversityNameAr { get; set; }
        public string UniversityNameEn { get; set; }
        public string CollegeNameAr { get; set; }
        public string CollegeNameEn { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string GovernorateNameAr { get; set; }
        public string GovernorateNameEn { get; set; }
        public string AreaNameAr { get; set; }
        public string AreaNameEn { get; set; }
        public string BranchNameAr { get; set; }
        public string BranchNameEn { get; set; }
        public int UserType { get; set; }
        public bool Verified { get; set; }
        public string ProfilePic { get; set; }
        public string MajorNameAr { get; set; }
        public string MajorNameEn { get; set; }
        //public string AccessToken { get; set; }

    }
}
