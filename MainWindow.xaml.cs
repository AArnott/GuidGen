namespace GuidGen
{
    using System;
    using System.Collections.Generic;
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
            this.ViewModel.Format = (CodeSnippetFormat)Settings.Default.PreferredFormat;
        }

        public GuidGenViewModel ViewModel { get; set; }

        protected override void OnClosed(EventArgs e)
        {
            Settings.Default.PreferredFormat = (int)this.ViewModel.Format;
            Settings.Default.Save();
            base.OnClosed(e);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
