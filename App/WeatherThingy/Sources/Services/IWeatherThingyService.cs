using static WeatherThingy.Sources.Model.GUI_JSON_parse;

namespace WeatherThingy.Sources.Services
{
    internal interface IWeatherThingyService
    {
        Task<Root> GetNodeData();
    }
}
