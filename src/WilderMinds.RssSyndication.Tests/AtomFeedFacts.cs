using System;
using System.Linq;
using System.Xml.Linq;
using RssSyndication.Tests;
using WilderMinds.RssSyndication.Channel;
using Xunit;

namespace WilderMinds.RssSyndication.Tests
{
    public class AtomFeedFacts
    {
        [Fact]
        public void CreatesCorrectFeed()
        {
            var feed = new Feed(TestData.FeedTitle, TestData.FeedUrl, TestData.FeedDescription);

            var atom = feed.Serialize(FeedType.Atom1);

            var doc = XDocument.Parse(atom);

            var outputFeed = doc.Descendants().Single(x => x.Name.LocalName == "feed");

            Assert.Equal("feed", outputFeed.Name.LocalName);

            Assert.Equal("http://www.w3.org/2005/Atom", outputFeed.Name.Namespace);
        }

        [Fact]
        public void CanCreateSampleAtomFeed()
        {
            var sample = @"<?xml version=""1.0"" encoding=""utf-8""?>
<feed xmlns=""http://www.w3.org/2005/Atom"">
  <title>Example Feed</title>
  <link href=""http://example.org/"" rel=""alternate"" />
  <subtitle>My Feed</subtitle>
  <id>http://example.org/</id>
  <updated>2003-12-13T18:30:02Z</updated>
  <author>
    <name>John Doe</name>
  </author>
  <entry>
    <title>Atom-Powered Robots Run Amok</title>
    <content type=""text"">Some text.</content>
    <link href=""http://example.org/2003/12/13/atom03"" rel=""alternate"" />
    <id>http://example.org/2003/12/13/atom03</id>
    <updated>2003-12-13T18:30:02Z</updated>
  </entry>
</feed>";
            var feed = new Feed("Example Feed", new Uri("http://example.org/"), "My Feed");

            feed.OptionalInformation = new OptionalInformation
            {
                Author = new Author { Name = "John Doe" }
            };

            feed.Items.Add(new Item
            {
                Title = "Atom-Powered Robots Run Amok",
                Link = new Uri("http://example.org/2003/12/13/atom03"),
                PublishDate = new DateTime(2003, 12, 13, 18, 30, 2),
                Body = "Some text."
            });

            var atom = feed.Serialize(FeedType.Atom1);

            Assert.Equal(sample, atom);
        }
    }
}