using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class TransactionsAndBalanceData
    {
        public List<TransactionsData> LatestTransactions { get; set; }
        public decimal Balance { get; set; } = 0;
    }

    public class TransactionsData
    {
        public int TransactionID { get; set; } = 0;
        public string TitleAr { get; set; } = "";
        public string TitleEn { get; set; } = "";
        public decimal? Price { get; set; } = 0;
        public string TransTypeNameAr { get; set; } = "";
        public string TransTypeNameEn { get; set; } = "";
        public string Date { get; set; } = "";
        public string Time { get; set; } = "";
    }
}
