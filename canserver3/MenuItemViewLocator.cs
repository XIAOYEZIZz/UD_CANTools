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
    public class MenuItemTemplateSelector : IDataTemplate
    {
        // 定义你的两个模板
        public IDataTemplate? DefaultTemplate { get; set; }
        public IDataTemplate? CANChannelTemplate { get; set; }

        public Control? Build(object? param)
        {
            if (param is CANChannelMenuItemViewModel)
                return CANChannelTemplate?.Build(param);

            return DefaultTemplate?.Build(param);
        }

        public bool Match(object? data)
        {
            return data is MenuItemViewModel || data is CANChannelMenuItemViewModel;
        }
    }
}
