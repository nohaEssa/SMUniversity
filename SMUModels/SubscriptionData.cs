using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class SubscriptionData
    {
        public int ID { get; set; } = 0;
        public int SessionID { get; set; } = 0;
        public string Code { get; set; } = "";
        public bool SessionType { get; set; } = false;
        public decimal SessionCost { get; set; } = 0;
        public string SubjectPicture { get; set; } = "";
        public string SubjectNameAr { get; set; } = "";
        public string SubjectNameEn { get; set; } = "";
        public string HallCodeAr { get; set; } = "";
        public string HallCodeEn { get; set; } = "";
        public int LecturesCount { get; set; } = 0;
        public int Attendance { get; set; } = 0;
        public int Absence { get; set; } = 0;
        public int LecturesLeft { get; set; } = 0;
        public string StartDate { get; set; } = "";
        public string Time { get; set; } = "";
        public string LecturerName { get; set; } = "";
        public string LecturerNameEn { get; set; } = "";
        public string LecturerPic { get; set; } = "";
        public string CollegeNameAr { get; set; } = "";
        public string CollegeNameEn { get; set; } = "";
        public string MajorNameAr { get; set; } = "";
        public string MajorNameEn { get; set; } = "";
        public bool Evaluated { get; set; } = false;
        public int LecturerID { get; set; } = 0;
        public string SessionPicture { get; set; } = "";
        public double LecturerRate { get; set; } = 0;
        public string DescriptionAr { get; set; } = "";
        public string DescriptionEn { get; set; } = "";
        public string SessionNameAr { get; set; } = "";
        public string SessionNameEn { get; set; } = "";
        public bool GeneralSession { get; set; } = false;
        public bool Favourite { get; set; } = false;
        public bool Subscribed { get; set; } = false;
        public bool Rated { get; set; } = false;
    }
}
