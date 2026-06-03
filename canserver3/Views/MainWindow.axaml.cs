using Avalonia.Media;
using canserver3.Dialogs;
using canserver3.Services;
using canserver3.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.Diagnostics;
using System.Threading.Tasks;
using Ursa.Controls;
namespace canserver3.Views;

public partial class MainWindow : UrsaWindow
{
    private bool _defaultHide = false;
    public MainWindow()
    {
        InitializeComponent();
    }
    protected async override Task<bool> CanClose()
    {
        if (_defaultHide)
        {
            this.Hide();
            return false; // 取消关闭
        }
        var result = await ShowDialog();
        if (result == MessageBoxResult.No)
        {
            this.Hide();
            return false; // 取消关闭
        }
        if (result == MessageBoxResult.OK)
        {
            if (this.DataContext is MainViewModel vm)
            {
                vm.StopSimulatorCommand.Execute(null); // 停止CANServer

            }
            return true;
        }
        return false;
    }
    private async Task<MessageBoxResult> ShowDialog()
    {
        var options = new OverlayDialogOptions()
        {
            Title = "退出",
            CanLightDismiss = false,
            CanDragMove = true,
            IsCloseButtonVisible = true,
            CanResize = false,
            Mode = DialogMode.Question
        };
        var vm = new CloseOrHideDialogViewModel();
        var result = await OverlayDialog.ShowCustomAsync<CloseOrHideDialog, CloseOrHideDialogViewModel, object>(vm, null, options: options);

        if (result is null)
        {
            return MessageBoxResult.Yes; // 默认返回Yes
        }
        if ((MessageBoxResult)result is MessageBoxResult.Cancel)//取消就不要给默认值了
        {
            return MessageBoxResult.Cancel;
        }
        _defaultHide = vm.IsHide;
        return (MessageBoxResult)result;
    }
}
