// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft;

namespace GuidGen;

/// <summary>
/// The view model for the <see cref="MainWindow"/>.
/// </summary>
public class GuidGenViewModel : BindableBase
{
    private Guid value;

    private CodeSnippetFormat format;

    public GuidGenViewModel()
    {
        this.NewGuidCommand = new SimpleCommand(() => this.NewGuid());
        this.CopyCommand = new SimpleCommand(() => Clipboard.SetText(this.CodeSnippet));

        this.Value = Guid.NewGuid();
        this.Format = CodeSnippetFormat.RegistryFormat;
        this.RegisterDependentProperty(nameof(this.Value), nameof(this.CodeSnippet));
        this.RegisterDependentProperty(nameof(this.Format), nameof(this.CodeSnippet));
    }

    public ICommand NewGuidCommand { get; private set; }

    public ICommand CopyCommand { get; private set; }

    public Guid Value
    {
        get { return this.value; }
        set { this.SetProperty(ref this.value, value); }
    }

    public CodeSnippetFormat Format
    {
        get { return this.format; }
        set { this.SetProperty(ref this.format, value); }
    }

    public string CodeSnippet
    {
        get { return GuidCodeSnippetFormatter.GetCodeSnippet(this.Value, this.Format); }
    }

    public void NewGuid()
    {
        this.Value = Guid.NewGuid();
    }
}
