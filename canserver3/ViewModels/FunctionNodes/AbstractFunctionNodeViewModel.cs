using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;

namespace canserver3.ViewModels
{
    public partial class AbstractFunctionNodeViewModel : ObservableObject
    {
        public event Action? FunctionNodeChanged;
        [ObservableProperty]
        private string _functionType = string.Empty;
        [ObservableProperty]
        private string _functionName = string.Empty;
        [ObservableProperty]
        private bool _isEnable;
        public WindowToastManager? ToastManager { get; set; }
        public WindowNotificationManager? NotificationManager { get; set; }
        public string FunctionDecription => UpdateFunctionDescription();

        protected virtual string UpdateFunctionDescription()
        {
            return "undefined";
        }

        public virtual async Task EditAsync()
        {
            await Task.CompletedTask;
        }
        partial void OnIsEnableChanged(bool value)
        {
            FunctionNodeChanged?.Invoke();
        }
    }
    public static class FunctionNodeFactory
    {
        public static AbstractFunctionNodeViewModel CreateNode(string type)
        {
            return type switch
            {
                _ => throw new NotSupportedException($"Function node type '{type}' is not supported.")
            };
        }
        public const string BMUSimu = "BMU";
        public const string UDSTester = "UDS";
        public const string UDSServer = "UDSServer";
        public const string Shell = "Shell";
        public const string DBC = "DBC";
    }
}
