using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Text;
using System.Threading;
using System.Windows.Media;

namespace CSharpLabs
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private readonly object _locker = new object();

        private static readonly string _colorBlack = Color.FromRgb(0, 0, 0).ToString();
        private static readonly string _colorRed = Color.FromRgb(255, 0, 0).ToString();

        private Person _person;

        private string _firstName;

        private string _lastName;

        private string _email;

        private DateTime _birthDate;

        private bool _isProceedButtonEnabled;

        private bool _isNotBusy;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _firstNameColor;
        private string _lastNameColor;
        private string _emailColor;
        private string _birthDateColor;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
                UpdateProceedButtonState();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
                UpdateProceedButtonState();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
                UpdateProceedButtonState();
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set 
            {
                _birthDate = value;
                Console.WriteLine(_birthDate);
                OnPropertyChanged();
                UpdateProceedButtonState();
            }
        }

        public bool IsProceedButtonEnabled
        {
            get => _isProceedButtonEnabled;
            set
            {
                _isProceedButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsNotBusy
        {
            get
            {
                bool val;
                lock (_locker)
                {
                    val = _isNotBusy;
                }
                return val;
            }
            set
            {
                lock (_locker)
                {
                    _isNotBusy = value;
                }
                OnPropertyChanged();
            }
        }

        public string SunSign { get; private set; }
        
        public string ChineseSign { get; private set; }
        
        public bool IsAdult { get; private set; }
        
        public bool IsBirthday { get; private set; }

        public string FirstNameColor
        {
            get => _firstNameColor;
            set
            {
                _firstNameColor = value;
                OnPropertyChanged();
            }
        }

        public string LastNameColor
        {
            get => _lastNameColor;
            set
            {
                _lastNameColor = value;
                OnPropertyChanged();
            }
        }

        public string EmailColor
        {
            get => _emailColor;
            set
            {
                _emailColor = value;
                OnPropertyChanged();
            }
        }

        public string BirthDateColor
        {
            get => _birthDateColor;
            set
            {
                _birthDateColor = value;
                OnPropertyChanged();
            }
        }

        public PersonViewModel()
        {
            _firstName = string.Empty;
            _lastName = string.Empty;
            _email = string.Empty;
            _birthDate = DateTime.Now;
            _isProceedButtonEnabled = false;
            _isNotBusy = true;

            _firstNameColor = _colorBlack;
            _lastNameColor = _colorBlack;
            _emailColor = _colorBlack;
            _birthDateColor = _colorBlack;
        }

        public PersonViewModel(Person person)
        {
            _person = person;
            _firstName = person.FirstName;
            _lastName = person.LastName;
            _email = person.Email;
            _birthDate = person.BirthDate;
            _isProceedButtonEnabled = true;
            _isNotBusy = true;

            _firstNameColor = _colorBlack;
            _lastNameColor = _colorBlack;
            _emailColor = _colorBlack;
            _birthDateColor = _colorBlack;
        }

        private void UpdateProceedButtonState()
        {
            IsProceedButtonEnabled = !string.IsNullOrEmpty(FirstName) &&
                                     !string.IsNullOrEmpty(LastName) &&
                                     !string.IsNullOrEmpty(Email) &&
                                     BirthDate != DateTime.MinValue &&
                                     IsNotBusy;
        }

        public async Task<bool> ProceedAsync()
        {
            bool success = false;
            try
            {
                IsNotBusy = false;
                UpdateProceedButtonState();
                _person = await Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    if (_person == null)
                    {
                        return new Person(FirstName, LastName, Email, BirthDate);
                    }

                    return new Person(_person.ID, FirstName, LastName, Email, BirthDate);
                });

                SunSign = _person.SunSign;
                ChineseSign = _person.ChineseSign;
                IsAdult = _person.IsAdult;
                IsBirthday = _person.IsBirthday;

                OnPropertyChanged(nameof(SunSign));
                OnPropertyChanged(nameof(ChineseSign));
                OnPropertyChanged(nameof(IsAdult));
                OnPropertyChanged(nameof(IsBirthday));

                if (IsBirthday) ShowMessage("Happy birthday!!!");

                ShowMessage(BuildResultMessage());
                success = true;
            }
            catch (UnBurnException ex)
            {
                BirthDateColor = _colorRed;
                ShowMessage(ex.Message);
                BirthDateColor = _colorBlack;
                BirthDate = _person == null ? DateTime.Now : _person.BirthDate;
            }
            catch (TooOldException ex)
            {
                BirthDateColor = _colorRed;
                ShowMessage(ex.Message);
                BirthDateColor = _colorBlack;
                BirthDate = _person == null ? DateTime.Now : _person.BirthDate;
            }
            catch (InvalidEmailException ex)
            {
                EmailColor = _colorRed;
                ShowMessage(ex.Message);
                EmailColor = _colorBlack;
                Email = _person == null ? string.Empty : _person.Email;
            }
            catch (InvalidNameException ex)
            {
                switch (ex.Type)
                {
                    case InvalidNameType.FirstName:
                        FirstNameColor = _colorRed;
                        ShowMessage(ex.Message);
                        FirstNameColor = _colorBlack;
                        FirstName = _person == null ? string.Empty : _person.FirstName;
                        break;
                    case InvalidNameType.LastName:
                        LastNameColor = _colorRed;
                        ShowMessage(ex.Message);
                        LastNameColor = _colorBlack;
                        LastName = _person == null ? string.Empty : _person.LastName;
                        break;
                    default:
                        ShowMessage("Unreachable path!");
                        break;
                }
            }
            finally
            {
                IsNotBusy = true;
                UpdateProceedButtonState();
            }
            return success;
        }

        public Person GetPerson()
        {
            return _person;
        }

        private void ShowMessage(string msg)
        {
            MessageBox.Show(msg);
        }

        private string BuildResultMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"first name:   {FirstName}\n");
            sb.Append($"last name:    {LastName}\n");
            sb.Append($"birth date:   {BirthDate.ToShortDateString()}\n");
            sb.Append($"email:        {Email}\n");
            sb.Append($"is adult:     {IsAdult}\n");
            sb.Append($"sun sign:     {SunSign}\n");
            sb.Append($"chinese sign: {ChineseSign}\n");
            sb.Append($"is birthday:  {IsBirthday}\n");
            return sb.ToString();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}