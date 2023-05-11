
// 定義 C# 物件，反映 JSON 格式
// array of [timestamp, open, high, low, close, volume]
public class MaiCoinRateModel
{
    public long time { get; set; }
    public double open { get; set; }
    public double high { get; set; }
    public double low { get; set; }
    public double close { get; set; }
    public double volume { get; set; }

    public DateTimeOffset UtcTime => DateTimeOffset.FromUnixTimeSeconds(time).ToOffset(TimeSpan.FromHours(8));
}