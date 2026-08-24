using GestionAcademica.Api.Infraestructura.Entidades.Acad;

namespace GestionAcademica.Api.Infraestructura.Entidades.Cat;

public class EstadoAsistencia
{
    public int IdEstadoAsistencia { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();
}
