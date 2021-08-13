using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Extensions
    {
        public static T GetRandom<T>(this T[] array)
        {
            if (array.Length == 0)
                throw new Exception("Can't get random value from empty array");

            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        public static T GetRandom<T>(this List<T> list)
        {
            if (list == null || list.Count == 0)
                throw new Exception("Can't get random value from empty list");

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T GetRandom<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null || enumerable.Count() == 0)
                throw new Exception("Can't get random value from empty enumerable");

            return enumerable.ElementAt(UnityEngine.Random.Range(0, enumerable.Count()));
        }

        public static Color ChangeAlpha(this Color color, float alpha) => new Color(color.r, color.g, color.b, alpha);
    }
}
