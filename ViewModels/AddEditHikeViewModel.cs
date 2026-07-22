using MHike.Models;
using MHike.Services;
using Microsoft.Maui.Controls;

namespace MHike.ViewModels;

public class AddEditHikeViewModel : BaseViewModel
{
    private readonly HikeRepository _repository;
    private readonly int? _hikeId;

    public AddEditHikeViewModel(int? hikeId = null)
    {
        _repository = new HikeRepository();
        _hikeId = hikeId;

        SaveHikeCommand = new Command(async () => await SaveHikeAsync());
        CancelCommand = new Command(async () => await CancelAsync());

        ParkingOptions = new List<string> { "Yes", "No" };
        LengthOptions = new List<string> { "Under 5km", "5-10km", "10-20km", "20+ km" };
        LevelOptions = new List<string> { "Easy", "Moderate", "Difficult", "Expert" };
        WeatherOptions = new List<string> { "Sunny", "Cloudy", "Rainy", "Snowy", "Windy" };

        SelectedParking = "Yes";
        SelectedLength = "5-10km";
        SelectedLevel = "Moderate";
        SelectedWeather = "Sunny";
        Date = DateTime.Now;

        _ = LoadHikeDataAsync();
    }

    private async Task LoadHikeDataAsync()
    {
        if (_hikeId.HasValue)
        {
            var hike = await _repository.GetHikeAsync(_hikeId.Value);
            if (hike != null)
            {
                Name = hike.Name;
                Location = hike.Location;
                Date = hike.Date;
                SelectedParking = hike.ParkingAvailable ? "Yes" : "No";
                SelectedLength = hike.Length;
                SelectedLevel = hike.Level;
                Description = hike.Description;
                OnPropertyChanged(nameof(PageTitle));
            }
        }
    }

    public string PageTitle => _hikeId.HasValue ? "Edit Hike" : "Add New Hike";

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _location = string.Empty;
    public string Location
    {
        get => _location;
        set => SetProperty(ref _location, value);
    }

    private DateTime _date;
    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    private string _selectedParking = string.Empty;
    public string SelectedParking
    {
        get => _selectedParking;
        set => SetProperty(ref _selectedParking, value);
    }

    private string _selectedLength = string.Empty;
    public string SelectedLength
    {
        get => _selectedLength;
        set => SetProperty(ref _selectedLength, value);
    }

    private string _selectedLevel = string.Empty;
    public string SelectedLevel
    {
        get => _selectedLevel;
        set => SetProperty(ref _selectedLevel, value);
    }

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private string _estimatedTime = string.Empty;
    public string EstimatedTime
    {
        get => _estimatedTime;
        set => SetProperty(ref _estimatedTime, value);
    }

    private string _selectedWeather = string.Empty;
    public string SelectedWeather
    {
        get => _selectedWeather;
        set => SetProperty(ref _selectedWeather, value);
    }

    public List<string> ParkingOptions { get; }
    public List<string> LengthOptions { get; }
    public List<string> LevelOptions { get; }
    public List<string> WeatherOptions { get; }

    public Command SaveHikeCommand { get; }
    public Command CancelCommand { get; }

    public string NameError { get; private set; } = string.Empty;
    public bool HasNameError => !string.IsNullOrEmpty(NameError);

    public string LocationError { get; private set; } = string.Empty;
    public bool HasLocationError => !string.IsNullOrEmpty(LocationError);

    private bool ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(Name))
        {
            NameError = "Hike name is required";
            isValid = false;
        }
        else
        {
            NameError = string.Empty;
        }

        if (string.IsNullOrWhiteSpace(Location))
        {
            LocationError = "Location is required";
            isValid = false;
        }
        else
        {
            LocationError = string.Empty;
        }

        OnPropertyChanged(nameof(NameError));
        OnPropertyChanged(nameof(HasNameError));
        OnPropertyChanged(nameof(LocationError));
        OnPropertyChanged(nameof(HasLocationError));

        return isValid;
    }

    private async Task SaveHikeAsync()
    {
        if (!ValidateForm())
        {
            await Shell.Current.DisplayAlertAsync("Validation Error", "Please fill in all required fields", "OK");
            return;
        }

        try
        {
            var hike = new Hike
            {
                Name = Name?.Trim() ?? string.Empty,
                Location = Location?.Trim() ?? string.Empty,
                Date = Date,
                ParkingAvailable = SelectedParking == "Yes",
                Length = SelectedLength,
                Level = SelectedLevel,
                Description = Description?.Trim() ?? string.Empty
            };

            if (_hikeId.HasValue)
            {
                hike.Id = _hikeId.Value;
                await _repository.SaveHikeAsync(hike);
                await Shell.Current.DisplayAlertAsync("Success", "Hike updated successfully!", "OK");
            }
            else
            {
                await _repository.SaveHikeAsync(hike);
                await Shell.Current.DisplayAlertAsync("Success", "Hike added successfully!", "OK");
            }

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to save hike: {ex.Message}", "OK");
        }
    }

    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}