using System.Text;
using PostSharp.Aspects;
using PostSharp.Samples.CustomLogging.Helpers;
using PostSharp.Serialization;

namespace PostSharp.Samples.CustomLogging.Aspects
{
    /// <summary>
    ///     Aspect that, when applied to a field or property, appends a record to the <see cref="Logger" /> class whenever this
    ///     field or property is set to a new value.
    /// </summary>
    [PSerializable]
    public sealed class LogSetValueAttribute : LocationInterceptionAspect
    {
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            LogHelper.SaveValueInfo(args, LogHelper.LoggingType.SetValue);
            args.ProceedSetValue();
            //base.OnSetValue(null);
        }

        public override void OnGetValue(LocationInterceptionArgs args)
        {
            LogHelper.SaveValueInfo(args, LogHelper.LoggingType.GetValue);
            args.ProceedGetValue();
            //base.OnGetValue(args);()
        }

        //private static void AppendCallInformation(LocationInterceptionArgs args, StringBuilder stringBuilder)
        //{

        //    stringBuilder.Append(args.LocationName);

        //    if (args.Instance != null)
        //        stringBuilder.AppendFormat(", Hashcode:{0}\r\n", args.Instance.GetHashCode());
        //}
    }
}