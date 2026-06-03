using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace canserver3.Models
{
    public class ProjectModel
    {
        public CANChannelItemModel[]? CANChannels { get; set; }
        public Dictionary<string, AbstractFunctionNodeModel[]?>? CANChannelNodes { get; set; } = new Dictionary<string, AbstractFunctionNodeModel[]?>();
        public ProjectModel()
        {
            
        }
        ~ProjectModel()
        {
            
        }
    }
    [JsonSerializable(typeof(ProjectModel))]
    public partial class ProjectJsonContext : JsonSerializerContext
    {
    }

}
