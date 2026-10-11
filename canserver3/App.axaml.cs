using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using canserver3.Models;
using canserver3.Services;
using canserver3.ViewModels;
using canserver3.Views;
using canserver3.Views.Debugging;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using Ursa.Controls;
namespace canserver3;

public partial class App : Application
{
    public static string BaseUpperFolder { get; set; } = string.Empty;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Resources["DefaultFontFamily"] = null;
        Resources["SemiFontFamilyRegular"] = null;
#if DEBUG
        if (OperatingSystem.IsWindows())
        {
            AllocConsole(); // 显示控制台窗口
        }
        this.AttachDeveloperTools();
#endif
        string baseFolder = Environment.GetFolderPath(OperatingSystem.IsWindows()
            ? Environment.SpecialFolder.CommonApplicationData
            : Environment.SpecialFolder.LocalApplicationData);
        BaseUpperFolder = Path.Combine(baseFolder, "UDANTech", "canserver3");//注意Nlog中的配置同步
        try
        {
            if (!Path.Exists(App.BaseUpperFolder))
            {
                Directory.CreateDirectory(App.BaseUpperFolder);
            }
        }
        catch { }
    }
    [DllImport("kernel32.dll")]
    private static extern bool AllocConsole();
    public override void OnFrameworkInitializationCompleted()
    {
        DataTemplates.Add(DebugViewLocator.Instance);
        DataTemplates.Add(new ViewLocator());
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        //BindingPlugins.DataValidators.RemoveAt(0);

        var services = new ServiceCollection();
        ConfigServices(services);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopTemp)
        {
            services.AddSingleton<IClassicDesktopStyleApplicationLifetime>(desktopTemp);
        }

        var serviceProvider = services.BuildServiceProvider();
        Ioc.Default.ConfigureServices(serviceProvider);
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {



            desktop.MainWindow = Ioc.Default.GetRequiredService<MainWindow>();
            desktop.MainWindow.DataContext = Ioc.Default.GetRequiredService<MainViewModel>();//这里共用MainView的viewModel
            Ioc.Default.GetRequiredService<MainViewModel>().OnShowWindow += (sender, args) =>
            {
                desktop.MainWindow.Show();
                desktop.MainWindow.WindowState = Avalonia.Controls.WindowState.Normal;
                desktop.MainWindow.Activate();
            };
            Ioc.Default.GetRequiredService<MainViewModel>().OnExit += (sender, args) =>
            {
                desktop.Shutdown();
            };

        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            //singleViewPlatform.MainView = new MainView
            //{
            //    DataContext = new MainViewModel()
            //};
        }
        FeedbackService.Instance.Window = Ioc.Default.GetRequiredService<MainWindow>();
        DataContext = Ioc.Default.GetRequiredService<MainViewModel>();
        base.OnFrameworkInitializationCompleted();
    }
    private void ConfigServices(IServiceCollection services)
    {
        TypeAdapterConfigService.Configure();
        services.AddSingleton<IStorageProvider>(sp => Ioc.Default.GetRequiredService<MainWindow>().StorageProvider);
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainView>();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MenuViewModel>();
        services.AddSingleton<CANChannelViewModel>();

        services.AddSingleton<CANChannelView>();
        services.AddTransient<CANDebugView>();
    }

}
