using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class TimelineSessionDetailsData
    {
        public PendedSubsData PendedSubscriptions { get; set; }
    }

    public class PendedSubsData
    {
        public List<string> PendedSubscriptions { get; set; }
        public int Count { get; set; } = 0;
    }

    public class LecturerDetails
    {
        public string MyProperty { get; set; } = "";
        public List<string> PendedSubscriptions { get; set; }
        public int Count { get; set; } = 0;
    }
}
