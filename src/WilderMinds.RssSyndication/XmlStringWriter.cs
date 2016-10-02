using System.IO;
using System.Text;

namespace WilderMinds.RssSyndication
{
    internal class XmlStringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}