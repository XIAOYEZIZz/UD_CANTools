using canserver3.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace canserver3.ViewModels.Debugging
{
    public partial class CANDebugViewModel : AbstractDebugViewModel
    {
        private AbstractFunctionNodeModel[]? _nodes;
        public ObservableCollection<DebugNodeItem> DebugNodeItems { get; } = new ObservableCollection<DebugNodeItem>();
        [ObservableProperty]
        private DebugNodeItem? _selectedNodeItem;
        [ObservableProperty]
        private object? _content;
        public CANDebugViewModel(string canIndex, AbstractFunctionNodeModel[]? nodes)
        {
            CANIndex = canIndex;
            _nodes = nodes;
            if (nodes is null || nodes.Length == 0)
            {
                SelectedNodeItem = null;
            }
            else
            {
                foreach (var item in nodes)
                {
                    DebugNodeItems.Add(new DebugNodeItem(canIndex, $"{item.FunctionName}", item.IsEnable, DebugViewModelFactory.Create(canIndex, item)));
                }
                SelectedNodeItem = DebugNodeItems[0];
            }
        }
        public override void Dispose()
        {
            base.Dispose();
            foreach (var item in DebugNodeItems)
            {
                item.Content.Dispose();
            }
        }
        partial void OnSelectedNodeItemChanged(DebugNodeItem? oldValue, DebugNodeItem? newValue)
        {
            oldValue?.Content.GoToBackground();
            Content = newValue?.Content;
            newValue?.Content.Update();
        }
    }
    public partial class DebugNodeItem : ObservableObject
    {
        [ObservableProperty]
        private string _canIndex;
        [ObservableProperty]
        private string _title;
        [ObservableProperty]
        private bool _enable = false;
        [ObservableProperty]
        private AbstractDebugViewModel _content;
        public DebugNodeItem(string canIndex, string title, bool enable, AbstractDebugViewModel o)
        {
            _canIndex = canIndex;
            Title = title;
            Content = o;

            Enable = enable;//确保Enable在Content.CanNode赋值后再赋值
        }
        partial void OnEnableChanged(bool oldValue, bool newValue)
        {
            Content.IsEnable = newValue;
        }
    }
}
