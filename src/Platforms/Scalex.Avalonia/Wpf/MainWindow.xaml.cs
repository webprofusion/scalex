using System.Windows;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var scoreData = System.IO.File.ReadAllBytes(@"C:\Users\chris\OneDrive\Chris\Tablature\Andy James - War March.gp5");
            alpha.Api.Load(scoreData);
        }
    }
}
