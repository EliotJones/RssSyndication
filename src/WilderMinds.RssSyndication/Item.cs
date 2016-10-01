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

        internal bool TrySerialize(out XElement item)
        {
            item = null;

            if (string.IsNullOrWhiteSpace(Body) && string.IsNullOrWhiteSpace(Title))
            {
                return false;
            }

            item = new XElement("item");

            item.AddOptionalElement("title", Title);
            item.AddOptionalElement("link", Link?.AbsoluteUri);
            item.AddOptionalElement("description", Body);

            if (Author != null)
            {
                item.Add(new XElement("author", $"{Author.Email} ({Author.Name})"));
            }

            if (Categories != null && Categories.Count > 0)
            {
              foreach (var category in Categories)
              {
                item.AddOptionalElement("category", category);
              }
            }

            item.AddOptionalElement("comments", Comments?.AbsoluteUri);
            item.AddOptionalElement("guid", Permalink);
            item.AddOptionalElement("pubDate", PublishDate);

            return true;
        }
    }
}