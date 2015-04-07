namespace GuidGenTests
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public class STAFactDiscoverer : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink diagnosticMessageSink;

        public STAFactDiscoverer(IMessageSink diagnosticMessageSink)
        {
            this.diagnosticMessageSink = diagnosticMessageSink;
        }

        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttributeInfo)
        {
            var methodDisplay = discoveryOptions.MethodDisplayOrDefault();

            yield return new STATestCase(
                new XunitTestCase(this.diagnosticMessageSink, methodDisplay, testMethod));
        }
    }
}
