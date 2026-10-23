
namespace PortalProfesores.Domain.Entities;

public class ProfesorClase
{
    public long IdProfesorClase { get; set; }

    public long IdProfesor { get; set; }
    public long IdClase { get; set; }

    public DateTime FechaCreacion { get; set; }
    public long? UsuarioCreacion { get; set; }

    public ProfesorDetalle Profesor { get; set; } = null!;
    public Clase Clase { get; set; } = null!;
}
