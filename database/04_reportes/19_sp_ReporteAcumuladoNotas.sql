USE GestionAcademica;
GO

/*
    GET /api/cursos-ofertados/{id}/reportes/acumulado-notas

    Dos result sets: encabezado del curso y filas
    (CodigoSingu, Nombre, Notas).

    CodigoSingu = ISNULL(CodigoPersona, Carnet).
    Solo inscripcion Activa y Estudiante.Activo = 1.

    Examen = TipoEvaluacion.Nombre = 'Examen'.
    Acumulado = el resto del parcial (incluye Tarea de Curso en P2).
*/
CREATE OR ALTER PROCEDURE eval.sp_ReporteAcumuladoNotas
(
    @IdCursoOfertado            INT,
    @IncluirAcumuladoParcialI   BIT = 1,
    @IncluirExamenParcialI      BIT = 0,
    @IncluirAcumuladoParcialII  BIT = 0,
    @IncluirExamenParcialII     BIT = 0
)
AS
BEGIN

    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.CursoOfertado
        WHERE IdCursoOfertado = @IdCursoOfertado
    )
    BEGIN
        THROW 50120,
            'El CursoOfertado indicado no existe.',
            1;
    END;

    IF @IncluirAcumuladoParcialI = 0
       AND @IncluirExamenParcialI = 0
       AND @IncluirAcumuladoParcialII = 0
       AND @IncluirExamenParcialII = 0
    BEGIN
        THROW 50121,
            'Debe incluir al menos un acumulado o examen.',
            1;
    END;


    /* ============================================================
       1. ENCABEZADO
       ============================================================ */

    SELECT
        AC.Nombre AS AreaConocimiento,
        C.Nombre AS Carrera,
        A.Nombre AS Curso,
        G.CodigoGrupo AS Grupo,
        D.Nombre AS Docente,
        (
            SELECT COUNT(*)
            FROM acad.InscripcionCurso IC
            INNER JOIN acad.Estudiante E
                ON E.IdEstudiante = IC.IdEstudiante
            INNER JOIN cat.EstadoInscripcion EI
                ON EI.IdEstadoInscripcion = IC.IdEstadoInscripcion
            WHERE IC.IdCursoOfertado = @IdCursoOfertado
              AND EI.Nombre = N'Activa'
              AND E.Activo = 1
        ) AS CantEstudiantes
    FROM acad.CursoOfertado CO
    INNER JOIN acad.Asignatura A
        ON A.IdAsignatura = CO.IdAsignatura
    INNER JOIN acad.Grupo G
        ON G.IdGrupo = CO.IdGrupo
    INNER JOIN acad.Carrera C
        ON C.IdCarrera = G.IdCarrera
    INNER JOIN acad.AreaConocimiento AC
        ON AC.IdAreaConocimiento = C.IdAreaConocimiento
    INNER JOIN acad.Docente D
        ON D.IdDocente = CO.IdDocente
    WHERE CO.IdCursoOfertado = @IdCursoOfertado;


    /* ============================================================
       2. FILAS
       ============================================================ */

    ;WITH EvaluacionesFiltro AS
    (
        SELECT
            EV.IdEvaluacion
        FROM eval.Evaluacion EV
        INNER JOIN eval.Parcial P
            ON P.IdParcial = EV.IdParcial
        INNER JOIN cat.TipoEvaluacion TE
            ON TE.IdTipoEvaluacion = EV.IdTipoEvaluacion
        WHERE P.IdCursoOfertado = @IdCursoOfertado
          AND
          (
                (
                    @IncluirAcumuladoParcialI = 1
                    AND P.Orden = 1
                    AND TE.Nombre <> N'Examen'
                )
                OR
                (
                    @IncluirExamenParcialI = 1
                    AND P.Orden = 1
                    AND TE.Nombre = N'Examen'
                )
                OR
                (
                    @IncluirAcumuladoParcialII = 1
                    AND P.Orden = 2
                    AND TE.Nombre <> N'Examen'
                )
                OR
                (
                    @IncluirExamenParcialII = 1
                    AND P.Orden = 2
                    AND TE.Nombre = N'Examen'
                )
          )
    )

    SELECT
        ISNULL(E.CodigoPersona, E.Carnet) AS CodigoSingu,
        E.Nombre,
        CAST
        (
            COALESCE(SUM(CAL.PuntajeObtenido), 0)
            AS DECIMAL(5,2)
        ) AS Notas
    FROM acad.InscripcionCurso IC
    INNER JOIN acad.Estudiante E
        ON E.IdEstudiante = IC.IdEstudiante
    INNER JOIN cat.EstadoInscripcion EI
        ON EI.IdEstadoInscripcion = IC.IdEstadoInscripcion
    LEFT JOIN eval.Calificacion CAL
        ON CAL.IdInscripcion = IC.IdInscripcion
       AND CAL.IdEvaluacion IN (SELECT IdEvaluacion FROM EvaluacionesFiltro)
    WHERE IC.IdCursoOfertado = @IdCursoOfertado
      AND EI.Nombre = N'Activa'
      AND E.Activo = 1
    GROUP BY
        E.IdEstudiante,
        E.CodigoPersona,
        E.Carnet,
        E.Nombre
    ORDER BY
        E.Nombre;

END;
GO
