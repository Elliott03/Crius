
namespace Crius;

public partial class MainPage : ContentPage
{
	private readonly CriusDatabase _database;
	public MainPage(CriusDatabase database)
	{
		_database = database;
		InitializeComponent();
	}
	private async void CreatePassword (object sender, EventArgs e)
	{
		await _database.SaveAuthenticationPassword();
	}

	private async void Login (object sender, EventArgs e)
	{
		bool isAuthenticated = await _database.Authenticate(Password.Text);
		Constants.IsAuthenticated = isAuthenticated;
		if (isAuthenticated)
		{
			await Navigation.PushAsync(new PasswordPage(_database));
		} 
		else 
		{
			ErrorMessage.Text = "Incorrect Password";
		}
	}
}

