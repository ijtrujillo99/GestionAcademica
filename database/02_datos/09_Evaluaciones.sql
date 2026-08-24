USE GestionAcademica;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY

    BEGIN TRANSACTION;


    /* ============================================================
       1. OBTENER CURSO OFERTADO
       BASE DE DATOS I - 2T2-SIS-S - 2026-II
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
        THROW 50030,
            'No se encontró el CursoOfertado de Base de Datos I - 2T2-SIS-S - 2026-II.',
            1;
    END;


    /* ============================================================
       2. OBTENER TIPOS DE EVALUACIÓN
       ============================================================ */

    DECLARE @IdTipoTarea INT;
    DECLARE @IdTipoSistematico INT;
    DECLARE @IdTipoLaboratorio INT;
    DECLARE @IdTipoExposicion INT;
    DECLARE @IdTipoExamen INT;
    DECLARE @IdTipoTareaCurso INT;

    SELECT @IdTipoTarea = IdTipoEvaluacion
    FROM cat.TipoEvaluacion
    WHERE Nombre = 'Tarea';

    SELECT @IdTipoSistematico = IdTipoEvaluacion
    FROM cat.TipoEvaluacion
    WHERE Nombre = 'Sistemático';

    SELECT @IdTipoLaboratorio = IdTipoEvaluacion
    FROM cat.TipoEvaluacion
    WHERE Nombre = 'Laboratorio';

    SELECT @IdTipoExposicion = IdTipoEvaluacion
    FROM cat.TipoEvaluacion
    WHERE Nombre = 'Exposición';

    SELECT @IdTipoExamen = IdTipoEvaluacion
    FROM cat.TipoEvaluacion
    WHERE Nombre = 'Examen';

    SELECT @IdTipoTareaCurso = IdTipoEvaluacion
    FROM cat.TipoEvaluacion
    WHERE Nombre = 'Tarea de Curso';


    IF @IdTipoTarea IS NULL
       OR @IdTipoSistematico IS NULL
       OR @IdTipoLaboratorio IS NULL
       OR @IdTipoExposicion IS NULL
       OR @IdTipoExamen IS NULL
       OR @IdTipoTareaCurso IS NULL
    BEGIN
        THROW 50031,
            'No se encontraron todos los tipos de evaluación requeridos.',
            1;
    END;


    /* ============================================================
       3. CREAR PARCIALES
       ============================================================ */

    DECLARE @IdParcialI INT;
    DECLARE @IdParcialII INT;


    SELECT @IdParcialI = IdParcial
    FROM eval.Parcial
    WHERE IdCursoOfertado = @IdCursoBD
      AND Orden = 1;

    IF @IdParcialI IS NULL
    BEGIN
        INSERT INTO eval.Parcial
        (
            IdCursoOfertado,
            Nombre,
            Orden,
            PuntajeMaximo
        )
        VALUES
        (
            @IdCursoBD,
            'Parcial I',
            1,
            50
        );

        SET @IdParcialI = SCOPE_IDENTITY();
    END;


    SELECT @IdParcialII = IdParcial
    FROM eval.Parcial
    WHERE IdCursoOfertado = @IdCursoBD
      AND Orden = 2;

    IF @IdParcialII IS NULL
    BEGIN
        INSERT INTO eval.Parcial
        (
            IdCursoOfertado,
            Nombre,
            Orden,
            PuntajeMaximo
        )
        VALUES
        (
            @IdCursoBD,
            'Parcial II',
            2,
            50
        );

        SET @IdParcialII = SCOPE_IDENTITY();
    END;


    /* ============================================================
       4. PARCIAL I
       ============================================================ */

    /* ------------------------------------------------------------
       Evaluación 1
       ------------------------------------------------------------ */

    IF NOT EXISTS
    (
        SELECT 1
        FROM eval.Evaluacion
        WHERE IdParcial = @IdParcialI
          AND Orden = 1
    )
    BEGIN
        INSERT INTO eval.Evaluacion
        (
            IdParcial,
            IdTipoEvaluacion,
            Nombre,
            Descripcion,
            PuntajeMaximo,
            EsObligatoria,
            Orden,
            Semana
        )
        VALUES
        (
            @IdParcialI,
            @IdTipoExposicion,
            'Tarea 1: Mini-exposiciones sobre Gestores de BD del ranking DB-Engines',
            'Mini-exposiciones sobre gestores de bases de datos.',
            7,
            0,
            1,
            1
        );
    END;


    /* ------------------------------------------------------------
       Evaluación 2
       ------------------------------------------------------------ */

    IF NOT EXISTS
    (
        SELECT 1
        FROM eval.Evaluacion
        WHERE IdParcial = @IdParcialI
          AND Orden = 2
    )
    BEGIN
        INSERT INTO eval.Evaluacion
        (
            IdParcial,
            IdTipoEvaluacion,
            Nombre,
            Descripcion,
            PuntajeMaximo,
            EsObligatoria,
            Orden,
            Semana
        )
        VALUES
        (
            @IdParcialI,
            @IdTipoExposicion,
            'Tarea 2: Construyamos nuestro propio MER',
            'Exposiciones grupales sobre el diseño de un modelo entidad-relación.',
            5,
            0,
            2,
            3
        );
    END;


    /* ------------------------------------------------------------
       Evaluación 3
       ------------------------------------------------------------ */

    IF NOT EXISTS
    (
        SELECT 1
        FROM eval.Evaluacion
        WHERE IdParcial = @IdParcialI
          AND Orden = 3
    )
    BEGIN
        INSERT INTO eval.Evaluacion
        (
            IdParcial,
            IdTipoEvaluacion,
            Nombre,
            Descripcion,
            PuntajeMaximo,
            EsObligatoria,
            Orden,
            Semana
        )
        VALUES
        (
            @IdParcialI,
            @IdTipoLaboratorio,
            'Talleres de resolución de problemas de normalización',
            'Taller práctico de resolución de ejercicios de normalización.',
            8,
            0,
            3,
            4
        );
    END;


    /* ------------------------------------------------------------
       Evaluación 4
       ------------------------------------------------------------ */

    IF NOT EXISTS
    (
        SELECT 1
        FROM eval.Evaluacion
        WHERE IdParcial = @IdParcialI
          AND Orden = 4
    )
    BEGIN
        INSERT INTO eval.Evaluacion
        (
            IdParcial,
            IdTipoEvaluacion,
            Nombre,
            Descripcion,
            PuntajeMaximo,
            EsObligatoria,
            Orden,
            Semana
        )
        VALUES
        (
            @IdParcialI,
            @IdTipoSistematico,
            'Sistemático 1: Evaluación técnica y Laboratorio 3',
            'Evaluación técnica y actividad de laboratorio.',
            7,
            0,
            4,
            5
        );
    END;


    /* ------------------------------------------------------------
       Evaluación 5
       ------------------------------------------------------------ */

    IF NOT EXISTS
    (
        SELECT 1
        FROM eval.Evaluacion
        WHERE IdParcial = @IdParcialI
          AND Orden = 5
    )
    BEGIN
        INSERT INTO eval.Evaluacion
        (
            IdParcial,
            IdTipoEvaluacion,
            Nombre,
            Descripcion,
            PuntajeMaximo,
            EsObligatoria,
            Orden,
            Semana
        )
        VALUES
        (
            @IdParcialI,
            @IdTipoTarea,
            'Scripting de creación de esquemas y carga inicial',
            'Ejercicio de scripting SQL y carga inicial.',
            8,
            0,
            5,
            6
        );
    END;


    /* ------------------------------------------------------------
       Evaluación 6
       ------------------------------------------------------------ */

    IF NOT EXISTS
    (
        SELECT 1
        FROM eval.Evaluacion
        WHERE IdParcial = @IdParcialI
          AND Orden = 6
    )
    BEGIN
        INSERT INTO eval.Evaluacion
        (
            IdParcial,
            IdTipoEvaluacion,
            Nombre,
            Descripcion,
            PuntajeMaximo,
            EsObligatoria,
            Orden,
            Semana
        )
        VALUES
        (
            @IdParcialI,
            @IdTipoExamen,
            'Examen práctico sobre diseño y manipulación básica',
            'Examen práctico sobre diseño y manipulación básica de bases de datos.',
            15,
            0,
            6,
            7
        );
    END;


    /* ============================================================
       5. PARCIAL II
       ============================================================ */

    /* ------------------------------------------------------------
       Evaluación 1
       ------------------------------------------------------------ */

    IF NOT EXISTS
    (
        SELECT 1
        FROM eval.Evaluacion
        WHERE IdParcial = @IdParcialII
          AND Orden = 1
    )
    BEGIN
        INSERT INTO eval.Evaluacion
        (
            IdParcial,
            IdTipoEvaluacion,
            Nombre,
            Descripcion,
            PuntajeMaximo,
            EsObligatoria,
            Orden,
            Semana
        )
        VALUES
        (
            @IdParcialII,
            @IdTipoLaboratorio,
            'Laboratorio 4: Reportes gerenciales complejos',
            'Práctica de elaboración de reportes gerenciales complejos.',
            8,
            0,
            1,
            9
        );
    END;


    /* ------------------------------------------------------------
       Evaluación 2
       ------------------------------------------------------------ */

    IF NOT EXISTS
    (
        SELECT 1
        FROM eval.Evaluacion
        WHERE IdParcial = @IdParcialII
          AND Orden = 2
    )
    BEGIN
        INSERT INTO eval.Evaluacion
        (
            IdParcial,
            IdTipoEvaluacion,
            Nombre,
            Descripcion,
            PuntajeMaximo,
            EsObligatoria,
            Orden,
            Semana
        )
        VALUES
        (
            @IdParcialII,
            @IdTipoLaboratorio,
            'Laboratorio 5: Programación modular con excepciones',
            'Práctica evaluada de programación modular con manejo de excepciones.',
            7,
            0,
            2,
            12
        );
    END;


    /* ------------------------------------------------------------
       Evaluación 3
       ------------------------------------------------------------ */

    IF NOT EXISTS
    (
        SELECT 1
        FROM eval.Evaluacion
        WHERE IdParcial = @IdParcialII
          AND Orden = 3
    )
    BEGIN
        INSERT INTO eval.Evaluacion
        (
            IdParcial,
            IdTipoEvaluacion,
            Nombre,
            Descripcion,
            PuntajeMaximo,
            EsObligatoria,
            Orden,
            Semana
        )
        VALUES
        (
            @IdParcialII,
            @IdTipoExamen,
            'Evaluación final de programación y SQL avanzado',
            'Evaluación final de programación y SQL avanzado.',
            15,
            0,
            3,
            13
        );
    END;


    /* ------------------------------------------------------------
       Evaluación 4
       ------------------------------------------------------------ */

    IF NOT EXISTS
    (
        SELECT 1
        FROM eval.Evaluacion
        WHERE IdParcial = @IdParcialII
          AND Orden = 4
    )
    BEGIN
        INSERT INTO eval.Evaluacion
        (
            IdParcial,
            IdTipoEvaluacion,
            Nombre,
            Descripcion,
            PuntajeMaximo,
            EsObligatoria,
            Orden,
            Semana
        )
        VALUES
        (
            @IdParcialII,
            @IdTipoTareaCurso,
            'Tarea de Curso',
            'Entrega y presentación del trabajo práctico final.',
            20,
            1,
            4,
            14
        );
    END;


    /* ============================================================
       6. VALIDACIONES
       ============================================================ */

    /* ------------------------------------------------------------
       Evaluaciones del curso
       ------------------------------------------------------------ */

    SELECT
        P.Nombre AS Parcial,
        P.PuntajeMaximo AS PuntajeParcial,
        E.Orden,
        E.Semana,
        E.Nombre AS Evaluacion,
        TE.Nombre AS Tipo,
        E.PuntajeMaximo,
        E.EsObligatoria
    FROM eval.Evaluacion E
    INNER JOIN eval.Parcial P
        ON P.IdParcial = E.IdParcial
    INNER JOIN cat.TipoEvaluacion TE
        ON TE.IdTipoEvaluacion = E.IdTipoEvaluacion
    WHERE P.IdCursoOfertado = @IdCursoBD
    ORDER BY
        P.Orden,
        E.Orden;


    /* ------------------------------------------------------------
   Validar suma de cada parcial
   ------------------------------------------------------------ */

SELECT
    P.Nombre AS Parcial,
    P.PuntajeMaximo AS MaximoParcial,
    SUM(E.PuntajeMaximo) AS PuntajeEvaluaciones,
    P.PuntajeMaximo - SUM(E.PuntajeMaximo) AS Diferencia
FROM eval.Parcial P
INNER JOIN eval.Evaluacion E
    ON E.IdParcial = P.IdParcial
WHERE P.IdCursoOfertado = @IdCursoBD
GROUP BY
    P.IdParcial,
    P.Nombre,
    P.Orden,
    P.PuntajeMaximo
ORDER BY
    P.Orden;


    /* ------------------------------------------------------------
       Validar total del semestre
       ------------------------------------------------------------ */

    SELECT
        SUM(P.PuntajeMaximo) AS PuntajeTotalSemestre
    FROM eval.Parcial P
    WHERE P.IdCursoOfertado = @IdCursoBD;


    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;
GO