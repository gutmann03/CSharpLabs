using System.Windows;

namespace CSharpLabs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PersonViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new PersonViewModel();
            DataContext = _viewModel;
        }

        private async void ProceedButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.ProceedAsync();
        }
    }
}
