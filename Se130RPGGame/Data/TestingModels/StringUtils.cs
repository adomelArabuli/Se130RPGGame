using System.Diagnostics.Contracts;

namespace Se130RPGGame.Data.TestingModels
{
    public class StringUtils
    {
        public string ConcatString(string str1, string str2)
        {
            return str1 + str2;
        }
        public bool IsPalindrome(string str)
        {
            string reversed = new string(str.Reverse().ToArray());
            return str.Equals(reversed, StringComparison.OrdinalIgnoreCase);
        }

    }
}
