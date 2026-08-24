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
       Las columnas representan las sesiones del Excel.

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

        ('2024-1669U',
         'Presente','Presente','Ausente','Presente','Presente',
         'Presente','Presente','Presente','Ausente'),

        ('2023-0916U',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Presente','Presente','Ausente'),

        ('2024-1361U',
         'Presente','Presente','Presente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('2024-1372U',
         'Presente','Presente','Presente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        /* Anthony */
        ('2024-1735U',
         'Ausente','Presente','Retirado','Retirado','Retirado',
         'Retirado','Retirado','Retirado','Retirado'),

        ('2024-1369U',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Presente','Presente','Presente'),

        ('2022-0950U',
         'Presente','Presente','Presente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('2022-0294I',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Presente','Presente','Ausente'),

        ('2024-1568U',
         'Presente','Presente','Presente','Presente','Ausente',
         'Presente','Presente','Presente','Presente'),

        ('2024-1664U',
         'Presente','Presente','Ausente','Presente','Ausente',
         'Presente','Presente','Presente','Ausente'),

        ('2023-0235I',
         'Presente','Presente','Ausente','Presente','Presente',
         'Ausente','Presente','Presente','Presente'),

        ('2023-0415I',
         'Presente','Ausente','Ausente','Presente','Presente',
         'Presente','Presente','Presente','Ausente'),

        ('2023-0819I',
         'Presente','Presente','Presente','Presente','Ausente',
         'Presente','Presente','Presente','Presente'),

        ('2024-1607U',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Presente','Presente','Presente'),

        ('2023-0394I',
         'Presente','Presente','Presente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('2024-1648U',
         'Presente','Presente','Ausente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('2023-1040U',
         'Ausente','Presente','Presente','Presente','Presente',
         'Presente','Presente','Presente','Ausente'),

        ('2024-1662U',
         'Presente','Ausente','Ausente','Presente','Presente',
         'Ausente','Presente','Ausente','Presente'),

        ('2023-0396I',
         'Presente','Presente','Ausente','Presente','Presente',
         'Presente','Justificado','Presente','Presente'),

        ('2023-0253I',
         'Presente','Presente','Ausente','Presente','Presente',
         'Ausente','Presente','Presente','Ausente'),

        ('2022-0804U',
         'Presente','Presente','Ausente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('2023-0234I',
         'Presente','Presente','Ausente','Ausente','Ausente',
         'Ausente','Presente','Presente','Presente'),

        ('2023-0233I',
         'Presente','Presente','Presente','Presente','Presente',
         'Justificado','Presente','Presente','Presente'),

        ('2024-1633U',
         'Presente','Presente','Justificado','Presente','Presente',
         'Presente','Presente','Presente','Ausente'),

        ('2020-0636I',
         'Presente','Presente','Presente','Presente','Ausente',
         'Ausente','Presente','Presente','Ausente'),

        ('2024-1657U',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Presente','Presente','Presente'),

        ('2023-0221I',
         'Presente','Presente','Ausente','Presente','Presente',
         'Ausente','Presente','Presente','Ausente'),

        ('2024-1688U',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Presente','Presente','Presente'),

        ('2023-1083U',
         'Presente','Presente','Presente','Presente','Presente',
         'Justificado','Presente','Presente','Presente'),

        ('2023-0390I',
         'Presente','Presente','Presente','Presente','Presente',
         'Presente','Presente','Presente','Presente'),

        ('2024-1676U',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Presente','Presente','Presente'),

        ('2024-1630U',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Presente','Presente','Presente'),

        ('2023-0217I',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Ausente','Presente','Presente'),

        ('2024-1693U',
         'Presente','Presente','Presente','Presente','Presente',
         'Ausente','Presente','Presente','Justificado');


    /* ============================================================
       TRANSFORMAR LAS COLUMNAS DEL EXCEL EN FILAS

       Esto convierte:

       Estudiante | 04/08 | 06/08 | 07/08 ...

       en:

       Estudiante | Fecha | Estado
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


    /* Anthony: debe tener únicamente 2 asistencias */
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
    WHERE E.Carnet = '2024-1735U'
    ORDER BY CL.Fecha;


    /* Alberto y Harvey: no deberían tener asistencias */
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
    WHERE E.Carnet IN ('6FA99262', '6B8F1E77')
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