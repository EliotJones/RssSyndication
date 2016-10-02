namespace WilderMinds.RssSyndication.Channel
{
    public enum CloudProtocol
    {
        /// <summary>
        /// Clients should communicate with the cloud using HTTP POST.
        /// </summary>
        HttpPost = 1,

        /// <summary>
        /// Clients should communicate with the cloud using XML RPC.
        /// </summary>
        XmlRpc = 2,
    
        /// <summary>
        /// Clients should communicate with the cloud using SOAP 1.1.
        /// </summary>
        Soap11 = 3
    }
}
