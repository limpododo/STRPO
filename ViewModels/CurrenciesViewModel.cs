using System;
using CurrencyConverter.Models;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;

namespace CurrencyConverter.ViewModels;

public class CurrenciesViewModel : ViewModelBase
{
    private Currencies _currencies;
    public CurrenciesViewModel()
    {
        _currencies = Currencies.Deserialize();

        Chart = new()
        {
            PlotAreaBorderColor = OxyColors.Black,
            PlotMargins = new OxyThickness(10, 10, 10, 10),
            Padding = new OxyThickness(35, 10, 2, 40)
        };
        
        Chart.Legends.Add(new Legend());
        
        Chart.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            MajorGridlineStyle = LineStyle.Solid, 
            MajorGridlineColor = OxyColors.Gray
        });
        Chart.Axes.Add(new DateTimeAxis()
        {
            Position = AxisPosition.Bottom,
            MajorGridlineStyle = LineStyle.Solid,
            MajorGridlineColor = OxyColors.Gray
        });
        
        Chart.Series.Add(new LineSeries()
        {
            Title = "Euro"
        });
        Chart.Series.Add(new LineSeries()
        {
            Title = "Dollar"
        });
        Chart.Series.Add(new LineSeries()
        {
            Title = "Yuan"
        });

        CurrenciesIsVisible = true;
        ChartIsVisible = false;
    }

    #region Prop
    public bool ChartIsVisible
    {
        get => GetProperty(() => ChartIsVisible);
        set => SetProperty(() => ChartIsVisible, value);
    }
    public bool CurrenciesIsVisible
    {
        get => GetProperty(() => CurrenciesIsVisible);
        set => SetProperty(() => CurrenciesIsVisible, value);
    }

    public string ChangeViewButtonString => !ChartIsVisible && CurrenciesIsVisible ? "Go to Chart" : "Go to Currency";
    
    public decimal EuroNew
    {
        get => GetProperty(() => EuroNew);
        set => SetProperty(() => EuroNew, value);
    }
    public decimal DollarNew
    {
        get => GetProperty(() => DollarNew);
        set => SetProperty(() => DollarNew, value);
    }
    public decimal YuanNew
    {
        get => GetProperty(() => YuanNew);
        set => SetProperty(() => YuanNew, value);
    }
    
    public decimal EuroCurrent => _currencies.Euro;
    public decimal DollarCurrent => _currencies.Dollar;
    public decimal YuanCurrent => _currencies.Yuan;
    #endregion

    #region Commands

    [Command]
    public void Change(object p)
    {
        var currency = Currencies.StringToEnum(p as string);
        var currentDateTime = DateTimeAxis.ToDouble(DateTime.Now);
        switch (currency)
        {
            case Currency.EUR:
                _currencies.Euro = EuroNew;
                (Chart.Series[0] as LineSeries).Points.Add(new DataPoint(currentDateTime, (double)_currencies.Euro));
                break;
            case Currency.USD:
                _currencies.Dollar = DollarNew;
                (Chart.Series[1] as LineSeries).Points.Add(new DataPoint(currentDateTime, (double)_currencies.Dollar));
                break;
            case Currency.CNY:
                _currencies.Yuan = YuanNew;
                (Chart.Series[2] as LineSeries).Points.Add(new DataPoint(currentDateTime, (double)_currencies.Yuan));
                break;
        }
        
        Chart.InvalidatePlot(true);
        Currencies.Serialize(_currencies);
        RaisePropertiesChanged();
    }
    
    [Command]
    public void ChangeView()
    {
        ChartIsVisible = !ChartIsVisible;
        CurrenciesIsVisible = !CurrenciesIsVisible;
        
        RaisePropertyChanged(() => ChangeViewButtonString);
    }
    

    #endregion

    #region Chart
    public PlotModel Chart
    {
        get => GetProperty(() => Chart);
        set => SetProperty(() => Chart, value);
    }

    #endregion
}