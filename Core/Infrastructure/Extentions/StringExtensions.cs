using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Extentions
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
    }
}
