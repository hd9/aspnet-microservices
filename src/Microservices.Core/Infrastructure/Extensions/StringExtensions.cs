using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Core.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool HasValue(this string val, int minLength = 0)
        {
            if (val == null)
                return false;

            return minLength == 0 ?
                !string.IsNullOrEmpty(val) : 
                (val ?? "").Length >= minLength;
        }

        public static string FormatWith(this string val, params object[] args)
        {
            if (val == null)
                return null;

            return string.Format(val, args);
        }

        public static string Append(this string val, params object[] args)
        {
            if (val == null)
                return string.Join("", args);

            return val + string.Join("", args);
        }

        public static string MaskCC(this string number)
        {
            return number.HasValue(4) ?
                $"{number.Substring(0, 2)}-XXXX-XXXX-{number.Substring(number.Length - 2, 2)}" :
                null;
        }
    }
}
