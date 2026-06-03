using canserver3.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Irihi.Avalonia.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;

namespace canserver3.Dialogs
{
    public partial class CloseOrHideDialogViewModel : ViewModelBase, IDialogContext
    {
        public event EventHandler<object?>? RequestClose;
        [ObservableProperty]
        private bool _isHide;
        public void Close()
        {
            RequestClose?.Invoke(this, MessageBoxResult.Cancel);
        }
        [RelayCommand]
        private void Exit()
        {
            RequestClose?.Invoke(this, MessageBoxResult.OK);
        }
        [RelayCommand]
        private void Hide()
        {
            RequestClose?.Invoke(this, MessageBoxResult.No);
        }
    }
}
