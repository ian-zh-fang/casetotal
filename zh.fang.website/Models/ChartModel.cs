namespace zh.fang.website.Models
{
    using System;
    using System.Linq;

    public abstract class ChartModel
    {
        public abstract string[] legends { get; }
    }

    public class ClsTotalValue
    {
        public string name { get; set; }

        public int value { get; set; }
    }

    public class ClsTotalOnTodayModel:ChartModel
    {
        public ClsTotalValue[] itotals { get; set; }

        public ClsTotalValue[] ototals { get; set; }

        public override string[] legends
        {
            get
            {
                return 
                    itotals.Union(ototals).Select(t => t.name).Distinct().ToArray();
            }
        }
    }

    public class ClsTotalCompareValue<T>
    {
        public string name { get; set; }

        public T[] totals { get; set; }
    }

    public class ClsTotalCompareModel : ChartModel
    {
        public string title { get; set; }

        public ClsTotalCompareValue<int> item1 { get; set; }

        public ClsTotalCompareValue<int> item2 { get; set; }

        public ClsTotalCompareValue<double> lrr
        {
            get
            {
                var rrval = new double[item1.totals.Length];
                for (int i = 0; i < rrval.Length; i++)
                {
                    if (0 == item2.totals[i])
                    {
                        rrval[i] = item1.totals[i];
                        continue;
                    }

                    var addval = (double)(item1.totals[i] - item2.totals[2]);
                    rrval[i] = Math.Round(addval / item2.totals[i], 2);
                }
                return new ClsTotalCompareValue<double>
                {
                    name = "环比",
                    totals = rrval
                };
            }
        }

        public string[] categories { get; set; }

        public override string[] legends
        {
            get
            {
                return new string[] { item1.name, item2.name, lrr.name };
            }
        }
    }
}