public static class Constants
{
    public const string DatabaseFilename = "CriusDatabase.db3";
    public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache | SQLite.SQLiteOpenFlags.ProtectionComplete;

    public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    public static bool IsAuthenticated = false;
}