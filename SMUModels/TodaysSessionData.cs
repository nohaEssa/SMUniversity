using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class TodaysSessionData
    {
        public decimal CurrentBalance { get; set; }
        public decimal TotalBalance { get; set; }
        public List<TodaysSessions> TodaysSessions { get; set; }
        
    }

    public class TodaysSessions
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string SubjectNameAr { get; set; }
        public string SubjectNameEn { get; set; }
        public bool SessionType { get; set; }
        public string HallCodeAr { get; set; }
        public string HallCodeEn { get; set; }
        public string Time { get; set; }
        public int StudentCount { get; set; }
    }
}
