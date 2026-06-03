using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using canserver3.ViewModels.Debugging;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using Ursa.Controls;

namespace canserver3.Views.Debugging;

public partial class CANDebugView : UserControl
{
    public CANDebugView()
    {
        InitializeComponent();

    }

    private void IconButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (e.Source is IconButton iconButton && iconButton.DataContext is DebugNodeItem node)
        {
            if (DebugViewLocator.Instance.Match(node.Content))
            {
                var param = node.Content;
                // 缓存 View 实例
                var dataType = param.GetType();
                // 只处理特定命名空间下的 ViewModel
                if (dataType.FullName!.StartsWith("canserver3.ViewModels.Debugging"))
                {
                    var name = dataType.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
                    var viewType = Type.GetType(name);
                    if (viewType == null)
                    {
                        return;
                    }
                    var viewInstance = (Control?)Ioc.Default.GetService(viewType);

                    if (viewInstance is not null)
                    {
                        viewInstance.DataContext = node.Content;
                    }
                }
            }
        }

    }

    private void TabStrip_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}