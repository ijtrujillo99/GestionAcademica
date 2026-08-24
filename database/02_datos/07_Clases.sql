USE GestionAcademica;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY

    BEGIN TRANSACTION;

    /* ============================================================
       OBTENER CURSO OFERTADO
       Base de Datos I - Grupo 2T2-SIS-S - 2026-II
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
        THROW 50010,
            'No se encontró el CursoOfertado de Base de Datos I - 2T2-SIS-S - 2026-II.',
            1;
    END;


    /* ============================================================
       FECHAS DE EJEMPLO DE LAS SESIONES

       DíaSemana:
       1 = Lunes
       2 = Martes
       3 = Miércoles
       4 = Jueves
       5 = Viernes
       6 = Sábado
       7 = Domingo
       ============================================================ */

    DECLARE @Clases TABLE
    (
        Fecha DATE NOT NULL,
        DiaSemana TINYINT NOT NULL
    );

    INSERT INTO @Clases
    (
        Fecha,
        DiaSemana
    )
    VALUES
        ('2026-08-04', 2), -- Martes
        ('2026-08-06', 4), -- Jueves
        ('2026-08-07', 5), -- Viernes
        ('2026-08-11', 2), -- Martes
        ('2026-08-13', 4), -- Jueves
        ('2026-08-14', 5), -- Viernes
        ('2026-08-18', 2), -- Martes
        ('2026-08-20', 4), -- Jueves
        ('2026-08-21', 5); -- Viernes


    /* ============================================================
       VALIDAR QUE EXISTE EL HORARIO CORRESPONDIENTE
       ============================================================ */

    IF EXISTS
    (
        SELECT 1
        FROM @Clases C
        LEFT JOIN acad.Horario H
            ON H.IdCursoOfertado = @IdCursoBD
           AND H.DiaSemana = C.DiaSemana
        WHERE H.IdHorario IS NULL
    )
    BEGIN
        THROW 50011,
            'No existe horario configurado para uno o más días de las clases.',
            1;
    END;


    /* ============================================================
       CREAR LAS SESIONES DE CLASE
       ============================================================ */

    INSERT INTO acad.Clase
    (
        IdCursoOfertado,
        IdHorario,
        Fecha,
        Tema,
        Descripcion
    )
    SELECT
        @IdCursoBD,
        H.IdHorario,
        C.Fecha,
        NULL,
        NULL
    FROM @Clases C
    INNER JOIN acad.Horario H
        ON H.IdCursoOfertado = @IdCursoBD
       AND H.DiaSemana = C.DiaSemana
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM acad.Clase CL
        WHERE CL.IdCursoOfertado = @IdCursoBD
          AND CL.Fecha = C.Fecha
          AND CL.IdHorario = H.IdHorario
    );


    /* ============================================================
       VALIDACIÓN
       ============================================================ */

    SELECT
        CL.IdClase,
        A.Nombre AS Asignatura,
        G.CodigoGrupo,
        CL.Fecha,
        H.DiaSemana,
        H.HoraInicio,
        H.HoraFin,
        CL.Tema
    FROM acad.Clase CL
    INNER JOIN acad.CursoOfertado CO
        ON CO.IdCursoOfertado = CL.IdCursoOfertado
    INNER JOIN acad.Asignatura A
        ON A.IdAsignatura = CO.IdAsignatura
    INNER JOIN acad.Grupo G
        ON G.IdGrupo = CO.IdGrupo
    INNER JOIN acad.Horario H
        ON H.IdHorario = CL.IdHorario
    WHERE CL.IdCursoOfertado = @IdCursoBD
    ORDER BY CL.Fecha;


    /* Debe devolver 9 */
    SELECT
        COUNT(*) AS TotalClases
    FROM acad.Clase
    WHERE IdCursoOfertado = @IdCursoBD;


    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;
GO