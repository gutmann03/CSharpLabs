using System.Windows;

namespace CSharpLabs
{
    /// <summary>
    /// Interaction logic for PersonEditWindow.xaml
    /// </summary>
    public partial class PersonEditWindow : Window
    {
        private PersonViewModel _viewModel;

        public PersonEditWindow()
        {
            InitializeComponent();
            _viewModel = new PersonViewModel();
            DataContext = _viewModel;

            Title += " [Create]";
        }

        public PersonEditWindow(Person person)
        {
            InitializeComponent();
            _viewModel = new PersonViewModel(person);
            DataContext = _viewModel;

            Title += " [Edit]";
        }

        private async void ProceedButton_Click(object sender, RoutedEventArgs e)
        {
            if (await _viewModel.ProceedAsync()) DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public Person GetPerson() { return _viewModel.GetPerson(); }
    }
}
