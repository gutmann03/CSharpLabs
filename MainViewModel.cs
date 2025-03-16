using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;

namespace CSharpLabs
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private User _userModel;
        private string _age = "Вік: ";
        private string _westernZodiac = "Знак зодіаку: ";
        private string _chineseZodiac = "Знак зодіаку (китайський): ";
        private string _birthdayMessage;
        private DateTime _birthDate;

        public event PropertyChangedEventHandler PropertyChanged;

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _userModel.BirthDate = value;
                if (!IsValidAge()) return;

                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
                CalculateAge();
                CalculateZodiacSigns();
            }
        }

        public string Age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public string WesternZodiac
        {
            get => _westernZodiac;
            set
            {
                _westernZodiac = value;
                OnPropertyChanged(nameof(WesternZodiac));
            }
        }

        public string ChineseZodiac
        {
            get => _chineseZodiac;
            set
            {
                _chineseZodiac = value;
                OnPropertyChanged(nameof(ChineseZodiac));
            }
        }

        public string BirthdayMessage
        {
            get => _birthdayMessage;
            set
            {
                _birthdayMessage = value;
                OnPropertyChanged(nameof(BirthdayMessage));
            }
        }

        public MainViewModel()
        {
            _userModel = new User();
            _birthDate = DateTime.Now;
        }

        private bool IsValidAge()
        {
            var isValid = _userModel.IsValidAge();
            if (!isValid) {
                MessageBox.Show($"Некоректна дата народження. Допустимий вік від {User.MinAge} до {User.MaxAge} років");
            }
            return isValid;
        }

        private void CalculateAge()
        {
            Age = "Вік: " + _userModel.GetAge().ToString();

            if (_userModel.IsBirthdayToday())
            {
                BirthdayMessage = "З Днем народження!";
            }
            else
            {
                BirthdayMessage = string.Empty;
            }
        }

        private void CalculateZodiacSigns()
        {
            WesternZodiac = "Знак зодіаку: " + _userModel.GetWesternZodiac();
            ChineseZodiac = "Знак зодіаку (китайський): " + _userModel.GetChineseZodiac();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
