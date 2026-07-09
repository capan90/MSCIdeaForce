using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MSC.IdeaForge.Infrastructure.AI;

/// <summary>
/// Gemini bazı string alanları dizi (array) olarak döndürebiliyor.
/// Bu dönüştürücü, gelen JSON değerini güvenli biçimde string'e çevirir:
/// - Dizi ise elemanları ", " ile birleştirir
/// - String ise doğrudan alır
/// - Diğer türleri (sayı, boolean, nesne) metne çevirir
/// </summary>
public class FlexibleStringJsonConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Değerin türü ne olursa olsun tek seferde okuyup metne çeviriyoruz
        using var doc = JsonDocument.ParseValue(ref reader);
        return ElementToString(doc.RootElement);
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }

    // Bir JSON elemanını Türkçe metne dönüştürür
    private static string ElementToString(JsonElement element) => element.ValueKind switch
    {
        JsonValueKind.String => element.GetString() ?? string.Empty,
        JsonValueKind.Array => string.Join(", ", element.EnumerateArray().Select(ElementToString)),
        JsonValueKind.Number => element.GetRawText(),
        JsonValueKind.True => "true",
        JsonValueKind.False => "false",
        JsonValueKind.Null => string.Empty,
        JsonValueKind.Object => element.GetRawText(),
        _ => element.GetRawText()
    };
}
