using System;
using PostSharp.Samples.CustomLogging.Aspects;


// Add logging to System.Math to show we can add logging to anything.

//[assembly:
//    LogMethod(AttributePriority = 2, AttributeTargetAssemblies = "mscorlib", AttributeTargetTypes = "System.Math")]

namespace PostSharp.Samples.CustomLogging
{
    internal static class Program
    {
        [LogSetValue] private static int Value;

        public class TestClass
        {
            public void TestCall()
            {
                Console.WriteLine(String.Format("True hashcode: {0}\r\n", this.GetHashCode()));
            }

            [LogSetValue]
            public int Value;
        }

        private static void Main()
        {

            Helpers.Database.Instance.Query();
            //var Toto = new TestClass();
            //Toto.TestCall();
            //// Demonstrate that we can create a nice hierarchical log including parameter and return values.
            //Value = 55;

            //Toto.Value = 4333;

            //// Demonstrate how exceptions are logged.
            //try
            //{
            //    Fibonacci(-1);
            //}
            //catch
            //{
            //}

            //// Demonstrate that we can add logging to system methods, too.
            //Console.WriteLine(Math.Sin(5));
            //Helpers.Database.Instance.Query();
        }


        private static int Fibonacci(int n)
        {
            if ( n < 0 )
                throw new ArgumentOutOfRangeException();
            if (n == 0)
                return 0;
            if (n == 1)
                return 1;

            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }
    }
}