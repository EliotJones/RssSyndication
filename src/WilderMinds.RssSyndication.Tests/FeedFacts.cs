using System;
using System.Linq;
using WilderMinds.RssSyndication;
using WilderMinds.RssSyndication.Channel;
using Xunit;

namespace RssSyndication.Tests
{
  public class FeedFacts
  {
    [Fact]
    public void FeedIsCreated()
    {
      var feed = new Feed(TestData.FeedTitle, TestData.FeedUrl, TestData.FeedDescription)
        {
          ChannelInformation = new OptionalChannelInformation
          {
            Copyright = TestData.FeedCopyright
          }
        };

      Assert.NotNull(feed);
      Assert.Equal(feed.Title, TestData.FeedTitle);
      Assert.Equal(feed.Description, TestData.FeedDescription);
      Assert.Equal(feed.Link, TestData.FeedUrl);
      Assert.Equal(feed.ChannelInformation.Copyright, TestData.FeedCopyright);
    }

    

    [Fact]
    public void FeedAddsItems()
    {
      var feed = TestData.CreateTestFeed();

      Assert.NotNull(feed.Items.First());
      Assert.Equal(feed.Items.First().Title, "Foo Bar");
      Assert.Equal(feed.Items.ElementAt(1).Title, "Quux");
      Assert.Equal(feed.Items.First().Author.Name, "Shawn Wildermuth");
    }

    [Fact]
    public void CreatesValidRssWithItems()
    {
      var feed = TestData.CreateTestFeed();

      var doc = TestData.ReadSerialized(feed);

      Assert.NotNull(doc);
      var item = doc.Descendants("item").FirstOrDefault();
      Assert.NotNull(item);
      Assert.True(item.Element("title").Value == "Foo Bar", "First Item was correct");
    }

    public void CreatesValidRssNoChildItems()
    {
      var feed = new Feed(TestData.FeedTitle, TestData.FeedUrl, TestData.FeedDescription);

      var doc = TestData.ReadSerialized(feed);

      Assert.Equal("rss", doc.Root.Name);
      
      var version = doc.Root.Attribute("version");

      Assert.Equal("2.0", version.Value);

      var feedElement = doc.Root.Descendants().Single().Name;

      Assert.Equal("channel", feedElement);

      Assert.Empty(doc.Descendants("item"));
      
      Assert.Empty(doc.Descendants("copyright"));
    }

    [Fact]
    public void CreatesValidRssContainingTitleLinkDescription()
    {
      var feed = new Feed(TestData.FeedTitle, TestData.FeedUrl, TestData.FeedDescription);

      var doc = TestData.ReadSerialized(feed);

      var channel = doc.Descendants("channel").Single();

      var title = channel.Descendants("title").Single();
      var link = channel.Descendants("link").Single();
      var description = channel.Descendants("description").Single();

      Assert.Equal(TestData.FeedTitle, title.Value);
      Assert.Equal(TestData.FeedUrl.AbsoluteUri, link.Value);
      Assert.Equal(TestData.FeedDescription, description.Value);
    }

    [Fact]
    public void Create_ChannelLinkRequired()
    {      
      Assert.Throws<ArgumentNullException>(() => new Feed(TestData.FeedTitle, null, TestData.FeedDescription));
    }

    [Fact]
    public void Create_ChannelTitleRequired()
    {
      Assert.Throws<ArgumentNullException>(() => new Feed(null, TestData.FeedUrl, TestData.FeedDescription));
      Assert.Throws<ArgumentException>(() => new Feed(string.Empty, TestData.FeedUrl, TestData.FeedDescription));
      Assert.Throws<ArgumentException>(() => new Feed("    ", TestData.FeedUrl, TestData.FeedDescription));
    }

    [Fact]
    public void Create_ChannelDescriptionRequired()
    {
      Assert.Throws<ArgumentNullException>(() => new Feed(TestData.FeedTitle, TestData.FeedUrl, null));
      Assert.Throws<ArgumentException>(() => new Feed(TestData.FeedTitle, TestData.FeedUrl, string.Empty));
      Assert.Throws<ArgumentException>(() => new Feed(TestData.FeedTitle, TestData.FeedUrl, "\t"));
    }

    [Fact]
    public void ChannelCopyrightOptional()
    {
      var feed = new Feed(TestData.FeedTitle, TestData.FeedUrl, TestData.FeedDescription);

      var rss = feed.Serialize();

      Assert.NotEmpty(rss);
    }
  }
}
