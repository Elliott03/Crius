using System.Security.Cryptography;
using System.Text;
using SQLite;
using System.IO;
public class CriusDatabase
{
    SQLiteAsyncConnection Database;
    string passwordFileTextPath;

    public CriusDatabase()
    {
        passwordFileTextPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "password.txt"); 
    }
    public async Task<List<PasswordItem>> GetPasswords()
    {
        await Init();
        return await Database.Table<PasswordItem>().ToListAsync();
    }

    public async Task SavePassword(PasswordItem passwordItem)
    {
        await Init();
        passwordItem.Id = await GeneratePrimaryKey();
        await Database.InsertOrReplaceAsync(passwordItem);
    }
    async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await Database.CreateTableAsync<PasswordItem>();
    }

    public async Task<bool> Authenticate(string incomingPassword)
    {
        await Init();
        await SaveAuthenticationPassword();
        // Get stored password from file
        string authenticationPassword = "";
        try 
        {
            StreamReader sr = new StreamReader(passwordFileTextPath);
            authenticationPassword = sr.ReadLine();
            sr.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
        var storedPassword = authenticationPassword;

        if (storedPassword != null)
            return await CheckPassword(incomingPassword, storedPassword);
        return false;
    }

    private async Task<bool> CheckPassword(string incomingPassword, string storedPasswordHashAsByteArray)
    {
        byte[] incomingPasswordAsByteArray = Encoding.UTF8.GetBytes(incomingPassword);
        HashAlgorithm sha = SHA256.Create();
        byte[] incomingPasswordHash = sha.ComputeHash(incomingPasswordAsByteArray);
        return Encoding.ASCII.GetString(incomingPasswordHash) == storedPasswordHashAsByteArray;
    }

    public async Task UpdatePasswordItem(PasswordItem passwordItem) 
    {
        await Database.UpdateAsync(passwordItem);
    }

    // Use this method to create your authentication password for first use
    public async Task SaveAuthenticationPassword()
    {
        await Init();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(" ");
        HashAlgorithm sha = SHA256.Create();
        byte[] passwordHash = sha.ComputeHash(passwordBytes);
        string passwordText = Encoding.ASCII.GetString(passwordHash);

        using (StreamWriter outputFile = new StreamWriter(passwordFileTextPath))
        {
            outputFile.WriteLine(passwordText);
        }
    }

    // The struggles of dealing with .NET MAUI strike again -- implementing custom primary key auto incrementing logic
    private async Task<int> GeneratePrimaryKey() 
    {
        var x = await Database.Table<PasswordItem>().ToListAsync();
        if (x.Count <= 0)
            return 1;
        return x.Max(p => p.Id) + 1;


    }
    public async Task DeleteById(int id)
    {
        await Database.Table<PasswordItem>().DeleteAsync(p => p.Id == id);
    }
    // Not hooked up, but use if you need to reset passwords for any reason
    public async Task DeleteAll()
    {
        await Init();
        await Database.DeleteAllAsync<PasswordItem>();
    }
}