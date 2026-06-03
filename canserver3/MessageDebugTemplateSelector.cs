using Avalonia.Controls;
using Avalonia.Controls.Templates;
using canserver3.ViewModels.Debugging;
using canserver3.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;

namespace canserver3
{
    public class MessageDebugTemplateSelector : IDataTemplate
    {
        // 定义你的两个模板
        public IDataTemplate? CyclicMessageTemplate { get; set; }
        public IDataTemplate? EventMessageTemplate { get; set; }

        public Control? Build(object? param)
        {
            return null;
        }

        public bool Match(object? data)
        {
            return false;
        }
    }
}
