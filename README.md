# Crius Password Manager

Crius is an offline password manager application built using .NET MAUI framework. It provides a secure and convenient way to manage your passwords and sensitive credentials locally on your device.

## Features

- **Secure Authentication**: Crius employs password-based authentication with hashed passwords stored locally, ensuring the security and privacy of user credentials.
  
- **SQLite Database**: Company/password key-value pairs are stored in an SQLite database, enabling efficient retrieval and management of credentials.

- **Password Management**: Users can easily add, delete, edit, and search through stored passwords, providing a seamless experience for managing credentials.

## Getting Started

To get started with Crius, follow these steps:

1. Clone the repository to your local machine.
2. Build the solution using Visual Studio or the .NET CLI.
3. Run the application on your preferred platform (Windows, macOS, iOS, Android).

## Usage

Upon launching Crius, you will be prompted to authenticate using your password. Once authenticated, you will have access to the main dashboard where you can manage your passwords. Here are some key actions you can perform:

- **Add**: Click the "Add" button to add a new company/password pair.
- **Edit**: Double-click on an existing entry to edit its details.
- **Delete**: Select an entry and click the "Delete" button to remove it from the database.
- **Search**: Use the search bar to quickly find specific passwords.

## Security Considerations

- **Local Storage**: All passwords are stored locally on your device, ensuring that your sensitive information does not leave your control.
- **Hashed Passwords**: User passwords are hashed before storage, providing an additional layer of security against unauthorized access.

## Contributing

Contributions to Crius are welcome! If you encounter any bugs or have suggestions for improvements, please open an issue or submit a pull request on GitHub.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
