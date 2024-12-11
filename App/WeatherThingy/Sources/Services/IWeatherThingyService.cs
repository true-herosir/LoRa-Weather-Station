using static WeatherThingy.Sources.Model.GUI_JSON_parse;

namespace WeatherThingy.Sources.Services
{
    internal interface IWeatherThingyService
    {
        Task<Root> GetNodeData(); //should return most recent
        Task<Root> GetBattData(); //should return most recent
        Task<Root> GetNodeData(string location, DateTime start, DateTime end, int page);
    }
}
