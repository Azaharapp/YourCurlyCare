using System;
using System.Text.Json.Serialization;

namespace YourCurlyCareApi.Models;

public class DatosEscaneo
{
    [JsonPropertyName("codigoBarras")]                                      //JsonPropertyName para que acepte las minúsculas del ts
    public string CodigoBarras { get; set; } = string.Empty;

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [JsonPropertyName("marca")]
    public string Marca { get; set; } = string.Empty;

    [JsonPropertyName("ingredientes")]
    public string Ingredientes { get; set; } = string.Empty;

    [JsonPropertyName("usuarioId")]
    public int UsuarioId { get; set; }

    [JsonPropertyName("alcohol")]
    public bool Alcohol { get; set; }

    [JsonPropertyName("silicona")]
    public bool Silicona { get; set; }

    [JsonPropertyName("sulfato")]
    public bool Sulfato { get; set; }

    [JsonPropertyName("esApto")]
    public bool EsApto { get; set; }

    
}
