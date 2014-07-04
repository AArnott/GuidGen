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

    [Export(typeof(ICompletionSourceProvider))]
    [ContentType("plaintext")]
    [Name("token completion")]
    internal class GuidCompletionSourceProvider : ICompletionSourceProvider
    {
        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        public ICompletionSource TryCreateCompletionSource(VisualStudio.Text.ITextBuffer textBuffer)
        {
            return new GuidCompletionSource(this, textBuffer);
        }
    }
}
