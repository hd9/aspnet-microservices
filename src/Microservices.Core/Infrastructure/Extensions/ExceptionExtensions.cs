using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Core.Infrastructure.Extensions
{
    public static class ExceptionExtensions
    {

        /// <summary>
        /// A one-liner way to throw expressions.
        /// Read: https://blog.hildenco.com/2017/08/improving-code-readability-with-generic.html
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class Throw<T>
            where T : Exception
        {
            public static void If(bool isInvalid, string message = null)
            {
                if (isInvalid)
                {
                    var e = Activator.CreateInstance(typeof(T), message) as Exception;
                    if (e != null) throw e;

                    throw Activator.CreateInstance<T>();
                }
            }
        }
    }
}
