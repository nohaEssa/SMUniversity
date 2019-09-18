using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class EmployeeObj
    {
        public int EmployeeID { get; set; } = 0;
        public string NameAr { get; set; } = "";
        public string NameEn { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public int BranchID { get; set; } = 0;
        public string ProfilePic { get; set; } = "";
    }
}
