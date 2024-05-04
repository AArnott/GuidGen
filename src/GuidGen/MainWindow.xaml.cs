// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using GuidGen.Properties;

namespace GuidGen;

/// <summary>
/// Interaction logic for MainWindow.xaml.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();

        this.ViewModel = (GuidGenViewModel)this.Resources["ViewModel"];
        try
        {
            this.ViewModel.Format = (CodeSnippetFormat)Settings.Default.PreferredFormat;
        }
        catch (Exception ex)
        {
            // This is *so* not worth crashing the app over if it fails wrong.
            Debug.Fail("Failed to restore previous format preference. " + ex.Message);
        }

        ((ListBoxItem)this.FormatListBox.SelectedItem).Focus();
    }

    public GuidGenViewModel ViewModel { get; set; }

    protected override void OnClosed(EventArgs e)
    {
        Settings.Default.PreferredFormat = (int)this.ViewModel.Format;
        Settings.Default.Save();
        base.OnClosed(e);
    }
}
