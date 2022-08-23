// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.GuidGenVsPackage;

#pragma warning disable SA1303 // Const field names should begin with upper-case letter
#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter
#pragma warning disable SA1311 // Static readonly fields should begin with upper-case letter

internal static class GuidList
{
    public const string guidGuidGenVsPackagePkgString = "f4938f46-b76f-4b6f-91a7-05978aab4426";
    public const string guidGuidGenVsPackageCmdSetString = "c1712e1a-dc7d-4cf6-9982-13bfd42b80e9";

    public static readonly Guid guidGuidGenVsPackageCmdSet = new Guid(guidGuidGenVsPackageCmdSetString);
}
