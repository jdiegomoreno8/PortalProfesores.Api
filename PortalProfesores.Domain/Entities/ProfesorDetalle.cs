namespace PortalProfesores.Domain.Entities;

public class ProfesorDetalle
{
    public long IdProfesor { get; set; }
    public long IdPersona { get; set; }

    public string? Presentacion { get; set; }
    public string? Experiencia { get; set; }
    public string? Biografia { get; set; }

    public DateTime FechaIngreso { get; set; }
    public DateTime FechaCreacion { get; set; }
    public long? UsuarioCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    public long? UsuarioActualizacion { get; set; }

    public Persona Persona { get; set; } = null!;
}