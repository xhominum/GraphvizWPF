using System.Reflection;
using System.Text;
using PostSharp.Aspects;
using PostSharp.Serialization;
using PostSharp.Samples.CustomLogging.Helpers;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace PostSharp.Samples.CustomLogging.Aspects
{
    /// <summary>
    ///     Aspect that, when applied to a method, appends a record to the <see cref="Logger" /> class whenever this method is
    ///     executed.
    /// </summary>
    [PSerializable]
    public sealed class LogMethodAttribute : OnMethodBoundaryAspect
    {


        public LogMethodAttribute()
        {

        }
        /// <summary>
        ///     Method invoked before the target method is executed.
        /// </summary>
        /// <param name="args">Method execution context.</param>
        //public override void OnEntry(MethodExecutionArgs args)
        //{
        //    try
        //    { 
        //        LogHelper.SaveCallInfo(args, LogHelper.LoggingType.MethodEntry);
        //    }
        //    catch (Exception e)
        //    {
        //        System.Diagnostics.Debug.WriteLine(e.Message);
        //        throw;
        //    }
        //}

        


        /// <summary>
        ///     Method invoked after the target method has successfully completed.
        /// </summary>
        /// <param name="args">Method execution context.</param>
        public override void OnSuccess(MethodExecutionArgs args)
        {
            LogHelper.SaveCallInfo(args, LogHelper.LoggingType.MethodSuccess);
        }

        /// <summary>
        ///     Method invoked when the target method has failed.
        /// </summary>
        /// <param name="args">Method execution context.</param>
        public override void OnException(MethodExecutionArgs args)
        {
            LogHelper.SaveCallInfo(args, LogHelper.LoggingType.MethodException);
  
            args.FlowBehavior = FlowBehavior.RethrowException;
        }

        //private static void AppendCallInformation(MethodExecutionArgs args, StringBuilder stringBuilder)
        //{
        //    var declaringType = args.Method.DeclaringType;
        //    Formatter.AppendTypeName(stringBuilder, declaringType);
        //    stringBuilder.Append('.');
        //    stringBuilder.Append(args.Method.Name);

        //    if(args.Instance != null)
        //      stringBuilder.AppendFormat(", Hashcode:{0}\r\n", args.Instance.GetHashCode());

        //    if (args.Method.IsGenericMethod)
        //    {
        //        var genericArguments = args.Method.GetGenericArguments();
        //        Formatter.AppendGenericArguments(stringBuilder, genericArguments);
        //    }

        //    var arguments = args.Arguments;

        //    Formatter.AppendArguments(stringBuilder, arguments);
        //}
    }
}