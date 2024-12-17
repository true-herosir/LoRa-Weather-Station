using Microsoft.Extensions.Logging;
//using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using SkiaSharp.Views.Maui.Controls.Hosting;
using WeatherThingy.Pages;
using WeatherThingy.Sources.ViewModels;
using WeatherThingy.Sources.Views;

namespace WeatherThingy
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseSkiaSharp()
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddTransient<DetailPage>();
            builder.Services.AddSingleton<DetailViewModel>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
