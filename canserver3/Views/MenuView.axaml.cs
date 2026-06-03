using Avalonia.Controls;

namespace canserver3.Views;

public partial class MenuView : UserControl
{
    public MenuView()
    {
        InitializeComponent();
    }

    private void Avatar_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        menu.IsHorizontalCollapsed = !menu.IsHorizontalCollapsed;
    }

    private void TextBlock_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        
    }
}