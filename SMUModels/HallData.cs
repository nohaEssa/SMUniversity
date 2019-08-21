using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class HallData
    {
        public int HallID { get; set; }
        public string HallCodeAr { get; set; }
        public string HallCodeEn { get; set; }
        public int Capacity { get; set; }
        public string BranchNameAr { get; set; }
        public string BranchNameEn { get; set; }
        public int UserType { get; set; }
        public string AccessToken { get; set; }
    }
}
