namespace GestionAcademica.Api.Estudiantes.Aplicacion;

public interface IEstudiantesServicio
{
    Task<IReadOnlyList<EstudianteResumenDto>> ListarAsync(string? q, CancellationToken cancellationToken);

    Task<EstudianteResumenDto> ObtenerAsync(int idEstudiante, CancellationToken cancellationToken);

    Task<EstudianteResumenDto> CrearAsync(
        EstudianteEscrituraDto datos,
        CancellationToken cancellationToken);

    Task<EstudianteResumenDto> EditarAsync(
        int idEstudiante,
        EstudianteEscrituraDto datos,
        CancellationToken cancellationToken);
}

public record EstudianteResumenDto(
    int IdEstudiante,
    string Carnet,
    string Nombre,
    string Sexo,
    string? Correo,
    string? Telefono,
    DateTime? FechaNacimiento,
    bool Activo);

public record EstudianteEscrituraDto(
    string Carnet,
    string Nombre,
    string Sexo,
    string? Correo,
    string? Telefono,
    DateTime? FechaNacimiento,
    bool Activo = true);
