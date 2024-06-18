using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using DevExpress.Mvvm;
using OxyPlot;

namespace CurrencyConverter.Models;
public class Currencies : BindableBase
{
    public Currencies()
    {
        EuroLine = new();
        DollarLine = new();
        YuanLine = new();
    }
    
    [JsonPropertyName("EUR")] public decimal Euro { get; set; }
    [JsonPropertyName("USD")] public decimal Dollar { get; set; }
    [JsonPropertyName("CNY")] public decimal Yuan { get; set; }
    
    public List<DataPoint> EuroLine { get; set; } 
    public List<DataPoint> DollarLine { get; set; }
    public List<DataPoint> YuanLine { get; set; }

    public static Currencies? Deserialize()
    {
        using var fs = new FileStream("Assets/Currencies.json", FileMode.OpenOrCreate);
        return JsonSerializer.Deserialize<Currencies>(fs);
    }
    
    public static void Serialize(Currencies value)
    {
        using var fs = new FileStream("Assets/Currencies.json", FileMode.Create);
        JsonSerializer.Serialize(fs, value);
    }

    public static Currency StringToEnum(string s)
    {
        switch (s)
        {
            case "EUR":
                return Currency.EUR;
            case "USD":
                return Currency.USD;
            case "CNY":
                return Currency.CNY;
            default:
                return Currency.None;
        } 
    }
}

public enum Currency
{
    None,
    EUR,
    USD,
    CNY
}