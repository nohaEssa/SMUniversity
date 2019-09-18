using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class HallData
    {
        public int HallID { get; set; } = 0;
        public string HallCodeAr { get; set; } = "";
        public string HallCodeEn { get; set; } = "";
        public int Capacity { get; set; } = 0;
        public string BranchNameAr { get; set; } = "";
        public string BranchNameEn { get; set; } = "";
        public int UserType { get; set; } = 0;
        public string AccessToken { get; set; } = "";
        public bool Available { get; set; } = false;
    }
}
