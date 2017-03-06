//#define DUMMY_TIMESPAN

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Conv.Concrete;
using Conv.Dto;
using DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConvTests
{

    public static class DoubleHelpers
    {
        public static Boolean SignChanged(this Double first, Double second)
        {
            if (first > 0 && second > 0) return false;
            if (first < 0 && second < 0) return false;
            if (first > 0 && second < 0) return true;
            if (first < 0 && second > 0) return true;
            return false;
        }
    }
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod3()
        {
            List<DayClass> rawData;
            using (var stream = new FileStream("C:/Users/nomuu/Desktop/ss4.xml", FileMode.Open))
            {
                rawData = new ExcelXMLToDay(stream).Import().Where(x=>Math.Abs(x.Temperature) > 1e-5).ToList();
            }
            var transitionCount = 0;
            var previousState = 0d;
            foreach (var t in rawData)
            {
                var current= t.Temperature;
                if (previousState.SignChanged(current))
                    transitionCount++;
                previousState = current;
            }

        }

        [TestMethod]
        public void TestMethod2()
        {
            List<Bridge> rawData;
            using (var stream = new FileStream("C:/Users/nomuu/Desktop/AllDefectsV2.xml", FileMode.Open))
            {
                rawData = new ExcelXmlV2ToBridge(stream).Import().ToList();
            }
        }



        [TestMethod]
        public void TestMethod1()
        {
            List<Bridge> rawData;
            using (var stream = new FileStream("C:/Users/nomuu/Desktop/AllDefects.xml", FileMode.Open))
            {
                rawData = new ExcelXmlToBridge(stream).Import().ToList();
            }

            var preparedData = rawData.Select(bridge => new
            {
                bridge,
                bridge.Superstructures[0].Defects,
                TimeUntilFirstDefect = bridge
                    .Superstructures[0].Defects
                    .Min(x => x.DetectionDate - bridge.Superstructures[0].PlaceDate)
            });

            var transfer = new List<Tuple<Bridge, Double, Double, Double>>();

            foreach (var bridge in preparedData)
            {
                var timeSpans = new {bridge.bridge, Spans = new List<TimeSpan> {bridge.TimeUntilFirstDefect}};

                for (var i = 0; i < bridge.Defects.Count - 1; i++)
                    timeSpans.Spans.Add(bridge.Defects[i + 1].FactRepairDate - bridge.Defects[i].DetectionDate);

#if DUMMY_TIMESPAN
                var dummyTimeSpans = new List<TimeSpan>
                {
                    new TimeSpan(16, 0, 0, 0),
                    new TimeSpan(34, 0, 0, 0),
                    new TimeSpan(53, 0, 0, 0),
                    new TimeSpan(75, 0, 0, 0),
                    new TimeSpan(93, 0, 0, 0),
                    new TimeSpan(120, 0, 0, 0),
                    //new TimeSpan(63, 0, 0, 0),
                    //new TimeSpan(115, 0, 0, 0),
                    //new TimeSpan(148, 0, 0, 0),
                    //new TimeSpan(210, 0, 0, 0),
                    //new TimeSpan(307, 0, 0, 0),
                    //new TimeSpan(635, 0, 0, 0),
                };

                timeSpans.Spans.Clear();
                timeSpans.Spans.AddRange(dummyTimeSpans);
#endif

                var data = timeSpans.Spans
                    .Select((x, i) => new {X= x.Days, Log = Math.Log(x.Days), F = (i + 0.7)/(timeSpans.Spans.Count + 0.4)})
                    .Select(y => new {y.X, y.Log, Y = Math.Log(-Math.Log(1 - y.F)), Log2 = y.Log*y.Log})
                    .Select(z => new {z.X, z.Log, z.Log2, z.Y, YLog = z.Log*z.Y}).ToList();

                var b = (data.Sum(x => x.YLog) - data.Sum(x => x.Log)*data.Sum(x => x.Y)/data.Count)/
                        (data.Sum(x => x.Log2) - Math.Pow(data.Sum(x => x.Log), 2)/data.Count);
                var a = data.Sum(x => x.Y)/data.Count - b*data.Sum(x => x.Log)/data.Count;
                var η = Math.Exp(-(a/b));

                var meanX = data.Average(x => x.X);
                var meanY= data.Average(x => x.Y);
                var numerator = data.Sum(x => (x.X - meanX)*(x.Y - meanY));
                var denominator = Math.Sqrt(data.Sum(x => Math.Pow(x.X - meanX, 2))* data.Sum(x => Math.Pow(x.Y - meanY, 2)));

                var t = numerator/denominator;

                transfer.Add(new Tuple<Bridge, Double, Double, Double>(bridge.bridge, b, a, η));
            }
            var result = transfer.Select(res => new {Bridge = res.Item1, α = res.Item3, β = res.Item4 }).ToList();
        }
    }
}