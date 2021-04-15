using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratePriceData
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dt = DateTime.Now;
            for(int i = 0; i < 40; i++)
            {
                dt = dt.AddDays(-1);
                if ((dt.DayOfWeek == DayOfWeek.Saturday)  || (dt.DayOfWeek == DayOfWeek.Sunday))
                {
                    continue;
                }
                string dateStr = dt.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                Generate(dateStr);
            }
        }

        private static void Generate(string priceDate)
        {
            List<PriceData> priceDataList = new List<PriceData>();
            using (var reader = new StreamReader(@"C:\GitHub\SampleData\sp-500-index-golden-copy.csv"))
            {
                List<string> listA = new List<string>();
                int count = 0;
                while (!reader.EndOfStream)
                {
                    count++;
                    var line = reader.ReadLine();
                    if (count <= 1)
                    {
                        continue; //skip header
                    }


                    var values = line.Split(',');
                    PriceData priceData = new PriceData();
                    priceData.symbol = values[0];
                    priceData.last = Convert.ToDecimal(values[2]);
                    priceData.last = AddRandom(priceData.last);

                    priceData.change = Convert.ToDecimal(values[3]);
                    priceData.change = AddRandom(priceData.change);

                    priceData.percent_change = Convert.ToDecimal(values[4]);
                    priceData.percent_change = AddRandom(priceData.percent_change);

                    priceData.high = Convert.ToDecimal(values[5]);
                    priceData.high = AddRandom(priceData.high);

                    priceData.low = Convert.ToDecimal(values[6]);
                    priceData.low = AddRandom(priceData.low);

                    decimal high = priceData.high;
                    if (priceData.low > priceData.high)
                    {
                        priceData.high = priceData.low;
                        priceData.low = high;
                    }

                    priceData.volume = Convert.ToDecimal(values[7]);
                    priceData.volume = AddRandom(priceData.volume);

                    priceData.pricedate = priceDate;

                    priceDataList.Add(priceData);

                }


            }

            // Write file using StreamWriter  
            string fileName = $"sp-500-index-{priceDate}.csv";
            string fullPath = Path.Combine(@"C:\GitHub\SampleData", fileName.Replace(@"/", "-"));

            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine("Symbol,Last,Change,Chg,High,Low,Volume,Time");
                foreach (PriceData priceData in priceDataList)
                {
                    writer.WriteLine($"{priceData.symbol},{Math.Round(priceData.last, 2)},{Math.Round(priceData.change, 2)},{Math.Round(priceData.percent_change, 2)},{Math.Round(priceData.high, 2)},{Math.Round(priceData.low, 2)},{Math.Round(priceData.volume, 0)},{priceData.pricedate}");
                }
            }
        }

        public static decimal AddRandom(decimal val)
        {
            Random random = new Random();
            int r = random.Next(-5000, 5000);
            decimal increment_percent = ((decimal)r / (decimal)1000.00);
            decimal increment = (increment_percent / (decimal)100.00) * val;
            return val + increment;
        }

    }
}
