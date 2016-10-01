using System;
using System.Collections.Generic;
using System.Xml.Linq;
using WilderMinds.RssSyndication.Channel;

namespace WilderMinds.RssSyndication
{
  public class Feed
  {
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

    public OptionalChannelInformation ChannelInformation { get; set; }

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

    public string Serialize()
    {
      var doc = new XDocument(new XElement("rss"));
      doc.Root.Add(new XAttribute("version", "2.0"));
      
      var channel = CreateChannel(); 

      doc.Root.Add(channel);

      foreach (var item in Items)
      {
        XElement itemElement;
        if (item.TrySerialize(out itemElement))
        {
          channel.Add(itemElement);
        }
      }

      return doc.ToString();
    }

    private XElement CreateChannel()
    {
      var channel = new XElement("channel");

      // Add the required elements.
      channel.Add(new XElement("title", this.Title));
      channel.Add(new XElement("link", this.Link.AbsoluteUri));
      channel.Add(new XElement("description", this.Description));
      
      ChannelInformation?.AddOptionalElements(channel);

      return channel;
    }
  }
}
