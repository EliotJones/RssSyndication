namespace WilderMinds.RssSyndication
{
    /// <summary>
    /// Which feed format to serialize/read.
    /// </summary>
    public enum FeedType
    {
        /// <summary>
        /// Use the RSS 2.0 standard.
        /// </summary>
        Rss2 = 1,

        /// <summary>
        /// Use the Atom 1.0 standard.
        /// </summary>
        Atom1 = 2
    }
}