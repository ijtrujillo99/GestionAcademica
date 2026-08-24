using GestionAcademica.Api.Infraestructura.Entidades.Acad;

namespace GestionAcademica.Api.Infraestructura.Entidades.Eval;

public class Parcial
{
    public int IdParcial { get; set; }
    public int IdCursoOfertado { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public byte Orden { get; set; }
    public decimal PuntajeMaximo { get; set; }

    public CursoOfertado CursoOfertado { get; set; } = null!;
    public ICollection<Evaluacion> Evaluaciones { get; set; } = new List<Evaluacion>();
}
