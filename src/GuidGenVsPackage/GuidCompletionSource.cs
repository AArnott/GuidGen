// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.GuidGenVsPackage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;
    using GuidGen;
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Utilities;

    internal class GuidCompletionSource : ICompletionSource
    {
        private readonly ITextStructureNavigatorSelectorService navigatorService;
        private readonly ITextBuffer textBuffer;
        private readonly bool vb;

        public GuidCompletionSource(ITextStructureNavigatorSelectorService navigatorService, ITextBuffer textBuffer, bool vb)
        {
            this.navigatorService = navigatorService;
            this.textBuffer = textBuffer;
            this.vb = vb;
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            Guid guid = Guid.NewGuid();

            var compList = new List<Completion>();
            if (this.vb)
            {
                compList.Add(new Completion("GuidA", GuidCodeSnippetFormatter.GetCodeSnippet(guid, CodeSnippetFormat.GuidAttributeWithAngleBrackets), "A GUID formatted as a VB GuidAttribute.", null, null));
                compList.Add(new Completion("GuidB", GuidCodeSnippetFormatter.GetCodeSnippet(guid, CodeSnippetFormat.RegistryFormat), "A GUID formatted in the human-readable (registry) format.", null, null));
                compList.Add(new Completion("GuidF", GuidCodeSnippetFormatter.GetCodeSnippet(guid, CodeSnippetFormat.VBFieldFieldDefinition), "A VB definition of a field containing a GUID.", null, null));
            }
            else
            {
                compList.Add(new Completion("GuidA", GuidCodeSnippetFormatter.GetCodeSnippet(guid, CodeSnippetFormat.GuidAttributeWithBrackets), "A GUID formatted as a C# GuidAttribute.", null, null));
                compList.Add(new Completion("GuidB", GuidCodeSnippetFormatter.GetCodeSnippet(guid, CodeSnippetFormat.RegistryFormat), "A GUID formatted in the human-readable (registry) format.", null, null));
                compList.Add(new Completion("GuidF", GuidCodeSnippetFormatter.GetCodeSnippet(guid, CodeSnippetFormat.CSharpFieldDefinition), "A C# definition of a field containing a GUID.", null, null));
            }

            completionSets.Add(new CompletionSet(
                "Guids",    // the non-localized title of the tab
                "Guids",    // the display title of the tab
                this.FindTokenSpanAtPosition(session.GetTriggerPoint(this.textBuffer), session),
                compList,
                null));
        }

        public void Dispose()
        {
        }

        private ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session)
        {
            SnapshotPoint currentPoint = session.TextView.Caret.Position.BufferPosition - 1;
            ITextStructureNavigator navigator = this.navigatorService.GetTextStructureNavigator(this.textBuffer);
            TextExtent extent = navigator.GetExtentOfWord(currentPoint);
            return currentPoint.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
        }
    }
}
