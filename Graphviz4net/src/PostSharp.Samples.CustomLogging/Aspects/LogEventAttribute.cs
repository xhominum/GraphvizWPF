using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;
using PostSharp.Serialization;
using PostSharp.Samples.CustomLogging.Helpers;

namespace PostSharp.Samples.CustomLogging.Aspects
{

    [PSerializable]
    public sealed class LogEventAttribute : EventInterceptionAspect
    {
        public override void OnInvokeHandler(EventInterceptionArgs args)
        {

            LogHelper.SaveEventInfo(args, LogHelper.LoggingType.EventRaised);
            base.OnInvokeHandler(args);
        }
    }
}
