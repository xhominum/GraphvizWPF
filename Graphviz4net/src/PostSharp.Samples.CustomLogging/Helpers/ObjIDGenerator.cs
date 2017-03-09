using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PostSharp.Samples.CustomLogging.Helpers
{
    public sealed class ObjIDGenerator
    {
        private static volatile ObjIDGenerator instance;
        private static object syncRoot = new Object();

        private readonly ObjectIDGenerator _objIDGenerator = new ObjectIDGenerator();

        private ObjIDGenerator() { }

        public static ObjIDGenerator Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ObjIDGenerator();
                    }
                }

                return instance;
            }
        }

        public long GetId(object obj)
        {
            bool firstTime;
            return _objIDGenerator.GetId(obj, out firstTime);
        }

        public long GetId(object obj, out bool firstTime)
        {
            return _objIDGenerator.GetId(obj, out firstTime);
        }

        public long HasId(object obj, out bool firstTime)
        {
            return _objIDGenerator.HasId(obj, out firstTime);
        }
    }
}


