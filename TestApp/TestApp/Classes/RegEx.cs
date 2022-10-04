using System.Text.RegularExpressions;

namespace TestApp
{
    public class RegEx
    {
        //validate integer
        public static bool isInt(string str)
        {
            return Regex.IsMatch(str, @"^[0-9]*$");
        }

        //validates decimal, allow negative, allows pass in number of places after decimal
        public static bool isDecimal(string str, int numPlaces)
        {
            if (numPlaces == 1) { return Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]?$"); }
            if (numPlaces == 2) { return Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]?[0-9]?$"); }
            if (numPlaces == 3) { return Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]?[0-9]?[0-9]?$"); }
            if (numPlaces == 4) { return Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]?[0-9]?[0-9]?[0-9]?$"); }
            if (numPlaces == 5) { return Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?$"); }
            if (numPlaces == 6) { return Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?$"); }
            if (numPlaces == 15) { return Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?$"); }
            return false;
        }

        //validates email
        public static bool isValidEmail(string str)
        {
            return Regex.IsMatch(str, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        public static bool checkLength(string text, int length)
        {
            //if the length of the text is less than or equal than the length pass in return true(valid)
            if (text.Length <= length)
            {
                return true;
            }
            else
            {
                //else return false, string exceeds max length
                return false;
            }
        }
    }
}