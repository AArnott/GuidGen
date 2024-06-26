﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows;

public class GuidGenViewModelTests
{
    private readonly GuidGenViewModel viewModel = new GuidGenViewModel();

    [Fact]
    public void InitialGuidIsNonEmpty()
    {
        Assert.NotEqual(Guid.Empty, this.viewModel.Value);
    }

    [Fact]
    public void NewGuid_ChangesGuid()
    {
        Guid originalGuid = this.viewModel.Value;
        this.viewModel.NewGuid();
        Assert.NotEqual(originalGuid, this.viewModel.Value);
        Assert.NotEqual(Guid.Empty, this.viewModel.Value);
    }

    [Fact]
    public void NewGuidCommand_CanExecute()
    {
        Guid originalGuid = this.viewModel.Value;
        Assert.True(this.viewModel.NewGuidCommand.CanExecute(null));
    }

    [Fact]
    public void NewGuidCommand_ChangesGuid()
    {
        Guid originalGuid = this.viewModel.Value;
        this.viewModel.NewGuidCommand.Execute(null);
        Assert.NotEqual(originalGuid, this.viewModel.Value);
        Assert.NotEqual(Guid.Empty, this.viewModel.Value);
    }

    [StaFact]
    public void CopyCommand_CopiesGuid()
    {
        Clipboard.Clear();
        this.viewModel.CopyCommand.Execute(null);
        string copiedText = Clipboard.GetText();
        Assert.Equal(this.viewModel.CodeSnippet, copiedText);
    }

    [Fact]
    public void Format_ChangesCodeSnippet()
    {
        var codeSnippets = new HashSet<string>();
        foreach (CodeSnippetFormat format in Enum.GetValues(typeof(CodeSnippetFormat)))
        {
            this.viewModel.Format = format;

            // Make sure that the resulting snippet is unique.
            Assert.True(codeSnippets.Add(this.viewModel.CodeSnippet));
        }
    }
}
