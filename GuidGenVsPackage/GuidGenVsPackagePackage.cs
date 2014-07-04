namespace Microsoft.GuidGenVsPackage
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using System.ComponentModel.Design;
    using Microsoft.Win32;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// Exposes the GuidGen window as a command in VS.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidGuidGenVsPackagePkgString)]
    public sealed class GuidGenVsPackagePackage : Package
    {
        private GuidGen.MainWindow window;

        /// <summary>
        /// Initializes a new instance of the <see cref="GuidGenVsPackagePackage"/> class.
        /// </summary>
        public GuidGenVsPackagePackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        /// <summary>
        /// Called when the VSPackage is loaded by Visual Studio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                CommandID menuCommandID = new CommandID(GuidList.guidGuidGenVsPackageCmdSet, (int)PkgCmdIDList.cmdidCreateGuid);
                MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                mcs.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Releases the resources used by the <see cref="T:Microsoft.VisualStudio.Shell.Package" /> object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed, false if it is being finalized.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.window != null)
            {
                this.window.Close();
            }

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
}
