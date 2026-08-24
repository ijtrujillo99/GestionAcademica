USE GestionAcademica;
GO

/* ============================================================
   ESTADOS DE INSCRIPCIÓN
   ============================================================ */

INSERT INTO cat.EstadoInscripcion
(
    Nombre,
    Descripcion
)
VALUES
    ('Activa', 'El estudiante participa activamente en el curso.'),
    ('Pendiente', 'La situación del estudiante está pendiente de regularización.'),
    ('Retirada', 'El estudiante se retiró del curso.');
GO


/* ============================================================
   ESTADOS DE ASISTENCIA
   ============================================================ */

INSERT INTO cat.EstadoAsistencia
(
    Nombre,
    Descripcion
)
VALUES
    ('Presente', 'El estudiante asistió a la sesión.'),
    ('Ausente', 'El estudiante no asistió a la sesión.'),
    ('Tardanza', 'El estudiante llegó después de la hora establecida.'),
    ('Justificado', 'La ausencia fue justificada.');
GO


/* ============================================================
   TIPOS DE EVALUACIÓN
   ============================================================ */

INSERT INTO cat.TipoEvaluacion
(
    Nombre,
    Descripcion
)
VALUES
    ('Tarea', 'Actividad o tarea asignada al estudiante.'),
    ('Quiz', 'Evaluación corta.'),
    ('Sistemático', 'Evaluación sistemática.'),
    ('Laboratorio', 'Actividad práctica o de laboratorio.'),
    ('Exposición', 'Presentación o exposición.'),
    ('Examen', 'Examen parcial o evaluación formal.'),
    ('Tarea de Curso', 'Actividad académica obligatoria de curso.');
GO