using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using canserver3.ViewModels;

namespace canserver3.Views;

public partial class CANChannelView : UserControl
{

    public CANChannelView()
    {
        InitializeComponent();
    }

    private void DataGrid_DoubleTapped(object? sender, TappedEventArgs e)
    {
    }
}