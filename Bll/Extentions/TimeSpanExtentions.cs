using System;
using System.Collections.Generic;
using System.Linq;

namespace Bll.Extentions
{
    public static class TimeSpanExtentions
    {
        public static Tuple<Double, Double, Double> CalcVeibull(this List<TimeSpan> spans)
        {
            var data = spans.OrderBy(x => x)
                    .Select(
                        (x, i) => new { X = x.Days, Log = Math.Log(x.Days), F = (i + 0.7) / (spans.Count + 0.4) })
                    .Select(y => new { y.X, y.Log, Y = Math.Log(-Math.Log(1 - y.F)), Log2 = y.Log * y.Log })
                    .Select(z => new { z.X, z.Log, z.Log2, z.Y, YLog = z.Log * z.Y }).ToList();

            var b = (data.Sum(x => x.YLog) - data.Sum(x => x.Log) * data.Sum(x => x.Y) / data.Count) /
                    (data.Sum(x => x.Log2) - Math.Pow(data.Sum(x => x.Log), 2) / data.Count);
            var a = data.Sum(x => x.Y) / data.Count - b * data.Sum(x => x.Log) / data.Count;
            var η = Math.Exp(-(a / b));

            var meanX = data.Average(x => x.X);
            var meanY = data.Average(x => x.Y);
            var numerator = data.Sum(x => (x.X - meanX) * (x.Y - meanY));
            var denominator =
                Math.Sqrt(data.Sum(x => Math.Pow(x.X - meanX, 2)) * data.Sum(x => Math.Pow(x.Y - meanY, 2)));

            var t = numerator / denominator;
            return new Tuple<Double, Double, Double>(b, η, t);
        }
    }
}