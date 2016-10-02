using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using WilderMinds.RssSyndication.Channel;

namespace WilderMinds.RssSyndication
{
    public class Feed
    {
        internal static readonly XNamespace AtomNamespace = "http://www.w3.org/2005/Atom";

        /// <summary> 
        /// Required: The name of the channel (usually the name of the website).
        /// </summary>
        public string Title { get; }

        /// <summary> 
        /// Required: The URL to the website corresponding to the channel.
        /// </summary>
        public Uri Link { get; }

        /// <summary> 
        /// Required: A phrase or sentence describing the channel.
        /// </summary>
        public string Description { get; }

        public OptionalInformation OptionalInformation { get; set; }

        public ICollection<Item> Items { get; set; } = new List<Item>();

        /// <summary>
        /// Creates a new <see cref="Feed"/> with the channel information required by the RSS 2.0 specification.
        /// </summary>
        /// <param name="title">The name of the channel (usually the name of the website).</param>
        /// <param name="link">The URL to the website corresponding to the channel.</param>
        /// <param name="description">A phrase or sentence describing the channel.</param>
        public Feed(string title, Uri link, string description)
        {
            Guard.CheckNullOrWhitespace(nameof(title), title);
            Guard.CheckNullOrWhitespace(nameof(description), description);
            Guard.CheckNull(nameof(link), link);

            Title = title;
            Link = link;
            Description = description;
        }

        public string Serialize(FeedType type = FeedType.Rss2)
        {
            var doc = new XDocument(new XDeclaration("1.0", "utf-8", string.Empty),
              GetRootElement(type));

            var channel = type == FeedType.Atom1 ? doc.Root : new XElement("channel");

            AddChannelElements(type, channel);

            if (type != FeedType.Atom1)
            {
                doc.Root.Add(channel);
            }

            foreach (var item in Items)
            {
                XElement itemElement;
                if (item.TrySerialize(type, out itemElement))
                {
                    channel.Add(itemElement);
                }
            }

            using (var writer = new XmlStringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings{
                    Indent = true
                }))
                {
                    doc.Save(xmlWriter);
                }

                return writer.ToString();
            }
        }

        private static XElement GetRootElement(FeedType type)
        {
            switch (type)
            {
                case FeedType.Atom1:
                    return new XElement(AtomNamespace + "feed");
                default:
                    return new XElement("rss", new XAttribute("version", "2.0"));
            }
        }

        private void AddChannelElements(FeedType type, XElement channel)
        {
            // Add the required elements.
            channel.Add(new XElement(type.GetTypedName("title"), this.Title));
            channel.Add(type.GetTypedLinkElement("link", this.Link));
            channel.Add(new XElement(type.GetTypedName("subtitle", "description"), this.Description));

            if (type == FeedType.Atom1)
            {
                AddAtomRequiredElements(channel);
            }

            OptionalInformation?.AddOptionalElements(type, channel);
        }

        private void AddAtomRequiredElements(XElement channel)
        {
            channel.Add(new XElement(AtomNamespace + "id", this.Link.AbsoluteUri));

            var localItems = Items ?? new Item[0];

            var lastUpdate = DateTime.UtcNow;

            if (OptionalInformation != null && OptionalInformation.BuildDate != DateTime.MinValue)
            {
                lastUpdate = OptionalInformation.BuildDate;
            }
            else
            {
                var newestItem = localItems
                  .OrderByDescending(x => x.PublishDate)
                  .FirstOrDefault();

                if (newestItem != null && newestItem.PublishDate != DateTime.MinValue)
                {
                    lastUpdate = newestItem.PublishDate;
                }
            }

            channel.Add(new XElement(AtomNamespace + "updated", lastUpdate.ToAtomCompatibleString()));

            AddAtomAuthor(channel, localItems);
        }

        private void AddAtomAuthor(XElement channel, IEnumerable<Item> localItems)
        {
            if (localItems.All(x => x.Author != null))
            {
                return;
            }

            var author = OptionalInformation?.Author;
            if (author == null)
            {
                var authorFromItems = localItems.FirstOrDefault()?.Author;
            }

            if (author == null)
            {
                author = new Author { Name = "Unknown" };
            }

            channel.Add(author.Serialize(FeedType.Atom1));
        }
    }
}
