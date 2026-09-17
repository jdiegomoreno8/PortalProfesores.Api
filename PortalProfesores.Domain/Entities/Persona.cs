namespace PortalProfesores.Domain.Entities;

public class Persona
{
    public long IdPersona { get; set; }

    public long IdTipoDocumento { get; set; }

    public long? IdCiudad { get; set; }

    public string NumeroDocumento { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string CorreoElectronico { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public DateTime FechaIngreso { get; set; }

    public Usuario? Usuario { get; set; }

    public TipoDocumento? TipoDocumento { get; set; }

    public Ciudad? Ciudad { get; set; }
}