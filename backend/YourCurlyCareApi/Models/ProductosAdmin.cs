public class ProductosAdmin
{
    public int Id { get; set; }
    public string CodigoBarras { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string Marca { get; set; } = null!;
    public string Ingredientes { get; set; } = null!;
    public bool Alcohol { get; set; }
    public bool Silicona { get; set; }
    public bool Sulfato { get; set; }
    public bool EsApto { get; set; }
    public int IdUsuario { get; set; }
    public DateTime FechaRegistro { get; set; }
}