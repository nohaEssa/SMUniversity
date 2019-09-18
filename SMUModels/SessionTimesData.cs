using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class SessionTimesData
    {
        public int ID { get; set; } = 0;
        public string LectureAr { get; set; } = "";
        public string LectureEn { get; set; } = "";
        public string Date { get; set; } = "";
        public string Time { get; set; } = "";
        public int Attend { get; set; } = 0;
        public int AttendanceCount { get; set; } = 0;
        public int AbsenceCount { get; set; } = 0;
    }
}
