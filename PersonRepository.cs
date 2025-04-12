using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSharpLabs
{
    public class PersonRepository
    {
        private readonly string _storeFileName = "persons.json";
        private readonly int _generateNumber = 50;

        private List<Person> _persons;

        public PersonRepository()
        {
            _persons = new List<Person>();
        }

        public List<Person> ListAll()
        {
            return _persons;
        }

        public Person Get(Guid id)
        {
            return _persons.FirstOrDefault(p => p.ID == id);
        }

        public void Add(Person person)
        {
            _persons.Add(person);
        }

        public void Update(Person person)
        {
            int index = _persons.FindIndex(p => p.ID == person.ID);
            if (index >= 0) _persons[index] = person;
        }

        public void Delete(Guid id)
        {
            _persons.RemoveAll(p => p.ID == id);
        }

        public void SaveData()
        {
            using (StreamWriter file = File.CreateText(_storeFileName))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                List<PersonRepositorySchema> payload = new List<PersonRepositorySchema>();
                foreach (Person person in _persons) payload.Add(new PersonRepositorySchema(person));
                serializer.Serialize(file, payload);
            }
        }

        public void LoadData()
        {
            if (File.Exists(_storeFileName))
            {
                using (StreamReader file = File.OpenText(_storeFileName))
                {
                    Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                    List<PersonRepositorySchema> payload = (List<PersonRepositorySchema>)serializer.Deserialize(file, typeof(List<PersonRepositorySchema>));
                    foreach (PersonRepositorySchema person in payload) _persons.Add(person.ToPerson());
                }

                return;
            }


            GeneratePersons();
            SaveData();
        }

        private void GeneratePersons()
        {
            Random random = new Random();
            string[] firstNames = { "Anna", "John", "Mike", "Olga", "Diana", "Max", "Kate", "Paul" };
            string[] lastNames = { "Smith", "Brown", "Taylor", "Ivanova", "Doe", "Kovalenko", "Novak", "Petrenko" };
            string[] emails = { "main.com", "gmail.com", "yahoo.com", "outlook.com", "example.com" };

            for (int i = 0; i < _generateNumber; i++)
            {
                string firstName = firstNames[random.Next(firstNames.Length)];
                string lastName = lastNames[random.Next(lastNames.Length)];
                string email = $"{firstName.ToLower()}.{lastName.ToLower()}@{emails[random.Next(emails.Length)]}";
                DateTime dateOfBirth = RandomDate(random, DateTime.Now.AddYears(-Person.MaxAge), DateTime.Now);
                Add(new Person(firstName ,lastName ,email ,dateOfBirth));
            }
        }

        private static DateTime RandomDate(Random random, DateTime from, DateTime to)
        {
            int range = (to - from).Days - 1;
            return from.AddDays(random.Next(range) + 1);
        }
    }
}
