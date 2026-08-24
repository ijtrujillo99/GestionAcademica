USE GestionAcademica;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY

    BEGIN TRANSACTION;

    /* ============================================================
       1. OBTENER CURSO OFERTADO
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
        THROW 50040,
            'No se encontró el CursoOfertado de Base de Datos I - 2T2-SIS-S - 2026-II.',
            1;
    END;


    /* ============================================================
       2. OBTENER EVALUACIONES

       Orden 1 = Tarea 1: Mini-exposiciones
       Orden 2 = Tarea 2: Construyamos nuestro propio MER
       ============================================================ */

    DECLARE @IdEvaluacion1 INT;
    DECLARE @IdEvaluacion2 INT;

    SELECT @IdEvaluacion1 = E.IdEvaluacion
    FROM eval.Evaluacion E
    INNER JOIN eval.Parcial P
        ON P.IdParcial = E.IdParcial
    WHERE P.IdCursoOfertado = @IdCursoBD
      AND P.Orden = 1
      AND E.Orden = 1;


    SELECT @IdEvaluacion2 = E.IdEvaluacion
    FROM eval.Evaluacion E
    INNER JOIN eval.Parcial P
        ON P.IdParcial = E.IdParcial
    WHERE P.IdCursoOfertado = @IdCursoBD
      AND P.Orden = 1
      AND E.Orden = 2;


    IF @IdEvaluacion1 IS NULL
       OR @IdEvaluacion2 IS NULL
    BEGIN
        THROW 50041,
            'No se encontraron las dos primeras evaluaciones del Parcial I.',
            1;
    END;


    /* ============================================================
       3. CARGA DE CALIFICACIONES

       Fuente:
       Hoja "Grupos 1" del Excel

       Evaluación 1:
       Tarea 1 - Mini-exposiciones
       Puntaje máximo: 7

       Se cargan únicamente los estudiantes que tienen nota.
       ============================================================ */

    DECLARE @NotasEvaluacion1 TABLE
    (
        Carnet         VARCHAR(30) NOT NULL,
        PuntajeObtenido DECIMAL(5,2) NOT NULL
    );

    INSERT INTO @NotasEvaluacion1
    (
        Carnet,
        PuntajeObtenido
    )
    VALUES
        ('2022-0950U', 7), -- CANDIDA
        ('2023-0394I', 7), -- IAN
        ('2022-0804U', 7), -- KAROLINA
        ('2023-0234I', 7), -- LEONARDO
        ('2023-0235I', 7), -- DAVID ALEXANDER
        ('2023-0233I', 7), -- LUIS
        ('2023-0819I', 7), -- GERARDO
        ('2023-0390I', 7), -- MIGUEL
        ('2024-1662U', 7), -- JAIRO
        ('2024-1676U', 7), -- RICHARD
        ('2024-1630U', 7), -- SOFÍA
        ('2024-1607U', 7), -- HANIEL
        ('2024-1657U', 7), -- MARVIN
        ('2024-1688U', 7), -- MICHELLE ALEJANDRA
        ('2024-1361U', 7), -- AMALIA
        ('2024-1372U', 7), -- AMMY
        ('2024-1369U', 7), -- ANYELICA
        ('2024-1568U', 7), -- CRISTÓBAL
        ('2024-1648U', 7), -- IXEL
        ('2023-0221I', 7), -- MELANIA
        ('2023-0415I', 7), -- GENESIS
        ('2023-0396I', 7), -- JESLYNG
        ('2023-1083U', 7), -- MICHELLE TATIANA
        ('2020-0636I', 7), -- MARÍA
        ('2023-0916U', 7), -- ALEJANDRO
        ('2024-1669U', 7), -- ABEL
        ('2024-1664U', 7), -- DAVID ALBERTO
        ('2024-1633U', 7), -- MAIKELI
        ('2023-0217I', 7), -- WILLIAN
        ('2024-1693U', 7), -- ZAHID
        ('2022-0294I', 7), -- CORINA
        ('2023-1040U', 7); -- JACQUELINE


    /* ============================================================
       4. INSERTAR CALIFICACIONES - EVALUACIÓN 1
       ============================================================ */

    INSERT INTO eval.Calificacion
    (
        IdEvaluacion,
        IdInscripcion,
        PuntajeObtenido,
        Observacion
    )
    SELECT
        @IdEvaluacion1,
        IC.IdInscripcion,
        N.PuntajeObtenido,
        'Calificación migrada desde hoja Grupos 1.'
    FROM @NotasEvaluacion1 N
    INNER JOIN acad.Estudiante E
        ON E.Carnet = N.Carnet
    INNER JOIN acad.InscripcionCurso IC
        ON IC.IdEstudiante = E.IdEstudiante
       AND IC.IdCursoOfertado = @IdCursoBD
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM eval.Calificacion C
        WHERE C.IdEvaluacion = @IdEvaluacion1
          AND C.IdInscripcion = IC.IdInscripcion
    );


    /* ============================================================
       5. CARGA DE CALIFICACIONES
       
       Fuente:
       Hoja "Grupo 2" del Excel

       Evaluación 2:
       Tarea 2 - Construyamos nuestro propio MER
       Puntaje máximo: 5
       ============================================================ */

    DECLARE @NotasEvaluacion2 TABLE
    (
        Carnet          VARCHAR(30) NOT NULL,
        PuntajeObtenido DECIMAL(5,2) NOT NULL
    );

    INSERT INTO @NotasEvaluacion2
    (
        Carnet,
        PuntajeObtenido
    )
    VALUES
        ('2024-1693U', 5), -- ZAHID
        ('2022-0294I', 5), -- CORINA
        ('2023-0217I', 5), -- WILLIAN
        ('2023-0234I', 5), -- LEONARDO
        ('2023-0390I', 3), -- MIGUEL
        ('2023-0819I', 5), -- GERARDO
        ('2024-1669U', 5), -- ABEL
        ('2024-1633U', 5), -- MAIKELI
        ('2024-1664U', 5), -- DAVID ALBERTO
        ('2023-0396I', 4), -- JESLYNG
        ('2023-0415I', 4), -- GENESIS
        ('2023-0233I', 4), -- LUIS
        ('2022-0804U', 4), -- KAROLINA
        ('2022-0950U', 4), -- CANDIDA
        ('2023-0394I', 3), -- IAN
        ('2024-1369U', 5), -- ANYELICA
        ('2024-1568U', 5), -- CRISTÓBAL
        ('2024-1648U', 5), -- IXEL
        ('2024-1361U', 5), -- AMALIA
        ('2024-1372U', 5), -- AMMY
        ('2023-1040U', 5), -- JACQUELINE
        ('2020-0636I', 3), -- MARÍA
        ('2023-1083U', 5), -- MICHELLE TATIANA
        ('2023-0916U', 5), -- ALEJANDRO
        ('2024-1688U', 4), -- MICHELLE ALEJANDRA
        ('2024-1657U', 4), -- MARVIN
        ('2024-1607U', 4), -- HANIEL
        ('2024-1662U', 2), -- JAIRO
        ('2024-1676U', 4), -- RICHARD
        ('2024-1630U', 4), -- SOFÍA
        ('2023-0235I', 4); -- DAVID ALEXANDER


    /* ============================================================
       6. INSERTAR CALIFICACIONES - EVALUACIÓN 2
       ============================================================ */

    INSERT INTO eval.Calificacion
    (
        IdEvaluacion,
        IdInscripcion,
        PuntajeObtenido,
        Observacion
    )
    SELECT
        @IdEvaluacion2,
        IC.IdInscripcion,
        N.PuntajeObtenido,
        'Calificación migrada desde hoja Grupo 2.'
    FROM @NotasEvaluacion2 N
    INNER JOIN acad.Estudiante E
        ON E.Carnet = N.Carnet
    INNER JOIN acad.InscripcionCurso IC
        ON IC.IdEstudiante = E.IdEstudiante
       AND IC.IdCursoOfertado = @IdCursoBD
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM eval.Calificacion C
        WHERE C.IdEvaluacion = @IdEvaluacion2
          AND C.IdInscripcion = IC.IdInscripcion
    );


    /* ============================================================
       7. VALIDACIÓN DE EVALUACIÓN 1
       ============================================================ */

    SELECT
        E.Carnet,
        E.Nombre,
        C.PuntajeObtenido
    FROM eval.Calificacion C
    INNER JOIN acad.InscripcionCurso IC
        ON IC.IdInscripcion = C.IdInscripcion
    INNER JOIN acad.Estudiante E
        ON E.IdEstudiante = IC.IdEstudiante
    WHERE C.IdEvaluacion = @IdEvaluacion1
    ORDER BY E.Nombre;


    SELECT
        COUNT(*) AS CantidadCalificaciones,
        SUM(C.PuntajeObtenido) AS SumaPuntajes
    FROM eval.Calificacion C
    WHERE C.IdEvaluacion = @IdEvaluacion1;


    /* ============================================================
       8. VALIDACIÓN DE EVALUACIÓN 2
       ============================================================ */

    SELECT
        E.Carnet,
        E.Nombre,
        C.PuntajeObtenido
    FROM eval.Calificacion C
    INNER JOIN acad.InscripcionCurso IC
        ON IC.IdInscripcion = C.IdInscripcion
    INNER JOIN acad.Estudiante E
        ON E.IdEstudiante = IC.IdEstudiante
    WHERE C.IdEvaluacion = @IdEvaluacion2
    ORDER BY E.Nombre;


    SELECT
        COUNT(*) AS CantidadCalificaciones,
        SUM(C.PuntajeObtenido) AS SumaPuntajes
    FROM eval.Calificacion C
    WHERE C.IdEvaluacion = @IdEvaluacion2;


    /* ============================================================
       9. ESTUDIANTES SIN CALIFICACIÓN EN CADA EVALUACIÓN
       ============================================================ */

    SELECT
        'Evaluación 1' AS Evaluacion,
        E.Carnet,
        E.Nombre
    FROM acad.InscripcionCurso IC
    INNER JOIN acad.Estudiante E
        ON E.IdEstudiante = IC.IdEstudiante
    WHERE IC.IdCursoOfertado = @IdCursoBD
      AND NOT EXISTS
      (
          SELECT 1
          FROM eval.Calificacion C
          WHERE C.IdEvaluacion = @IdEvaluacion1
            AND C.IdInscripcion = IC.IdInscripcion
      )

    UNION ALL

    SELECT
        'Evaluación 2' AS Evaluacion,
        E.Carnet,
        E.Nombre
    FROM acad.InscripcionCurso IC
    INNER JOIN acad.Estudiante E
        ON E.IdEstudiante = IC.IdEstudiante
    WHERE IC.IdCursoOfertado = @IdCursoBD
      AND NOT EXISTS
      (
          SELECT 1
          FROM eval.Calificacion C
          WHERE C.IdEvaluacion = @IdEvaluacion2
            AND C.IdInscripcion = IC.IdInscripcion
      )

    ORDER BY
        Evaluacion,
        Nombre;


    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;
GO