using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class SessionObj
    {
        public int SessionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string[] CourseFromTime { get; set; }
        public string[] CourseToTime { get; set; }
        public int Type { get; set; }
        public int Cost { get; set; }
        public int Price1 { get; set; }
        public int Price2 { get; set; }
        public int Price3 { get; set; }
        public int UniversityID { get; set; }
        public int CollegeID { get; set; }
        public int MajorID { get; set; }
        public int SubjectID { get; set; }
        public int LecturerID { get; set; }
        public int BranchID { get; set; }
        public int LecturesCount { get; set; }
        public int HallID { get; set; }
        public byte LecturerAccountMethod { get; set; }
        public string NameEn { get; set; }
        public string DescriptionEn { get; set; }

    }
}
