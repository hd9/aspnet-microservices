using System;
using System.Collections.Generic;
using System.Text;

namespace Microservices.Core.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToExpDate(this DateTime date)
        {
            return date == null ?
                null :
                date.ToString("MM/yy");
        }
    }
}
