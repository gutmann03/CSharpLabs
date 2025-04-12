using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace CSharpLabs
{
    class MainViewModel : INotifyPropertyChanged
    {
        private PersonRepository _personRepository;
        private ObservableCollection<Person> _users;
        public ICollectionView UsersView { get; }

        public MainViewModel(PersonRepository personRepository)
        {
            _personRepository = personRepository;
            _personRepository.LoadData();
            _users = new ObservableCollection<Person>(_personRepository.ListAll());
            UsersView = CollectionViewSource.GetDefaultView(_users);
            UsersView.SortDescriptions.Add(new SortDescription(nameof(Person.FirstName), ListSortDirection.Ascending));
        }

        public void Filter(string search)
        {
            if (search == string.Empty)
            {
                UsersView.Filter = user => true;
                return;
            }

            UsersView.Filter = user =>
            {
                if (user is Person p)
                {
                    string s = search.ToLower();
                    return p.FirstName.ToLower().Contains(s) ||
                           p.LastName.ToLower().Contains(s) ||
                           p.Email.ToLower().Contains(s) ||
                           p.ChineseSign.ToLower().Contains(s) ||
                           p.SunSign.ToLower().Contains(s) ||
                           p.BirthDate.ToShortDateString().Contains(s);
                }
                return false;
            };
        }

        public void AddPerson(Person newUser)
        {
            _personRepository.Add(newUser);
            _personRepository.SaveData();
            _users.Add(newUser);
            UsersView.Refresh();
        }

        public void EditPerson(Person original, Person updated)
        {
            if (original == null || updated == null) return;

            Person.CopyFields(original, updated);
            _personRepository.SaveData();
            UsersView.Refresh();
        }

        public void DeletePerson(Person user)
        {
            if (user != null)
            {
                _personRepository.Delete(user.ID);
                _personRepository.SaveData();
                _users.Remove(user);
                UsersView.Refresh();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
