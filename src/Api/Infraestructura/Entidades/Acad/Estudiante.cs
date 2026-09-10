namespace GestionAcademica.Api.Infraestructura.Entidades.Acad;

public class Estudiante
{
    public int IdEstudiante { get; set; }
    public string Carnet { get; set; } = string.Empty;
    public string? CodigoPersona { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;
    public string? Correo { get; set; }
    public string? Telefono { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<InscripcionCurso> Inscripciones { get; set; } = new List<InscripcionCurso>();
}
