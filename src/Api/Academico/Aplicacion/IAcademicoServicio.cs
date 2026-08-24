namespace GestionAcademica.Api.Academico.Aplicacion;

public interface IAcademicoServicio
{
    Task<AcademicoResumenDto> ObtenerResumenAsync(CancellationToken cancellationToken);
}

public record AcademicoResumenDto(
    int Areas,
    int Carreras,
    int Asignaturas,
    int Docentes,
    int Estudiantes,
    int Periodos,
    int Grupos,
    int Aulas);
