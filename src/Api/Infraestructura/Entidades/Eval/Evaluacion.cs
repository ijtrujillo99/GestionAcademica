using GestionAcademica.Api.Infraestructura.Entidades.Cat;

namespace GestionAcademica.Api.Infraestructura.Entidades.Eval;

public class Evaluacion
{
    public int IdEvaluacion { get; set; }
    public int IdParcial { get; set; }
    public int IdTipoEvaluacion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal PuntajeMaximo { get; set; }
    public bool EsObligatoria { get; set; }
    public byte Orden { get; set; }
    public byte? Semana { get; set; }

    public Parcial Parcial { get; set; } = null!;
    public TipoEvaluacion TipoEvaluacion { get; set; } = null!;
    public ICollection<Calificacion> Calificaciones { get; set; } = new List<Calificacion>();
}
