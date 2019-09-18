using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMUniversity.Models
{
    public class LatestVoucherViewModel
    {
        public int ID { get; set; }
        public string lecturerName { get; set; }
        public string HallCode { get; set; }
        public double Serial { get; set; }
        public string PaymentMethod { get; set; }
        public string createdDate { get; set; }
        public decimal Cost { get; set; }

    }
}

