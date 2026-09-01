namespace PortalProfesores.Domain.Entities;

public class Persona
{
    public long IdPersona { get; set; }

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public string CorreoElectronico { get; set; } = string.Empty;

    // Navegación
    public Usuario? Usuario { get; set; }
}