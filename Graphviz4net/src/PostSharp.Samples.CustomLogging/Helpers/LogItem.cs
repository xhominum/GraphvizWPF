using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace PostSharp.Samples.CustomLogging.Helpers
{
    public class Void
    {
        
    }

    public class LogItem
    {
        public Guid LogItemGuid { get; set; }
        public int LogItemSessionId { get; set; }
        public Guid SessionGuid { get; set; }
        public int HashCode { get; set; }
        public long ObjectId { get; set; }
        public int ThreadId { get; set; }
        public int ProcessId { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string Namespace { get; set; }
        public long Milliseconds { get; set; }
        public string ParentNamespace { get; set; }
        public string ParentClass { get; set; }
        public string ParentMethod { get; set; }
        public LogHelper.LoggingType LoggingType { get; set; }
        //public object ReturnValue { get; set; }
    }
}
