using canserver3.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace canserver3.ViewModels.Debugging
{

    public partial class AbstractDebugViewModel : ObservableObject, IDisposable
    {
        [ObservableProperty]
        private string _cANIndex = string.Empty;
        [ObservableProperty]
        private bool _isEnable = false;
        [ObservableProperty]
        private string _identifier = Guid.NewGuid().ToString();
        [ObservableProperty]
        private bool _isBackground = true;
        public virtual void Dispose()
        {
           
        }
        public virtual void Update()
        {
            IsBackground = false;
        }
        public virtual void GoToBackground()
        {
            IsBackground = true;
        }
    }

    public static class DebugViewModelFactory
    {
        public static AbstractDebugViewModel Create(string canIndex, AbstractFunctionNodeModel model)
        {
                throw new NotSupportedException($"Debug view model for function node type '{model.FunctionType}' is not supported.");
            
        }
    }
}
