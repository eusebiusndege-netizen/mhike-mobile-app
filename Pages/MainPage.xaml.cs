using MHike.ViewModels;
using MHike.Models;

namespace MHike;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new HikeViewModel();
    }

    private async void OnAddHikeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///AddEditHikePage");
    }

    private async void OnEditHikeClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Hike hike)
        {
            await Shell.Current.GoToAsync($"///AddEditHikePage?HikeId={hike.Id}");
        }
    }
}