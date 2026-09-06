namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Representa un rol del sistema.
/// </summary>
public class Rol
{
    public long IdRol { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    // Navegación
    public ICollection<UsuarioRol> Usuarios { get; set; }
        = new List<UsuarioRol>();
}