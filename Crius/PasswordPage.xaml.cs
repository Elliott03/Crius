using System.ComponentModel;

namespace Crius;
public partial class PasswordPage: ContentPage, INotifyPropertyChanged
{
        CriusDatabase _database;
        List<PasswordItem> _passwords;
        public List<PasswordItem> Passwords;
        public List<PasswordItem> FilteredPasswords
        {
            get { return _passwords; }
            set
            {
                _passwords = value;
                OnPropertyChanged(nameof(FilteredPasswords));
            }
        }

        public PasswordPage(CriusDatabase database)
        {
            BindingContext = this;
            _database = database;

            InitializeComponent();
            LoadPasswordsAsync();
        }
        private async Task LoadPasswordsAsync()
        {
            Passwords = await _database.GetPasswords();
            FilteredPasswords = Passwords;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

	private async void CreatePassword (object sender, EventArgs e)
	{

        if (!Constants.IsAuthenticated) 
        {
            return;
        }
        string designation = await DisplayPromptAsync("Add New Password", "What designation is this password for?:");
        if (designation == null)
            return;
        string password = await DisplayPromptAsync("Add New Password", "What is your password for this designation?");
        if (password == null)
            return;
        PasswordItem passwordItem = new PasswordItem
        {
            Designation = designation,
            Password = password
        };
        await _database.SavePassword(passwordItem);
        await LoadPasswordsAsync();
        OnPropertyChanged(nameof(Passwords));
	}
    void SearchTextChanged(object sender, EventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;
        FilteredPasswords = Passwords.Where(p => p.Designation != null && p.Designation.ToLower().Contains(searchBar.Text.ToLower()) || p.Password != null && p.Password.ToLower().Contains(searchBar.Text.ToLower())).ToList();
    }
    private async void OnPasswordSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (!Constants.IsAuthenticated) 
        {
            return;
        }
        if (e.SelectedItem == null)
            return; 
        var selectedItem = (PasswordItem)e.SelectedItem;

        // This is a horrible solution, but DisplayActionSheet is stll broken, and the .NET MAUI team seem either incapable or unwilling to fix it.
        bool deleteEditAnswer = await DisplayAlert($"{selectedItem.Designation} \n {selectedItem.Password}", "What would you like to do to this password item?", "Delete", "Edit"); // Delete = true, Edit = false
        if (deleteEditAnswer == false)
        {
            bool designationPasswordAnswer = await DisplayAlert($"{selectedItem.Designation} \n {selectedItem.Password}", "Would you like to edit the Designation or the Password?", "Password", "Designation"); // Designation = true, Password = false
            if (designationPasswordAnswer == false) 
            {
                string designation = await DisplayPromptAsync(selectedItem.Designation, "What do you want to change the designation to?");
                if (designation != null)
                {
                    selectedItem.Designation = designation;
                    await _database.UpdatePasswordItem(selectedItem);
                    await LoadPasswordsAsync();
                    OnPropertyChanged(nameof(Passwords)); 

                }
            } else if (designationPasswordAnswer == true)
            {
                string pass = await DisplayPromptAsync(selectedItem.Password, "What do you want to change the password to?");
                if (pass != null)
                {
                    selectedItem.Password = pass;
                    await _database.UpdatePasswordItem(selectedItem);
                    await LoadPasswordsAsync();
                    OnPropertyChanged(nameof(Passwords));
                }
            }
        } else if (deleteEditAnswer == true)
        {
            bool confirmation = await DisplayAlert($"{selectedItem.Designation} \n {selectedItem.Password}", "Are you sure you want to delete this password item?", "Yes", "No");
            if (confirmation == true) 
            {
                await _database.DeleteById(selectedItem.Id);
                await LoadPasswordsAsync();
                OnPropertyChanged(nameof(Passwords));
            }
        }
        // Reset the selected item to null (optional)
        ((ListView)sender).SelectedItem = null;
    }

}