using Microsoft.Data.SqlClient;

namespace GestionAcademica.Api.Infraestructura.Reportes;

public interface IEjecutorProcedimientos
{
    Task<IReadOnlyList<IReadOnlyList<IReadOnlyDictionary<string, object?>>>> EjecutarAsync(
        string nombreProcedimiento,
        IReadOnlyList<SqlParameter> parametros,
        CancellationToken cancellationToken);
}
