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

    internal class GuidCompletionSource : ICompletionSource
    {
        private GuidCompletionSourceProvider m_sourceProvider;
        private ITextBuffer m_textBuffer;
        private List<Completion> m_compList;

        public GuidCompletionSource(GuidCompletionSourceProvider sourceProvider, ITextBuffer textBuffer)
        {
            m_sourceProvider = sourceProvider;
            m_textBuffer = textBuffer;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            List<string> strList = new List<string>();
            strList.Add("addition");
            strList.Add("adaptation");
            strList.Add("subtraction");
            strList.Add("summation");
            m_compList = new List<Completion>();
            foreach (string str in strList)
            {
                m_compList.Add(new Completion(str, str, str, null, null));
            }

            completionSets.Add(new CompletionSet(
                "Guids",    //the non-localized title of the tab 
                "Guids",    //the display title of the tab
                FindTokenSpanAtPosition(session.GetTriggerPoint(m_textBuffer), session),
                m_compList,
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
