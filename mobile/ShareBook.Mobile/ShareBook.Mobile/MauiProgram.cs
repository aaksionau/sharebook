using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Namespace.ShareBook.Mobile.Services;
using Namespace.ShareBook.Mobile.ViewModels;
using ShareBook.Mobile.ViewModels;
using ShareBook.Mobile.Views;
using UraniumUI;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace ShareBook.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("ShareBook.Mobile.appSettings.json");

        var config = new ConfigurationBuilder().AddJsonStream(stream).Build();

        var builder = MauiApp.CreateBuilder();

        builder.Configuration.AddConfiguration(config);

        builder.Services.AddSingleton(new HttpHelperService(builder.Configuration));

        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<HomeViewModel>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<HomePage>();
        builder.Services.AddSingleton<RegistrationViewModel>();
        builder.Services.AddSingleton<RegisterPage>();

        builder
            .UseMauiApp<App>()
            .UseUraniumUI()
            .UseUraniumUIMaterial()
            .UseBarcodeReader()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFontAwesomeIconFonts();
            });

        return builder.Build();
    }
}
