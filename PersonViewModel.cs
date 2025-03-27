using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Text;
using System.Threading;

namespace CSharpLabs
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private readonly object _locker = new object();

        private Person _person;

        private string _firstName;

        private string _lastName;

        private string _email;

        private DateTime _birthDate;

        private bool _isProceedButtonEnabled;

        private bool _isNotBusy;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public PersonViewModel()
        {
            _firstName = string.Empty;
            _lastName = string.Empty;
            _email = string.Empty;
            _birthDate = DateTime.Now;
            _isProceedButtonEnabled = false;
            _isNotBusy = true;
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
            catch (ArgumentException ex)
            {
                ShowMessage(ex.Message);
                BirthDate = DateTime.Now;
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