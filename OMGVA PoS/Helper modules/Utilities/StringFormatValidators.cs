using System.Text.RegularExpressions;

namespace OMGVA_PoS.Helper_modules.Utilities
{
    public static class StringFormatValidators
    {
        public static bool IsValidEmail(this string str) {
            if (str == null || str.Length == 0) 
                return false;
            Regex validateEmailRegex = new Regex("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$");
            if (validateEmailRegex.IsMatch(str))
                return true;
            return false;
        }

        public static bool IsValidPhone(this string str) {
            if (str == null || str.Length == 0 || str.Length > 40) 
                return false;
            Regex validatePhoneRegex = new Regex("\\+?[0-9 -]+");
            if (validatePhoneRegex.IsMatch(str))
                return true;
            return false;
        }
    }
}
