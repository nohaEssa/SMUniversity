using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class LecturerData
    {
        public int LecturerID { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePic { get; set; }
        public string BranchNameAr { get; set; }
        public string BranchNameEn { get; set; }
        public int UserType { get; set; }
        //public string AccessToken { get; set; }
    }

    public class LecturerDataDDL
    {
        public int LecturerID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
