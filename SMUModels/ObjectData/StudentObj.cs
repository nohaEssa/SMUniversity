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
        public int StudentID { get; set; } = 0;
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string SecondName { get; set; } = "";
        public string ThirdName { get; set; } = "";
        public string FirstNameEn { get; set; } = "";
        public string SecondNameEn { get; set; } = "";
        public string ThirdNameEn { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; } = false;
        public int? UniversityID { get; set; } = 0;
        public int? CollegeID { get; set; } = 0;
        public int? MajorID { get; set; } = 0;
        public int GovernorateID { get; set; } = 0;
        public int AreaID { get; set; } = 0;
        public int BranchID { get; set; } = 0;
        public bool StudentType { get; set; } = false;
        public int CardCategoryID { get; set; } = 0;
        public string Token { get; set; } = "";
        public decimal NewChargeValue { get; set; } = 0;
        public int DeviceTypeID { get; set; } = 0;
    }

    public class NameAndDOBObj
    {
        public int StudentID { get; set; } = 0;
        public string FirstName { get; set; } = "";
        public string FirstNameEn { get; set; } = "";
        public string SecondName { get; set; } = "";
        public string SecondNameEn { get; set; } = "";
        public string ThirdName { get; set; } = "";
        public string ThirdNameEn { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
    }

    public class StageOfEduObj
    {
        public int StudentID { get; set; } = 0;
        public int UniversityID { get; set; } = 0;
        public int CollegeID { get; set; } = 0;
        public int MajorID { get; set; } = 0;
        public int GovernorateID { get; set; } = 0;
        public int AreaID { get; set; } = 0;
        public int BranchID { get; set; } = 0;
    }

    public class PhoneNumberObj
    {
        public int StudentID { get; set; } = 0;
        public string PhoneNumber { get; set; } = "";
    }

    public class EmailObj
    {
        public int StudentID { get; set; } = 0;
        public string Email { get; set; } = "";
    }

    public class PasswordObj
    {
        public int StudentID { get; set; } = 0;
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
    }

    public class GenderAndPicObj
    {
        public int StudentID { get; set; } = 0;
        public bool Gender { get; set; } = false;
        public string ProfilePic { get; set; } = "";
        public byte[] Image { get; set; }
    }

    public class UpdateEmail
    {
        public string Email { get; set; } = "";
    }

}
