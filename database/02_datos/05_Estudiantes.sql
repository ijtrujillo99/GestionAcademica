USE GestionAcademica;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY

    BEGIN TRANSACTION;

    /* ============================================================
       1. ABEL MELQUISEDEC BERMÚDEZ DÍAZ
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1669U'
           OR Correo = 'abel.bermudez873@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet,
            Nombre,
            Sexo,
            Correo,
            Telefono
        )
        VALUES
        (
            '2024-1669U',
            'ABEL MELQUISEDEC BERMÚDEZ DÍAZ',
            'M',
            'abel.bermudez873@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       2. ALBERTO VINERTE RAFAEL CARDOZE FLORES
       Sin carnet y sin correo
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Nombre = 'ALBERTO VINERTE RAFAEL CARDOZE FLORES'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet,
            Nombre,
            Sexo,
            Correo,
            Telefono
        )
        VALUES
        (
            '6FA99262',
            'ALBERTO VINERTE RAFAEL CARDOZE FLORES',
            'M',
            NULL,
            NULL
        );
    END;


    /* ============================================================
       3. ALEJANDRO JOSE ALVAREZ ESPINOSA
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-0916U'
           OR Correo = 'alejandro.alvarez17u@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet,
            Nombre,
            Sexo,
            Correo,
            Telefono
        )
        VALUES
        (
            '2023-0916U',
            'ALEJANDRO JOSE ALVAREZ ESPINOSA',
            'M',
            'alejandro.alvarez17u@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       4. AMALIA DEL CARMEN AGUIRRE GARCIA
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1361U'
           OR Correo = 'amalia.aguirre887@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet,
            Nombre,
            Sexo,
            Correo,
            Telefono
        )
        VALUES
        (
            '2024-1361U',
            'AMALIA DEL CARMEN AGUIRRE GARCIA',
            'F',
            'amalia.aguirre887@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       5. AMMY ENOLA ESPINOZA VALLE
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1372U'
           OR Correo = 'ammy.espinoza748@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet,
            Nombre,
            Sexo,
            Correo,
            Telefono
        )
        VALUES
        (
            '2024-1372U',
            'AMMY ENOLA ESPINOZA VALLE',
            'F',
            'ammy.espinoza748@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       6. ANTHONY DEREK RAMIREZ ANDINO
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1735U'
           OR Correo = 'anthony.ramirez964@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet,
            Nombre,
            Sexo,
            Correo,
            Telefono
        )
        VALUES
        (
            '2024-1735U',
            'ANTHONY DEREK RAMIREZ ANDINO',
            'M',
            'anthony.ramirez964@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       7. ANYELICA MARIA ORTIZ HERNANDEZ
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1369U'
           OR Correo = 'anyelica.ortiz862@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet,
            Nombre,
            Sexo,
            Correo,
            Telefono
        )
        VALUES
        (
            '2024-1369U',
            'ANYELICA MARIA ORTIZ HERNANDEZ',
            'F',
            'anyelica.ortiz862@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       8. CANDIDA CAROLINA SANDINO GAITAN
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2022-0950U'
           OR Correo = 'candida.sandino85u@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2022-0950U',
            'CANDIDA CAROLINA SANDINO GAITAN',
            'F',
            'candida.sandino85u@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       9. CORINA PATRICIA CHOW MENDEZ
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2022-0294I'
           OR Correo = 'corina.chow67i@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2022-0294I',
            'CORINA PATRICIA CHOW MENDEZ',
            'F',
            'corina.chow67i@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       10. CRISTÓBAL DARIO DÍAZ MORA
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1568U'
           OR Correo = 'cristobal.diaz157@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2024-1568U',
            'CRISTÓBAL DARIO DÍAZ MORA',
            'M',
            'cristobal.diaz157@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       11. DAVID ALBERTO REYES CASTILLO
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1664U'
           OR Correo = 'david.reyes253@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2024-1664U',
            'DAVID ALBERTO REYES CASTILLO',
            'M',
            'david.reyes253@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       12. DAVID ALEXANDER LACAYO URRUTIA
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-0235I'
           OR Correo = 'david.lacayo69i@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2023-0235I',
            'DAVID ALEXANDER LACAYO URRUTIA',
            'M',
            'david.lacayo69i@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       13. GENESIS PRISCILA MEJIA AGUILAR
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-0415I'
           OR Correo = 'genesis.mejia85i@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2023-0415I',
            'GENESIS PRISCILA MEJIA AGUILAR',
            'F',
            'genesis.mejia85i@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       14. GERARDO JOSE REYES MALTEZ
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-0819I'
           OR Correo = 'gerardo.reyes88i@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2023-0819I',
            'GERARDO JOSE REYES MALTEZ',
            'M',
            'gerardo.reyes88i@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       15. HANIEL SAMIR ARAGON TAISIGUE
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1607U'
           OR Correo = 'haniel.aragon791@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2024-1607U',
            'HANIEL SAMIR ARAGON TAISIGUE',
            'M',
            'haniel.aragon791@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       16. HARVEY JOSÉ LÓPEZ RIVERA
       Sin carnet y sin correo
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Nombre = 'HARVEY JOSÉ LÓPEZ RIVERA'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet,
            Nombre,
            Sexo,
            Correo,
            Telefono
        )
        VALUES
        (
            '6B8F1E77',
            'HARVEY JOSÉ LÓPEZ RIVERA',
            'M',
            NULL,
            NULL
        );
    END;


    /* ============================================================
       17. IAN ARIEL LOPEZ MORALES
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-0394I'
           OR Correo = 'ian.lopez31i@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2023-0394I',
            'IAN ARIEL LOPEZ MORALES',
            'M',
            'ian.lopez31i@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       18. IXEL GABRIELA RAMÍREZ PÉREZ
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1648U'
           OR Correo = 'ixel.ramirez522@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2024-1648U',
            'IXEL GABRIELA RAMÍREZ PÉREZ',
            'F',
            'ixel.ramirez522@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       19. JACQUELINE DAYANA CARCACHE PAYAN
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-1040U'
           OR Correo = 'jacqueline.carcache94u@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2023-1040U',
            'JACQUELINE DAYANA CARCACHE PAYAN',
            'F',
            'jacqueline.carcache94u@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       20. JAIRO JOSE SANCHEZ POTOSME
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1662U'
           OR Correo = 'jairo.sanchez657@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2024-1662U',
            'JAIRO JOSE SANCHEZ POTOSME',
            'M',
            'jairo.sanchez657@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       21. JESLYNG DAVID MALDONADO VIVAS
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-0396I'
           OR Correo = 'jeslyng.maldonado25i@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2023-0396I',
            'JESLYNG DAVID MALDONADO VIVAS',
            'M',
            'jeslyng.maldonado25i@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       22. JORDY AZAEL DIAZ RUGAMA
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-0253I'
           OR Correo = 'jordy.diaz67i@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2023-0253I',
            'JORDY AZAEL DIAZ RUGAMA',
            'M',
            'jordy.diaz67i@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       23. KAROLINA DE LAS MERCEDES DIAZ ALVAREZ
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2022-0804U'
           OR Correo = 'karolina.diaz20u@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2022-0804U',
            'KAROLINA DE LAS MERCEDES DIAZ ALVAREZ',
            'F',
            'karolina.diaz20u@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       24. LEONARDO JOSUE GARCIA ALEMAN
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-0234I'
           OR Correo = 'leonardo.garcia63i@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2023-0234I',
            'LEONARDO JOSUE GARCIA ALEMAN',
            'M',
            'leonardo.garcia63i@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       25. LUIS RODRIGO BOSQUE BALTODANO
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-0233I'
           OR Correo = 'luis.bosque11i@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2023-0233I',
            'LUIS RODRIGO BOSQUE BALTODANO',
            'M',
            'luis.bosque11i@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       26. MAIKELI GIUVELKI RODRIGUEZ CENTENO
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1633U'
           OR Correo = 'maikeli.rodriguez573@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2024-1633U',
            'MAIKELI GIUVELKI RODRIGUEZ CENTENO',
            'F',
            'maikeli.rodriguez573@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       27. MARIA ALEJANDRA GUTIÉRREZ MEJÍA
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2020-0636I'
           OR Correo = 'maria.gutierrez50i@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2020-0636I',
            'MARIA ALEJANDRA GUTIÉRREZ MEJÍA',
            'F',
            'maria.gutierrez50i@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       28. MARVIN FRANCISCO PÉREZ HERNÁNDEZ
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1657U'
           OR Correo = 'marvin.perez258@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2024-1657U',
            'MARVIN FRANCISCO PÉREZ HERNÁNDEZ',
            'M',
            'marvin.perez258@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       29. MELANIA DEL CARMEN CEDEÑO MEZA
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-0221I'
           OR Correo = 'melania.cedeno27i@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2023-0221I',
            'MELANIA DEL CARMEN CEDEÑO MEZA',
            'F',
            'melania.cedeno27i@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       30. MICHELLE ALEJANDRA ARCE VARGAS
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1688U'
           OR Correo = 'michelle.arce986@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2024-1688U',
            'MICHELLE ALEJANDRA ARCE VARGAS',
            'F',
            'michelle.arce986@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       31. MICHELLE TATIANA DIAZ LOPEZ
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-1083U'
           OR Correo = 'michelle.diaz30u@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2023-1083U',
            'MICHELLE TATIANA DIAZ LOPEZ',
            'F',
            'michelle.diaz30u@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       32. MIGUEL ANGEL MONTIEL GOMEZ
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-0390I'
           OR Correo = 'miguel.montiel76i@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2023-0390I',
            'MIGUEL ANGEL MONTIEL GOMEZ',
            'M',
            'miguel.montiel76i@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       33. RICHARD JOSUE QUEZADA SOLANO
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1676U'
           OR Correo = 'richard.quezada356@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2024-1676U',
            'RICHARD JOSUE QUEZADA SOLANO',
            'M',
            'richard.quezada356@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       34. SOFÍA ALEJANDRA CALERO HUETE
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1630U'
           OR Correo = 'sofia.calero501@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2024-1630U',
            'SOFÍA ALEJANDRA CALERO HUETE',
            'F',
            'sofia.calero501@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       35. WILLIAN ALBERTO PÉREZ MARTÍNEZ
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2023-0217I'
           OR Correo = 'willian.perez40i@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2023-0217I',
            'WILLIAN ALBERTO PÉREZ MARTÍNEZ',
            'M',
            'willian.perez40i@std.uni.edu.ni',
            NULL
        );
    END;


    /* ============================================================
       36. ZAHID ENRRIQUE ESQUIVEL LAINEZ
       ============================================================ */
    IF NOT EXISTS
    (
        SELECT 1
        FROM acad.Estudiante
        WHERE Carnet = '2024-1693U'
           OR Correo = 'zahid.esquivel775@std.uni.edu.ni'
    )
    BEGIN
        INSERT INTO acad.Estudiante
        (
            Carnet, Nombre, Sexo, Correo, Telefono
        )
        VALUES
        (
            '2024-1693U',
            'ZAHID ENRRIQUE ESQUIVEL LAINEZ',
            'M',
            'zahid.esquivel775@std.uni.edu.ni',
            NULL
        );
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