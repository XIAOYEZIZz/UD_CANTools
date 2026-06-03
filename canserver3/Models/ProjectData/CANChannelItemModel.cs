using canserver3.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace canserver3.Models
{
    public class CANChannelItemModel
    {
        public int CANIndex { get; set; } = 0;
        public bool IsEnable { get; set; } = true;
        public string? DeviceType { get; set; }
        public int DeviceIndex { get; set; } = 0;
        public int ChannelIndex { get; set; } = 0;
        public bool SupportCANFD { get; set; } = false;
        public string? ArbBaudrate { get; set; }
        public string? DataBaudrate { get; set; } 
        public string? Description { get; set; } = string.Empty;
    }
    [JsonSerializable(typeof(CANChannelItemModel[]))]
    public partial class CANChannelItemJsonContext : JsonSerializerContext { }
}
