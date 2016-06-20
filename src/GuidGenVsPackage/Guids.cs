// Guids.cs
// MUST match guids.h
namespace Microsoft.GuidGenVsPackage
{
    using System;

    static class GuidList
    {
        public const string guidGuidGenVsPackagePkgString = "f4938f46-b76f-4b6f-91a7-05978aab4426";
        public const string guidGuidGenVsPackageCmdSetString = "c1712e1a-dc7d-4cf6-9982-13bfd42b80e9";

        public static readonly Guid guidGuidGenVsPackageCmdSet = new Guid(guidGuidGenVsPackageCmdSetString);
    };
}