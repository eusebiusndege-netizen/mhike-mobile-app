using MHike.ViewModels;

namespace MHike.Pages;

public partial class ObservationsPage : ContentPage
{
    public ObservationsPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (Shell.Current?.CurrentState?.Location != null)
        {
            var uri = Shell.Current.CurrentState.Location.ToString();
            var queryParams = System.Web.HttpUtility.ParseQueryString(new Uri(uri).Query);

            if (int.TryParse(queryParams["HikeId"], out int hikeId))
            {
                BindingContext = new ObservationsViewModel(hikeId);
            }
        }
    }
}