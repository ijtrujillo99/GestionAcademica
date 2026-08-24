USE GestionAcademica;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY

    BEGIN TRANSACTION;


    /* ============================================================
       OBTENER CURSO OFERTADO
       ============================================================ */

    DECLARE @IdCursoBD INT;

    SELECT @IdCursoBD = CO.IdCursoOfertado
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


    IF @IdCursoBD IS NULL
    BEGIN
        THROW 50020,
            'No se encontró el CursoOfertado de Base de Datos I - 2T2-SIS-S - 2026-II.',
            1;
    END;


    /* ============================================================
       CARGA TEMPORAL DE ASISTENCIA

       Cada fila representa un estudiante.
       Las columnas representan las sesiones de ejemplo.

       NULL = no existe registro de asistencia.
       Retirado = se ignora aquí porque el retiro pertenece
                  a InscripcionCurso.
       ============================================================ */

    DECLARE @Carga TABLE
    (
        Carnet VARCHAR(30) NOT NULL,

        Fecha0408 VARCHAR(20) NULL,
        Fecha0608 VARCHAR(20) NULL,
        Fecha0708 VARCHAR(20) NULL,
        Fecha1108 VARCHAR(20) NULL,
        Fecha1308 VARCHAR(20) NULL,
        Fecha1408 VARCHAR(20) NULL,
        Fecha1808 VARCHAR(20) NULL,
        Fecha2008 VARCHAR(20) NULL,
        Fecha2108 VARCHAR(20) NULL
    );


    INSERT INTO @Carga
    (
        Carnet,
        Fecha0408,
        Fecha0608,
        Fecha0708,
        Fecha1108,
        Fecha1308,
        Fecha1408,
        Fecha1808,
        Fecha2008,
        Fecha2108
    )
    VALUES

        ('E2024-001',
         'Presente','Ausente','Presente','Presente','Presente',
         'Presente','Presente','Ausente','Presente'),

        ('E2023-002',
         'Presente','Presente','Presente','Ausente','Presente',
         'Presente','Presente','Presente','Ausente'),

        ('E2024-003',
         'Presente','Presente','Presente','Presente','Ausente',
         'Presente','Presente','Presente','Presente'),

        ('E2024-004',
         'Presente','Presente','Ausente','Presente','Presente',
         'Presente','Justificado','Presente','Presente'),

        /* Gabriel: retiro a partir de la tercera sesión */
        ('E2024-005',
         'Presente','Ausente','Retirado','Retirado','Retirado',
         'Retirado','Retirado','Retirado','Retirado'),

        ('E2024-006',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Presente','Presente','Presente'),

        ('E2022-007',
         'Ausente','Presente','Presente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('E2022-008',
         'Presente','Presente','Presente','Ausente','Presente',
         'Presente','Presente','Ausente','Presente'),

        ('E2024-009',
         'Presente','Presente','Presente','Presente','Presente',
         'Presente','Ausente','Presente','Presente'),

        ('E2024-010',
         'Presente','Ausente','Presente','Presente','Presente',
         'Presente','Presente','Presente','Ausente'),

        ('E2023-011',
         'Presente','Presente','Ausente','Presente','Presente',
         'Justificado','Presente','Presente','Presente'),

        ('E2023-012',
         'Presente','Presente','Presente','Presente','Ausente',
         'Presente','Presente','Presente','Ausente'),

        ('E2023-013',
         'Ausente','Presente','Presente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('E2024-014',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Presente','Presente','Presente'),

        ('E2023-015',
         'Presente','Presente','Presente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('E2024-016',
         'Presente','Presente','Ausente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('E2023-017',
         'Presente','Ausente','Presente','Presente','Presente',
         'Presente','Presente','Ausente','Presente'),

        ('E2024-018',
         'Presente','Presente','Presente','Ausente','Presente',
         'Presente','Ausente','Presente','Presente'),

        ('E2023-019',
         'Presente','Presente','Justificado','Presente','Presente',
         'Presente','Presente','Presente','Ausente'),

        ('E2023-020',
         'Presente','Presente','Presente','Presente','Ausente',
         'Ausente','Presente','Presente','Presente'),

        ('E2022-021',
         'Presente','Presente','Presente','Presente','Presente',
         'Presente','Presente','Presente','Justificado'),

        ('E2023-022',
         'Presente','Ausente','Ausente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('E2023-023',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Presente','Presente','Presente'),

        ('E2024-024',
         'Ausente','Presente','Presente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('E2020-025',
         'Presente','Presente','Presente','Ausente','Presente',
         'Presente','Presente','Ausente','Presente'),

        ('E2024-026',
         'Presente','Presente','Presente','Presente','Presente',
         'Presente','Ausente','Presente','Presente'),

        ('E2023-027',
         'Presente','Presente','Ausente','Presente','Presente',
         'Presente','Presente','Presente','Ausente'),

        ('E2024-028',
         'Presente','Presente','Presente','Presente','Justificado',
         'Presente','Presente','Presente','Presente'),

        ('E2023-029',
         'Presente','Presente','Presente','Presente','Presente',
         'Presente','Presente','Ausente','Presente'),

        ('E2023-030',
         'Presente','Ausente','Presente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('E2024-031',
         'Presente','Presente','Presente','Ausente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('E2024-032',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Presente','Presente','Presente'),

        ('E2023-033',
         'Presente','Presente','Ausente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('E2024-034',
         'Presente','Presente','Presente','Presente','Presente',
         'Presente','Justificado','Presente','Ausente');


    /* ============================================================
       TRANSFORMAR LAS COLUMNAS DEL EXCEL EN FILAS

       Esto convierte columnas de fecha en filas Estudiante | Fecha | Estado.
       ============================================================ */

    ;WITH DatosNormalizados AS
    (
        SELECT
            C.Carnet,
            V.Fecha,
            V.Estado
        FROM @Carga C
        CROSS APPLY
        (
            VALUES
                (CONVERT(DATE, '2026-08-04'), C.Fecha0408),
                (CONVERT(DATE, '2026-08-06'), C.Fecha0608),
                (CONVERT(DATE, '2026-08-07'), C.Fecha0708),
                (CONVERT(DATE, '2026-08-11'), C.Fecha1108),
                (CONVERT(DATE, '2026-08-13'), C.Fecha1308),
                (CONVERT(DATE, '2026-08-14'), C.Fecha1408),
                (CONVERT(DATE, '2026-08-18'), C.Fecha1808),
                (CONVERT(DATE, '2026-08-20'), C.Fecha2008),
                (CONVERT(DATE, '2026-08-21'), C.Fecha2108)
        ) V(Fecha, Estado)
        WHERE V.Estado IS NOT NULL
          AND V.Estado <> 'Retirado'
    )


    /* ============================================================
       INSERTAR ASISTENCIAS
       ============================================================ */

    INSERT INTO acad.Asistencia
    (
        IdClase,
        IdInscripcion,
        IdEstadoAsistencia,
        Observacion
    )
    SELECT
        CL.IdClase,
        IC.IdInscripcion,
        EA.IdEstadoAsistencia,
        NULL
    FROM DatosNormalizados D
    INNER JOIN acad.Estudiante E
        ON E.Carnet = D.Carnet
    INNER JOIN acad.InscripcionCurso IC
        ON IC.IdEstudiante = E.IdEstudiante
       AND IC.IdCursoOfertado = @IdCursoBD
    INNER JOIN acad.Clase CL
        ON CL.IdCursoOfertado = @IdCursoBD
       AND CL.Fecha = D.Fecha
    INNER JOIN cat.EstadoAsistencia EA
        ON EA.Nombre = D.Estado
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM acad.Asistencia A
        WHERE A.IdClase = CL.IdClase
          AND A.IdInscripcion = IC.IdInscripcion
    );


    /* ============================================================
       VALIDACIONES
       ============================================================ */

    /* Total de registros cargados */
    SELECT
        COUNT(*) AS TotalAsistencias
    FROM acad.Asistencia A
    INNER JOIN acad.Clase CL
        ON CL.IdClase = A.IdClase
    WHERE CL.IdCursoOfertado = @IdCursoBD;


    /* Resumen por fecha */
    SELECT
        CL.Fecha,
        EA.Nombre AS Estado,
        COUNT(*) AS Cantidad
    FROM acad.Asistencia A
    INNER JOIN acad.Clase CL
        ON CL.IdClase = A.IdClase
    INNER JOIN cat.EstadoAsistencia EA
        ON EA.IdEstadoAsistencia = A.IdEstadoAsistencia
    WHERE CL.IdCursoOfertado = @IdCursoBD
    GROUP BY
        CL.Fecha,
        EA.Nombre
    ORDER BY
        CL.Fecha,
        EA.Nombre;


    /* Gabriel: debe tener únicamente 2 asistencias */
    SELECT
        E.Carnet,
        E.Nombre,
        CL.Fecha,
        EA.Nombre AS EstadoAsistencia
    FROM acad.Asistencia A
    INNER JOIN acad.InscripcionCurso IC
        ON IC.IdInscripcion = A.IdInscripcion
    INNER JOIN acad.Estudiante E
        ON E.IdEstudiante = IC.IdEstudiante
    INNER JOIN acad.Clase CL
        ON CL.IdClase = A.IdClase
    INNER JOIN cat.EstadoAsistencia EA
        ON EA.IdEstadoAsistencia = A.IdEstadoAsistencia
    WHERE E.Carnet = 'E2024-005'
    ORDER BY CL.Fecha;


    /* Pablo y Raul: no deberían tener asistencias */
    SELECT
        E.Carnet,
        E.Nombre,
        COUNT(A.IdAsistencia) AS RegistrosAsistencia
    FROM acad.Estudiante E
    LEFT JOIN acad.InscripcionCurso IC
        ON IC.IdEstudiante = E.IdEstudiante
       AND IC.IdCursoOfertado = @IdCursoBD
    LEFT JOIN acad.Asistencia A
        ON A.IdInscripcion = IC.IdInscripcion
    WHERE E.Carnet IN ('TMP-A01', 'TMP-B02')
    GROUP BY
        E.Carnet,
        E.Nombre;


    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;
GO