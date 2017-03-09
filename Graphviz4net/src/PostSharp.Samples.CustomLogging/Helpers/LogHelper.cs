using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PostSharp.Aspects;

namespace PostSharp.Samples.CustomLogging.Helpers
{
    public class LogHelper
    {
        public enum LoggingType
        {
            MethodEntry,
            MethodSuccess,
            MethodException,
            SetValue,
            GetValue,
            EventRaised,
        }

        internal static void SaveCallInfo(MethodExecutionArgs args, LoggingType loggingType)
        {
            try
            {
                LogItem item = LogSessionManager.Instance.CurrentSession.GetNewLogItem();
                item.HashCode = -1;

                
                if (args.Instance != null)
                {
                    bool firstTime;
                    item.ObjectId = ObjIDGenerator.Instance.GetId(args.Instance, out firstTime);
                }
                item.ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                item.ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
                item.Name = args.Method.Name;
                Debug.Assert(args.Method.DeclaringType != null, "args.Method.DeclaringType != null");   
                item.ClassName = args.Method.DeclaringType.Name;
                item.Namespace = args.Method.DeclaringType.Namespace;
                item.LoggingType = loggingType;
                //if (args.ReturnValue == null)
                //{
                //    item.ReturnValue = new Void();
                //}
                //else
                //{
                //    item.ReturnValue = args.ReturnValue;
                //}



                try
                {
                    var stackFrame = new StackFrame(3);
                    item.ParentMethod = stackFrame.GetMethod().Name;
                    item.ParentClass = stackFrame.GetMethod().DeclaringType.Name;
                    item.ParentNamespace = stackFrame.GetMethod().DeclaringType.Namespace;
                }
                catch 
                {
                }

                Database.Instance.Add(item);
            }
            catch (Exception ex)
            {
                SimpleLogger.Logger.Log("SaveCallInfo", ex);
                throw;
            }
        }

        internal static void SaveValueInfo(LocationInterceptionArgs args, LoggingType loggingType)
        {
            try
            {
                LogItem item = LogSessionManager.Instance.CurrentSession.GetNewLogItem();

                item.HashCode = -1;

                if (args.Instance != null)
                {
                    bool firstTime;
                    item.ObjectId = ObjIDGenerator.Instance.GetId(args.Instance, out firstTime);
                }

                item.ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                item.ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
                item.Name = args.LocationName;
                
                item.ClassName = args.Location.DeclaringType.Name;
                item.Namespace = args.Location.DeclaringType.Namespace;
                item.LoggingType = loggingType;

                try
                {
                    var stackFrame = new StackFrame(3);
                    item.ParentMethod = stackFrame.GetMethod().Name;
                    item.ParentClass = stackFrame.GetMethod().DeclaringType.Name;
                    item.ParentNamespace = stackFrame.GetMethod().DeclaringType.Namespace;
                }
                catch
                {
                }

                Database.Instance.Add(item);
            }
            catch (Exception ex)
            {
                SimpleLogger.Logger.Log("SaveValueInfo", ex);
                throw;
            }
        }

        internal static void SaveEventInfo(EventInterceptionArgs args, LoggingType loggingType)
        {
            try
            {
                LogItem item = LogSessionManager.Instance.CurrentSession.GetNewLogItem();
                item.HashCode = -1;


                if (args.Instance != null)
                {
                    bool firstTime;
                    item.ObjectId = ObjIDGenerator.Instance.GetId(args.Instance, out firstTime);
                }
                item.ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                item.ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
                item.Name = args.Event.Name;
                Debug.Assert(args.Event.DeclaringType != null, "args.Event.DeclaringType != null");
                item.ClassName = args.Event.DeclaringType.Name;
                item.Namespace = args.Event.DeclaringType.Namespace;
                item.LoggingType = loggingType;
                
                try
                {
                    var stackFrame = new StackFrame(3);
                    item.ParentMethod = stackFrame.GetMethod().Name;
                    item.ParentClass = stackFrame.GetMethod().DeclaringType.Name;
                    item.ParentNamespace = stackFrame.GetMethod().DeclaringType.Namespace;
                }
                catch
                {
                }

                Database.Instance.Add(item);
            }
            catch (Exception ex)
            {
                SimpleLogger.Logger.Log("SaveCallInfo", ex);
                throw;
            }
        }

    }
}
