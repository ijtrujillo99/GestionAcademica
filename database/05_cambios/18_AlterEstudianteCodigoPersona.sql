USE GestionAcademica;
GO

SET NOCOUNT ON;
GO

/* Columna nueva. Idempotente si la base ya la tiene. */
IF COL_LENGTH('acad.Estudiante', 'CodigoPersona') IS NULL
BEGIN
    ALTER TABLE acad.Estudiante
        ADD CodigoPersona VARCHAR(8) NULL;
END;
GO
