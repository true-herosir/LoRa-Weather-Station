using static WeatherThingy.Sources.Model.GUI_JSON_parse;

namespace WeatherThingy.Sources.Services
{
    internal interface IWeatherThingyService
    {
        
        Task<Root> GetNodeData(); //should return EVERYTHIIIIIIIING
        //Task<Root> GetNodeData(string recent = "recent");
        //Task<Root> GetNodeData(string location, DateTime start, DateTime end);
        //Task<Root> GetNodeData(string location, DateOnly day);

    }
}
