using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class TodaysSessionData
    {
        public decimal CurrentBalance { get; set; } = 0;
        public decimal TotalBalance { get; set; } = 0;
        public List<TodaysSessions> TodaysSessions { get; set; }
    }

    public class TodaysSessions
    {
        public int ID { get; set; } = 0;
        public string Code { get; set; } = "";
        public string SubjectNameAr { get; set; } = "";
        public string SubjectNameEn { get; set; } = "";
        public string SessionNameAr { get; set; } = "";
        public string SessionNameEn { get; set; } = "";
        public bool SessionType { get; set; } = false;
        public string HallCodeAr { get; set; } = "";
        public string HallCodeEn { get; set; } = "";
        public string Time { get; set; } = "";
        public int StudentCount { get; set; } = 0;
        public string DescriptionAr { get; set; } = "";
        public string DescriptionEn { get; set; } = "";
        public bool GeneralSession { get; set; } = false;
        public string SubjectPicture { get; set; } = "";
        public decimal SessionCost { get; set; } = 0;
    }
}
