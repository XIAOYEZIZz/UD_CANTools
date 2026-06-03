using Avalonia.Controls;
using Avalonia.Controls.Templates;
using canserver3.ViewModels;
using canserver3.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace canserver3
{
    public class ViewLocator : IDataTemplate
    {
        public Control? Build(object? param)
        {
            if (param is null)
                return null;
            var dataType = param.GetType();
            var name = dataType.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var viewType = Type.GetType(name);
            if (viewType == null)
            {
                return null;
            }
            if (viewType != null && typeof(Control).IsAssignableFrom(viewType))
            {
                var control = (Control?)Ioc.Default.GetService(viewType);
                var code = control?.GetHashCode();
                // 从 IoC 容器获取 View 实例
                return control;
            }

            return null; // 未找到匹配的 View
        }

        public bool Match(object? data)
        {
            return data is ObservableObject;
        }
    }
}
