using System;
using System.Xml.Linq;

namespace WilderMinds.RssSyndication
{
    internal static class RssSerializationExtensions
    {
        public static void AddOptionalElement(this XElement parent, string name, object value)
        {
            if (value == null)
            {
                return;
            }

            var valueString = value as string;
            if (string.IsNullOrWhiteSpace(valueString))
            {
                return;
            }

            if (value is DateTime)
            {
                var date = (DateTime)value;

                if (date != DateTime.MinValue)
                {
                    parent.Add(new XElement(name, date.ToRssCompatibleString()));
                }

                return;
            }

            parent.Add(new XElement(name, value));
        }

        public static string ToRssCompatibleString(this DateTime date)
        {
            return string.Concat(date.ToString("ddd',' d MMM yyyy HH':'mm':'ss"), " ", date.ToString("zzzz").Replace(":", ""));
        }
    }
}