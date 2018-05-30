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
        private MainWindow window;

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
            var listBox = (ListBox)window.FindName("FormatListBox");
            var codeSnippetText = (TextBlock)window.FindName("CodeSnippetText");

            foreach (CodeSnippetFormat format in Enum.GetValues(typeof(CodeSnippetFormat)))
            {
                window.ViewModel.Format = format;
                var listBoxSelectedFormat = (CodeSnippetFormat)listBox.SelectedValue;
                Assert.Equal(format, listBoxSelectedFormat);
                Assert.Equal(window.ViewModel.CodeSnippet, codeSnippetText.Text);
            }
        }
    }
}
