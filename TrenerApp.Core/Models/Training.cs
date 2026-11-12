using System.Globalization;
using System.Text.Json.Serialization;

namespace TrenerApp.Core.Models;

// Jeden trénink včetně docházky
public class Training
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Focus { get; set; } = string.Empty;           // zaměření, např. "Kondice"
    public DateTime Date { get; set; } = DateTime.Today;
    public TimeSpan StartTime { get; set; } = new TimeSpan(17, 0, 0);
    public int DurationMinutes { get; set; } = 90;

    // Docházka = seznam Id hráčů, kteří na tréninku byli
    public List<Guid> AttendingPlayerIds { get; set; } = new List<Guid>();

    // Pomocné vlastnosti pro zobrazení, do JSON se neukládají
    private static readonly CultureInfo czech = new CultureInfo("cs-CZ");

    [JsonIgnore]
    public TimeSpan EndTime => StartTime + TimeSpan.FromMinutes(DurationMinutes);

    // např. "16:30–18:00"
    [JsonIgnore]
    public string TimeLabel => StartTime.ToString(@"hh\:mm") + "–" + EndTime.ToString(@"hh\:mm");

    // např. "22. října 2026 · čtvrtek"
    [JsonIgnore]
    public string DateLabel => Date.ToString("d. MMMM yyyy · dddd", czech);

    // Den a měsíc do čtverečku v seznamu tréninků ("22" a "ŘÍJ")
    [JsonIgnore]
    public string DayLabel => Date.Day.ToString();

    [JsonIgnore]
    public string MonthLabel => Date.ToString("MMM", czech).ToUpper();

    [JsonIgnore]
    public string AttendanceCount => AttendingPlayerIds.Count.ToString();
}
