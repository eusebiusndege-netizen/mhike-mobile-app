using MHike.ViewModels;

namespace MHike.Pages;

public partial class AddEditHikePage : ContentPage
{
    public AddEditHikePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (Shell.Current?.CurrentState?.Location != null)
        {
            var uri = Shell.Current.CurrentState.Location.ToString();
            var query = System.Web.HttpUtility.ParseQueryString(new Uri(uri).Query);

            if (int.TryParse(query["HikeId"], out int hikeId))
            {
                BindingContext = new AddEditHikeViewModel(hikeId);
            }
            else
            {
                BindingContext = new AddEditHikeViewModel();
            }
        }
        else
        {
            BindingContext = new AddEditHikeViewModel();
        }
    }
}