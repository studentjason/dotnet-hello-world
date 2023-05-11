
// 定義 C# 物件，反映 JSON 格式
public class RybitCoinRateModel
{
    public double price { get; set; }
    public long time { get; set; }

    public DateTimeOffset UtcTime => DateTimeOffset.FromUnixTimeMilliseconds(time).ToOffset(TimeSpan.FromHours(8));
}