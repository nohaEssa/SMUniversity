using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class HallRentObj
    {
        public int LecturerID { get; set; } = 0;
        public int HallID { get; set; } = 0;
        public object FromDate { get; set; } = "";
        public object ToDate { get; set; } = "";
    }
}
