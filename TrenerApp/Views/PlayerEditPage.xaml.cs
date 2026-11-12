using TrenerApp.Core.Models;
using TrenerApp.Core.Services;

namespace TrenerApp.Views;

// Formulář pro přidání nového hráče nebo úpravu existujícího
public partial class PlayerEditPage : ContentPage
{
    private Player player;   // hráč, kterého upravujeme
    private bool isNew;      // true = přidáváme nového hráče

    // Když přijde null, jde o nového hráče
    public PlayerEditPage(Player? playerToEdit)
    {
        InitializeComponent();

        // Posty v Pickeru jsou ve stejném pořadí jako v enumu PlayerPosition,
        // takže SelectedIndex odpovídá číslu postu (0 = brankář, 1 = obránce...)
        PositionPicker.ItemsSource = new List<string> { "Brankář", "Obránce", "Záložník", "Útočník" };

        if (playerToEdit == null)
        {
            player = new Player();
            isNew = true;
        }
        else
        {
            player = playerToEdit;
            isNew = false;
        }

        ShowPlayer();
    }

    // Vyplní formulář údaji hráče
    private void ShowPlayer()
    {
        if (isNew)
        {
            ModeLabel.Text = "NOVÝ HRÁČ";
            TitleLabel.Text = "Nový hráč";
            DeleteButton.IsVisible = false;
        }
        else
        {
            ModeLabel.Text = "ÚPRAVA HRÁČE";
            TitleLabel.Text = player.FullName;
            DeleteButton.IsVisible = true;
            NumberEntry.Text = player.JerseyNumber.ToString();
        }

        FirstNameEntry.Text = player.FirstName;
        LastNameEntry.Text = player.LastName;
        PositionPicker.SelectedIndex = (int)player.Position;
        NoteEditor.Text = player.Note;
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        string firstName = FirstNameEntry.Text?.Trim() ?? "";
        string lastName = LastNameEntry.Text?.Trim() ?? "";

        // Kontrola, že je vyplněné jméno a příjmení
        if (firstName == "" || lastName == "")
        {
            await DisplayAlertAsync("Chybí údaje", "Vyplňte jméno i příjmení hráče.", "OK");
            return;
        }

        // Kontrola čísla dresu (musí to být číslo 1 až 99)
        int number;
        if (!int.TryParse(NumberEntry.Text, out number) || number < 1 || number > 99)
        {
            await DisplayAlertAsync("Číslo dresu", "Zadejte číslo dresu od 1 do 99.", "OK");
            return;
        }

        List<Player> players = PlayerStorage.LoadPlayers();

        // Dva hráči nemůžou mít stejné číslo dresu
        foreach (Player p in players)
        {
            if (p.JerseyNumber == number && p.Id != player.Id)
            {
                await DisplayAlertAsync("Číslo dresu", "Číslo " + number + " už má " + p.FullName + ".", "OK");
                return;
            }
        }

        // Přepíšeme údaje z formuláře do hráče
        player.FirstName = firstName;
        player.LastName = lastName;
        player.JerseyNumber = number;
        player.Position = (PlayerPosition)PositionPicker.SelectedIndex;
        player.Note = NoteEditor.Text?.Trim() ?? "";

        if (isNew)
        {
            players.Add(player);
        }
        else
        {
            // Najdeme hráče podle Id a nahradíme ho upravenou verzí
            int index = players.FindIndex(p => p.Id == player.Id);
            players[index] = player;
        }

        PlayerStorage.SavePlayers(players);

        // Návrat zpět na soupisku
        await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        // Zeptáme se, jestli to trenér myslí vážně
        bool answer = await DisplayAlertAsync("Odebrat hráče",
            "Opravdu odebrat hráče " + player.FullName + " ze soupisky?", "Odebrat", "Zrušit");

        if (answer == false)
            return;

        List<Player> players = PlayerStorage.LoadPlayers();
        players.RemoveAll(p => p.Id == player.Id);
        PlayerStorage.SavePlayers(players);

        // Hráče musíme odebrat i z docházky na trénincích
        List<Training> trainings = TrainingStorage.LoadTrainings();
        foreach (Training training in trainings)
        {
            training.AttendingPlayerIds.Remove(player.Id);
        }
        TrainingStorage.SaveTrainings(trainings);

        await Navigation.PopAsync();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
