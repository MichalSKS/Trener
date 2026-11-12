using TrenerApp.Core.Models;

namespace TrenerApp.Views;

// Jeden řádek v seznamu docházky: hráč + jestli byl na tréninku
public class AttendanceRow
{
    public Player Player { get; set; } = new Player();
    public bool IsPresent { get; set; }       // zaškrtnuto = hráč byl přítomen
    public bool ShowLine { get; set; }        // oddělovací čára nad řádkem (u prvního ne)
}
