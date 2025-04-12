using System.Windows;

namespace CSharpLabs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;
        private PersonRepository _personRepository;

        public MainWindow()
        {
            _personRepository = new PersonRepository();
            _viewModel = new MainViewModel(_personRepository);
            InitializeComponent();
            DataGridUsers.ItemsSource = _viewModel.UsersView;
        }

        private void FilterTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (FilterTextBox.Text != "Filter...") _viewModel.Filter(FilterTextBox.Text);
        }

        private void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new PersonEditWindow();
            if (editWindow.ShowDialog() == true)
            {
                var person = editWindow.GetPerson();
                if (person != null) _viewModel.AddPerson(person);
            }
        }

        private void EditPerson_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridUsers.SelectedItem is Person selectedUser)
            {
                var editWindow = new PersonEditWindow(selectedUser);
                if (editWindow.ShowDialog() == true)
                {
                    var person = editWindow.GetPerson();
                    if (person != null) _viewModel.EditPerson(selectedUser, person);
                }
            }
        }

        private void DeletePerson_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridUsers.SelectedItem is Person selectedUser)
            {
                if (MessageBox.Show($"Delete {selectedUser.FirstName} {selectedUser.LastName}?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _viewModel.DeletePerson(selectedUser);
                }
            }
        }
    }
}
