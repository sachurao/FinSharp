namespace StreamCipher.Common.Ioc.Impl
{
    /// <summary>
    /// Acts as entry-point for instantiating interception.
    /// All interception should be configuration-based.
    /// All objects should be POCO, i.e. no library-specific attributes
    /// </summary>
    public static class InterceptionManager
    {
        public static void SetUp(IConfigureInterception configurator)
        {
            configurator.Configure();
        }
    }
}
