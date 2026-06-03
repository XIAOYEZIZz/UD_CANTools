using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace canserver3.Models
{
    public class AbstractFunctionNodeModel
    {
        public string FunctionType { get; set; } = string.Empty;
        public string FunctionName { get; set; } = string.Empty;
        public bool IsEnable { get; set; }
    }
}
