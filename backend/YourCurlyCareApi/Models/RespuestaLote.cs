using System.Text.Json.Serialization;
using YourCurlyCareApi.Models;

public class RespuestaLote
{
    [JsonPropertyName("id_usuario")]
    public int UsuarioId { get; set; }

    [JsonPropertyName("id_cuestionario")]
    public int CuestionarioId { get; set; }

    [JsonPropertyName("respuestas")]
    public List<DetalleRespuesta> Respuestas { get; set; }
}

public class DetalleRespuesta
{
    [JsonPropertyName("id_pregunta")]
    public int PreguntaId { get; set; }

    [JsonPropertyName("opcion")]
    public Opcion Opcion { get; set; }
}