using System;

namespace CSharpLabs
{
    class Person
    {
        private static int MinAge => 0;

        private static int MaxAge => 135;

        private bool _isAdult;

        private string _sunSign;

        private string _chineseSign;

        private bool _isBirthday;

        private int _age;

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }

        public DateTime BirthDate { get; private set; }

        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;

            _age = CalculateAge(birthDate);
            if (!IsAgeValid(birthDate)) throw new ArgumentException($"Некоректна дата народження. Допустимий вік від {Person.MinAge} до {Person.MaxAge} років"); ;

            _isAdult = _age >= 18;
            _sunSign = Zodiacs.GetWesternZodiac(BirthDate);
            _chineseSign = Zodiacs.GetChinenseZodiac(BirthDate);
            _isBirthday = BirthDate.Month == DateTime.Now.Month && BirthDate.Day == DateTime.Now.Day;
        }

        public Person(string firstName, string lastName, string email) : this(firstName, lastName, email, DateTime.Now) { }

        public Person(string firstName, string lastName, DateTime birthDate) : this(firstName, lastName, string.Empty, birthDate) { }

        private static int CalculateAge(DateTime birthDate)
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Now.AddYears(-age)) age--;
            return age;
        }

        private bool IsAgeValid(DateTime birthDate)
        {
            return birthDate <= DateTime.Now && _age <= Person.MaxAge;
        }

        public bool IsAdult => _isAdult;

        public string SunSign => _sunSign;

        public string ChineseSign => _chineseSign;

        public bool IsBirthday => _isBirthday;
    }
}
