using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class SaveAttendanceObj
    {
        public int SessionTimesID { get; set; } = 0;
        public List<StudentAttendanceData> Students { get; set; }
    }

    public class StudentAttendanceData
    {
        public int SubscriptionID { get; set; } = 0;
        public int StudentID { get; set; } = 0;
        public int Attend { get; set; } = 0;//1 attend 0 not attend
    }
}