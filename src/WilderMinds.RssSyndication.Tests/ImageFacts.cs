using System;
using System.Linq;
using WilderMinds.RssSyndication.Channel;
using Xunit;

namespace RssSyndication.Tests
{
  public class ImageFacts
  {
    private static readonly Uri ImageUri = new Uri("http://example.com/nyancat.jpg");

    [Fact]
    public void ImageIsOptional()
    {
      var feed = TestData.CreateTestFeed();

      feed.OptionalInformation = new OptionalInformation();

      var doc = TestData.ReadSerialized(feed);

      Assert.Empty(doc.Descendants("image"));
    }

    [Fact]
    public void AllImageParametersRequired()
    {
      ThrowsArgumentNull(() => new ChannelImage(ImageUri, TestData.FeedUrl, null));
      ThrowsArgument(() => new ChannelImage(ImageUri, TestData.FeedUrl, string.Empty));
      ThrowsArgument(() => new ChannelImage(ImageUri, TestData.FeedUrl, "    "));
      ThrowsArgumentNull(() => new ChannelImage(ImageUri, null, TestData.FeedTitle));
      ThrowsArgumentNull(() => new ChannelImage(null, TestData.FeedUrl, TestData.FeedTitle));
    }

    [Fact]
    public void ImageCreatedFromFeed()
    {
      var feed = TestData.CreateTestFeed();

      var image = new ChannelImage(ImageUri, feed);

      Assert.Equal(feed.Link, image.Link);
      Assert.Equal(feed.Title, image.Title);

      Assert.Equal(ImageUri, image.Url);
    }

    [Fact]
    public void ImageSerializedCorrectly()
    {
      var feed = TestData.CreateTestFeed();

      feed.OptionalInformation = new OptionalInformation
      {
        Image = new ChannelImage(ImageUri, feed)
      };

      var doc = TestData.ReadSerialized(feed);

      var image = doc.Descendants("image").Single();

      var link = image.Descendants("link").Single().Value;
      var url = image.Descendants("url").Single().Value;
      var title = image.Descendants("title").Single().Value;

      Assert.Equal(feed.Link.AbsoluteUri, link);
      Assert.Equal(ImageUri.AbsoluteUri, url);
      Assert.Equal(feed.Title, title);

      Assert.Equal(3, image.Descendants().Count());
    }

    [Fact]
    public void ImageOptionalElementsSerialzedCorrectly()
    {
      const string imageDescription = "Cabbage";

      var feed = TestData.CreateTestFeed();

      feed.OptionalInformation = new OptionalInformation
      {
        Image = new ChannelImage(ImageUri, feed)
        {
          Width = 52,
          Height = 25,
          Description = imageDescription
        }
      };

      var doc = TestData.ReadSerialized(feed);

      var image = doc.Descendants("image").Single();

      var width = image.Descendants("width").Single().Value;
      var height = image.Descendants("height").Single().Value;
      var description = image.Descendants("description").Single().Value;

      Assert.Equal("52", width);
      Assert.Equal("25", height);
      Assert.Equal(imageDescription, description);
    }

    private static void ThrowsArgumentNull(Action action)
    {
      Assert.Throws<ArgumentNullException>(action);
    }

    private static void ThrowsArgument(Action action)
    {
      Assert.Throws<ArgumentException>(action);
    }
  }
}