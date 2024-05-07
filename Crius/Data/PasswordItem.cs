using SQLite;

public class PasswordItem
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string? Designation { get; set; }
    public string? Password { get; set; }
}