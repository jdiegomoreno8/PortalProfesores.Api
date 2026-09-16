namespace PortalProfesores.Domain.Entities;

public class TipoDocumento
{
    public long IdTipoDocumento { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Abreviatura { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }

    public ICollection<Persona> Personas { get; set; }
        = new List<Persona>();
}