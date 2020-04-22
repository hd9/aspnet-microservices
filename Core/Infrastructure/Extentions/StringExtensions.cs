using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Extentions
{
    public static class StringExtensions
    {
        public static bool HasValue(this string val)
        {
            if (val == null)
                return false;

            return !string.IsNullOrEmpty(val);
        }
    }
}
