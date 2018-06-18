// Copyright (c) Microsoft. All rights reserved.

namespace GuidGenTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Threading;
    using GuidGen;
    using Xunit;

    [Trait("UI", "")]
    public class UITests : IDisposable
    {
        private readonly MainWindow window;

        public UITests()
        {
            this.window = new MainWindow();
        }

        public void Dispose()
        {
            this.window.Close();
        }

        [StaFact]
        public void UIRespondsToViewModelChanges()
        {
            var listBox = (ListBox)this.window.FindName("FormatListBox");
            var codeSnippetText = (TextBlock)this.window.FindName("CodeSnippetText");

            foreach (CodeSnippetFormat format in Enum.GetValues(typeof(CodeSnippetFormat)))
            {
                this.window.ViewModel.Format = format;
                var listBoxSelectedFormat = (CodeSnippetFormat)listBox.SelectedValue;
                Assert.Equal(format, listBoxSelectedFormat);
                Assert.Equal(this.window.ViewModel.CodeSnippet, codeSnippetText.Text);
            }
        }
    }
}
