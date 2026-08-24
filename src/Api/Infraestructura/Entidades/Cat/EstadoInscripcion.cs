using GestionAcademica.Api.Infraestructura.Entidades.Acad;

namespace GestionAcademica.Api.Infraestructura.Entidades.Cat;

public class EstadoInscripcion
{
    public int IdEstadoInscripcion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<InscripcionCurso> Inscripciones { get; set; } = new List<InscripcionCurso>();
}
