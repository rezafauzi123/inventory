using System.Text.RegularExpressions;

namespace InventoryManagement.WebApi.Common
{
    public class IsbnValidator
    {
        public bool IsValidISBNCode(string str)
        {
            string strRegex
                = @"^(?=(?:[^0-9]*[0-9]){10}(?:(?:[^0-9]*[0-9]){3})?$)[\d-]+$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(str))
                return (true);
            else
                return (false);
        }
    }
}
