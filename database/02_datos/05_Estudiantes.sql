USE GestionAcademica;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY

    BEGIN TRANSACTION;

    /* ============================================================
       Estudiantes de ejemplo. No corresponden a personas reales.
       TMP-A01 y TMP-B02 simulan registros incompletos (sin correo).
       ============================================================ */

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-001' OR Correo = 'carlos.molina@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-001', 'CARLOS ANDRES MOLINA RIVAS', 'M', 'carlos.molina@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'TMP-A01' OR Nombre = 'PABLO ESTEBAN NUNEZ SALAS')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('TMP-A01', 'PABLO ESTEBAN NUNEZ SALAS', 'M', NULL, NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-002' OR Correo = 'diego.solis@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-002', 'DIEGO ARMANDO SOLIS PAREDES', 'M', 'diego.solis@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-003' OR Correo = 'elena.castro@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-003', 'ELENA MARIA CASTRO LUNA', 'F', 'elena.castro@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-004' OR Correo = 'fabiola.mora@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-004', 'FABIOLA ISABEL MORA QUINTERO', 'F', 'fabiola.mora@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-005' OR Correo = 'gabriel.torres@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-005', 'GABRIEL ENRIQUE TORRES SILVA', 'M', 'gabriel.torres@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-006' OR Correo = 'helena.vargas@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-006', 'HELENA PATRICIA VARGAS ROJAS', 'F', 'helena.vargas@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2022-007' OR Correo = 'irene.navarro@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2022-007', 'IRENE SOFIA NAVARRO PENA', 'F', 'irene.navarro@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2022-008' OR Correo = 'julia.pimentel@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2022-008', 'JULIA ANDREA PIMENTEL ORTEGA', 'F', 'julia.pimentel@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-009' OR Correo = 'kevin.mendoza@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-009', 'KEVIN RAFAEL MENDOZA CRUZ', 'M', 'kevin.mendoza@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-010' OR Correo = 'luis.ocampo@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-010', 'LUIS FERNANDO OCAMPO BRAVO', 'M', 'luis.ocampo@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-011' OR Correo = 'manuel.herrera@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-011', 'MANUEL IGNACIO HERRERA PAZ', 'M', 'manuel.herrera@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-012' OR Correo = 'natalia.flores@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-012', 'NATALIA BEATRIZ FLORES CAMPOS', 'F', 'natalia.flores@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-013' OR Correo = 'oscar.rivas@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-013', 'OSCAR DANIEL RIVAS GUZMAN', 'M', 'oscar.rivas@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-014' OR Correo = 'pedro.salazar@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-014', 'PEDRO ANTONIO SALAZAR NUNEZ', 'M', 'pedro.salazar@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'TMP-B02' OR Nombre = 'RAUL ESTEBAN MIRANDA LEON')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('TMP-B02', 'RAUL ESTEBAN MIRANDA LEON', 'M', NULL, NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-015' OR Correo = 'sebastian.acosta@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-015', 'SEBASTIAN JOSE ACOSTA VIDAURRE', 'M', 'sebastian.acosta@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-016' OR Correo = 'tamara.benitez@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-016', 'TAMARA LUCIA BENITEZ ROMERO', 'F', 'tamara.benitez@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-017' OR Correo = 'ursula.delgado@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-017', 'URSULA CARMEN DELGADO PONCE', 'F', 'ursula.delgado@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-018' OR Correo = 'victor.espinoza@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-018', 'VICTOR MANUEL ESPINOZA REYES', 'M', 'victor.espinoza@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-019' OR Correo = 'walter.fuentes@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-019', 'WALTER ALFREDO FUENTES DIAZ', 'M', 'walter.fuentes@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-020' OR Correo = 'xavier.gomez@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-020', 'XAVIER ANDRES GOMEZ PALMA', 'M', 'xavier.gomez@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2022-021' OR Correo = 'yadira.hernandez@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2022-021', 'YADIRA ELENA HERNANDEZ SOTO', 'F', 'yadira.hernandez@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-022' OR Correo = 'andres.ibarra@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-022', 'ANDRES FELIPE IBARRA MEJIA', 'M', 'andres.ibarra@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-023' OR Correo = 'bruno.jimenez@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-023', 'BRUNO ALEJANDRO JIMENEZ CANO', 'M', 'bruno.jimenez@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-024' OR Correo = 'camila.lopez@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-024', 'CAMILA ROCIO LOPEZ DUARTE', 'F', 'camila.lopez@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2020-025' OR Correo = 'daniela.martinez@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2020-025', 'DANIELA ISABEL MARTINEZ RUIZ', 'F', 'daniela.martinez@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-026' OR Correo = 'eduardo.navas@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-026', 'EDUARDO JOSE NAVAS CORDERO', 'M', 'eduardo.navas@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-027' OR Correo = 'fernanda.ortiz@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-027', 'FERNANDA LUCIA ORTIZ VELASQUEZ', 'F', 'fernanda.ortiz@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-028' OR Correo = 'gina.pacheco@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-028', 'GINA MARISOL PACHECO BLANCO', 'F', 'gina.pacheco@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-029' OR Correo = 'hilda.quintana@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-029', 'HILDA VANESSA QUINTANA MORALES', 'F', 'hilda.quintana@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-030' OR Correo = 'ivan.ramos@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-030', 'IVAN RODRIGO RAMOS AGUILAR', 'M', 'ivan.ramos@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-031' OR Correo = 'jorge.santos@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-031', 'JORGE LUIS SANTOS MEDINA', 'M', 'jorge.santos@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-032' OR Correo = 'karen.tejada@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-032', 'KAREN PAOLA TEJADA VILLALOBOS', 'F', 'karen.tejada@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2023-033' OR Correo = 'lorenzo.urbina@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2023-033', 'LORENZO DAVID URBINA CASTRO', 'M', 'lorenzo.urbina@estudiante.ejemplo', NULL);
    END;

    IF NOT EXISTS (SELECT 1 FROM acad.Estudiante WHERE Carnet = 'E2024-034' OR Correo = 'marco.valdez@estudiante.ejemplo')
    BEGIN
        INSERT INTO acad.Estudiante (Carnet, Nombre, Sexo, Correo, Telefono)
        VALUES ('E2024-034', 'MARCO ANTONIO VALDEZ PINEDA', 'M', 'marco.valdez@estudiante.ejemplo', NULL);
    END;


    /* ============================================================
       VALIDACIÓN
       ============================================================ */

    SELECT
        IdEstudiante,
        Carnet,
        Nombre,
        Sexo,
        Correo,
        Telefono,
        Activo
    FROM acad.Estudiante
    ORDER BY Nombre;


    SELECT COUNT(*) AS TotalEstudiantes
    FROM acad.Estudiante;


    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;
GO
