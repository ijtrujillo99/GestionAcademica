namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class PlanEstudioDetalle
{
    public int IdPlanDetalle { get; set; }
    public int IdPlanEstudio { get; set; }
    public int IdAsignatura { get; set; }
    public byte SemestrePlan { get; set; }
    public byte Creditos { get; set; }
    public byte? HorasTeoria { get; set; }
    public byte? HorasPractica { get; set; }
    public short? Orden { get; set; }
    public bool EsElectiva { get; set; }

    public PlanEstudio PlanEstudio { get; set; } = null!;
    public Asignatura Asignatura { get; set; } = null!;
}
