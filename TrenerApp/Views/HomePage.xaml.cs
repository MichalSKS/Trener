using TrenerApp.Core.Models;
using TrenerApp.Core.Services;

namespace TrenerApp.Views;

// Úvodní obrazovka s dlaždicemi
public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();

        TeamLabel.Text = TeamInfo.Name;
        SubtitleLabel.Text = TeamInfo.Category + " · " + TeamInfo.Season;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Počet hráčů na dlaždici Soupiska
        List<Player> players = PlayerStorage.LoadPlayers();
        RosterCountLabel.Text = TeamInfo.PlayersCount(players.Count) + " v týmu";
    }

    // "//roster" = přepne na záložku Soupiska ve spodní liště (Route v AppShell.xaml)
    private async void OnRosterTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//roster");
    }

    private async void OnTrainingsTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//trainings");
    }

    private async void OnMatchesTapped(object? sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync("//matches");
    }
}
