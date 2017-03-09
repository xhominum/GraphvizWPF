using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostSharp.Samples.CustomLogging.Helpers
{
    public sealed class LogSessionManager
    {
        private static volatile LogSessionManager _instance;
        private static object SyncRoot = new Object();

        //private readonly LogSessionManager _logSessionManager = new LogSessionManager();

        private readonly LogSession _currentSession;
        public LogSession CurrentSession
        {
            get
            {
                return _currentSession;
            }
        }

        private LogSessionManager()
        { 
            _currentSession = new LogSession();
            
        }

        public static LogSessionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new LogSessionManager();
                    }
                }

                return _instance;
            }
        }

        

        public void EndSession()
        {
            //TODO
        }

        public void StartSession()
        {
            //TODO
        }
    }
}
