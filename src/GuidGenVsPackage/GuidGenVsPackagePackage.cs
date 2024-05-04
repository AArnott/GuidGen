// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.GuidGenVsPackage;

/// <summary>
/// Exposes the GuidGen window as a command in VS.
/// </summary>
[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
[ProvideMenuResource("Menus.ctmenu", 1)]
[Guid(GuidList.guidGuidGenVsPackagePkgString)]
public sealed class GuidGenVsPackagePackage : AsyncPackage
{
    private GuidGen.MainWindow? window;

    protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
    {
        await base.InitializeAsync(cancellationToken, progress);

        // When initialized asynchronously, we *may* be on a background thread at this point.
        // Do any initialization that requires the UI thread after switching to the UI thread.
        // Otherwise, remove the switch to the UI thread if you don't need it.
        await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        // Add our command handlers for menu (commands must exist in the .vsct file)
        if (await this.GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService mcs)
        {
            // Create the command for the menu item.
            CommandID menuCommandID = new CommandID(GuidList.guidGuidGenVsPackageCmdSet, (int)PkgCmdIDList.cmdidCreateGuid);
            MenuCommand menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
            mcs.AddCommand(menuItem);
        }
    }

    /// <summary>
    /// Releases the resources used by the <see cref="T:Microsoft.VisualStudio.Shell.Package" /> object.
    /// </summary>
    /// <param name="disposing">true if the object is being disposed, false if it is being finalized.</param>
    protected override void Dispose(bool disposing)
    {
        this.window?.Close();
        base.Dispose(disposing);
    }

    /// <summary>
    /// Opens the GuidGen window.
    /// </summary>
    private void MenuItemCallback(object sender, EventArgs e)
    {
        if (this.window == null)
        {
            this.window = new GuidGen.MainWindow();
            this.window.Closed += (s, args) => this.window = null;
        }

        this.window.Show();
    }
}
