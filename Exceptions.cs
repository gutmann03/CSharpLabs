using System;

namespace CSharpLabs
{
    public class UnBurnException : Exception
    {
        public UnBurnException() : base("Birth data cannot be in the future.") { }
    }

    public class TooOldException : Exception
    {
        public TooOldException() : base("Birth data is further than 135 years in the past.") { }
    }

    public class InvalidEmailException : Exception
    {
        public InvalidEmailException() : base("Invalid email address was provided. The correct one must match pattern '<userName>@<mailServer>.<domain>'.") { }
    }

    public class InvalidNameException : Exception
    {
       private InvalidNameType _type;

        public InvalidNameException(InvalidNameType type) : base($"{type.ToString()} has invalid format.")
        {
            _type = type;
        }

        public InvalidNameType Type { get => _type; }
    }

    public enum InvalidNameType
    {
        FirstName,
        LastName,
    }
}
