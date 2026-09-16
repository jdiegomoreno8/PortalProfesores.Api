namespace PortalProfesores.Domain.Entities;

public class Ciudad
{
    public long IdCiudad { get; set; }

    public long IdPais { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }

    public long? UsuarioCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public long? UsuarioActualizacion { get; set; }

    public ICollection<Persona> Personas { get; set; }
        = new List<Persona>();
}