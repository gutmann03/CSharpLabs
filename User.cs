using System;

namespace CSharpLabs
{
    class User
    {
        public static int MinAge = 0;
        public static int MaxAge = 135;

        public DateTime BirthDate { get; set; }

        public int GetAge()
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - BirthDate.Year;
            if (BirthDate.Date > DateTime.Now.AddYears(-age)) age--;
            return age;
        }

        public bool IsValidAge()
        {
            int age = GetAge();
            return age >= User.MinAge && age <= User.MaxAge;
        }

        public bool IsBirthdayToday()
        {
            return BirthDate.Month == DateTime.Now.Month && BirthDate.Day == DateTime.Now.Day;
        }

        public string GetWesternZodiac()
        {
            return Zodiacs.GetWesternZodiac(this.BirthDate);
        }

        public string GetChineseZodiac()
        {
            return Zodiacs.GetChinenseZodiac(this.BirthDate);
        }
    }
}
