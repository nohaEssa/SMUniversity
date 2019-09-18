using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class SessionStudentData
    {
        public List<string> AttendedStudents { get; set; }
        //public List<string> StudentsInAttendance { get; set; }
        public List<AttendanceStudentData> StudentsData { get; set; }
    }

    public class AttendanceStudentData
    {
        public int StudentID { get; set; } = 0;
        public int SubscriptionID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string NameEn { get; set; } = "";
        public string ProfilePic { get; set; } = "";
        public string UniversityNameAr { get; set; } = "";
        public string UniversityNameEn { get; set; } = "";
        public int Attend { get; set; } = 0;
        public bool SubscripedAsSession { get; set; } = false;
    }
}
