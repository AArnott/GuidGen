// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.GuidGenVsPackage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Utilities;

    // Disabled for now since it causes a lot of trouble sometimes.
    // It breaks Roslyn in Dev14 in C# sometimes, and it's busted in VB.
    ////[Export(typeof(ICompletionSourceProvider))]
    ////[ContentType("Basic")]
    ////[ContentType("CSharp")]
    ////[Name("Guid completion")]
    internal class GuidCompletionSourceProvider : ICompletionSourceProvider
    {
        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        public ICompletionSource TryCreateCompletionSource(VisualStudio.Text.ITextBuffer textBuffer)
        {
            return new GuidCompletionSource(this.NavigatorService, textBuffer, vb: textBuffer.ContentType.IsOfType("Basic"));
        }
    }
}
