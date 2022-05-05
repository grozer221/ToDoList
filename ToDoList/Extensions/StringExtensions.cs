using System.Text.RegularExpressions;

namespace ToDoList.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceInEnd(this string source, string find, string replace)
        {
            int place = source.LastIndexOf(find);

            if (place == -1 || place < source.Length - find.Length)
                return source;

            return source.Remove(place, find.Length).Insert(place, replace);
        }

        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(str, "(\\B[A-Z])", " $1");
        }

        public static int? ToNullableInt(this string str)
        {
            int number;
            if (int.TryParse(str, out number)) 
                return number;
            return null;
        }
    }
}
