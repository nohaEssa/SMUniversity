using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Classes
{
    public class ResultHandler
    {
        public bool IsSuccessful { get; set; }
        public string MessageAr { get; set; }
        public string MessageEn { get; set; }
        public int Count { get; set; }
        public object Result { get; set; }
    }
}