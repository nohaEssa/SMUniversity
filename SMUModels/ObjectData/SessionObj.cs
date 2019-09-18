using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class SessionObj
    {
        public int SessionID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string FromDate { get; set; } = "";
        public string ToDate { get; set; } = "";
        public string[] CourseFromTime { get; set; }
        public string[] CourseToTime { get; set; }
        public int Type { get; set; } = 0;
        public int Cost { get; set; } = 0;
        public int Price1 { get; set; } = 0;
        public int Price2 { get; set; } = 0;
        public int Price3 { get; set; } = 0;
        public int SessionPrice { get; set; } = 0;
        public int UniversityID { get; set; } = 0;
        public int CollegeID { get; set; } = 0;
        public int MajorID { get; set; } = 0;
        public int SubjectID { get; set; } = 0;
        public int LecturerID { get; set; } = 0;
        public int BranchID { get; set; } = 0;
        public int LecturesCount { get; set; } = 0;
        public int HallID { get; set; } = 0;
        public byte LecturerAccountMethod { get; set; } = 0;
        public string NameEn { get; set; } = "";
        public string DescriptionEn { get; set; } = "";
        public bool GeneralSession { get; set; } = false;
    }
}
