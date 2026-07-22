using MHike.Models;
using MHike.Services;
using Microsoft.Maui.Controls;

namespace MHike.ViewModels;

public class RegisterViewModel : BaseViewModel
{
    private readonly HikeRepository _repository;

    public RegisterViewModel()
    {
        _repository = new HikeRepository();
        RegisterCommand = new Command(async () => await RegisterAsync());
        NavigateToLoginCommand = new Command(async () => await NavigateToLoginAsync());
    }

    private string _username = string.Empty;
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    private string _confirmPassword = string.Empty;
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    public Command RegisterCommand { get; }
    public Command NavigateToLoginCommand { get; }

    private async Task RegisterAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ErrorMessage = "Please fill in all fields";
            OnPropertyChanged(nameof(HasError));
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match";
            OnPropertyChanged(nameof(HasError));
            return;
        }

        if (Password.Length < 3)
        {
            ErrorMessage = "Password must be at least 3 characters long";
            OnPropertyChanged(nameof(HasError));
            return;
        }

        try
        {
            await Shell.Current.DisplayAlertAsync("Success", "Account created successfully! Please sign in.", "OK");
            ErrorMessage = string.Empty;
            OnPropertyChanged(nameof(HasError));

            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;

            await Shell.Current.GoToAsync("///LoginPage");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Registration failed: {ex.Message}";
            OnPropertyChanged(nameof(HasError));
        }
    }

    private async Task NavigateToLoginAsync()
    {
        await Shell.Current.GoToAsync("///LoginPage");
    }
}