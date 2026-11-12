using TrenerApp.Core.Models;

namespace TrenerApp.Core.Services;

// Fiktivní testovací data (11 hráčů podle návrhu z 1. konzultace)
public static class SampleData
{
    public static List<Player> CreatePlayers() => new()
    {
        Create(1, "Jan", "Novák", PlayerPosition.Goalkeeper),
        Create(2, "Petr", "Svoboda", PlayerPosition.Defender),
        Create(3, "Tomáš", "Dvořák", PlayerPosition.Defender),
        Create(4, "Pavel", "Černý", PlayerPosition.Defender),
        Create(5, "Jiří", "Procházka", PlayerPosition.Defender),
        Create(6, "Lukáš", "Kučera", PlayerPosition.Midfielder),
        Create(7, "Martin", "Veselý", PlayerPosition.Midfielder),
        Create(8, "David", "Krejčí", PlayerPosition.Midfielder),
        Create(9, "Ondřej", "Horák", PlayerPosition.Midfielder),
        Create(10, "Michal", "Němec", PlayerPosition.Forward),
        Create(11, "Jakub", "Pokorný", PlayerPosition.Forward)
    };

    private static Player Create(int number, string firstName, string lastName, PlayerPosition position) => new()
    {
        JerseyNumber = number,
        FirstName = firstName,
        LastName = lastName,
        Position = position
    };

    // 3 testovací tréninky s docházkou (podle JSON z 1. konzultace).
    // Docházku zadáváme čísly dresů a převedeme je na Id hráčů.
    public static List<Training> CreateTrainings(List<Player> players)
    {
        List<Training> trainings = new List<Training>();

        trainings.Add(new Training
        {
            Name = "Úterní kondice a technika",
            Focus = "Kondice",
            Date = new DateTime(2026, 10, 20),
            StartTime = new TimeSpan(16, 30, 0),
            DurationMinutes = 90,
            AttendingPlayerIds = IdsByNumbers(players, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11)
        });

        trainings.Add(new Training
        {
            Name = "Čtvrteční příprava na zápas",
            Focus = "Taktika a střelba",
            Date = new DateTime(2026, 10, 22),
            StartTime = new TimeSpan(17, 0, 0),
            DurationMinutes = 75,
            AttendingPlayerIds = IdsByNumbers(players, 1, 2, 4, 5, 6, 8, 9)
        });

        trainings.Add(new Training
        {
            Name = "Dobrovolný víkendový trénink",
            Focus = "Individuální dovednosti",
            Date = new DateTime(2026, 10, 24),
            StartTime = new TimeSpan(10, 0, 0),
            DurationMinutes = 60,
            AttendingPlayerIds = IdsByNumbers(players, 3, 7, 8, 9, 10, 11)
        });

        return trainings;
    }

    // Najde hráče s danými čísly dresů a vrátí jejich Id
    private static List<Guid> IdsByNumbers(List<Player> players, params int[] numbers)
    {
        List<Guid> ids = new List<Guid>();
        foreach (Player player in players)
        {
            if (numbers.Contains(player.JerseyNumber))
                ids.Add(player.Id);
        }
        return ids;
    }
}
