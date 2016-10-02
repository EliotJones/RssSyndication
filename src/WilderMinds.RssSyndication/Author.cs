using System.Xml.Linq;

namespace WilderMinds.RssSyndication
{
  public class Author
  {
    public string Name { get; set; }
    public string Email { get; set; }

    internal XElement Serialize(FeedType type, string entityName = "author")
    {
      if (type == FeedType.Atom1)
      {
        var author = new XElement(Feed.AtomNamespace + entityName);

        author.AddOptionalElement(Feed.AtomNamespace + "name", Name);
        author.AddOptionalElement(Feed.AtomNamespace + "email", Email);

        return author;
      }

      return new XElement(entityName, $"{Email} ({Name})");
    }
  }
}