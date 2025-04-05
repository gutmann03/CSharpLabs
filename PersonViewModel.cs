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

        private void UpdateProceedButtonState()
        {
            IsProceedButtonEnabled = !string.IsNullOrEmpty(FirstName) &&
                                     !string.IsNullOrEmpty(LastName) &&
                                     !string.IsNullOrEmpty(Email) &&
                                     BirthDate != DateTime.MinValue &&
                                     IsNotBusy;
        }

        public async Task ProceedAsync()
        {
            try
            {
                IsNotBusy = false;
                UpdateProceedButtonState();
                _person = await Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    return new Person(FirstName, LastName, Email, BirthDate);
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
            }
            catch (UnBurnException ex)
            {
                BirthDateColor = _colorRed;
                ShowMessage(ex.Message);
                BirthDateColor = _colorBlack;
                BirthDate = DateTime.Now;
            }
            catch (TooOldException ex)
            {
                BirthDateColor = _colorRed;
                ShowMessage(ex.Message);
                BirthDateColor = _colorBlack;
                BirthDate = DateTime.Now;
            }
            catch (InvalidEmailException ex)
            {
                EmailColor = _colorRed;
                ShowMessage(ex.Message);
                EmailColor = _colorBlack;
                Email = string.Empty;
            }
            catch (InvalidNameException ex)
            {
                switch (ex.Type)
                {
                    case InvalidNameType.FirstName:
                        FirstNameColor = _colorRed;
                        ShowMessage(ex.Message);
                        FirstNameColor = _colorBlack;
                        FirstName = string.Empty;
                        break;
                    case InvalidNameType.LastName:
                        LastNameColor = _colorRed;
                        ShowMessage(ex.Message);
                        LastNameColor = _colorBlack;
                        LastName = string.Empty;
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