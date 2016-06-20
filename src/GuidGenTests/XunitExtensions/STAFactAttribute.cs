namespace GuidGenTests
{
    using System;
    using Xunit;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("GuidGenTests.STAFactDiscoverer", "GuidGenTests")]
    public class STAFactAttribute : FactAttribute
    {
    }
}
