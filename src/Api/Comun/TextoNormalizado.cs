namespace GestionAcademica.Api.Comun;

public static class TextoNormalizado
{
    public static string Requerido(string? valor, int maximo, string campo)
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

    public static string? Opcional(string? valor, int maximo, string campo)
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
}
