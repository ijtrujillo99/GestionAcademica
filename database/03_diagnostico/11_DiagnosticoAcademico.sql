USE GestionAcademica;
GO

SET NOCOUNT ON;
GO


/* ============================================================
   PARÁMETROS DEL REPORTE
   ============================================================ */

DECLARE @IdCursoOfertado INT;
DECLARE @FechaDesde DATE = '2026-08-04';
DECLARE @FechaHasta DATE = '2026-08-21';


/* ============================================================
   OBTENER CURSO OFERTADO
   ============================================================ */

SELECT @IdCursoOfertado = CO.IdCursoOfertado
FROM acad.CursoOfertado CO
INNER JOIN acad.Asignatura A
    ON A.IdAsignatura = CO.IdAsignatura
INNER JOIN acad.Grupo G
    ON G.IdGrupo = CO.IdGrupo
INNER JOIN acad.PeriodoAcademico PA
    ON PA.IdPeriodo = G.IdPeriodo
WHERE A.Nombre = 'Base de Datos I'
  AND G.CodigoGrupo = '2T2-SIS-S'
  AND PA.Anio = 2026
  AND PA.Semestre = 2;


IF @IdCursoOfertado IS NULL
BEGIN
    THROW 50050,
        'No se encontró el curso ofertado de Base de Datos I - 2T2-SIS-S - 2026-II.',
        1;
END;


/* ============================================================
   1. INFORMACIÓN GENERAL DEL CURSO
   ============================================================ */

SELECT
    CO.IdCursoOfertado,
    A.Nombre AS Asignatura,
    G.CodigoGrupo,
    D.Nombre AS Docente,
    AU.CodigoAula,
    PA.Anio,
    PA.Semestre
FROM acad.CursoOfertado CO
INNER JOIN acad.Asignatura A
    ON A.IdAsignatura = CO.IdAsignatura
INNER JOIN acad.Grupo G
    ON G.IdGrupo = CO.IdGrupo
INNER JOIN acad.Docente D
    ON D.IdDocente = CO.IdDocente
INNER JOIN acad.Aula AU
    ON AU.IdAula = CO.IdAula
INNER JOIN acad.PeriodoAcademico PA
    ON PA.IdPeriodo = G.IdPeriodo
WHERE CO.IdCursoOfertado = @IdCursoOfertado;


/* ============================================================
   2. RESUMEN DE INSCRIPCIONES
   ============================================================ */

SELECT
    EI.Nombre AS EstadoInscripcion,
    IC.EsOficial,
    COUNT(*) AS Cantidad
FROM acad.InscripcionCurso IC
INNER JOIN cat.EstadoInscripcion EI
    ON EI.IdEstadoInscripcion = IC.IdEstadoInscripcion
WHERE IC.IdCursoOfertado = @IdCursoOfertado
GROUP BY
    EI.Nombre,
    IC.EsOficial
ORDER BY
    EI.Nombre,
    IC.EsOficial;


/* ============================================================
   3. SITUACIÓN DE CADA ESTUDIANTE

   SesionesEsperadas:
   sesiones que correspondían al estudiante según su inscripción.

   Para un estudiante retirado:
   las sesiones posteriores a FechaRetiro NO cuentan.

   Para estudiantes activos/pedientes:
   se consideran todas las sesiones hasta @FechaHasta.
   ============================================================ */

;WITH SesionesEsperadas AS
(
    SELECT
        IC.IdInscripcion,
        COUNT(CL.IdClase) AS SesionesEsperadas
    FROM acad.InscripcionCurso IC
    INNER JOIN acad.Clase CL
        ON CL.IdCursoOfertado = IC.IdCursoOfertado
       AND CL.Fecha >= IC.FechaInicio
       AND CL.Fecha >= @FechaDesde
       AND CL.Fecha <= @FechaHasta
       AND
       (
            IC.FechaRetiro IS NULL
            OR CL.Fecha < IC.FechaRetiro
       )
    WHERE IC.IdCursoOfertado = @IdCursoOfertado
    GROUP BY
        IC.IdInscripcion
),
AsistenciaResumen AS
(
    SELECT
        IC.IdInscripcion,

        COUNT(A.IdAsistencia) AS AsistenciasRegistradas,

        SUM
        (
            CASE
                WHEN EA.Nombre = 'Presente' THEN 1
                ELSE 0
            END
        ) AS Presentes,

        SUM
        (
            CASE
                WHEN EA.Nombre = 'Ausente' THEN 1
                ELSE 0
            END
        ) AS Ausentes,

        SUM
        (
            CASE
                WHEN EA.Nombre = 'Tardanza' THEN 1
                ELSE 0
            END
        ) AS Tardanzas,

        SUM
        (
            CASE
                WHEN EA.Nombre = 'Justificado' THEN 1
                ELSE 0
            END
        ) AS Justificados

    FROM acad.InscripcionCurso IC

    INNER JOIN acad.Clase CL
        ON CL.IdCursoOfertado = IC.IdCursoOfertado
       AND CL.Fecha >= IC.FechaInicio
       AND CL.Fecha >= @FechaDesde
       AND CL.Fecha <= @FechaHasta
       AND
       (
            IC.FechaRetiro IS NULL
            OR CL.Fecha < IC.FechaRetiro
       )

    LEFT JOIN acad.Asistencia A
        ON A.IdClase = CL.IdClase
       AND A.IdInscripcion = IC.IdInscripcion

    LEFT JOIN cat.EstadoAsistencia EA
        ON EA.IdEstadoAsistencia = A.IdEstadoAsistencia

    WHERE IC.IdCursoOfertado = @IdCursoOfertado

    GROUP BY
        IC.IdInscripcion
)

