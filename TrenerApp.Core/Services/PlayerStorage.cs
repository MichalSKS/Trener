using System.Text.Json;
using TrenerApp.Core.Models;

namespace TrenerApp.Core.Services;

// Ukládání a načítání hráčů ze souboru players.json
public static class PlayerStorage
{
    // Cesta k souboru, nastavuje se při spuštění aplikace v MauiProgram.cs
    public static string FilePath { get; set; } = "players.json";

    // Načte všechny hráče ze souboru
    public static List<Player> LoadPlayers()
    {
        // Při prvním spuštění soubor neexistuje -> vytvoříme ho z testovacích dat
        if (!File.Exists(FilePath))
        {
            List<Player> samplePlayers = SampleData.CreatePlayers();
            SavePlayers(samplePlayers);
            return samplePlayers;
        }

        string json = File.ReadAllText(FilePath);
        List<Player>? players = JsonSerializer.Deserialize<List<Player>>(json, JsonSettings.Options);

        // Kdyby byl soubor prázdný, vrátíme prázdný seznam
        if (players == null)
            return new List<Player>();

        // Seřadíme podle čísla dresu
        return players.OrderBy(p => p.JerseyNumber).ToList();
    }

    // Uloží celý seznam hráčů do souboru (starý obsah se přepíše)
    public static void SavePlayers(List<Player> players)
    {
        string json = JsonSerializer.Serialize(players, JsonSettings.Options);
        File.WriteAllText(FilePath, json);
    }
}
