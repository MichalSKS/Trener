using System.Globalization;
using TrenerApp.Core.Models;
using TrenerApp.Core.Services;

namespace TrenerApp.Views;

// Formulář tréninku: údaje o tréninku + zápis docházky
public partial class TrainingEditPage : ContentPage
{
    private Training training;    // trénink, který upravujeme
    private bool isNew;           // true = nový trénink
    private List<AttendanceRow> rows = new List<AttendanceRow>();   // řádky docházky

    // Když přijde null, jde o nový trénink
    public TrainingEditPage(Training? trainingToEdit)
    {
        InitializeComponent();

        if (trainingToEdit == null)
        {
            training = new Training();
            isNew = true;
        }
        else
        {
            training = trainingToEdit;
            isNew = false;
        }

        ShowTraining();
    }

    // Vyplní formulář údaji tréninku
    private void ShowTraining()
    {
        if (isNew)
        {
            ModeLabel.Text = "NOVÝ TRÉNINK";
            DeleteButton.IsVisible = false;
        }
        else
        {
            ModeLabel.Text = "ÚPRAVA TRÉNINKU";
            DeleteButton.IsVisible = true;
        }

        NameEntry.Text = training.Name;
        FocusEntry.Text = training.Focus;
        TrainingDatePicker.Date = training.Date;
        StartTimePicker.Time = training.StartTime;
        DurationEntry.Text = training.DurationMinutes.ToString();

        // Pro každého hráče ze soupisky vytvoříme řádek docházky.
        // Zaškrtnutí jsou ti, jejichž Id je v seznamu docházky tréninku.
        List<Player> players = PlayerStorage.LoadPlayers();
        rows = new List<AttendanceRow>();
        for (int i = 0; i < players.Count; i++)
        {
            AttendanceRow row = new AttendanceRow();
            row.Player = players[i];
            row.IsPresent = training.AttendingPlayerIds.Contains(players[i].Id);
            row.ShowLine = i > 0;
            rows.Add(row);
        }
        BindableLayout.SetItemsSource(AttendanceLayout, rows);

        UpdateHeader();
        UpdateAttendanceLabel();
    }

    // Aktualizuje datum a čas v hlavičce podle toho, co je ve formuláři
    private void UpdateHeader()
    {
        CultureInfo czech = new CultureInfo("cs-CZ");
        DateTime date = TrainingDatePicker.Date ?? DateTime.Today;
        TimeSpan start = StartTimePicker.Time ?? training.StartTime;

        int minutes;
        if (!int.TryParse(DurationEntry.Text, out minutes))
            minutes = 0;

        TimeSpan end = start + TimeSpan.FromMinutes(minutes);

        DateLabel.Text = date.ToString("d. MMMM yyyy · dddd", czech);
        TimeLabel.Text = start.ToString(@"hh\:mm") + "–" + end.ToString(@"hh\:mm");
    }

    // Např. "5 ze 7 přítomno"
    private void UpdateAttendanceLabel()
    {
        int present = rows.Count(r => r.IsPresent);
        AttendanceLabel.Text = present + " ze " + rows.Count + " přítomno";
    }

    private void OnDateOrTimeChanged(object? sender, EventArgs e)
    {
        UpdateHeader();
    }

    private void OnDurationChanged(object? sender, TextChangedEventArgs e)
    {
        UpdateHeader();
    }

    private void OnAttendanceChanged(object? sender, CheckedChangedEventArgs e)
    {
        // Pro jistotu zapíšeme hodnotu do řádku sami a pak přepočítáme počet
        CheckBox checkBox = (CheckBox)sender!;
        if (checkBox.BindingContext is AttendanceRow row)
            row.IsPresent = e.Value;

        UpdateAttendanceLabel();
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        string name = NameEntry.Text?.Trim() ?? "";
        if (name == "")
        {
            await DisplayAlertAsync("Chybí název", "Vyplňte název tréninku.", "OK");
            return;
        }

        int minutes;
        if (!int.TryParse(DurationEntry.Text, out minutes) || minutes < 1 || minutes > 300)
        {
            await DisplayAlertAsync("Délka tréninku", "Zadejte délku v minutách (1 až 300).", "OK");
            return;
        }

        // Přepíšeme údaje z formuláře do tréninku
        training.Name = name;
        training.Focus = FocusEntry.Text?.Trim() ?? "";
        training.Date = TrainingDatePicker.Date ?? DateTime.Today;
        training.StartTime = StartTimePicker.Time ?? training.StartTime;
        training.DurationMinutes = minutes;

        // Docházka: uložíme Id všech zaškrtnutých hráčů
        training.AttendingPlayerIds = new List<Guid>();
        foreach (AttendanceRow row in rows)
        {
            if (row.IsPresent)
                training.AttendingPlayerIds.Add(row.Player.Id);
        }

        List<Training> trainings = TrainingStorage.LoadTrainings();
        if (isNew)
        {
            trainings.Add(training);
        }
        else
        {
            int index = trainings.FindIndex(t => t.Id == training.Id);
            trainings[index] = training;
        }
        TrainingStorage.SaveTrainings(trainings);

        await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        bool answer = await DisplayAlertAsync("Smazat trénink",
            "Opravdu smazat trénink " + training.Name + "?", "Smazat", "Zrušit");

        if (answer == false)
            return;

        List<Training> trainings = TrainingStorage.LoadTrainings();
        trainings.RemoveAll(t => t.Id == training.Id);
        TrainingStorage.SaveTrainings(trainings);

        await Navigation.PopAsync();
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
