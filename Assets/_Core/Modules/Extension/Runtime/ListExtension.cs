using System;
using System.Collections.Generic;
using System.Linq;

namespace Kelsey.Extension
{
    public static class ListExtension
    {
        public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }

        public static void KSwap<T>(this IList<T> list, int i, int j)
        {
            (list[i], list[j]) = (list[j], list[i]);
        }

        public static void KShuffle<T>(this IList<T> list, int seed)
        {
            UnityEngine.Random.InitState(seed);
            for (int i = 0; i < list.Count; i++)
            {
                list.KSwap(i, UnityEngine.Random.Range(i, list.Count));
            }
        }

        public static void KShuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list.KSwap(i, UnityEngine.Random.Range(i, list.Count));
            }
        }

        public static bool KIsNullOrMT<T>(this IList<T> list) => list == null || list.Count == 0;
    }
}