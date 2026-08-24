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
       3. CARGA DE CALIFICACIONES DE EJEMPLO

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
        ('E2024-001', 6), -- CARLOS
        ('E2023-002', 7), -- DIEGO
        ('E2024-003', 5), -- ELENA
        ('E2024-004', 7), -- FABIOLA
        ('E2024-006', 6), -- HELENA
        ('E2022-007', 7), -- IRENE
        ('E2022-008', 4), -- JULIA
        ('E2024-009', 7), -- KEVIN
        ('E2024-010', 6), -- LUIS
        ('E2023-011', 7), -- MANUEL
        ('E2023-012', 5), -- NATALIA
        ('E2023-013', 6), -- OSCAR
        ('E2024-014', 7), -- PEDRO
        ('E2023-015', 7), -- SEBASTIAN
        ('E2024-016', 6), -- TAMARA
        ('E2023-017', 5), -- URSULA
        ('E2024-018', 7), -- VICTOR
        ('E2023-019', 6), -- WALTER
        ('E2022-021', 7), -- YADIRA
        ('E2023-022', 4), -- ANDRES
        ('E2023-023', 7), -- BRUNO
        ('E2024-024', 6), -- CAMILA
        ('E2020-025', 5), -- DANIELA
        ('E2024-026', 7), -- EDUARDO
        ('E2023-027', 6), -- FERNANDA
        ('E2024-028', 7), -- GINA
        ('E2023-029', 5), -- HILDA
        ('E2023-030', 6), -- IVAN
        ('E2024-031', 7), -- JORGE
        ('E2024-032', 6), -- KAREN
        ('E2023-033', 7), -- LORENZO
        ('E2024-034', 5); -- MARCO


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
        'Calificación de ejemplo.'
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
       5. CARGA DE CALIFICACIONES DE EJEMPLO

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
        ('E2024-001', 4), -- CARLOS
        ('E2023-002', 5), -- DIEGO
        ('E2024-003', 3), -- ELENA
        ('E2024-004', 5), -- FABIOLA
        ('E2024-006', 4), -- HELENA
        ('E2022-007', 5), -- IRENE
        ('E2022-008', 2), -- JULIA
        ('E2024-009', 5), -- KEVIN
        ('E2024-010', 4), -- LUIS
        ('E2023-011', 5), -- MANUEL
        ('E2023-012', 3), -- NATALIA
        ('E2023-013', 4), -- OSCAR
        ('E2024-014', 5), -- PEDRO
        ('E2023-015', 4), -- SEBASTIAN
        ('E2024-016', 5), -- TAMARA
        ('E2023-017', 3), -- URSULA
        ('E2024-018', 5), -- VICTOR
        ('E2023-019', 4), -- WALTER
        ('E2022-021', 5), -- YADIRA
        ('E2023-022', 4), -- ANDRES
        ('E2023-023', 3), -- BRUNO
        ('E2024-024', 5), -- CAMILA
        ('E2020-025', 4), -- DANIELA
        ('E2024-026', 5), -- EDUARDO
        ('E2024-028', 4), -- GINA
        ('E2023-029', 5), -- HILDA
        ('E2023-030', 3), -- IVAN
        ('E2024-031', 4), -- JORGE
        ('E2024-032', 5), -- KAREN
        ('E2023-033', 4), -- LORENZO
        ('E2024-034', 5); -- MARCO


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
        'Calificación de ejemplo.'
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