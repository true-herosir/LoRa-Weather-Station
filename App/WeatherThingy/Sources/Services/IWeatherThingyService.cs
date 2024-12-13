namespace WeatherThingy.Sources.Services;
public partial interface IWeatherThingyService
{
    Task<Root> GetNodeData(); //should return most recent
    Task<Root> GetBattData(); //should return most recent
    Task<Root> GetNodeData(string location, DateTime start, DateTime end, int page);
}
