using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class SessionCourseData
    {
        public int ID { get; set; }
        public bool SessionType { get; set; }
        public int LecturesCount { get; set; }
        public string CollegeNameAr { get; set; }
        public string CollegeNameEn { get; set; }
        public string MajorNameAr { get; set; }
        public string MajorNameEn { get; set; }
        public object StartDate { get; set; }
        public object Time { get; set; }
        public string SubjectPicture { get; set; }
        public string SubjectNameAr { get; set; }
        public string SubjectNameEn { get; set; }
        public int LecturerID { get; set; }
        public string LecturerName { get; set; }
        public string LecturerNameEn { get; set; }
        public string LecturerPic { get; set; }
        public double LecturerRate { get; set; }
        public decimal SessionCost { get; set; }
        public string QRCode { get; set; }
        public bool IsCourse { get; set; }
        public string SessionCode { get; set; }
    }
}
