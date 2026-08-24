USE GestionAcademica;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY

    BEGIN TRANSACTION;

    /* ============================================================
       OBTENER CURSO OFERTADO: BASE DE DATOS I - 2T2-SIS-S - 2026-II
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
        THROW 50001,
            'No se encontró el CursoOfertado de Base de Datos I - 2T2-SIS-S - 2026-II.',
            1;
    END;


    /* ============================================================
       OBTENER ESTADOS DE INSCRIPCIÓN
       ============================================================ */

    DECLARE @IdEstadoActiva INT;
    DECLARE @IdEstadoPendiente INT;
    DECLARE @IdEstadoRetirada INT;

    SELECT @IdEstadoActiva = IdEstadoInscripcion
    FROM cat.EstadoInscripcion
    WHERE Nombre = 'Activa';

    SELECT @IdEstadoPendiente = IdEstadoInscripcion
    FROM cat.EstadoInscripcion
    WHERE Nombre = 'Pendiente';

    SELECT @IdEstadoRetirada = IdEstadoInscripcion
    FROM cat.EstadoInscripcion
    WHERE Nombre = 'Retirada';


    IF @IdEstadoActiva IS NULL
       OR @IdEstadoPendiente IS NULL
       OR @IdEstadoRetirada IS NULL
    BEGIN
        THROW 50002,
            'No se encontraron todos los estados requeridos de inscripción.',
            1;
    END;


    /* ============================================================
       INSERTAR INSCRIPCIONES
       
       Por defecto:
       Activa + Oficial

       Excepciones de ejemplo:
       E2024-005 (Gabriel)  -> Retirada
       E2020-025 (Daniela)  -> Pendiente / No oficial
       ============================================================ */

    INSERT INTO acad.InscripcionCurso
    (
        IdEstudiante,
        IdCursoOfertado,
        IdEstadoInscripcion,
        FechaInicio,
        FechaEstado,
        FechaRetiro,
        EsOficial,
        Observacion
    )
    SELECT
        E.IdEstudiante,
        @IdCursoBD,

        CASE
            WHEN E.Carnet = 'E2024-005'
                THEN @IdEstadoRetirada

            WHEN E.Carnet = 'E2020-025'
                THEN @IdEstadoPendiente

            ELSE @IdEstadoActiva
        END,

        '2026-08-04',

        CASE
            WHEN E.Carnet = 'E2024-005'
                THEN '2026-08-07'
            ELSE '2026-08-04'
        END,

        CASE
            WHEN E.Carnet = 'E2024-005'
                THEN '2026-08-07'
            ELSE NULL
        END,

        CASE
            WHEN E.Carnet = 'E2020-025'
                THEN 0
            ELSE 1
        END,

        CASE
            WHEN E.Carnet = 'E2024-005'
                THEN 'El estudiante comunicó su retiro después de haber asistido a sesiones anteriores.'

            WHEN E.Carnet = 'E2020-025'
                THEN 'Situación pendiente de confirmación con la estudiante debido a discrepancia con la lista oficial.'

            ELSE NULL
        END

    FROM acad.Estudiante E
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM acad.InscripcionCurso IC
        WHERE IC.IdEstudiante = E.IdEstudiante
          AND IC.IdCursoOfertado = @IdCursoBD
    );


    /* ============================================================
       VALIDACIÓN
       ============================================================ */

    SELECT
        IC.IdInscripcion,
        E.Carnet,
        E.Nombre AS Estudiante,
        C.Nombre AS Asignatura,
        G.CodigoGrupo,
        EI.Nombre AS Estado,
        IC.EsOficial,
        IC.FechaInicio,
        IC.FechaEstado,
        IC.FechaRetiro,
        IC.Observacion
    FROM acad.InscripcionCurso IC
    INNER JOIN acad.Estudiante E
        ON E.IdEstudiante = IC.IdEstudiante
    INNER JOIN acad.CursoOfertado CO
        ON CO.IdCursoOfertado = IC.IdCursoOfertado
    INNER JOIN acad.Asignatura C
        ON C.IdAsignatura = CO.IdAsignatura
    INNER JOIN acad.Grupo G
        ON G.IdGrupo = CO.IdGrupo
    INNER JOIN cat.EstadoInscripcion EI
        ON EI.IdEstadoInscripcion = IC.IdEstadoInscripcion
    WHERE IC.IdCursoOfertado = @IdCursoBD
    ORDER BY E.Nombre;


    SELECT
        EI.Nombre AS Estado,
        IC.EsOficial,
        COUNT(*) AS Cantidad
    FROM acad.InscripcionCurso IC
    INNER JOIN cat.EstadoInscripcion EI
        ON EI.IdEstadoInscripcion = IC.IdEstadoInscripcion
    WHERE IC.IdCursoOfertado = @IdCursoBD
    GROUP BY
        EI.Nombre,
        IC.EsOficial
    ORDER BY
        EI.Nombre,
        IC.EsOficial;


    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;
GO