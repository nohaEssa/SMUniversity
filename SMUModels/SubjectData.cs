using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class SubjectData
    {
        public int SubjectID { get; set; } = 0;
        public string NameAr { get; set; } = "";
        public string NameEn { get; set; } = "";
        public int MajorID { get; set; } = 0;
        public bool GeneralSubject { get; set; } = false;
        public string SubjectCode { get; set; } = "";

    }
}
