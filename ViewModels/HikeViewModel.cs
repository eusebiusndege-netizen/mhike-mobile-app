using MHike.Models;
using MHike.Services;
using Microsoft.Maui.Controls;

namespace MHike.ViewModels;

public class HikeViewModel : BaseViewModel
{
    private readonly HikeRepository _repository;
    private List<Hike> _hikes = new List<Hike>();

    public HikeViewModel()
    {
        _repository = new HikeRepository();
        DeleteHikeCommand = new Command<Hike>(OnDeleteHike);
        ViewObservationsCommand = new Command<Hike>(OnViewObservations);
        _ = LoadHikesAsync();
    }

    public List<Hike> Hikes
    {
        get => _hikes;
        set => SetProperty(ref _hikes, value);
    }

    public Command<Hike> DeleteHikeCommand { get; }
    public Command<Hike> ViewObservationsCommand { get; }

    public async Task LoadHikesAsync()
    {
        try
        {
            var hikes = await _repository.GetHikesAsync();
            Hikes = hikes;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", $"Failed to load hikes: {ex.Message}", "OK");
        }
    }

    private async void OnDeleteHike(Hike? hike)
    {
        if (hike == null) return;

        bool answer = await Shell.Current.DisplayAlertAsync(
            "Delete Hike",
            $"Are you sure you want to delete {hike.Name}?",
            "Yes", "No");

        if (answer)
        {
            try
            {
                await _repository.DeleteHikeAsync(hike);
                await LoadHikesAsync();
                await Shell.Current.DisplayAlertAsync("Success", "Hike deleted successfully!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to delete hike: {ex.Message}", "OK");
            }
        }
    }

    private async void OnViewObservations(Hike hike)
    {
        if (hike != null)
        {
            await Shell.Current.GoToAsync($"///ObservationsPage?HikeId={hike.Id}");
        }
    }
}