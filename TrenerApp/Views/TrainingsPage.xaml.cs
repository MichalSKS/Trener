using TrenerApp.Core.Models;
using TrenerApp.Core.Services;

namespace TrenerApp.Views;

// Obrazovka se seznamem tréninků
public partial class TrainingsPage : ContentPage
{
    public TrainingsPage()
    {
        InitializeComponent();

        TeamLabel.Text = TeamInfo.Name.ToUpper();
    }

    // Seznam se obnoví pokaždé, když se obrazovka zobrazí (např. po uložení tréninku)
    protected override void OnAppearing()
    {
        base.OnAppearing();

        List<Training> trainings = TrainingStorage.LoadTrainings();

        BindableLayout.SetItemsSource(TrainingsLayout, trainings);
        CountLabel.Text = trainings.Count + " celkem";
        EmptyLabel.IsVisible = trainings.Count == 0;
    }

    // Klepnutí na trénink -> otevře se jeho detail s docházkou
    private async void OnTrainingTapped(object? sender, TappedEventArgs e)
    {
        Border border = (Border)sender!;
        Training training = (Training)border.BindingContext;

        await Navigation.PushAsync(new TrainingEditPage(training));
    }

    // Tlačítko + -> nový trénink
    private async void OnAddClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new TrainingEditPage(null));
    }
}
