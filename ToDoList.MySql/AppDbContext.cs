namespace ToDoList.MySql
{
    public class AppDbContext
    {
        public static string ConvertMySqlConnectionString(string connectionString)
        {
            connectionString = connectionString.Split("//")[1];
            string user = connectionString.Split(':')[0];
            connectionString = connectionString.Replace(user, "").Substring(1);
            string password = connectionString.Split('@')[0];
            if (!string.IsNullOrEmpty(password))
                connectionString = connectionString.Replace(password, "");
            connectionString = connectionString.Substring(1);
            string server = connectionString.Split(':')[0];
            connectionString = connectionString.Replace(server, "").Substring(1);
            string port = connectionString.Split('/')[0];
            string database = connectionString.Split('/')[1];
            connectionString = $"server={server};database={database};user={user};password={password};port={port};";
            return connectionString;
        }
    }
}
