using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class FinancialReportData
    {
        public string SessionNameAr { get; set; } = "";
        public string SessionNameEn { get; set; } = "";
        public bool SessionType { get; set; } = false;
        public string SessionTypeEn { get; set; } = "";
        public string SessionTypeAr { get; set; } = "";
        public int StudentsNumber { get; set; }
        public string HallCodeAr { get; set; } = "";
        public string HallCodeEn { get; set; } = "";
        public string Date { get; set; } = "";
        public decimal Price { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal InstituePercentage { get; set; } = 0;
        public decimal LecturerPercentage { get; set; } = 0;
    }

    //public class ReportSessionData
    //{
    //    public string SessionNameAr { get; set; }
    //    public string SessionNameEn { get; set; }
    //    public bool SessionType { get; set; }
    //    public int StudentsNumber { get; set; }
    //    public string HallCodeAr { get; set; }
    //    public string HallCodeEn { get; set; }
    //    public string Date { get; set; }
    //}
}
