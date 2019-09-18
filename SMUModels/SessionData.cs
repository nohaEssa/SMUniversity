using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class SessionData
    {
        public int SessionID { get; set; } = 0;
        public bool Type { get; set; } = false;
        public string SubjectNameAr { get; set; } = "";
        public string SubjectNameEn { get; set; } = "";
        public string Lecturer { get; set; } = "";
        public string LecturerNameEn { get; set; } = "";
        public string SubjectPicture { get; set; } = "";
        public decimal Cost { get; set; } = 0;
        public string TitleAr { get; set; } = "";
        public string TitleEn { get; set; } = "";
        public string ProductCostMsgAr { get; set; } = "";
        public string ProductCostMsgEn { get; set; } = "";
        public string SessionNameAr { get; set; } = "";
        public string SessionNameEn { get; set; } = "";
        public bool GeneralSession { get; set; } = false;
    }
}
