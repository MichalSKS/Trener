using Microsoft.Extensions.DependencyInjection;

namespace TrenerApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Návrh počítá se světlým vzhledem (lépe čitelné venku na hřišti)
            UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}