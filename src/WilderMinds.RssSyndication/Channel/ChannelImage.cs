using System;
using System.Xml.Linq;

namespace WilderMinds.RssSyndication.Channel
{
    public class ChannelImage
    {
        /// <summary>
        /// The URL of a GIF, JPEG or PNG image that represents the channel.
        /// </summary>
        public Uri Url { get; }

        /// <summary>
        /// Text to describe the image. 
        /// Used in the ALT attribute of the HTML &lt;img&gt; tag when the channel is rendered in HTML. 
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The URL of the site, when the channel is rendered, the image is a link to the site. 
        /// </summary>
        public Uri Link { get; }

        private bool hasSetWidth;
        private int width = 88;

        /// <summary>
        /// Width of the image in pixels, max: 144, default: 88.
        /// </summary>
        public int Width
        {
            get { return width; }
            set
            {
                hasSetWidth = true;

                if (value < 0)
                {
                    width = 0;
                }

                if (value > 144)
                {
                    width = 144;
                }

                width = value;
            }
        }

        private bool hasSetHeight;
        private int height = 31;

        /// <summary>
        /// Height of the image in pixels, max: 400, default: 31.
        /// </summary>
        public int Height
        {
            get { return height; }
            set
            {
                hasSetHeight = true;

                if (value < 0)
                {
                    height = 0;
                }

                if (value > 400)
                {
                    height = 400;
                }

                height = value;
            }
        }

        /// <summary>
        /// The text that is included in the TITLE attribute of the link formed around the image in the HTML rendering.
        /// </summary>
        public string Description { get; set; }

        public ChannelImage(Uri imageUrl, Uri channelLink, string title)
        {
            Guard.CheckNull(nameof(imageUrl), imageUrl);
            Guard.CheckNull(nameof(channelLink), channelLink);
            Guard.CheckNullOrWhitespace(nameof(title), title);

            Url = imageUrl;
            Link = channelLink;
            Title = title;
        }

        public ChannelImage(Uri imageUrl, Feed feed) 
            : this(imageUrl, feed?.Link, feed?.Title)
        {
        }

        internal XElement Serialize(FeedType type)
        {
            if (type == FeedType.Atom1)
            {
                return new XElement(Feed.AtomNamespace + "logo", Url.AbsoluteUri);
            }

            var image = new XElement("image");

            image.Add(new XElement("url", Url.AbsoluteUri));
            image.Add(new XElement("link", Link.AbsoluteUri));
            image.Add(new XElement("title", Title));

            image.AddOptionalElement("description", Description);

            if (hasSetHeight)
            {
                image.Add(new XElement("height", Height));
            }

            if (hasSetWidth)
            {
                image.Add(new XElement("width", Width));
            }

            return image;
        }
    }
}