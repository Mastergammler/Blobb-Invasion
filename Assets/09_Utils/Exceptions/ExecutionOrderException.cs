using System;

namespace BlobbInvasion.Utilities.Exceptions
{
    public class ExecutionOrderException : Exception
    {
        public ExecutionOrderException() 
                : base("Another method was expected to be called before."){}

        public ExecutionOrderException(String calledMethodName, String expectedMethodCallBefore, String reason) 
                : base (buildExceptionMessage(calledMethodName,expectedMethodCallBefore,reason)){}

        private static String buildExceptionMessage(String arg1,String arg2, String arg3)
        {
            String message = "";

            message += $"The method '{arg1}()' was called before '{arg2}()'!";
            message += $"It was expected the other way around because: {arg3}";

            return message;
        }
    }
}