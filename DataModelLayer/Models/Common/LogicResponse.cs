using DataModelLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModelLayer.Models.Common
{
    public class LogicResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public IList<string>? ErrorList { get; set; }

        public string CreatedId { get; set; } = string.Empty;
        public ResponseMessageType MessageType { get; set; }
    }
}
