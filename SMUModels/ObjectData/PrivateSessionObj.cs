using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class PrivateSessionObj
    {
        public int StudentID { get; set; } = 0;
        public int SubjectID { get; set; } = 0;
        public int LecturerID { get; set; } = 0;
        public string RequestMsg { get; set; } = "";
        public DateTime? RequestDate { get; set; }
        public bool IsCourse { get; set; } = false;
    }

    public class PrivateSessionsList
    {
        public int ID { get; set; } = 0;
        public int StudentID { get; set; } = 0;
        public int SubjectID { get; set; } = 0;
        public int LecturerID { get; set; } = 0;
        public string StudentName { get; set; } = "";
        public string LecturerNameAr { get; set; } = "";
        public string LecturerNameEn { get; set; } = "";
        public string SubjectNameAr { get; set; } = "";
        public string SubjectNameEn { get; set; } = "";
        public string RequestMsg { get; set; } = "";
        public bool? Approved { get; set; } = false;
        public bool IsCourse { get; set; } = false;
        public string RequestDate { get; set; } = "";
    }
}
