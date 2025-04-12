using System;

namespace CSharpLabs
{
    class PersonRepositorySchema
    {
        public Guid ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public PersonRepositorySchema(Person person)
        {
            ID = person.ID;
            FirstName = person.FirstName;
            LastName = person.LastName;
            Email = person.Email;
            BirthDate = person.BirthDate;
        }

        public PersonRepositorySchema() { }

        public Person ToPerson()
        {
            return new Person(ID, FirstName, LastName, Email, BirthDate);
        }
    }
}
