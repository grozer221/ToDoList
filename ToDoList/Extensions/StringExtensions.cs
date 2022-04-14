namespace ToDoList.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceInEnd(this string source, string find, string replace)
        {
            int place = source.LastIndexOf(find);

            if (place == -1 || place < source.Length - find.Length)
                return source;

            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }
    }
}
