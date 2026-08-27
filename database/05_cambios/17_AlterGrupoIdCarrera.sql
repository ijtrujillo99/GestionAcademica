USE GestionAcademica;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

/* Columna nueva. En el mismo lote no se puede usar todavía. */
IF COL_LENGTH('acad.Grupo', 'IdCarrera') IS NULL
BEGIN
    ALTER TABLE acad.Grupo
        ADD IdCarrera INT NULL;
END;
GO

DECLARE @TotalSistemas INT;
DECLARE @IdCarreraSistemas INT;

SELECT @TotalSistemas = COUNT(*)
FROM acad.Carrera
WHERE Nombre = N'Ingeniería en Sistemas';

IF @TotalSistemas <> 1
BEGIN
    THROW 50020,
        'Se esperaba exactamente una carrera llamada Ingeniería en Sistemas para rellenar acad.Grupo.IdCarrera.',
        1;
END;

SELECT @IdCarreraSistemas = IdCarrera
FROM acad.Carrera
WHERE Nombre = N'Ingeniería en Sistemas';

UPDATE acad.Grupo
SET IdCarrera = @IdCarreraSistemas
WHERE IdCarrera IS NULL;

IF EXISTS (SELECT 1 FROM acad.Grupo WHERE IdCarrera IS NULL)
BEGIN
    THROW 50021,
        'Quedaron grupos sin IdCarrera. No se puede exigir NOT NULL.',
        1;
END;
GO

IF EXISTS
(
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'acad.Grupo')
      AND name = N'IdCarrera'
      AND is_nullable = 1
)
BEGIN
    ALTER TABLE acad.Grupo
        ALTER COLUMN IdCarrera INT NOT NULL;
END;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = N'FK_Grupo_Carrera'
      AND parent_object_id = OBJECT_ID(N'acad.Grupo')
)
BEGIN
    ALTER TABLE acad.Grupo WITH CHECK
        ADD CONSTRAINT FK_Grupo_Carrera
        FOREIGN KEY (IdCarrera)
        REFERENCES acad.Carrera (IdCarrera);

    ALTER TABLE acad.Grupo
        CHECK CONSTRAINT FK_Grupo_Carrera;
END;
GO
