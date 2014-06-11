using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace IIUSchoolSystem.Core.Helpers
{
    public static class Extensions
    {
        public static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;

            return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
        }

        public static string CamelCaseToWords(this string input)
        {
            return Regex.Replace(input.FirstCharToUpper(), "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }
    }
}
