using System.Text.Json.Serialization;

namespace WebApp.ViewModels;

public class CookieConsent
{
    [JsonPropertyName("essential")]
    public bool Essential { get; set; }

    [JsonPropertyName("analytics")]
    public bool Analytics { get; set; }

    [JsonPropertyName("functional")]
    public bool Functional { get; set; }

    [JsonPropertyName("marketing")]
    public bool Marketing { get; set; }
}
