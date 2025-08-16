using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schemio.Core.Helpers
{
    public static class EnumerableExtentions
    {
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable), "Enumerable cannot be null");

            if (action == null)
                throw new ArgumentNullException(nameof(action), "Action cannot be null");

            foreach (var item in enumerable)
                action(item);
        }

        public static string ToCSV<T>(this IEnumerable<T> instance, char separator)
        {
            if (instance == null)
                return null;

            if (!instance.Any())
                return string.Empty;

            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.Each(value => csv.AppendFormat("{0}{1}", value, separator));
                return csv.ToString(0, csv.Length - 1);
            }
            return null;
        }

        public static string ToCSV<T>(this IEnumerable<T> instance)
        {
            if (instance == null)
                return null;

            return instance.ToCSV(',');
        }
    }
}