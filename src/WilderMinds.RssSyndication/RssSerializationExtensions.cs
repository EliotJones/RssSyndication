using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace WilderMinds.RssSyndication
{
    internal static class RssSerializationExtensions
    {
        public static void AddOptionalElement(this XElement parent, XName name, object value)
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

        public static string ToAtomCompatibleString(this DateTime date)
        {
            return XmlConvert.ToString(date, XmlDateTimeSerializationMode.Utc);
        }

        public static string GetTypedDate(this FeedType type, DateTime date)
        {
            return (type == FeedType.Atom1) ? date.ToAtomCompatibleString() : date.ToRssCompatibleString();
        }

        public static XName GetTypedName(this FeedType type, string name)
        {
            return type.GetTypedName(name, name);
        }

        public static XName GetTypedName(this FeedType type, string atomName, string rssName)
        {
            return (type == FeedType.Atom1) ? Feed.AtomNamespace + atomName : rssName;
        }

        public static XElement GetTypedLinkElement(this FeedType type, string name, Uri link)
        {
            if (type == FeedType.Atom1)
            {
                return new XElement(Feed.AtomNamespace + name,
                    new XAttribute("href", link.AbsoluteUri),
                    new XAttribute("rel", "alternate"));
            }

            return new XElement(name, link.AbsoluteUri);
        }

        public static void AddTypedCategories(this FeedType type, XElement parent, IEnumerable<string> categories)
        {
            if (categories == null)
            {
                return;
            }

            foreach (var category in categories)
            {
                if (type == FeedType.Atom1)
                {
                    if (string.IsNullOrWhiteSpace(category))
                    {
                        continue;
                    }

                    parent.Add(new XElement(Feed.AtomNamespace + "category", new XAttribute("term", category)));
                }
                else
                {
                    parent.AddOptionalElement("category", category);
                }
            }
        }
    }
}