
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace Inferno_Mod_Manager.Utils
{
    static class Extensions
    {
        public static T WriteL<T>(this T t, ulong ul) where T : BinaryWriter
        {
            t.Write(ul);
            return t;
        }
        public static T WriteL<T>(this T t, int i) where T : BinaryWriter
        {
            t.Write(i);
            return t;
        }
        public static T WriteL<T>(this T t, byte[] bar) where T : BinaryWriter
        {
            t.Write(bar);
            return t;
        }

        public static double[] ToDoubleArray(this string[] t)
        {
            var doubles = new List<double>();
            for (int i = 0; i < t.Length; i++)
                doubles.Add(double.Parse(t[i]));
            return doubles.ToArray();
        }

        public static ItemCollection AddAll<T>(this ItemCollection t, IEnumerable<T> enumerables)
        {
            IEnumerator<T> enumerator = enumerables.GetEnumerator();
            while (enumerator.MoveNext())
                t.Add(enumerator.Current);
            return t;
        }

        public static string[] Combine(this string[] t, string[] a)
        {
            var l = new List<string>();
            foreach (var v in t)
                l.Add(v);
            foreach (var v in a)
                l.Add(v);
            return l.ToArray();
        }

        public static bool ContainsAll(this string t, params string[] a) {
            foreach (var b in a)
                if (!t.ToLower().Contains(b.ToLower()))
                    return false;

            return true;
        }
    }
}
