using System;
using System.Xml.Linq;

namespace WilderMinds.RssSyndication.Channel
{
  /// <summary>
  /// The Text Input settings allow a data entry box to be displayed within your feed. 
  /// It can be used to collect information from the reader, or act on data they enter. 
  /// Most readers ignore this option.
  /// </summary>
  public class TextInput
  {
    /// <summary>
    /// A title for the textbox.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// A short description of what the user should enter into the textbox. 
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// The variable name of the text in the textbox.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The URL of the CGI script that processes text input requests.
    /// </summary>
    public Uri Link { get; } 

    public TextInput (string title, string description, string name, Uri link)
    {
      Guard.CheckNullOrWhitespace(nameof(title), title);
      Guard.CheckNullOrWhitespace(nameof(description), description);
      Guard.CheckNullOrWhitespace(nameof(name), name);
      Guard.CheckNull(nameof(link), link);

      Title = title;
      Description = description;
      Name = name;
      Link = link;
    }

    internal XElement Serialize()
    {
      var textInput = new XElement("textInput");

      textInput.Add("title", Title);
      textInput.Add("description", Description);
      textInput.Add("name", Name);
      textInput.Add("link", Link.AbsoluteUri);

      return textInput;
    }
  }
}