using Avalonia.Controls;
using Avalonia.Controls.Templates;
using canserver3.ViewModels;
using canserver3.ViewModels.Debugging;
using canserver3.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace canserver3
{
    public class DebugViewLocator : IDataTemplate
    {
        private static readonly Lazy<DebugViewLocator> _instance =
            new Lazy<DebugViewLocator>(() => new DebugViewLocator());

        public static DebugViewLocator Instance => _instance.Value;
        //缓存会导致可视化树异常，暂时禁用
        private static Dictionary<string, Control?> _viewCache = new Dictionary<string, Control?>();
        public Control? Build(object? param)
        {
            if (param is null)
                return null;
            var dataType = param.GetType();
            // 只处理特定命名空间下的 ViewModel
            if (!dataType.FullName!.StartsWith("canserver3.ViewModels.Debugging"))
                return null;
            if (param is AbstractDebugViewModel debugModel)
            {
                var name = dataType.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
                var viewType = Type.GetType(name);
                if (viewType == null)
                {
                    return null;
                }
                if (viewType != null && typeof(Control).IsAssignableFrom(viewType))
                {
                    return (Control?)Ioc.Default.GetService(viewType);
                }
            }
            return null; // 未找到匹配的 View
        }

        public bool Match(object? data)
        {
            return data is AbstractDebugViewModel && data.GetType().FullName!.StartsWith("canserver3.ViewModels.Debugging");
        }
        public void ClearCache()
        {
            _viewCache.Clear();
        }
    }
}
