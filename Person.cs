using System;

namespace CSharpLabs
{
    public class Person
    {
        private static int MinAge => 0;

        public static int MaxAge => 135;

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
            ID = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;

            if (!StringsValidator.ValidateName(firstName)) throw new InvalidNameException(InvalidNameType.FirstName);
            if (!StringsValidator.ValidateName(lastName)) throw new InvalidNameException(InvalidNameType.LastName);

            _age = CalculateAge(birthDate);
            if (birthDate > DateTime.Now) throw new UnBurnException();
            if (_age > Person.MaxAge) throw new UnBurnException();
            if (!StringsValidator.ValidateEmail(email)) throw new InvalidEmailException();

            _isAdult = _age >= 18;
            _sunSign = Zodiacs.GetWesternZodiac(BirthDate);
            _chineseSign = Zodiacs.GetChinenseZodiac(BirthDate);
            _isBirthday = BirthDate.Month == DateTime.Now.Month && BirthDate.Day == DateTime.Now.Day;
        }

        public Person(string firstName, string lastName, string email) : this(firstName, lastName, email, DateTime.Now) { }

        public Person(string firstName, string lastName, DateTime birthDate) : this(firstName, lastName, string.Empty, birthDate) { }

        public Person(Guid id, string firstName, string lastName, string email, DateTime birthDate) : this(firstName, lastName, email, birthDate) { ID = id; }

        public Person() { }

        private static int CalculateAge(DateTime birthDate)
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Now.AddYears(-age)) age--;
            return age;
        }

        public bool IsAdult => _isAdult;

        public string SunSign => _sunSign;

        public string ChineseSign => _chineseSign;

        public bool IsBirthday => _isBirthday;

        public Guid ID { get; set; }

        public static void CopyFields(Person origin, Person other)
        {
            origin.FirstName = other.FirstName;
            origin.LastName = other.LastName;
            origin.Email = other.Email;
            origin.BirthDate = other.BirthDate;
            origin._isBirthday = other._isBirthday;
            origin._isAdult = other._isAdult;
            origin._sunSign = other._sunSign;
            origin._chineseSign = other._chineseSign;
        }
    }
}
