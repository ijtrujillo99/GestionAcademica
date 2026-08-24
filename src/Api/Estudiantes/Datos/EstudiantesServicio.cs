using GestionAcademica.Api.Comun;
using GestionAcademica.Api.Estudiantes.Aplicacion;
using GestionAcademica.Api.Infraestructura.Entidades.Acad;
using GestionAcademica.Api.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.Estudiantes.Datos;

public class EstudiantesServicio : IEstudiantesServicio
{
    private readonly GestionAcademicaDbContext _contexto;

    public EstudiantesServicio(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<EstudianteResumenDto>> ListarAsync(
        string? q,
        CancellationToken cancellationToken)
    {
        var consulta = _contexto.Estudiantes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var termino = q.Trim();
            consulta = consulta.Where(e =>
                e.Carnet.Contains(termino) || e.Nombre.Contains(termino));
        }

        return await consulta
            .OrderBy(e => e.Nombre)
            .Select(e => new EstudianteResumenDto(
                e.IdEstudiante,
                e.Carnet,
                e.Nombre,
                e.Sexo,
                e.Correo,
                e.Telefono,
                e.FechaNacimiento,
                e.Activo))
            .ToListAsync(cancellationToken);
    }

    public async Task<EstudianteResumenDto> ObtenerAsync(
        int idEstudiante,
        CancellationToken cancellationToken)
    {
        var estudiante = await _contexto.Estudiantes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IdEstudiante == idEstudiante, cancellationToken);

        if (estudiante is null)
        {
            throw new KeyNotFoundException($"No existe el estudiante {idEstudiante}.");
        }

        return Mapear(estudiante);
    }

    public async Task<EstudianteResumenDto> CrearAsync(
        EstudianteEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var carnet = NormalizarRequerido(datos.Carnet, 30, "carnet");
        var nombre = NormalizarRequerido(datos.Nombre, 150, "nombre");
        var sexo = NormalizarSexo(datos.Sexo);

        await AsegurarCarnetLibreAsync(carnet, excluirId: null, cancellationToken);

        var estudiante = new Estudiante
        {
            Carnet = carnet,
            Nombre = nombre,
            Sexo = sexo,
            Correo = NormalizarOpcional(datos.Correo, 150, "correo"),
            Telefono = NormalizarOpcional(datos.Telefono, 30, "teléfono"),
            FechaNacimiento = datos.FechaNacimiento?.Date,
            Activo = datos.Activo
        };

        _contexto.Estudiantes.Add(estudiante);
        await _contexto.SaveChangesAsync(cancellationToken);

        return Mapear(estudiante);
    }

    public async Task<EstudianteResumenDto> EditarAsync(
        int idEstudiante,
        EstudianteEscrituraDto datos,
        CancellationToken cancellationToken)
    {
        var estudiante = await _contexto.Estudiantes
            .FirstOrDefaultAsync(e => e.IdEstudiante == idEstudiante, cancellationToken);

        if (estudiante is null)
        {
            throw new KeyNotFoundException($"No existe el estudiante {idEstudiante}.");
        }

        var carnet = NormalizarRequerido(datos.Carnet, 30, "carnet");
        var nombre = NormalizarRequerido(datos.Nombre, 150, "nombre");
        var sexo = NormalizarSexo(datos.Sexo);

        await AsegurarCarnetLibreAsync(carnet, idEstudiante, cancellationToken);

        estudiante.Carnet = carnet;
        estudiante.Nombre = nombre;
        estudiante.Sexo = sexo;
        estudiante.Correo = NormalizarOpcional(datos.Correo, 150, "correo");
        estudiante.Telefono = NormalizarOpcional(datos.Telefono, 30, "teléfono");
        estudiante.FechaNacimiento = datos.FechaNacimiento?.Date;
        estudiante.Activo = datos.Activo;

        await _contexto.SaveChangesAsync(cancellationToken);

        return Mapear(estudiante);
    }

    private async Task AsegurarCarnetLibreAsync(
        string carnet,
        int? excluirId,
        CancellationToken cancellationToken)
    {
        var ocupado = await _contexto.Estudiantes
            .AsNoTracking()
            .AnyAsync(
                e => e.Carnet == carnet && e.IdEstudiante != (excluirId ?? 0),
                cancellationToken);

        if (ocupado)
        {
            throw new ReglaNegocioException($"Ya existe un estudiante con el carnet {carnet}.");
        }
    }

    private static EstudianteResumenDto Mapear(Estudiante estudiante)
    {
        return new EstudianteResumenDto(
            estudiante.IdEstudiante,
            estudiante.Carnet,
            estudiante.Nombre,
            estudiante.Sexo,
            estudiante.Correo,
            estudiante.Telefono,
            estudiante.FechaNacimiento,
            estudiante.Activo);
    }

    private static string NormalizarRequerido(string? valor, int maximo, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ReglaNegocioException($"El {campo} es obligatorio.");
        }

        var texto = valor.Trim();
        if (texto.Length > maximo)
        {
            throw new ReglaNegocioException(
                $"El {campo} no puede superar {maximo} caracteres.");
        }

        return texto;
    }

    private static string? NormalizarOpcional(string? valor, int maximo, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return null;
        }

        var texto = valor.Trim();
        if (texto.Length > maximo)
        {
            throw new ReglaNegocioException(
                $"El {campo} no puede superar {maximo} caracteres.");
        }

        return texto;
    }

    private static string NormalizarSexo(string? sexo)
    {
        if (string.IsNullOrWhiteSpace(sexo))
        {
            throw new ReglaNegocioException("El sexo es obligatorio.");
        }

        var valor = sexo.Trim().ToUpperInvariant();
        if (valor is not ("F" or "M"))
        {
            throw new ReglaNegocioException("El sexo debe ser F o M.");
        }

        return valor;
    }
}
