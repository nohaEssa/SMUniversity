using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class TimelineSessionData
    {
        public int ID { get; set; } = 0;
        public string Code { get; set; } = "";
        public string CollegeNameAr { get; set; } = "";
        public string CollegeNameEn { get; set; } = "";
        public string MajorNameAr { get; set; } = "";
        public string MajorNameEn { get; set; } = "";
        public string HallCodeAr { get; set; } = "";
        public string HallCodeEn { get; set; } = "";
        public bool SessionType { get; set; } = false;
        public decimal SessionCost { get; set; } = 0;
        public string SubjectPicture { get; set; } = "";
        public string SubjectNameAr { get; set; } = "";
        public string SubjectNameEn { get; set; } = "";
        public string HallCode { get; set; } = "";
        public int LecturesCount { get; set; } = 0;
        public string StartDate { get; set; } = "";
        public string Time { get; set; } = "";
        public string LecturerName { get; set; } = "";
        public string LecturerNameEn { get; set; } = "";
        public string LecturerPic { get; set; } = "";
        public double LecturerRate { get; set; } = 0;
        public string SessionDescription { get; set; } = "";
        public bool Favourite { get; set; } = false;
        public bool Subscribed { get; set; } = false;
        public int LecturerID { get; set; } = 0;
        public string SessionPicture { get; set; } = "";
        public string SessionNameAr { get; set; } = "";
        public string SessionNameEn { get; set; } = "";
        public bool GeneralSession { get; set; } = false;
        public bool Evaluated { get; set; } = false;
        public string DescriptionAr { get; set; } = "";
        public string DescriptionEn { get; set; } = "";
        public int SessionID { get; set; } = 0;
    }
}
