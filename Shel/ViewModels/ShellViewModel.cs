using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Bll.Abstract;
using Caliburn.Micro;
using DTO;
using OxyPlot;
using OxyPlot.Series;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

public class Mat
{
    public Double Pat { get; set; }
    public Double Mats { get; set; }
    public ObservableCollection<Double> Pes { get; set; }

}

namespace Shel.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        public void ChangeMessage(Object ob)
        {
            var args = ob as RoutedPropertyChangedEventArgs<Object>;
            var plot = args?.NewValue as BridgeWeibull;

            if (plot == null) return;
            MyModel = new PlotModel { Title = "Weibull"};
            MyModel.Series.Add(new FunctionSeries(x => Wb(x, plot.Alpha, plot.Beta), 0, 20000, 0.5,
                    $"Id моста: {plot.Bridge.Id}, {plot.Alpha} форма, {plot.Beta} масштаб, Корреляция {plot.Correlation}"));
            NotifyOfPropertyChange(() => MyModel);
        }

        private readonly IShellTools tools;

        public ShellViewModel(IShellTools tools)
        {
            MatList=new ObservableCollection<BridgeWeibull>();
            this.tools = tools;
        }

        private static Double Wb(Double x, Double beta, Double eta)
            => beta/eta*Math.Pow(x/eta, beta - 1)*Math.Exp(-Math.Pow(x/eta, beta))*1e2;

        public void OpnDlg()
        {
            if (tools.OpenFile()) return;
            MyModel = new PlotModel { Title = "Weibull" };

            foreach (var plot in tools.Plots)
            {
                MyModel.Series.Add(new FunctionSeries(x => Wb(x, plot.Alpha, plot.Beta), 0, 700, 0.8,
                    $"Id моста: {plot.Bridge.Id}, {plot.Bridge.RoadValue} дорога, {plot.Bridge.ObstecleVal}"));
                MatList.Add(plot);
            }


            NotifyOfPropertyChange(() => MyModel);
            NotifyOfPropertyChange(() => MatList);
        }

        public ObservableCollection<BridgeWeibull> MatList { get; }

        public List<BridgeWeibull> Bridges => tools.Plots;
        public PlotModel MyModel { get; private set; }
    }
}
