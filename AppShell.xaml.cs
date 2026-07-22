using MHike.Pages;

namespace MHike;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
        Routing.RegisterRoute("AddEditHikePage", typeof(AddEditHikePage));
        Routing.RegisterRoute("ObservationsPage", typeof(ObservationsPage));
    }
}