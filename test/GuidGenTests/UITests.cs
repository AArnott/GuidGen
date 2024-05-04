// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Controls;

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
