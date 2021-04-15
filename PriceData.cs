using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratePriceData
{
    public class PriceData
    {
        public string symbol;
        public decimal last;
        public decimal change;
        public decimal percent_change;
        public decimal high;
        public decimal low;
        public decimal volume;
        public string pricedate;
    }
}
