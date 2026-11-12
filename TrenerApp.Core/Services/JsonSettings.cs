using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TrenerApp.Core.Services;

// Společné nastavení JSON pro všechny soubory (hráči, tréninky)
public static class JsonSettings
{
    // - WriteIndented = soubor je hezky odsazený a dá se přečíst
    // - Encoder = čeština se uloží normálně (ne jako č)
    // - JsonStringEnumConverter = post se uloží jako text "Defender" místo čísla 1
    public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters = { new JsonStringEnumConverter() }
    };
}
