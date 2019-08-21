using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class EmployeeData
    {
        public int EmployeeID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string BranchNameAr { get; set; }
        public string BranchNameEn { get; set; }
        public string ProfilePic { get; set; }
        public int UserCategoryID { get; set; }
        public int UserType { get; set; }
        //public string AccessToken { get; set; }
    }
}
