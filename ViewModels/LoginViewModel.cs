using MHike.Models;
using MHike.Services;
using Microsoft.Maui.Controls;

namespace MHike.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly HikeRepository _repository;

    public LoginViewModel()
    {
        _repository = new HikeRepository();
        LoginCommand = new Command(async () => await LoginAsync());
        NavigateToRegisterCommand = new Command(async () => await NavigateToRegisterAsync());
    }

    private string _username = "demo";
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    private string _password = "demo";
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public Command LoginCommand { get; }
    public Command NavigateToRegisterCommand { get; }

    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please enter both username and password";
            OnPropertyChanged(nameof(HasError));
            return;
        }

        try
        {
            if (Username.ToLower() == "demo" && Password == "demo")
            {
                ErrorMessage = string.Empty;
                OnPropertyChanged(nameof(HasError));
                await Shell.Current.GoToAsync("///MainPage");
            }
            else
            {
                ErrorMessage = "Invalid credentials. Use 'demo' for both username and password.";
                OnPropertyChanged(nameof(HasError));
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Login failed: {ex.Message}";
            OnPropertyChanged(nameof(HasError));
        }
    }

    private async Task NavigateToRegisterAsync()
    {
        await Shell.Current.GoToAsync("///RegisterPage");
    }
}