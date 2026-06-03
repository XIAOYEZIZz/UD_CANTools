using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;

namespace canserver3.ViewModels
{
    public partial class MenuViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<MenuItemViewModel> _menuItems = new ObservableCollection<MenuItemViewModel>()
        {
           new MenuItemViewModel(MenuKeys.MenuKeyCANChannel,true){MenuHeader="Channel",MenuIconName = "CANChannel"},
           new MenuItemViewModel(MenuKeys.MenuKeyCANNode,true){MenuHeader="Node",MenuIconName = "CANNode"},
        };
        [ObservableProperty]
        private MenuItemViewModel _selectedMenuItem;
        public MenuViewModel()
        {
            SelectedMenuItem = MenuItems[0];
            WeakReferenceMessenger.Default.Register<ValueChangedMessage<bool>, string>(this, MessageTokens.ServerStopped, (o, m) =>
            {
                SelectedMenuItem = MenuItems[0];
            });
        }
    }
    public static class MenuKeys
    {
        public const string MenuKeyCANChannel = "CAN Channel";
        public const string MenuKeyCANNode = "CAN Node";
        public const string MenuKeyCANDebug = "CAN Debug";
    }
    public static class MessageTokens
    {
        public const string Menu = "Menu";
        public const string CANChannels = "CANChannels";
        public const string ProjectCANChannels = "ProjectCANChannels";
        public const string CANNodes = "CANNodes";
        public const string CANCopyAddNode = "CANCopyAddNode";
        public const string CANExchangeNode = "CANExchangeNode";
        public const string ProjectCANNodes = "ProjectCANNodes";
        public const string Running = "Running";
        public const string AddCANNode = "AddCANNode";
        public const string RemoveCANNode = "RemoveCANNode";
        public const string ServerStopped = "ServerStopped";
        public const string ForwardCanFrame = "ForwardCanFrame";
    }
}
