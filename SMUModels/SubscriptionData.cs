using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class SubscriptionData
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public bool SessionType { get; set; }
        public decimal SessionCost { get; set; }
        public string SubjectPicture { get; set; }
        public string SubjectNameAr { get; set; }
        public string SubjectNameEn { get; set; }
        public string HallCodeAr { get; set; }
        public string HallCodeEn { get; set; }
        public int LecturesCount { get; set; }
        public int Attendance { get; set; }
        public int Absence { get; set; }
        public int LecturesLeft { get; set; }
        public string StartDate { get; set; }
        public string Time { get; set; }
        public string LecturerName { get; set; }
        public string LecturerNameEn { get; set; }
        public string LecturerPic { get; set; }
        public string CollegeNameAr { get; set; }
        public string CollegeNameEn { get; set; }
        public string MajorNameAr { get; set; }
        public string MajorNameEn { get; set; }
        public bool Evaluated { get; set; }
        public int LecturerID { get; set; }
        public string SessionPicture { get; set; }
        public double LecturerRate { get; set; }
    }
}
