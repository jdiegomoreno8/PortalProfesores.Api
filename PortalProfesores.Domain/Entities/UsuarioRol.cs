namespace PortalProfesores.Domain.Entities;

/// <summary>
/// Relación entre Usuario y Rol.
/// </summary>
public class UsuarioRol
{
    public long IdUsuario { get; set; }

    public long IdRol { get; set; }

    // Navegación
    public Usuario Usuario { get; set; } = null!;

    public Rol Rol { get; set; } = null!;
}