//#define DUMMY_TIMESPAN

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Bll.Abstract;
using Bll.Extentions;
using Conv.Concrete;
using DTO;

namespace Bll.Concrete
{
    public class ShellTool : IShellTools
    {
        public ShellTool()
        {
            Plots = new List<BridgeWeibull>();
        }

        public FileDialog GeDialog(DialogType dType)
        {
            switch (dType)
            {
                case DialogType.Open:
                    return new OpenFileDialog();
                case DialogType.Save:
                    return new SaveFileDialog();
                default:
                    throw new ArgumentOutOfRangeException(nameof(dType), dType, null);
            }
        }


        public Boolean OpenFile()
        {
            var dlg = new OpenFileDialog
            {
                Filter = Dlg.OpnExelFilter,
                RestoreDirectory = true
            };

            if (dlg.ShowDialog() != DialogResult.OK)
                return true;

            List<Bridge> rawData;
            using (var stream = dlg.OpenFile())
            {
                rawData = new ExcelXmlV2ToBridge(stream).Import().ToList();
            }



            var br = new List<Tuple<Bridge, List<TimeSpan>>>();
            foreach (var bridge in rawData)
            {
                var tspns = new List<TimeSpan>();
                foreach (var superstructure in bridge.Superstructures)
                {
                    if (superstructure.Defects.Count == 0) continue;
                    var defOrd = superstructure.Defects.OrderBy(x => x.DetectionDate).ToList();
                    var spns = new List<TimeSpan>
                    {
                        defOrd.Min(x => x.DetectionDate - superstructure.PlaceDate)
                    };
                    for (var i = 0; i < defOrd.Count - 1; i++)
                        spns.Add(defOrd[i + 1].FactRepairDate - defOrd[i].DetectionDate);
                    tspns.AddRange(spns);
                }

                br.Add(new Tuple<Bridge, List<TimeSpan>>(bridge, tspns));
            }

//            var preparedData = rawData.Select(bridge => new
//            {
//                bridge,
//                bridge.Superstructures[0].Defects,
//                TimeUntilFirstDefect = bridge.Superstructures[0].Defects
//                    .Min(x => x.DetectionDate - bridge.Superstructures[0].PlaceDate)
//            });
//            var tSpans = new List<TimeSpan>();

//            Plots.Clear();
//            foreach (var bridge in preparedData)
//            {
//                var timeSpans = new {bridge.bridge, Spans = new List<TimeSpan> {bridge.TimeUntilFirstDefect}};

//                for (var i = 0; i < bridge.Defects.Count - 1; i++)
//                    timeSpans.Spans.Add(bridge.Defects[i + 1].FactRepairDate - bridge.Defects[i].DetectionDate);
//                tSpans.AddRange(timeSpans.Spans);

//#if DUMMY_TIMESPAN
//                var dummyTimeSpans = new List<TimeSpan>
//                {
//                    new TimeSpan(16, 0, 0, 0),
//                    new TimeSpan(34, 0, 0, 0),
//                    new TimeSpan(53, 0, 0, 0),
//                    new TimeSpan(75, 0, 0, 0),
//                    new TimeSpan(93, 0, 0, 0),
//                    new TimeSpan(120, 0, 0, 0),
//                    //new TimeSpan(63, 0, 0, 0),
//                    //new TimeSpan(115, 0, 0, 0),
//                    //new TimeSpan(148, 0, 0, 0),
//                    //new TimeSpan(210, 0, 0, 0),
//                    //new TimeSpan(307, 0, 0, 0),
//                    //new TimeSpan(635, 0, 0, 0),
//                };

//                timeSpans.Spans.Clear();
//                timeSpans.Spans.AddRange(dummyTimeSpans);
//#endif
//                // var d = timeSpans.Spans.CalcVeibull();

//                // Plots.Add(new BridgeWeibull {Alpha = d.Item1, Beta = d.Item2, Bridge = bridge.bridge, Correlation = d.Item3});
//            }
            var dt = br.SelectMany(x => x.Item2).OrderBy(x => x).Where(x => x.Ticks > 0).ToList().CalcVeibull();
            Plots.Add(new BridgeWeibull {Alpha = dt.Item1, Beta = dt.Item2, Bridge = rawData[0], Correlation = dt.Item3});

            return false;
        }

        public List<BridgeWeibull> Plots { get; private set; }
    }

    public class Cmprr : IEqualityComparer<Defect>
    {
        private Defect dfxt;

        public Boolean Equals(Defect x, Defect y)
        {
            dfxt = x;
           return x.DetectionDate == y.DetectionDate && x.FactRepairDate == y.FactRepairDate;
        }

        public Int32 GetHashCode(Defect obj)
        {
            return dfxt.DefectCharCode ^ dfxt.Id ^ dfxt.DefectCharCode;
        }
    }
}