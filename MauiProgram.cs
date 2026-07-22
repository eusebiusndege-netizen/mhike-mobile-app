using Microsoft.Extensions.Logging;
using MHike.Pages;
using MHike.Services;
using MHike.ViewModels;

namespace MHike;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register services
        builder.Services.AddSingleton<HikeRepository>();

        // Register view models
        builder.Services.AddTransient<HikeViewModel>();
        builder.Services.AddTransient<ObservationsViewModel>();
        builder.Services.AddTransient<AddEditHikeViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegisterViewModel>();

        // Register pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<ObservationsPage>();
        builder.Services.AddTransient<AddEditHikePage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();

        return builder.Build();
    }
}