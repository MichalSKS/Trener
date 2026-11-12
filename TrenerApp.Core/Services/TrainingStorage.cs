using System.Text.Json;
using TrenerApp.Core.Models;

namespace TrenerApp.Core.Services;

// Ukládání a načítání tréninků ze souboru trainings.json (funguje stejně jako PlayerStorage)
public static class TrainingStorage
{
    // Cesta k souboru, nastavuje se při spuštění aplikace v MauiProgram.cs
    public static string FilePath { get; set; } = "trainings.json";

    public static List<Training> LoadTrainings()
    {
        // Při prvním spuštění vytvoříme 3 testovací tréninky.
        // Docházka potřebuje Id hráčů, proto nejdřív načteme soupisku.
        if (!File.Exists(FilePath))
        {
            List<Player> players = PlayerStorage.LoadPlayers();
            List<Training> sampleTrainings = SampleData.CreateTrainings(players);
            SaveTrainings(sampleTrainings);
        }

        string json = File.ReadAllText(FilePath);
        List<Training>? trainings = JsonSerializer.Deserialize<List<Training>>(json, JsonSettings.Options);

        if (trainings == null)
            return new List<Training>();

        // Nejnovější trénink bude nahoře
        return trainings.OrderByDescending(t => t.Date).ThenByDescending(t => t.StartTime).ToList();
    }

    public static void SaveTrainings(List<Training> trainings)
    {
        string json = JsonSerializer.Serialize(trainings, JsonSettings.Options);
        File.WriteAllText(FilePath, json);
    }
}
