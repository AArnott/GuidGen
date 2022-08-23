// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.GuidGenVsPackage;

// Disabled for now since it causes a lot of trouble sometimes.
// It breaks Roslyn in Dev14 in C# sometimes, and it's busted in VB.
////[Export(typeof(ICompletionSourceProvider))]
////[ContentType("Basic")]
////[ContentType("CSharp")]
////[Name("Guid completion")]
internal class GuidCompletionSourceProvider : ICompletionSourceProvider
{
    [Import]
    internal ITextStructureNavigatorSelectorService NavigatorService { get; set; } = null!;

    public ICompletionSource TryCreateCompletionSource(VisualStudio.Text.ITextBuffer textBuffer)
    {
        return new GuidCompletionSource(this.NavigatorService, textBuffer, vb: textBuffer.ContentType.IsOfType("Basic"));
    }
}
