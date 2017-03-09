using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamedPipeWrapper
{
    [Serializable]
    public class MsgNewSubGraph : MessageType
    {
        public string Name { get; set; }
        public string Parent { get; set; }
    }
}
