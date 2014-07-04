namespace Microsoft.GuidGenVsPackage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Utilities;
    using GuidGen;

    internal class GuidCompletionSource : ICompletionSource
    {
        private GuidCompletionSourceProvider m_sourceProvider;
        private ITextBuffer m_textBuffer;

        public GuidCompletionSource(GuidCompletionSourceProvider sourceProvider, ITextBuffer textBuffer)
        {
            m_sourceProvider = sourceProvider;
            m_textBuffer = textBuffer;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            Guid guid = Guid.NewGuid();

            var compList = new List<Completion>();
            compList.Add(new Completion("GuidA", GuidCodeSnippetFormatter.GetCodeSnippet(guid, CodeSnippetFormat.GuidAttributeWithBrackets), "A GUID formatted as a C# GuidAttribute.", null, null));
            compList.Add(new Completion("GuidB", GuidCodeSnippetFormatter.GetCodeSnippet(guid, CodeSnippetFormat.RegistryFormat), "A GUID formatted in the human-readable (registry) format.", null, null));
            compList.Add(new Completion("GuidF", GuidCodeSnippetFormatter.GetCodeSnippet(guid, CodeSnippetFormat.CSharpFieldDefinition), "A C# definition of a field containing a GUID.", null, null));

            completionSets.Add(new CompletionSet(
                "Guids",    //the non-localized title of the tab 
                "Guids",    //the display title of the tab
                FindTokenSpanAtPosition(session.GetTriggerPoint(m_textBuffer), session),
                compList,
                null));
        }

        public void Dispose()
        {
        }

        private ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session)
        {
            SnapshotPoint currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;
            ITextStructureNavigator navigator = m_sourceProvider.NavigatorService.GetTextStructureNavigator(m_textBuffer);
            TextExtent extent = navigator.GetExtentOfWord(currentPoint);
            return currentPoint.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
        }
    }
}
