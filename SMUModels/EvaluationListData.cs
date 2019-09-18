using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class EvaluationListData
    {
        public LecturerEvaluationData LecturerEvalData { get; set; }
        public List<StudentEvaluationData> StudentEvalData { get; set; }
    }
    public class LecturerEvaluationData
    {
        public string LecturerNameAr { get; set; } = "";
        public string LecturerNameEn { get; set; } = "";
        public string SubjectNameAr { get; set; } = "";
        public string SubjectNameEn { get; set; } = "";
        public int LecturerRate { get; set; } = 0;
        public string SessionNameAr { get; set; } = "";
        public string SessionNameEn { get; set; } = "";
        public string LecturerPicture { get; set; } = "";
    }
    public class StudentEvaluationData
    {
        public string StudentName { get; set; } = "";
        public string ProfilePic { get; set; } = "";
        public int StudentRate { get; set; } = 0;
        public string CollageAr { get; set; } = "";
        public string CollageEn { get; set; } = "";
        public string Evaluation { get; set; } = "";
        public string Time { get; set; } = "";
        public double Rate { get; set; } = 0;
    }
}