SELECT
    E.IdEstudiante,
    E.Carnet,
    E.Nombre,
    E.Sexo,

    EI.Nombre AS EstadoInscripcion,
    IC.EsOficial,
    IC.FechaInicio,
    IC.FechaRetiro,

    ISNULL(SE.SesionesEsperadas, 0) AS SesionesEsperadas,

    ISNULL(AR.AsistenciasRegistradas, 0) AS AsistenciasRegistradas,

    ISNULL(AR.Presentes, 0) AS Presentes,
    ISNULL(AR.Ausentes, 0) AS Ausentes,
    ISNULL(AR.Tardanzas, 0) AS Tardanzas,
    ISNULL(AR.Justificados, 0) AS Justificados,

    ISNULL(SE.SesionesEsperadas, 0)
        - ISNULL(AR.AsistenciasRegistradas, 0)
        AS SesionesSinRegistro,

    CAST
    (
        CASE
            WHEN ISNULL(SE.SesionesEsperadas, 0) = 0
                THEN 0

            ELSE
                (
                    ISNULL(AR.Presentes, 0) * 100.0
                )
                / SE.SesionesEsperadas
        END
        AS DECIMAL(5,2)
    ) AS PorcentajeAsistencia

FROM acad.InscripcionCurso IC

INNER JOIN acad.Estudiante E
    ON E.IdEstudiante = IC.IdEstudiante

INNER JOIN cat.EstadoInscripcion EI
    ON EI.IdEstadoInscripcion = IC.IdEstadoInscripcion

LEFT JOIN SesionesEsperadas SE
    ON SE.IdInscripcion = IC.IdInscripcion

LEFT JOIN AsistenciaResumen AR
    ON AR.IdInscripcion = IC.IdInscripcion

WHERE IC.IdCursoOfertado = @IdCursoOfertado

ORDER BY
    E.Nombre;


/* ============================================================
   4. CASOS ESPECIALES

   Gabriel, Daniela, Pablo y Raul
   ============================================================ */

SELECT
    E.Carnet,
    E.Nombre,
    EI.Nombre AS EstadoInscripcion,
    IC.EsOficial,
    IC.FechaInicio,
    IC.FechaRetiro,

    COUNT(DISTINCT CL.IdClase) AS SesionesEsperadas,
    COUNT(A.IdAsistencia) AS AsistenciasRegistradas

FROM acad.InscripcionCurso IC

INNER JOIN acad.Estudiante E
    ON E.IdEstudiante = IC.IdEstudiante

INNER JOIN cat.EstadoInscripcion EI
    ON EI.IdEstadoInscripcion = IC.IdEstadoInscripcion

LEFT JOIN acad.Clase CL
    ON CL.IdCursoOfertado = IC.IdCursoOfertado
   AND CL.Fecha >= IC.FechaInicio
   AND CL.Fecha >= @FechaDesde
   AND CL.Fecha <= @FechaHasta
   AND
   (
        IC.FechaRetiro IS NULL
        OR CL.Fecha < IC.FechaRetiro
   )

LEFT JOIN acad.Asistencia A
    ON A.IdClase = CL.IdClase
   AND A.IdInscripcion = IC.IdInscripcion

WHERE IC.IdCursoOfertado = @IdCursoOfertado
  AND E.Carnet IN
  (
      'E2024-005', -- Gabriel (retirado)
      'E2020-025', -- Daniela (pendiente)
      'TMP-A01',   -- Pablo (sin asistencia)
      'TMP-B02'    -- Raul (sin asistencia)
  )

GROUP BY
    E.Carnet,
    E.Nombre,
    EI.Nombre,
    IC.EsOficial,
    IC.FechaInicio,
    IC.FechaRetiro

ORDER BY
    E.Nombre;


/* ============================================================
   5. RESUMEN DE ASISTENCIA POR SESIÓN
   ============================================================ */

