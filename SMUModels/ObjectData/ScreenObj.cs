using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class ScreenObj
    {
        public int ScreenID { get; set; } = 0;
        public string NameAr { get; set; } = "";
        public string NameEn { get; set; } = "";
        public string[] HallIDs { get; set; }
        public int BranchID { get; set; } = 0;
    }
}
