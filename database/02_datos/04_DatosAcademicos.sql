USE GestionAcademica;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY

    BEGIN TRANSACTION;

    /* Datos académicos de ejemplo. No corresponden a una oferta real. */

    /* ============================================================
       1. ÁREA DE CONOCIMIENTO
       ============================================================ */

    DECLARE @IdAreaConocimiento INT;

    SELECT @IdAreaConocimiento = IdAreaConocimiento
    FROM acad.AreaConocimiento
    WHERE Nombre = 'Área de Conocimiento de Tecnología de la Información y Comunicación';

    IF @IdAreaConocimiento IS NULL
    BEGIN
        INSERT INTO acad.AreaConocimiento
        (
            Nombre,
            Descripcion
        )
        VALUES
        (
            'Área de Conocimiento de Tecnología de la Información y Comunicación',
            'Área de conocimiento relacionada con las tecnologías de información y comunicación.'
        );

        SET @IdAreaConocimiento = SCOPE_IDENTITY();
    END;


    /* ============================================================
       2. CARRERA
       ============================================================ */

    DECLARE @IdCarrera INT;

    SELECT @IdCarrera = IdCarrera
    FROM acad.Carrera
    WHERE IdAreaConocimiento = @IdAreaConocimiento
      AND Nombre = 'Ingeniería en Sistemas';

    IF @IdCarrera IS NULL
    BEGIN
        INSERT INTO acad.Carrera
        (
            IdAreaConocimiento,
            Nombre,
            Descripcion
        )
        VALUES
        (
            @IdAreaConocimiento,
            'Ingeniería en Sistemas',
            'Carrera de Ingeniería en Sistemas.'
        );

        SET @IdCarrera = SCOPE_IDENTITY();
    END;


    /* ============================================================
       3. PLAN DE ESTUDIOS
       
       Plan de estudios de ejemplo.
       ============================================================ */

    DECLARE @IdPlanEstudio INT;

    SELECT @IdPlanEstudio = IdPlanEstudio
    FROM acad.PlanEstudio
    WHERE IdCarrera = @IdCarrera
      AND Nombre = '2025'
      AND AnioInicio = 2026;

    IF @IdPlanEstudio IS NULL
    BEGIN
        INSERT INTO acad.PlanEstudio
        (
            IdCarrera,
            Nombre,
            AnioInicio
        )
        VALUES
        (
            @IdCarrera,
            '2025',
            2026
        );

        SET @IdPlanEstudio = SCOPE_IDENTITY();
    END;


    /* ============================================================
       4. ASIGNATURA: BASE DE DATOS I
       ============================================================ */

    DECLARE @IdAsignaturaBD INT;

    SELECT @IdAsignaturaBD = IdAsignatura
    FROM acad.Asignatura
    WHERE Codigo = 'BD-I';

    IF @IdAsignaturaBD IS NULL
    BEGIN
        INSERT INTO acad.Asignatura
        (
            Codigo,
            Nombre,
            Descripcion
        )
        VALUES
        (
            'BD-I',
            'Base de Datos I',
            'Asignatura orientada al diseño y gestión de bases de datos.'
        );

        SET @IdAsignaturaBD = SCOPE_IDENTITY();
    END;


    /* ============================================================
       5. ASIGNATURA: MANUFACTURA DE PRODUCTOS
       ============================================================ */

    DECLARE @IdAsignaturaManufactura INT;

    SELECT @IdAsignaturaManufactura = IdAsignatura
    FROM acad.Asignatura
    WHERE Codigo = 'MAN-PROD';

    IF @IdAsignaturaManufactura IS NULL
    BEGIN
        INSERT INTO acad.Asignatura
        (
            Codigo,
            Nombre,
            Descripcion
        )
        VALUES
        (
            'MAN-PROD',
            'Manufactura de Productos',
            'Asignatura de Manufactura de Productos.'
        );

        SET @IdAsignaturaManufactura = SCOPE_IDENTITY();
    END;


    /* ============================================================
       6. DETALLE DEL PLAN - BASE DE DATOS I
       
       Segundo año = cuarto semestre del plan.
       Créditos: 4.
       ============================================================ */

    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.PlanEstudioDetalle
        WHERE IdPlanEstudio = @IdPlanEstudio
          AND IdAsignatura = @IdAsignaturaBD
    )
    BEGIN
        INSERT INTO acad.PlanEstudioDetalle
        (
            IdPlanEstudio,
            IdAsignatura,
            SemestrePlan,
            Creditos,
            HorasTeoria,
            HorasPractica,
            Orden,
            EsElectiva
        )
        VALUES
        (
            @IdPlanEstudio,
            @IdAsignaturaBD,
            4,
            4,
            NULL,
            NULL,
            NULL,
            0
        );
    END;


    /* ============================================================
       7. PERÍODO ACADÉMICO
       
       Fechas de ejemplo del período 2026-II.
       ============================================================ */

    DECLARE @IdPeriodo INT;

    SELECT @IdPeriodo = IdPeriodo
    FROM acad.PeriodoAcademico
    WHERE Anio = 2026
      AND Semestre = 2;

    IF @IdPeriodo IS NULL
    BEGIN
        INSERT INTO acad.PeriodoAcademico
        (
            Anio,
            Semestre,
            FechaInicio,
            FechaFin
        )
        VALUES
        (
            2026,
            2,
            '2026-07-25',
            '2026-11-20'
        );

        SET @IdPeriodo = SCOPE_IDENTITY();
    END;


    /* ============================================================
       8. AULAS
       ============================================================ */

    DECLARE @IdAula201 INT;
    DECLARE @IdAula105 INT;

    SELECT @IdAula201 = IdAula
    FROM acad.Aula
    WHERE CodigoAula = 'A-201';

    IF @IdAula201 IS NULL
    BEGIN
        INSERT INTO acad.Aula
        (
            CodigoAula,
            Nombre,
            Ubicacion
        )
        VALUES
        (
            'A-201',
            'Aula 201',
            'Edificio Principal'
        );

        SET @IdAula201 = SCOPE_IDENTITY();
    END;


    SELECT @IdAula105 = IdAula
    FROM acad.Aula
    WHERE CodigoAula = 'A-105';

    IF @IdAula105 IS NULL
    BEGIN
        INSERT INTO acad.Aula
        (
            CodigoAula,
            Nombre,
            Ubicacion
        )
        VALUES
        (
            'A-105',
            'Aula 105',
            'Edificio Principal'
        );

        SET @IdAula105 = SCOPE_IDENTITY();
    END;


    /* ============================================================
       9. GRUPOS
       ============================================================ */

    DECLARE @IdGrupo2T2 INT;
    DECLARE @IdGrupo2T1 INT;

    SELECT @IdGrupo2T2 = IdGrupo
    FROM acad.Grupo
    WHERE IdPeriodo = @IdPeriodo
      AND CodigoGrupo = '2T2-SIS-S';

    IF @IdGrupo2T2 IS NULL
    BEGIN
        INSERT INTO acad.Grupo
        (
            IdPeriodo,
            IdCarrera,
            CodigoGrupo,
            Nombre,
            Descripcion
        )
        VALUES
        (
            @IdPeriodo,
            @IdCarrera,
            '2T2-SIS-S',
            '2T2-SIS-S',
            'Grupo de Segundo año de Ingeniería en Sistemas.'
        );

        SET @IdGrupo2T2 = SCOPE_IDENTITY();
    END;


    SELECT @IdGrupo2T1 = IdGrupo
    FROM acad.Grupo
    WHERE IdPeriodo = @IdPeriodo
      AND CodigoGrupo = '2T1-SIS-S';

    IF @IdGrupo2T1 IS NULL
    BEGIN
        INSERT INTO acad.Grupo
        (
            IdPeriodo,
            IdCarrera,
            CodigoGrupo,
            Nombre,
            Descripcion
        )
        VALUES
        (
            @IdPeriodo,
            @IdCarrera,
            '2T1-SIS-S',
            '2T1-SIS-S',
            'Grupo de Segundo año de Ingeniería en Sistemas.'
        );

        SET @IdGrupo2T1 = SCOPE_IDENTITY();
    END;


    /* ============================================================
       10. DOCENTES
       ============================================================ */

    DECLARE @IdDocenteAna INT;
    DECLARE @IdDocenteRoberto INT;

    SELECT @IdDocenteAna = IdDocente
    FROM acad.Docente
    WHERE Correo = 'ana.mendoza@docente.ejemplo';

    IF @IdDocenteAna IS NULL
    BEGIN
        INSERT INTO acad.Docente
        (
            Nombre,
            Correo,
            Telefono
        )
        VALUES
        (
            'Ana Lucía Mendoza Soto',
            'ana.mendoza@docente.ejemplo',
            NULL
        );

        SET @IdDocenteAna = SCOPE_IDENTITY();
    END;


    SELECT @IdDocenteRoberto = IdDocente
    FROM acad.Docente
    WHERE Correo = 'roberto.pena@docente.ejemplo';

    IF @IdDocenteRoberto IS NULL
    BEGIN
        INSERT INTO acad.Docente
        (
            Nombre,
            Correo,
            Telefono
        )
        VALUES
        (
            'Roberto Carlos Peña Aguilar',
            'roberto.pena@docente.ejemplo',
            NULL
        );

        SET @IdDocenteRoberto = SCOPE_IDENTITY();
    END;


    /* ============================================================
       11. CURSO OFERTADO
       
       Ana Lucía:
       Base de Datos I
       Grupo 2T2-SIS-S
       Aula A-201

       Roberto:
       Manufactura de Productos
       Grupo 2T2-SIS-S
       Aula A-201

       Roberto:
       Manufactura de Productos
       Grupo 2T1-SIS-S
       Aula A-105
       ============================================================ */

    DECLARE @IdCursoBD INT;
    DECLARE @IdCursoManufactura2T2 INT;
    DECLARE @IdCursoManufactura2T1 INT;


    SELECT @IdCursoBD = IdCursoOfertado
    FROM acad.CursoOfertado
    WHERE IdAsignatura = @IdAsignaturaBD
      AND IdGrupo = @IdGrupo2T2;

    IF @IdCursoBD IS NULL
    BEGIN
        INSERT INTO acad.CursoOfertado
        (
            IdAsignatura,
            IdGrupo,
            IdDocente,
            IdAula,
            CupoMaximo
        )
        VALUES
        (
            @IdAsignaturaBD,
            @IdGrupo2T2,
            @IdDocenteAna,
            @IdAula201,
            NULL
        );

        SET @IdCursoBD = SCOPE_IDENTITY();
    END;


    SELECT @IdCursoManufactura2T2 = IdCursoOfertado
    FROM acad.CursoOfertado
    WHERE IdAsignatura = @IdAsignaturaManufactura
      AND IdGrupo = @IdGrupo2T2;

    IF @IdCursoManufactura2T2 IS NULL
    BEGIN
        INSERT INTO acad.CursoOfertado
        (
            IdAsignatura,
            IdGrupo,
            IdDocente,
            IdAula
        )
        VALUES
        (
            @IdAsignaturaManufactura,
            @IdGrupo2T2,
            @IdDocenteRoberto,
            @IdAula201
        );

        SET @IdCursoManufactura2T2 = SCOPE_IDENTITY();
    END;


    SELECT @IdCursoManufactura2T1 = IdCursoOfertado
    FROM acad.CursoOfertado
    WHERE IdAsignatura = @IdAsignaturaManufactura
      AND IdGrupo = @IdGrupo2T1;

    IF @IdCursoManufactura2T1 IS NULL
    BEGIN
        INSERT INTO acad.CursoOfertado
        (
            IdAsignatura,
            IdGrupo,
            IdDocente,
            IdAula
        )
        VALUES
        (
            @IdAsignaturaManufactura,
            @IdGrupo2T1,
            @IdDocenteRoberto,
            @IdAula105
        );

        SET @IdCursoManufactura2T1 = SCOPE_IDENTITY();
    END;


    /* ============================================================
       12. HORARIO - BASE DE DATOS I
       
       Martes, Jueves y Viernes
       12:50 PM - 2:20 PM

       DíaSemana:
       1 = Lunes
       2 = Martes
       3 = Miércoles
       4 = Jueves
       5 = Viernes
       6 = Sábado
       7 = Domingo
       ============================================================ */

    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Horario
        WHERE IdCursoOfertado = @IdCursoBD
          AND DiaSemana = 2
          AND HoraInicio = '12:40'
          AND HoraFin = '14:20'
    )
    BEGIN
        INSERT INTO acad.Horario
        (
            IdCursoOfertado,
            DiaSemana,
            HoraInicio,
            HoraFin
        )
        VALUES
        (
            @IdCursoBD,
            2,
            '12:40',
            '14:20'
        );
    END;


    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Horario
        WHERE IdCursoOfertado = @IdCursoBD
          AND DiaSemana = 4
          AND HoraInicio = '12:40'
          AND HoraFin = '14:20'
    )
    BEGIN
        INSERT INTO acad.Horario
        (
            IdCursoOfertado,
            DiaSemana,
            HoraInicio,
            HoraFin
        )
        VALUES
        (
            @IdCursoBD,
            4,
            '12:40',
            '14:20'
        );
    END;


    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Horario
        WHERE IdCursoOfertado = @IdCursoBD
          AND DiaSemana = 5
          AND HoraInicio = '12:40'
          AND HoraFin = '14:20'
    )
    BEGIN
        INSERT INTO acad.Horario
        (
            IdCursoOfertado,
            DiaSemana,
            HoraInicio,
            HoraFin
        )
        VALUES
        (
            @IdCursoBD,
            5,
            '12:40',
            '14:20'
        );
    END;


    /* ============================================================
       13. VALIDACIÓN FINAL
       ============================================================ */

    SELECT
        PA.Anio,
        PA.Semestre,
        G.CodigoGrupo,
        A.Nombre AS Asignatura,
        D.Nombre AS Docente,
        AU.CodigoAula
    FROM acad.CursoOfertado CO
    INNER JOIN acad.Asignatura A
        ON A.IdAsignatura = CO.IdAsignatura
    INNER JOIN acad.Grupo G
        ON G.IdGrupo = CO.IdGrupo
    INNER JOIN acad.PeriodoAcademico PA
        ON PA.IdPeriodo = G.IdPeriodo
    INNER JOIN acad.Docente D
        ON D.IdDocente = CO.IdDocente
    INNER JOIN acad.Aula AU
        ON AU.IdAula = CO.IdAula
    ORDER BY
        G.CodigoGrupo,
        A.Nombre;


    SELECT
        A.Nombre AS Asignatura,
        G.CodigoGrupo,
        H.DiaSemana,
        H.HoraInicio,
        H.HoraFin
    FROM acad.Horario H
    INNER JOIN acad.CursoOfertado CO
        ON CO.IdCursoOfertado = H.IdCursoOfertado
    INNER JOIN acad.Asignatura A
        ON A.IdAsignatura = CO.IdAsignatura
    INNER JOIN acad.Grupo G
        ON G.IdGrupo = CO.IdGrupo
    ORDER BY
        A.Nombre,
        G.CodigoGrupo,
        H.DiaSemana;


    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;
GO