SELECT
    CL.IdClase,
    CL.Fecha,

    SUM
    (
        CASE
            WHEN EA.Nombre = 'Presente' THEN 1
            ELSE 0
        END
    ) AS Presentes,

    SUM
    (
        CASE
            WHEN EA.Nombre = 'Ausente' THEN 1
            ELSE 0
        END
    ) AS Ausentes,

    SUM
    (
        CASE
            WHEN EA.Nombre = 'Tardanza' THEN 1
            ELSE 0
        END
    ) AS Tardanzas,

    SUM
    (
        CASE
            WHEN EA.Nombre = 'Justificado' THEN 1
            ELSE 0
        END
    ) AS Justificados,

    COUNT(A.IdAsistencia) AS TotalRegistros

FROM acad.Clase CL

LEFT JOIN acad.Asistencia A
    ON A.IdClase = CL.IdClase

LEFT JOIN cat.EstadoAsistencia EA
    ON EA.IdEstadoAsistencia = A.IdEstadoAsistencia

WHERE CL.IdCursoOfertado = @IdCursoOfertado
  AND CL.Fecha BETWEEN @FechaDesde AND @FechaHasta

GROUP BY
    CL.IdClase,
    CL.Fecha

ORDER BY
    CL.Fecha;


/* ============================================================
   6. RESUMEN POR SEXO
   ============================================================ */

SELECT
    E.Sexo,

    COUNT(*) AS Estudiantes,

    SUM
    (
        CASE
            WHEN EI.Nombre = 'Retirada'
                THEN 0
            ELSE 1
        END
    ) AS EstudiantesConsiderados,

    SUM
    (
        CASE
            WHEN AR.Presentes > 0
                THEN AR.Presentes
            ELSE 0
        END
    ) AS TotalPresentes,

    SUM
    (
        CASE
            WHEN AR.Ausentes > 0
                THEN AR.Ausentes
            ELSE 0
        END
    ) AS TotalAusentes

FROM acad.InscripcionCurso IC

INNER JOIN acad.Estudiante E
    ON E.IdEstudiante = IC.IdEstudiante

INNER JOIN cat.EstadoInscripcion EI
    ON EI.IdEstadoInscripcion = IC.IdEstadoInscripcion

LEFT JOIN
(
    SELECT
        IC2.IdInscripcion,

        SUM
        (
            CASE
                WHEN EA.Nombre = 'Presente' THEN 1
                ELSE 0
            END
        ) AS Presentes,

        SUM
        (
            CASE
                WHEN EA.Nombre = 'Ausente' THEN 1
                ELSE 0
            END
        ) AS Ausentes

    FROM acad.InscripcionCurso IC2

    INNER JOIN acad.Clase CL
        ON CL.IdCursoOfertado = IC2.IdCursoOfertado
       AND CL.Fecha >= IC2.FechaInicio
       AND CL.Fecha BETWEEN @FechaDesde AND @FechaHasta
       AND
       (
            IC2.FechaRetiro IS NULL
            OR CL.Fecha < IC2.FechaRetiro
       )

    LEFT JOIN acad.Asistencia A
        ON A.IdClase = CL.IdClase
       AND A.IdInscripcion = IC2.IdInscripcion

    LEFT JOIN cat.EstadoAsistencia EA
        ON EA.IdEstadoAsistencia = A.IdEstadoAsistencia

    WHERE IC2.IdCursoOfertado = @IdCursoOfertado

    GROUP BY
        IC2.IdInscripcion

) AR
    ON AR.IdInscripcion = IC.IdInscripcion

WHERE IC.IdCursoOfertado = @IdCursoOfertado

GROUP BY
    E.Sexo

ORDER BY
    E.Sexo;


/* ============================================================
   7. RESUMEN DE CALIFICACIONES
   ============================================================ */

SELECT
    E.Carnet,
    E.Nombre,

    SUM
    (
        CASE
            WHEN P.Orden = 1
                THEN C.PuntajeObtenido
            ELSE 0
        END
    ) AS AcumuladoParcialI,

    SUM
    (
        CASE
            WHEN P.Orden = 2
                THEN C.PuntajeObtenido
            ELSE 0
        END
    ) AS AcumuladoParcialII,

    SUM(C.PuntajeObtenido) AS AcumuladoActual

FROM acad.InscripcionCurso IC

INNER JOIN acad.Estudiante E
    ON E.IdEstudiante = IC.IdEstudiante

LEFT JOIN eval.Calificacion C
    ON C.IdInscripcion = IC.IdInscripcion

LEFT JOIN eval.Evaluacion EV
    ON EV.IdEvaluacion = C.IdEvaluacion

LEFT JOIN eval.Parcial P
    ON P.IdParcial = EV.IdParcial

WHERE IC.IdCursoOfertado = @IdCursoOfertado

GROUP BY
    E.Carnet,
    E.Nombre

ORDER BY
    E.Nombre;
GO