using System.Xml.Linq;

namespace WilderMinds.RssSyndication.Channel
{
    /// <summary>
    /// Specifies a web service that supports the rssCloud interface which can be implemented in HTTP-POST, XML-RPC or SOAP 1.1. 
    /// Allows processes to register with a cloud to be notified of updates to the channel, implementing a lightweight publish-subscribe protocol for RSS feeds.
    /// </summary>
    public class Cloud
    {
        /// <summary>
        /// The domain name or IP address of the cloud.
        /// </summary>
        public string Domain { get; }

        /// <summary>
        /// The TCP/IP port to communicate with the cloud.
        /// </summary>
        public string Port { get; }

        /// <summary>
        /// The path to the procedure on the cloud.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// The name of the procedure to call on the cloud to request notification.
        /// </summary>
        public string RegisterProcedure { get; }

        /// <summary>
        /// Which protocol to use to communicate with the cloud.
        /// </summary>
        public CloudProtocol Protocol { get; }

        public Cloud(string domain, string port,
            string path,
            string registerProcedure,
            CloudProtocol protocol)
        {
            Guard.CheckNullOrWhitespace(nameof(domain), domain);
            Guard.CheckNullOrWhitespace(nameof(port), port);
            Guard.CheckNullOrWhitespace(nameof(path), path);
            Guard.CheckNullOrWhitespace(nameof(registerProcedure), registerProcedure);

            Domain = domain;
            Port = port;
            Path = path;
            RegisterProcedure = registerProcedure;
            Protocol = protocol;
        }

        internal XElement Serialize()
        {
            var cloud = new XElement("cloud");

            cloud.Add(new XAttribute("domain", Domain));
            cloud.Add(new XAttribute("port", Port));
            cloud.Add(new XAttribute("path", Path));
            cloud.Add(new XAttribute("registerProcedure", RegisterProcedure));

            string protocolAttributeValue = null;
            switch (Protocol)
            {
                case CloudProtocol.HttpPost:
                    protocolAttributeValue = "http-post";
                    break;
                case CloudProtocol.Soap11:
                    protocolAttributeValue = "soap";
                    break;
                default:
                    protocolAttributeValue = "xml-rpc";
                    break;
            }

            cloud.Add(new XAttribute("protocol", protocolAttributeValue));

            return cloud;
        }
    }
}