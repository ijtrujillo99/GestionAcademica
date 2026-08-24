using GestionAcademica.Api.Infraestructura.Persistencia;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GestionAcademica.Api.Infraestructura.Reportes;

public class EjecutorProcedimientos : IEjecutorProcedimientos
{
    private readonly GestionAcademicaDbContext _contexto;

    public EjecutorProcedimientos(GestionAcademicaDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IReadOnlyList<IReadOnlyList<IReadOnlyDictionary<string, object?>>>> EjecutarAsync(
        string nombreProcedimiento,
        IReadOnlyList<SqlParameter> parametros,
        CancellationToken cancellationToken)
    {
        var conexion = _contexto.Database.GetDbConnection();
        var cerroConexion = false;

        if (conexion.State != System.Data.ConnectionState.Open)
        {
            await conexion.OpenAsync(cancellationToken);
            cerroConexion = true;
        }

        try
        {
            await using var comando = conexion.CreateCommand();
            comando.CommandText = nombreProcedimiento;
            comando.CommandType = System.Data.CommandType.StoredProcedure;

            foreach (var parametro in parametros)
            {
                comando.Parameters.Add(parametro);
            }

            await using var lector = await comando.ExecuteReaderAsync(cancellationToken);
            var conjuntos = new List<IReadOnlyList<IReadOnlyDictionary<string, object?>>>();

            do
            {
                conjuntos.Add(LeerFilas(lector));
            }
            while (await lector.NextResultAsync(cancellationToken));

            return conjuntos;
        }
        finally
        {
            if (cerroConexion)
            {
                await conexion.CloseAsync();
            }
        }
    }

    private static IReadOnlyList<IReadOnlyDictionary<string, object?>> LeerFilas(System.Data.Common.DbDataReader lector)
    {
        var filas = new List<IReadOnlyDictionary<string, object?>>();

        while (lector.Read())
        {
            var fila = new Dictionary<string, object?>(StringComparer.Ordinal);
            for (var i = 0; i < lector.FieldCount; i++)
            {
                var valor = lector.GetValue(i);
                fila[lector.GetName(i)] = valor is DBNull ? null : valor;
            }

            filas.Add(fila);
        }

        return filas;
    }
}
