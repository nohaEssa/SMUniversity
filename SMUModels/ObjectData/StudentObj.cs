using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class StudentObj
    {
        public int StudentID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string PhoneNumber { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public int? UniversityID { get; set; }
        public int? CollegeID { get; set; }
        public int? MajorID { get; set; }
        public int GovernorateID { get; set; }
        public int AreaID { get; set; }
        public int BranchID { get; set; }
        public bool StudentType { get; set; }
        public int CardCategoryID { get; set; }
    }

    public class NameAndDOBObj
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class StageOfEduObj
    {
        public int StudentID { get; set; }
        public int UniversityID { get; set; }
        public int CollegeID { get; set; }
        public int MajorID { get; set; }
        public int GovernorateID { get; set; }
        public int AreaID { get; set; }
        public int BranchID { get; set; }
    }

    public class PhoneNumberObj
    {
        public int StudentID { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class EmailObj
    {
        public int StudentID { get; set; }
        public string Email { get; set; }
    }

    public class PasswordObj
    {
        public int StudentID { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class GenderAndPicObj
    {
        public int StudentID { get; set; }
        public bool Gender { get; set; }
        public string ProfilePic { get; set; }
    }

    public class UpdateEmail
    {
        public string Email { get; set; }
    }

}
