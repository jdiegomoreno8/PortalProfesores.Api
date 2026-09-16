namespace PortalProfesores.Domain.Entities;

public class Usuario
{
    public long IdUsuario { get; set; }

    public long IdPersona { get; set; }

    public long IdEstadoUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? ClaveHash { get; set; }

    public Persona? Persona { get; set; } = null!;

    public EstadoUsuario EstadoUsuario { get; set; } = null!;
	
	public DateTime? UltimoAcceso { get; set; }

    public ICollection<UsuarioRol> Roles { get; set; }
    = new List<UsuarioRol>();
}