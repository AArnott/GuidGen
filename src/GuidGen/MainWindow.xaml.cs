namespace GuidGen
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using GuidGen.Properties;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

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

            ((ListBoxItem)FormatListBox.SelectedItem).Focus();
        }

        public GuidGenViewModel ViewModel { get; set; }

        protected override void OnClosed(EventArgs e)
        {
            Settings.Default.PreferredFormat = (int)this.ViewModel.Format;
            Settings.Default.Save();
            base.OnClosed(e);
        }
    }
}
