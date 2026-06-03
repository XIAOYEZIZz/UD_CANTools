using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using canserver3.Models;
using canserver3.Services;
using canserver3.ViewModels.Debugging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Mapster;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ursa.Controls;

namespace canserver3.ViewModels;

public partial class MainViewModel : ObservableRecipient
{
    [ObservableProperty]
    private string? _projectName;
    [ObservableProperty]
    private string? _projectPath;
    [ObservableProperty]
    private ProjectModel? _project;
    [ObservableProperty]
    private object? _content;
    [ObservableProperty]
    private MenuViewModel _menu;
    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    private bool _isSimulatorRunning;
    private Dictionary<string, CANDebugViewModel> _canDebugViewModels = new Dictionary<string, CANDebugViewModel>();
    public event EventHandler? OnShowWindow;
    public event EventHandler? OnExit;

    public MainViewModel()
    {

        WeakReferenceMessenger.Default.Register<MainViewModel, MenuItemViewModel, string>(this, MessageTokens.Menu, OnNavigation);
        Menu = Ioc.Default.GetRequiredService<MenuViewModel>();
        Content = Ioc.Default.GetRequiredService<CANChannelViewModel>();

        WeakReferenceMessenger.Default.Register<MainViewModel, List<CANChannelItemModel>, string>(this, MessageTokens.CANChannels, (vm, channels) =>
        {
            Project!.CANChannels = channels.ToArray();
        });
    }
    private ProjectModel CreateDefaultProject()
    {
        ProjectName = "新建空白项目";
        ProjectPath = string.Empty;
        var project = new ProjectModel()
        {
            CANChannels = new CANChannelItemModel[]
            {
                new CANChannelItemModel()
                {
                    CANIndex = 0,
                    IsEnable = true,

                    DeviceIndex = 0,
                    ChannelIndex = 0,
                    Description = "这是CAN1"
                },
                new CANChannelItemModel()
                {
                    CANIndex = 1,
                    IsEnable = true,

                    DeviceIndex = 0,
                    ChannelIndex = 1,

                    Description = "这是CAN2"
                }
            }
        };
        WeakReferenceMessenger.Default.Send<List<CANChannelItemModel>, string>(project.CANChannels!.ToList(), MessageTokens.ProjectCANChannels);
        if (project.CANChannelNodes is null)
        { project.CANChannelNodes = new Dictionary<string, AbstractFunctionNodeModel[]?>(); }
        for (int i = 1; i <= 8; i++)
        {
            var nodes = project.CANChannelNodes.GetValueOrDefault("CAN" + i, Array.Empty<AbstractFunctionNodeModel>());
            WeakReferenceMessenger.Default.Send<List<AbstractFunctionNodeModel>, string>(nodes!.ToList(), MessageTokens.ProjectCANNodes + "CAN" + i);
        }
        return project;
    }
    private void OnNavigation(MainViewModel vm, MenuItemViewModel s)
    {
        Content = s.Key switch
        {
            MenuKeys.MenuKeyCANChannel => Ioc.Default.GetRequiredService<CANChannelViewModel>(),
            MenuKeys.MenuKeyCANDebug => ((Func<MenuItemViewModel, CANDebugViewModel>)((s) =>
            {
                if (_canDebugViewModels.TryGetValue(s.MenuHeader!, out var viewModels))
                    return viewModels;
                else
                {
                    var vm = new CANDebugViewModel(s.MenuHeader!, Project?.CANChannelNodes!.GetValueOrDefault(s.MenuHeader,
                                Array.Empty<AbstractFunctionNodeModel>()));
                    _canDebugViewModels[s.Key!] = vm; // 缓存新创建的 ViewModel
                    return vm;
                }
            }))(s),
            _ => null
            //_ => throw new ArgumentOutOfRangeException(nameof(s), s, null),
        };
    }
    protected override void Broadcast<T>(T oldValue, T newValue, string? propertyName)
    {
        PropertyChangedMessage<T> message = new(this, propertyName, oldValue, newValue);
        switch (propertyName)
        {
            case nameof(IsSimulatorRunning):
                _ = Messenger.Send(message, MessageTokens.Running);
                break;
            default:
                return; // 如果不是我们关心的属性，则不发送消息
        }
    }
    [RelayCommand]
    private async Task ShowWindow()
    {
        if (OnShowWindow != null)
        {
            OnShowWindow(this, EventArgs.Empty);
        }
        await Task.CompletedTask;
    }
    [RelayCommand]
    private void ExitApplication()
    {
        StopSimulator();
        if (OnExit != null)
        {
            OnExit(this, EventArgs.Empty);
        }
    }

    [RelayCommand]
    private async Task ShowAppSettingDialog()
    {
    }
    [RelayCommand]
    private void NewProject()
    {
        if (IsSimulatorRunning)
            return;
        Project = CreateDefaultProject();
    }
    [RelayCommand]
    private async Task OpenProjectAsync()
    {
    }

    private void LoadProjectFromFile(string filePath)
    {
    }

    [RelayCommand]
    private async Task SaveProjectAsync(string? b)
    {
        
    }
    [RelayCommand]
    private async Task SaveAsProjectAsync()
    {
        if (IsSimulatorRunning)
            return;
    }
    [RelayCommand]
    private void StartSimulator()
    {
        try
        {
           
            IsSimulatorRunning = true;
        }
        catch (Exception e)
        {
            FeedbackService.ShowErrorNotification("启动CANServer失败：" + e.Message);
            StopSimulator();
        }

    }
    [RelayCommand]
    private void StopSimulator()
    {
        try
        {
            
        }
        catch (Exception e)
        {
            FeedbackService.ShowErrorNotification("停止CANServer失败：" + e.Message);
        }
        finally
        {
            IsSimulatorRunning = false;
            DebugViewLocator.Instance.ClearCache();//这里把之前的View都清掉，避免内存泄漏
        }
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
