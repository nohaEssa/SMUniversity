using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels.ObjectData
{
    public class SubscriptionObj
    {
        public int SessionID { get; set; } = 0;
        public int StudentID { get; set; } = 0;
        public bool Pending { get; set; } = false;
        public bool AddOrRemove { get; set; } = false;
    }
}
