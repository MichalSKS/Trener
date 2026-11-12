namespace TrenerApp;

// Údaje o týmu, které se zobrazují v aplikaci
public static class TeamInfo
{
    public const string Name = "TJ Sokol Líšeň";
    public const string Category = "Mladší žáci";
    public const string Season = "Sezóna 2026/27";

    // Vrátí text se správným tvarem slova: 1 hráč, 2 hráči, 5 hráčů
    public static string PlayersCount(int count)
    {
        if (count == 1)
            return "1 hráč";
        else if (count >= 2 && count <= 4)
            return count + " hráči";
        else
            return count + " hráčů";
    }
}
