using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMUModels
{
    public class CardCategoryData
    {
        public int ID { get; set; } = 0;
        public string Title { get; set; } = "";
        public string TitleEn { get; set; } = "";
        public decimal Price { get; set; } = 0;
        public int ChargeCardsNo { get; set; } = 0;
        public int ForApplication { get; set; } = 0;
    }
}
