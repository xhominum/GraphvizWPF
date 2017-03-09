using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PostSharp.Samples.CustomLogging.Helpers
{
    public class LogSession
    {
        private int _logItemId;
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid SessionGuid{ get; private set; }
        private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        private int LogItemId
        {
            get { return Interlocked.Increment(ref _logItemId); }
        }

        public LogSession()
        {
            SessionGuid = Guid.NewGuid();
        }

        public LogItem GetNewLogItem()
        {
            return new LogItem {LogItemGuid = Guid.NewGuid(), LogItemSessionId = this.LogItemId, SessionGuid = SessionGuid, Milliseconds = _stopwatch.ElapsedMilliseconds };
        }
    }
}
