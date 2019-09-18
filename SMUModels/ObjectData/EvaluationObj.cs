using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class EvaluationObj
    {
        public List<int> QuestionIDs { get; set; }
        public List<int> AnswerIDs { get; set; }
        public int StudentID { get; set; } = 0;
        //public int SessionID { get; set; } = 0;
        public int SessionTimesID { get; set; }
        public string EvaluationNotes { get; set; } = "";
        //public List<QuesAnswer> QuesAnswers { get; set; }
    }

    public class QuesAnswer
    {
        public int QuestionID { get; set; } = 0;
        public int AnswerID { get; set; } = 0;
    }
}
