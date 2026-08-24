namespace GestionAcademica.Api.Comun;

public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string message)
        : base(message)
    {
    }
}
