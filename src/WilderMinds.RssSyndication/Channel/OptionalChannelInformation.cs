using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace WilderMinds.RssSyndication.Channel
{
    public class OptionalChannelInformation
    {
        /// <summary> 
        /// Copyright notice for the contents of the feed.
        /// </summary>
        public string Copyright { get; set; }

        /// <summary> 
        /// The language the feed is written in (e.g. en-US).
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// The date and time when the feed's content is to be published. 
        /// Some feed readers may choose not to display a feed until its Pub Date has arrived.
        /// </summary>
        public DateTime PubDate { get; set; }

        /// <summary>
        /// The last time the content of the channel changed.
        /// </summary>
        public DateTime BuildDate { get; set; }

        /// <summary>
        /// Email address for person responsible for editorial content.
        /// </summary>
        public string ManagingEditor { get; set; }

        /// <summary> 
        /// Email address for person responsible for technical issues relating to channel.
        /// </summary>
        public string WebMaster { get; set; }

        /// <summary> 
        /// One or more categories that the channel belongs to (e.g. Newspapers).
        /// </summary>
        public IList<string> Categories { get; set; } = new List<string>();

        /// <summary> 
        /// The number of minutes a channel can be cached by readers before refreshing from the source.
        /// </summary>
        public int TimeToLive { get; set; }

        /// <summary> 
        /// The URL for a webpage that describes the specification for this version of the RSS format.
        /// </summary>
        public Uri Docs { get; set; }

        /// <summary>
        /// Specifies a text input box that can be displayed with the channel. 
        /// </summary>
        public TextInput TextInput { get; set; }

        /// <summary>
        /// A string indicating the program used to generate the channel.
        /// </summary>
        public string Generator { get; set; }

        /// <summary>
        /// Specifies a GIF, JPEG or PNG image that can be displayed with the channel.
        /// </summary>
        public ChannelImage Image { get; set; }

        /// <summary>
        /// Allows processes to register with a cloud to be notified of updates to the channel, 
        /// implementing a lightweight publish-subscribe protocol for RSS feeds. 
        /// </summary>
        /// <example>
        /// &lt;cloud domain="rpc.sys.com" port="80" path="/RPC2" registerProcedure="myCloud.rssPleaseNotify" protocol="xml-rpc" /&gt;
        /// In this example, to request notification on the channel it appears in, you would send an XML-RPC message to rpc.sys.com on port 80, with a path of /RPC2. 
        /// The procedure to call is myCloud.rssPleaseNotify.
        /// </example>
        public Cloud Cloud { get; set; }

        internal void AddOptionalElements(XElement channel)
        {
            channel.AddOptionalElement("copyright", Copyright);
            channel.AddOptionalElement("language", Language);
            channel.AddOptionalElement("pubDate", PubDate);
            channel.AddOptionalElement("buildDate", BuildDate);
            channel.AddOptionalElement("managingEditor", ManagingEditor);
            channel.AddOptionalElement("webMaster", WebMaster);

            if (TimeToLive > 0)
            {
                channel.AddOptionalElement("ttl", TimeToLive);
            }

            channel.AddOptionalElement("docs", Docs);
            channel.AddOptionalElement("generator", Generator);

            if (Image != null)
            {
                channel.Add(Image.Serialize());
            }

            if (TextInput != null)
            {
                channel.Add(TextInput.Serialize());
            }

            if (Cloud != null)
            {
                channel.Add(Cloud.Serialize());
            }

            if (Categories != null && Categories.Count > 0)
            {
                foreach (var category in Categories)
                {
                    channel.AddOptionalElement("category", category);
                }
            }
        }
    }
}