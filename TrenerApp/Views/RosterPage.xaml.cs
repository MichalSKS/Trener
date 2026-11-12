using TrenerApp.Core.Models;
using TrenerApp.Core.Services;

namespace TrenerApp.Views;

// Obrazovka se seznamem hráčů
public partial class RosterPage : ContentPage
{
    public RosterPage()
    {
        InitializeComponent();

        TeamLabel.Text = TeamInfo.Name.ToUpper();
        SubtitleLabel.Text = TeamInfo.Category + " · Aktuální forma týmu";
    }

    // OnAppearing se zavolá vždy, když se obrazovka zobrazí.
    // Díky tomu se seznam obnoví i po návratu z formuláře.
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadPlayers();
    }

    private void LoadPlayers()
    {
        List<Player> players = PlayerStorage.LoadPlayers();

        BindableLayout.SetItemsSource(PlayersLayout, players);
        CountLabel.Text = TeamInfo.PlayersCount(players.Count);
        EmptyLabel.IsVisible = players.Count == 0;
    }

    // Klepnutí na hráče -> otevře se formulář s jeho údaji
    private async void OnPlayerTapped(object? sender, TappedEventArgs e)
    {
        Border border = (Border)sender!;
        Player player = (Player)border.BindingContext;

        await Navigation.PushAsync(new PlayerEditPage(player));
    }

    // Tlačítko + -> otevře se prázdný formulář
    private async void OnAddClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new PlayerEditPage(null));
    }
}
