using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class StudentData
    {
        public int StudentID { get; set; } = 0;
        public int? UniversityID { get; set; } = 0;
        public int? CollegeID { get; set; } = 0;
        public int? MajorID { get; set; } = 0;
        public int? AreaID { get; set; } = 0;
        public int? GovernorateID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string FirstNameAr { get; set; } = "";
        public string FirstNameEn { get; set; } = "";
        public string SecondNameAr { get; set; } = "";
        public string SecondNameEn { get; set; } = "";
        public string ThirdNameAr { get; set; } = "";
        public string ThirdNameEn { get; set; } = "";
        public bool Gender { get; set; } = false;
        public string PhoneNumber { get; set; } = "";
        public decimal Balance { get; set; } = 0;
        public string UniversityNameAr { get; set; } = "";
        public string UniversityNameEn { get; set; } = "";
        public string CollegeNameAr { get; set; } = "";
        public string CollegeNameEn { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = "";
        public string GovernorateNameAr { get; set; } = "";
        public string GovernorateNameEn { get; set; } = "";
        public string AreaNameAr { get; set; } = "";
        public string AreaNameEn { get; set; } = "";
        public string BranchNameAr { get; set; } = "";
        public string BranchNameEn { get; set; } = "";
        public int UserType { get; set; } = 0;
        public bool Verified { get; set; } = false;
        public string ProfilePic { get; set; } = "";
        public string MajorNameAr { get; set; } = "";
        public string MajorNameEn { get; set; } = "";
        public bool? StudentType { get; set; } = false;
        public int FavoritesCount { get; set; } = 0;
        public int SubscriptionsCount { get; set; } = 0;
        public string VerificationCode { get; set; } = "";
        //public string AccessToken { get; set; }

    }
}
