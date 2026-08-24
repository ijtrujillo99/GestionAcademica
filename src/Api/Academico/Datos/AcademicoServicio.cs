using GestionAcademica.Api.Academico.Aplicacion;
using GestionAcademica.Api.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.Academico.Datos;

public class AcademicoServicio : IAcademicoServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public AcademicoServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<AcademicoResumenDto> ObtenerResumenAsync(CancellationToken cancellationToken)
    {
        return new AcademicoResumenDto(
            await _contexto.AreasConocimiento.CountAsync(cancellationToken),
            await _contexto.Carreras.CountAsync(cancellationToken),
            await _contexto.Asignaturas.CountAsync(cancellationToken),
            await _contexto.Docentes.CountAsync(cancellationToken),
            await _contexto.Estudiantes.CountAsync(cancellationToken),
            await _contexto.PeriodosAcademicos.CountAsync(cancellationToken),
            await _contexto.Grupos.CountAsync(cancellationToken),
            await _contexto.Aulas.CountAsync(cancellationToken));
    }
}
