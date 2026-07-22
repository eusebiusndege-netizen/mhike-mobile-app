using MHike.Models;
using MHike.Services;
using Microsoft.Maui.Controls;

namespace MHike.ViewModels;

public class ObservationsViewModel : BaseViewModel
{
    private readonly HikeRepository _repository;
    private readonly int _hikeId;
    private string _hikeName = string.Empty;

    public ObservationsViewModel(int hikeId)
    {
        _repository = new HikeRepository();
        _hikeId = hikeId;

        Observations = new List<Observation>();
        AddObservationCommand = new Command(async () => await AddObservationAsync());
        EditObservationCommand = new Command<Observation>(async (obs) => await EditObservationAsync(obs));
        DeleteObservationCommand = new Command<Observation>(async (obs) => await DeleteObservationAsync(obs));

        _ = LoadDataAsync();
    }

    public string HikeName
    {
        get => _hikeName;
        set => SetProperty(ref _hikeName, value);
    }

    private List<Observation> _observations = new List<Observation>();
    public List<Observation> Observations
    {
        get => _observations;
        set => SetProperty(ref _observations, value);
    }

    public Command AddObservationCommand { get; }
    public Command<Observation> EditObservationCommand { get; }
    public Command<Observation> DeleteObservationCommand { get; }

    private async Task LoadDataAsync()
    {
        try
        {
            var hike = await _repository.GetHikeAsync(_hikeId);
            if (hike != null)
            {
                HikeName = hike.Name;
            }

            var observations = await _repository.GetObservationsAsync(_hikeId);
            Observations = observations ?? new List<Observation>();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to load observations: {ex.Message}", "OK");
        }
    }

    private async Task AddObservationAsync()
    {
        string title = await Shell.Current.DisplayPromptAsync("New Observation", "Enter observation title:", "Save", "Cancel", "e.g., Saw a deer");

        if (string.IsNullOrWhiteSpace(title)) return;

        string comment = await Shell.Current.DisplayPromptAsync("Observation Comment", "Enter additional comments (optional):", "Save", "Cancel", "", -1, Keyboard.Default, "");

        try
        {
            var observation = new Observation
            {
                HikeId = _hikeId,
                Title = title.Trim(),
                Comment = comment?.Trim() ?? "",
                Date = DateTime.Now
            };

            await _repository.SaveObservationAsync(observation);
            await LoadDataAsync();
            await Shell.Current.DisplayAlertAsync("Success", "Observation added successfully!", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to add observation: {ex.Message}", "OK");
        }
    }

    private async Task EditObservationAsync(Observation observation)
    {
        if (observation == null) return;

        string newTitle = await Shell.Current.DisplayPromptAsync("Edit Observation", "Edit observation title:", "Save", "Cancel", observation.Title);

        if (string.IsNullOrWhiteSpace(newTitle)) return;

        string newComment = await Shell.Current.DisplayPromptAsync("Edit Comment", "Edit comments:", "Save", "Cancel", observation.Comment, -1, Keyboard.Default, "");

        try
        {
            observation.Title = newTitle.Trim();
            observation.Comment = newComment?.Trim() ?? "";
            await _repository.SaveObservationAsync(observation);
            await LoadDataAsync();
            await Shell.Current.DisplayAlertAsync("Success", "Observation updated successfully!", "OK");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to update observation: {ex.Message}", "OK");
        }
    }

    private async Task DeleteObservationAsync(Observation observation)
    {
        if (observation == null) return;

        bool answer = await Shell.Current.DisplayAlertAsync(
            "Delete Observation",
            $"Are you sure you want to delete '{observation.Title}'?",
            "Yes", "No");

        if (answer)
        {
            try
            {
                await _repository.DeleteObservationAsync(observation);
                await LoadDataAsync();
                await Shell.Current.DisplayAlertAsync("Success", "Observation deleted successfully!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to delete observation: {ex.Message}", "OK");
            }
        }
    }
}