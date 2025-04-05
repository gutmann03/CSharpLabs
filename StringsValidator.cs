using System.Text.RegularExpressions;

namespace CSharpLabs
{
    class StringsValidator
    {
        private static readonly string _emailPattern = @"(^[a-zA-Z0-9_.]+[@]{1}[a-z0-9]+[\.][a-z]+$)";
        private static readonly string _namePattern = @"(^[A-Z]{1}[a-z]+(\-[A-Z]{1}[a-z]+)?$)";

        static public bool ValidateEmail(string email) => Regex.IsMatch(email, _emailPattern);

        static public bool ValidateName(string name) => Regex.IsMatch(name, _namePattern);
    }
}
