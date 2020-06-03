using System;

namespace Microservices.Core.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static TEnum Parse<TEnum>(this Enum val)
        {
            return (TEnum)Enum.Parse(
                typeof(TEnum),
                val.ToString(),
                true);
        }
    }
}
