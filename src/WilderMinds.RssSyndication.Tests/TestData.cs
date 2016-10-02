using System;
using System.Diagnostics;
using System.Xml.Linq;
using WilderMinds.RssSyndication;
using WilderMinds.RssSyndication.Channel;

namespace RssSyndication.Tests
{
  public class TestData
  {
    public const string FeedTitle = "Shawn Wildermuth's Blog";
    public const string FeedDescription = "My Favourite Rants and Raves";
    public const string FeedCopyright = "(c) 2016";
    public static readonly Uri FeedUrl = new Uri("http://wildermuth.com/feed");

    public static Feed CreateTestFeed()
    {
      var feed = new Feed(FeedTitle, FeedUrl, FeedDescription)
      {
        OptionalInformation = new OptionalInformation
        {
          Copyright = FeedCopyright
        } 
      };

      var item1 = new Item()
      {
        Title = "Foo Bar",
        Body = "<p>Foo bar</p>",
        Link = new Uri("http://foobar.com/item#1"),
        Permalink = "http://foobar.com/item#1",
        PublishDate = DateTime.UtcNow,
        Author = new Author() { Name = "Shawn Wildermuth", Email = "shawn@wildermuth.com" }
      };

      item1.Categories.Add("aspnet");
      item1.Categories.Add("foobar");

      item1.Comments = new Uri("http://foobar.com/item1#comments");

      feed.Items.Add(item1);

      var item2 = new Item()
      {
        Title = "Quux",
        Body = "<p>Quux</p>",
        Link = new Uri("http://quux.com/item#1"),
        Permalink = "http://quux.com/item#1",
        PublishDate = DateTime.UtcNow,
        Author = new Author() { Name = "Shawn Wildermuth", Email = "shawn@wildermuth.com" }
      };

      item1.Categories.Add("aspnet");
      item1.Categories.Add("quux");

      feed.Items.Add(item2);

      return feed;
    }

    public static XDocument ReadSerialized(Feed feed)
    {
      var rss = feed.Serialize();

      Debug.Write(rss);

      var doc = XDocument.Parse(rss);

      return doc;
    }
  }
}