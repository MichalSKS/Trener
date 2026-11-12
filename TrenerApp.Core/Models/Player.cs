using System.Text.Json.Serialization;

namespace TrenerApp.Core.Models;

// Pozice hráče na hřišti
public enum PlayerPosition
{
    Goalkeeper,
    Defender,
    Midfielder,
    Forward
}

// Jeden hráč na soupisce
public class Player
{
    public Guid Id { get; set; } = Guid.NewGuid();   // unikátní identifikátor
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int JerseyNumber { get; set; }
    public PlayerPosition Position { get; set; } = PlayerPosition.Midfielder;
    public DateTime? BirthDate { get; set; }
    public string Note { get; set; } = string.Empty;

    // Pomocné vlastnosti pro zobrazení, do JSON se neukládají
    [JsonIgnore]
    public string FullName => $"{FirstName} {LastName}".Trim();

    [JsonIgnore]
    public string PositionLabel => Position.ToCzech();

    // Název obrázku s ikonou postu (soubory jsou v TrenerApp/Resources/Images)
    [JsonIgnore]
    public string PositionIcon => Position switch
    {
        PlayerPosition.Goalkeeper => "ic_pos_goalkeeper.png",
        PlayerPosition.Defender => "ic_pos_defender.png",
        PlayerPosition.Midfielder => "ic_pos_midfielder.png",
        _ => "ic_pos_forward.png"
    };

    // Vytvoří kopii hráče (používáme při ukládání)
    public Player Clone() => (Player)MemberwiseClone();
}

// Překlad pozice do češtiny pro zobrazení v aplikaci
public static class PlayerPositionExtensions
{
    public static string ToCzech(this PlayerPosition position) => position switch
    {
        PlayerPosition.Goalkeeper => "Brankář",
        PlayerPosition.Defender => "Obránce",
        PlayerPosition.Midfielder => "Záložník",
        PlayerPosition.Forward => "Útočník",
        _ => position.ToString()
    };
}