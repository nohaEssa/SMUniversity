using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class TransactionsData
    {
        public int TransactionID { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public decimal? Price { get; set; }
        public string TransTypeNameAr { get; set; }
        public string TransTypeNameEn { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
