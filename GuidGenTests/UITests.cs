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
    public class UITests
    {
        private const bool visibleUITests = false;

        [Fact]
        public void UIRespondsToViewModelChanges()
        {
            this.UITest(window =>
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

                return Task.FromResult<object>(null);
            });
        }

        private void UITest(Func<MainWindow, Task> testMethod, bool visible = visibleUITests)
        {
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext());
            var window = new MainWindow();

            var frame = new DispatcherFrame();
            SynchronizationContext.Current.Post(async d =>
            {
                try
                {
                    if (visible)
                    {
                        window.Show();
                    }

                    await testMethod(window);
                }
                finally
                {
                    window.Close();
                    frame.Continue = false;
                }
            }, null);
            Dispatcher.PushFrame(frame);
        }
    }
}
