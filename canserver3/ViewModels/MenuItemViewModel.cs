using canserver3.Models;
using canserver3.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace canserver3.ViewModels
{
    public class MenuItemViewModel : ViewModelBase, IDisposable
    {
        public string? MenuHeader { get; set; }
        public string? MenuIconName { get; set; }
        public string? Key { get; set; }
        public string? Status { get; set; }

        public bool IsSeparator { get; set; }
        public ObservableCollection<MenuItemViewModel> Children { get; set; } = new();

        public ICommand ActivateCommand { get; set; }

        public MenuItemViewModel(string key, bool init = false)
        {
            Key = key;
            ActivateCommand = new RelayCommand(OnActivate);
            if (init)
                OnActivate();
        }
        public virtual void Dispose()
        {

        }
        private void OnActivate()
        {
            if (IsSeparator || Key is null) return;
            WeakReferenceMessenger.Default.Send(this, MessageTokens.Menu);
        }
    }
    public partial class CANChannelMenuItemViewModel : MenuItemViewModel
    {
        private bool _closed = false;
        private string _canIndex;
        [ObservableProperty]
        private bool _channelEnable;
        [ObservableProperty]
        private bool _channelInited;
        [ObservableProperty]
        private string? _description;
        private CANChannelItemModel _model;
        [ObservableProperty]
        private bool _supportCANFD;
        [ObservableProperty]
        private string? _ArbBaudrate;
        [ObservableProperty]
        private string? _DataBaudrate;
        private Action<CANChannelItemModel, bool>? _onChannelEnableChanged;
        private Action<CANChannelItemModel, uint, uint>? _onBaudrateChanged;
        private Func<CANChannelItemModel, bool>? _onQueryStatus;
        public ObservableCollection<ForwardCanFrameViewModel> ForwardCanFrameItems { get; }
        [ObservableProperty]
        private bool _enablePCAN;
        [ObservableProperty]
        private string _pcanVirtualNetName = "Untitled";

        public CANChannelMenuItemViewModel(string key, string canIndex, CANChannelItemModel model, int totalChannels, Action<CANChannelItemModel, bool> enableAction, Action<CANChannelItemModel, uint, uint> baudrateAction, Func<CANChannelItemModel, bool> queryInitFunc) : base(key, false)
        {
            _model = model;
            _canIndex = canIndex;
            SupportCANFD = model.SupportCANFD;
            ArbBaudrate = model.ArbBaudrate;
            DataBaudrate = model.DataBaudrate;
            ChannelEnable = model.IsEnable;
            Description = model.Description;
            _onChannelEnableChanged = enableAction;
            _onBaudrateChanged = baudrateAction;
            _onQueryStatus = queryInitFunc;
            ForwardCanFrameItems = new();
            for (int i = 0; i < totalChannels; i++)
            {
                var toCanIndex = $"CAN{i + 1}";
                if (toCanIndex == canIndex)
                    continue;
                ForwardCanFrameItems.Add(new ForwardCanFrameViewModel(canIndex, toCanIndex));
            }
            Task.Run(async () =>
            {
                while (!_closed)
                {
                    await Task.Delay(1000);
                    ChannelInited = _onQueryStatus?.Invoke(_model) ?? false;
                }
            });
            
        }
        public override void Dispose()
        {
            
            _closed = true;
            base.Dispose();
            _onChannelEnableChanged = null;
            _onBaudrateChanged = null;
            _onQueryStatus = null;
            try
            {//这里会被多次Dispose，直接catch异常
                foreach (var item in ForwardCanFrameItems)
                {
                }
            }
            catch
            {
            }

            ForwardCanFrameItems.Clear();
        }

        private void HandleCANEnable(string ch, object o)
        {
            ChannelEnable = o.ToString() == "1";
        }

        partial void OnChannelEnableChanged(bool value)
        {
            _onChannelEnableChanged?.Invoke(_model, value);
        }
        partial void OnEnablePCANChanged(bool value)
        {

        }
    }
    public partial class ForwardCanFrameViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _fromCanIndex = string.Empty;
        [ObservableProperty]
        private string _toCanIndex = string.Empty;
        [ObservableProperty]
        private bool _forwarded;
        public ForwardCanFrameViewModel(string fromCanIndex, string toCanIndex)
        {
            FromCanIndex = fromCanIndex;
            ToCanIndex = toCanIndex;
            WeakReferenceMessenger.Default.Register<ForwardCanFrameViewModel, ForwardCanFrameViewModel, string>(this, MessageTokens.ForwardCanFrame + FromCanIndex + ToCanIndex, (s, o) =>
            {
                //主动发起的一方和另一方的Node刚好是反过来的
                Forwarded = o.Forwarded;
            });
        }
        [RelayCommand]
        private void ForwardCanFrame()
        {
            if (Forwarded)
            {

            }
            else
            {
         
            }
            WeakReferenceMessenger.Default.Send(this, MessageTokens.ForwardCanFrame + ToCanIndex + FromCanIndex);
        }
    }
}
