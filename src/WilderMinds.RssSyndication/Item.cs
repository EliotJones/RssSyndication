using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace WilderMinds.RssSyndication
{
    public class Item
    {
        public Author Author { get; set; }

        public string Body { get; set; }

        public ICollection<string> Categories { get; set; } = new List<string>();

        public Uri Comments { get; set; }

        public Uri Link { get; set; }

        public string Permalink { get; set; }

        public DateTime PublishDate { get; set; }

        public string Title { get; set; }

        internal bool TrySerialize(FeedType type, out XElement item)
        {
            item = null;

            if (string.IsNullOrWhiteSpace(Body) && string.IsNullOrWhiteSpace(Title))
            {
                return false;
            }

            item = new XElement(type.GetTypedName("entry", "item"));

            item.AddOptionalElement(type.GetTypedName("title"), Title);

            if (type == FeedType.Atom1)
            {
                if (!string.IsNullOrWhiteSpace(Body))
                {
                    var contentType = Body.Contains("<") ? "html" : "text";

                    var contentElement = new XElement(Feed.AtomNamespace + "content", new XAttribute("type", contentType), Body);

                    item.Add(contentElement);
                }
            }
            else
            {
                item.AddOptionalElement("description", Body);
            }

            if (Link != null)
            {
                item.Add(type.GetTypedLinkElement("link", Link));

                if (type == FeedType.Atom1)
                {
                    item.Add(new XElement(type.GetTypedName("id"), Link.AbsoluteUri));
                }
            }

            if (Author != null)
            {
                item.Add(Author.Serialize(type));
            }

            type.AddTypedCategories(item, Categories);

            item.AddOptionalElement(type.GetTypedName("comments"), Comments?.AbsoluteUri);
            item.AddOptionalElement(type.GetTypedName("id", "guid"), Permalink);

            if (PublishDate != DateTime.MinValue)
            {
                item.Add(new XElement(type.GetTypedName("updated", "pubDate"), type.GetTypedDate(PublishDate)));
            }

            return true;
        }
    }
}