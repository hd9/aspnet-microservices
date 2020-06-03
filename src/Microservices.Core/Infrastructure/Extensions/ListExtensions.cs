using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservices.Core.Infrastructure.Extensions
{
    public static class ListExtensions
    {
        public static void AddIfDoesNotExist<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        public static bool HasAny<T>(this List<T> list)
        {
            return list != null && list.Any();
        }

        public static bool HasAny<T, U>(this Dictionary<T, U> dict)
        {
            return dict != null && dict.Any();
        }

        public static List<string> Intersect(this List<List<string>> listOfLists)
        {
            var ret = new List<string>();
            var init = false;

            foreach (var accessList in listOfLists)
            {
                if (!init)
                {
                    ret.AddRange(accessList);
                    init = true;
                    continue;
                }

                ret.RemoveAll(i => !accessList.Exists(access => access.Equals(i)));
            }

            return ret;
        }

        public static string ToStr(this Dictionary<string, string> d)
        {
            if (!HasAny(d)) return "";

            return string.Join("\n", d.Select(x => x.Key + ": " + x.Value));
        }
    }
}
