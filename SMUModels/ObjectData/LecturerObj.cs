using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class LecturerObj
    {
        public int LecturerID { get; set; }
        public string FirstNameAr { get; set; }
        public string FirstNameEn { get; set; }
        public string SecondNameAr { get; set; }
        public string SecondNameEn { get; set; }
        public string ThirdNameAr { get; set; }
        public string ThirdNameEn { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; }
        public int BranchID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte LecturerAccountMethod { get; set; }
        public int HourCost { get; set; }
        public int LectPercentage { get; set; }
        public int CoursePrice { get; set; }
        public int StudentPercentage { get; set; }
    }

    public class LecturerPasswordObj
    {
        public int LecturerID { get; set; } = 0;
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
    }

}
