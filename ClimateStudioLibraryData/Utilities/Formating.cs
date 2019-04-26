using System;
using System.Globalization;
using System.Text;

namespace ArchsimLib.Utilities
{
    public class Formating
    {

        // EP isn't culture-aware, so we have format everything in en-US
        private static readonly CultureInfo c = new CultureInfo("en-US");


        public static string RemoveSpecialCharactersNotStrict(string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ' || c == '_' || c == '-' || c == '(' || c == ')' || c == '/' || c == '.')
                {
                    sb.Append(c);
                }
            }

            string newString = sb.ToString();



            if (newString.Length > 70) return newString.Substring(0, 70);
            else return newString;
        }

        public static string RemoveSpecialCharactersLeaveSpaces(string str)
    {
        if (String.IsNullOrWhiteSpace(str)) return str;

        StringBuilder sb = new StringBuilder();
        foreach (char c in str)
        {
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c=='-' || c==' ')
            {
                sb.Append(c);
            }
        }
        return sb.ToString().Trim();
    }

        public static string RemoveSpecialCharacters(string str)
        {
            if (String.IsNullOrWhiteSpace(str)) return str;

            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == '-' || c == ' ')
                {
                    sb.Append(c);
                }
            }

            var sreturn = sb.ToString().Trim();

            sreturn = sreturn.Replace(' ', '_');

            return sreturn.Trim();
        }
    }

  

}
