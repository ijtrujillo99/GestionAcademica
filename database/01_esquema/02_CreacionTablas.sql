USE GestionAcademica;
GO

/* ============================================================
   1. ESTRUCTURA ACADÉMICA
   ============================================================ */


/* ------------------------------------------------------------
   Área de conocimiento
   ------------------------------------------------------------ */
CREATE TABLE acad.AreaConocimiento
(
    IdAreaConocimiento INT IDENTITY(1,1) NOT NULL,
    Nombre             VARCHAR(150) NOT NULL,
    Descripcion        VARCHAR(300) NULL,
    Activo             BIT NOT NULL
        CONSTRAINT DF_AreaConocimiento_Activo DEFAULT (1),

    CONSTRAINT PK_AreaConocimiento
        PRIMARY KEY (IdAreaConocimiento),

    CONSTRAINT UQ_AreaConocimiento_Nombre
        UNIQUE (Nombre)
);
GO


/* ------------------------------------------------------------
   Carrera
   ------------------------------------------------------------ */
CREATE TABLE acad.Carrera
(
    IdCarrera          INT IDENTITY(1,1) NOT NULL,
    IdAreaConocimiento INT NOT NULL,
    Nombre             VARCHAR(150) NOT NULL,
    Descripcion        VARCHAR(300) NULL,
    Activo             BIT NOT NULL
        CONSTRAINT DF_Carrera_Activo DEFAULT (1),

    CONSTRAINT PK_Carrera
        PRIMARY KEY (IdCarrera),

    CONSTRAINT FK_Carrera_AreaConocimiento
        FOREIGN KEY (IdAreaConocimiento)
        REFERENCES acad.AreaConocimiento(IdAreaConocimiento),

    CONSTRAINT UQ_Carrera_Area_Nombre
        UNIQUE (IdAreaConocimiento, Nombre)
);
GO


/* ------------------------------------------------------------
   Plan de estudio
   ------------------------------------------------------------ */
CREATE TABLE acad.PlanEstudio
(
    IdPlanEstudio INT IDENTITY(1,1) NOT NULL,
    IdCarrera     INT NOT NULL,
    Nombre        VARCHAR(150) NOT NULL,
    AnioInicio    SMALLINT NOT NULL,
    Activo        BIT NOT NULL
        CONSTRAINT DF_PlanEstudio_Activo DEFAULT (1),

    CONSTRAINT PK_PlanEstudio
        PRIMARY KEY (IdPlanEstudio),

    CONSTRAINT FK_PlanEstudio_Carrera
        FOREIGN KEY (IdCarrera)
        REFERENCES acad.Carrera(IdCarrera),

    CONSTRAINT UQ_PlanEstudio_Carrera_Anio
        UNIQUE (IdCarrera, AnioInicio),

    CONSTRAINT CK_PlanEstudio_Anio
        CHECK (AnioInicio >= 2000)
);
GO


/* ------------------------------------------------------------
   Asignatura
   ------------------------------------------------------------ */
CREATE TABLE acad.Asignatura
(
    IdAsignatura INT IDENTITY(1,1) NOT NULL,
    Codigo       VARCHAR(30) NOT NULL,
    Nombre       VARCHAR(150) NOT NULL,
    Descripcion  VARCHAR(300) NULL,
    Activo       BIT NOT NULL
        CONSTRAINT DF_Asignatura_Activo DEFAULT (1),

    CONSTRAINT PK_Asignatura
        PRIMARY KEY (IdAsignatura),

    CONSTRAINT UQ_Asignatura_Codigo
        UNIQUE (Codigo)
);
GO


/* ------------------------------------------------------------
   Detalle del plan de estudio
   Aquí vive semestre, créditos, horas, etc.
   ------------------------------------------------------------ */
CREATE TABLE acad.PlanEstudioDetalle
(
    IdPlanDetalle INT IDENTITY(1,1) NOT NULL,
    IdPlanEstudio INT NOT NULL,
    IdAsignatura INT NOT NULL,
    SemestrePlan  TINYINT NOT NULL,
    Creditos      TINYINT NOT NULL,
    HorasTeoria   TINYINT NULL,
    HorasPractica TINYINT NULL,
    Orden         SMALLINT NULL,
    EsElectiva    BIT NOT NULL
        CONSTRAINT DF_PlanEstudioDetalle_EsElectiva DEFAULT (0),

    CONSTRAINT PK_PlanEstudioDetalle
        PRIMARY KEY (IdPlanDetalle),

    CONSTRAINT FK_PlanEstudioDetalle_PlanEstudio
        FOREIGN KEY (IdPlanEstudio)
        REFERENCES acad.PlanEstudio(IdPlanEstudio),

    CONSTRAINT FK_PlanEstudioDetalle_Asignatura
        FOREIGN KEY (IdAsignatura)
        REFERENCES acad.Asignatura(IdAsignatura),

    CONSTRAINT UQ_PlanEstudioDetalle_Plan_Asignatura
        UNIQUE (IdPlanEstudio, IdAsignatura),

    CONSTRAINT CK_PlanEstudioDetalle_Semestre
        CHECK (SemestrePlan BETWEEN 1 AND 20),

    CONSTRAINT CK_PlanEstudioDetalle_Creditos
        CHECK (Creditos > 0),

    CONSTRAINT CK_PlanEstudioDetalle_HorasTeoria
        CHECK (HorasTeoria IS NULL OR HorasTeoria >= 0),

    CONSTRAINT CK_PlanEstudioDetalle_HorasPractica
        CHECK (HorasPractica IS NULL OR HorasPractica >= 0)
);
GO


/* ============================================================
   2. PERSONAS Y RECURSOS
   ============================================================ */


/* ------------------------------------------------------------
   Docente
   ------------------------------------------------------------ */
CREATE TABLE acad.Docente
(
    IdDocente INT IDENTITY(1,1) NOT NULL,
    Nombre    VARCHAR(150) NOT NULL,
    Correo    VARCHAR(150) NOT NULL,
    Telefono  VARCHAR(30) NULL,
    Activo    BIT NOT NULL
        CONSTRAINT DF_Docente_Activo DEFAULT (1),

    CONSTRAINT PK_Docente
        PRIMARY KEY (IdDocente),

    CONSTRAINT UQ_Docente_Correo
        UNIQUE (Correo)
);
GO


/* ------------------------------------------------------------
   Estudiante
   ------------------------------------------------------------ */
CREATE TABLE [acad].[Estudiante]
(
    [IdEstudiante]     INT IDENTITY(1,1) NOT NULL,
    [Carnet]           VARCHAR(30) NOT NULL,
    [Nombre]           VARCHAR(150) NOT NULL,
    [Sexo]             CHAR(1) NOT NULL,
    [Correo]           VARCHAR(150) NULL,
    [Telefono]         VARCHAR(30) NULL,
    [FechaNacimiento]  DATE NULL,
    [Activo]           BIT NOT NULL
        CONSTRAINT [DF_Estudiante_Activo] DEFAULT (1),
    [CodigoPersona]    VARCHAR(8) NULL,

    CONSTRAINT [PK_Estudiante]
        PRIMARY KEY CLUSTERED
        ([IdEstudiante]),

    CONSTRAINT [UQ_Estudiante_Carnet]
        UNIQUE NONCLUSTERED
        ([Carnet]),

    CONSTRAINT [CK_Estudiante_Sexo]
        CHECK ([Sexo] = 'F' OR [Sexo] = 'M')
)
GO

/* ------------------------------------------------------------
   Aula
   ------------------------------------------------------------ */
CREATE TABLE acad.Aula
(
    IdAula      INT IDENTITY(1,1) NOT NULL,
    CodigoAula  VARCHAR(30) NOT NULL,
    Nombre      VARCHAR(100) NULL,
    Capacidad   SMALLINT NULL,
    Ubicacion   VARCHAR(150) NULL,
    Activo      BIT NOT NULL
        CONSTRAINT DF_Aula_Activo DEFAULT (1),

    CONSTRAINT PK_Aula
        PRIMARY KEY (IdAula),

    CONSTRAINT UQ_Aula_Codigo
        UNIQUE (CodigoAula),

    CONSTRAINT CK_Aula_Capacidad
        CHECK (Capacidad IS NULL OR Capacidad > 0)
);
GO


/* ============================================================
   3. PERÍODOS Y GRUPOS
   ============================================================ */


/* ------------------------------------------------------------
   Período académico
   ------------------------------------------------------------ */
CREATE TABLE acad.PeriodoAcademico
(
    IdPeriodo    INT IDENTITY(1,1) NOT NULL,
    Anio         SMALLINT NOT NULL,
    Semestre     TINYINT NOT NULL,
    FechaInicio  DATE NOT NULL,
    FechaFin     DATE NOT NULL,
    Activo       BIT NOT NULL
        CONSTRAINT DF_PeriodoAcademico_Activo DEFAULT (1),

    CONSTRAINT PK_PeriodoAcademico
        PRIMARY KEY (IdPeriodo),

    CONSTRAINT UQ_PeriodoAcademico_Anio_Semestre
        UNIQUE (Anio, Semestre),

    CONSTRAINT CK_PeriodoAcademico_Semestre
        CHECK (Semestre IN (1, 2)),

    CONSTRAINT CK_PeriodoAcademico_Fechas
        CHECK (FechaFin >= FechaInicio)
);
GO


/* ------------------------------------------------------------
   Grupo
   ------------------------------------------------------------ */
CREATE TABLE acad.Grupo
(
    IdGrupo     INT IDENTITY(1,1) NOT NULL,
    IdPeriodo   INT NOT NULL,
    IdCarrera   INT NOT NULL,
    CodigoGrupo VARCHAR(30) NOT NULL,
    Nombre      VARCHAR(100) NULL,
    Descripcion VARCHAR(300) NULL,
    Activo      BIT NOT NULL
        CONSTRAINT DF_Grupo_Activo DEFAULT (1),

    CONSTRAINT PK_Grupo
        PRIMARY KEY (IdGrupo),

    CONSTRAINT FK_Grupo_PeriodoAcademico
        FOREIGN KEY (IdPeriodo)
        REFERENCES acad.PeriodoAcademico(IdPeriodo),

    CONSTRAINT FK_Grupo_Carrera
        FOREIGN KEY (IdCarrera)
        REFERENCES acad.Carrera(IdCarrera),

    CONSTRAINT UQ_Grupo_Periodo_Codigo
        UNIQUE (IdPeriodo, CodigoGrupo)
);
GO


/* ============================================================
   4. CURSO OFERTADO
   ============================================================ */


/* ------------------------------------------------------------
   Curso ofertado
   ------------------------------------------------------------ */
CREATE TABLE acad.CursoOfertado
(
    IdCursoOfertado INT IDENTITY(1,1) NOT NULL,
    IdAsignatura    INT NOT NULL,
    IdGrupo         INT NOT NULL,
    IdDocente       INT NOT NULL,
    IdAula          INT NOT NULL,
    CupoMaximo      SMALLINT NULL,
    Activo          BIT NOT NULL
        CONSTRAINT DF_CursoOfertado_Activo DEFAULT (1),

    CONSTRAINT PK_CursoOfertado
        PRIMARY KEY (IdCursoOfertado),

    CONSTRAINT FK_CursoOfertado_Asignatura
        FOREIGN KEY (IdAsignatura)
        REFERENCES acad.Asignatura(IdAsignatura),

    CONSTRAINT FK_CursoOfertado_Grupo
        FOREIGN KEY (IdGrupo)
        REFERENCES acad.Grupo(IdGrupo),

    CONSTRAINT FK_CursoOfertado_Docente
        FOREIGN KEY (IdDocente)
        REFERENCES acad.Docente(IdDocente),

    CONSTRAINT FK_CursoOfertado_Aula
        FOREIGN KEY (IdAula)
        REFERENCES acad.Aula(IdAula),

    CONSTRAINT UQ_CursoOfertado_Asignatura_Grupo
        UNIQUE (IdAsignatura, IdGrupo),

    CONSTRAINT CK_CursoOfertado_Cupo
        CHECK (CupoMaximo IS NULL OR CupoMaximo > 0)
);
GO


/* ============================================================
   5. HORARIOS
   ============================================================ */


/* ------------------------------------------------------------
   Horario
   ------------------------------------------------------------ */
CREATE TABLE acad.Horario
(
    IdHorario       INT IDENTITY(1,1) NOT NULL,
    IdCursoOfertado INT NOT NULL,
    DiaSemana       TINYINT NOT NULL,
    HoraInicio      TIME(0) NOT NULL,
    HoraFin         TIME(0) NOT NULL,

    CONSTRAINT PK_Horario
        PRIMARY KEY (IdHorario),

    CONSTRAINT FK_Horario_CursoOfertado
        FOREIGN KEY (IdCursoOfertado)
        REFERENCES acad.CursoOfertado(IdCursoOfertado),

    CONSTRAINT CK_Horario_DiaSemana
        CHECK (DiaSemana BETWEEN 1 AND 7),

    CONSTRAINT CK_Horario_Horas
        CHECK (HoraFin > HoraInicio),

    CONSTRAINT UQ_Horario_Curso_Dia_Hora
        UNIQUE (IdCursoOfertado, DiaSemana, HoraInicio, HoraFin)
);
GO


/* ============================================================
   6. CATÁLOGOS
   ============================================================ */


/* ------------------------------------------------------------
   Estado de inscripción
   ------------------------------------------------------------ */
CREATE TABLE cat.EstadoInscripcion
(
    IdEstadoInscripcion INT IDENTITY(1,1) NOT NULL,
    Nombre              VARCHAR(50) NOT NULL,
    Descripcion         VARCHAR(200) NULL,
    Activo              BIT NOT NULL
        CONSTRAINT DF_EstadoInscripcion_Activo DEFAULT (1),

    CONSTRAINT PK_EstadoInscripcion
        PRIMARY KEY (IdEstadoInscripcion),

    CONSTRAINT UQ_EstadoInscripcion_Nombre
        UNIQUE (Nombre)
);
GO


/* ------------------------------------------------------------
   Estado de asistencia
   ------------------------------------------------------------ */
CREATE TABLE cat.EstadoAsistencia
(
    IdEstadoAsistencia INT IDENTITY(1,1) NOT NULL,
    Nombre             VARCHAR(50) NOT NULL,
    Descripcion        VARCHAR(200) NULL,
    Activo             BIT NOT NULL
        CONSTRAINT DF_EstadoAsistencia_Activo DEFAULT (1),

    CONSTRAINT PK_EstadoAsistencia
        PRIMARY KEY (IdEstadoAsistencia),

    CONSTRAINT UQ_EstadoAsistencia_Nombre
        UNIQUE (Nombre)
);
GO


/* ------------------------------------------------------------
   Tipo de evaluación
   ------------------------------------------------------------ */
CREATE TABLE cat.TipoEvaluacion
(
    IdTipoEvaluacion INT IDENTITY(1,1) NOT NULL,
    Nombre           VARCHAR(80) NOT NULL,
    Descripcion      VARCHAR(200) NULL,
    Activo            BIT NOT NULL
        CONSTRAINT DF_TipoEvaluacion_Activo DEFAULT (1),

    CONSTRAINT PK_TipoEvaluacion
        PRIMARY KEY (IdTipoEvaluacion),

    CONSTRAINT UQ_TipoEvaluacion_Nombre
        UNIQUE (Nombre)
);
GO


/* ============================================================
   7. INSCRIPCIÓN Y ASISTENCIA
   ============================================================ */


/* ------------------------------------------------------------
   Inscripción al curso
   ------------------------------------------------------------ */
CREATE TABLE acad.InscripcionCurso
(
    IdInscripcion       INT IDENTITY(1,1) NOT NULL,
    IdEstudiante        INT NOT NULL,
    IdCursoOfertado     INT NOT NULL,
    IdEstadoInscripcion INT NOT NULL,
    FechaInicio         DATE NOT NULL
        CONSTRAINT DF_InscripcionCurso_FechaInicio
        DEFAULT (CONVERT(DATE, GETDATE())),
    FechaEstado         DATETIME2(0) NOT NULL
        CONSTRAINT DF_InscripcionCurso_FechaEstado
        DEFAULT (SYSDATETIME()),
    FechaRetiro         DATE NULL,
    EsOficial            BIT NOT NULL
        CONSTRAINT DF_InscripcionCurso_EsOficial DEFAULT (1),
    Observacion         VARCHAR(500) NULL,

    CONSTRAINT PK_InscripcionCurso
        PRIMARY KEY (IdInscripcion),

    CONSTRAINT FK_InscripcionCurso_Estudiante
        FOREIGN KEY (IdEstudiante)
        REFERENCES acad.Estudiante(IdEstudiante),

    CONSTRAINT FK_InscripcionCurso_CursoOfertado
        FOREIGN KEY (IdCursoOfertado)
        REFERENCES acad.CursoOfertado(IdCursoOfertado),

    CONSTRAINT FK_InscripcionCurso_Estado
        FOREIGN KEY (IdEstadoInscripcion)
        REFERENCES cat.EstadoInscripcion(IdEstadoInscripcion),

    CONSTRAINT UQ_InscripcionCurso_Estudiante_Curso
        UNIQUE (IdEstudiante, IdCursoOfertado),

    CONSTRAINT CK_InscripcionCurso_FechaRetiro
        CHECK (FechaRetiro IS NULL OR FechaRetiro >= FechaInicio)
);
GO


/* ------------------------------------------------------------
   Clase / sesión
   ------------------------------------------------------------ */
CREATE TABLE acad.Clase
(
    IdClase         INT IDENTITY(1,1) NOT NULL,
    IdCursoOfertado INT NOT NULL,
    IdHorario       INT NULL,
    Fecha           DATE NOT NULL,
    Tema            VARCHAR(200) NULL,
    Descripcion     VARCHAR(500) NULL,

    CONSTRAINT PK_Clase
        PRIMARY KEY (IdClase),

    CONSTRAINT FK_Clase_CursoOfertado
        FOREIGN KEY (IdCursoOfertado)
        REFERENCES acad.CursoOfertado(IdCursoOfertado),

    CONSTRAINT FK_Clase_Horario
        FOREIGN KEY (IdHorario)
        REFERENCES acad.Horario(IdHorario),

    CONSTRAINT UQ_Clase_Curso_Fecha_Horario
        UNIQUE (IdCursoOfertado, Fecha, IdHorario)
);
GO


/* ------------------------------------------------------------
   Asistencia
   ------------------------------------------------------------ */
CREATE TABLE acad.Asistencia
(
    IdAsistencia       INT IDENTITY(1,1) NOT NULL,
    IdClase            INT NOT NULL,
    IdInscripcion      INT NOT NULL,
    IdEstadoAsistencia INT NOT NULL,
    Observacion        VARCHAR(500) NULL,

    CONSTRAINT PK_Asistencia
        PRIMARY KEY (IdAsistencia),

    CONSTRAINT FK_Asistencia_Clase
        FOREIGN KEY (IdClase)
        REFERENCES acad.Clase(IdClase),

    CONSTRAINT FK_Asistencia_Inscripcion
        FOREIGN KEY (IdInscripcion)
        REFERENCES acad.InscripcionCurso(IdInscripcion),

    CONSTRAINT FK_Asistencia_Estado
        FOREIGN KEY (IdEstadoAsistencia)
        REFERENCES cat.EstadoAsistencia(IdEstadoAsistencia),

    CONSTRAINT UQ_Asistencia_Clase_Inscripcion
        UNIQUE (IdClase, IdInscripcion)
);
GO


/* ============================================================
   8. EVALUACIÓN
   ============================================================ */


/* ------------------------------------------------------------
   Parcial
   ------------------------------------------------------------ */
CREATE TABLE eval.Parcial
(
    IdParcial       INT IDENTITY(1,1) NOT NULL,
    IdCursoOfertado INT NOT NULL,
    Nombre          VARCHAR(100) NOT NULL,
    Orden           TINYINT NOT NULL,
    PuntajeMaximo   DECIMAL(5,2) NOT NULL,

    CONSTRAINT PK_Parcial
        PRIMARY KEY (IdParcial),

    CONSTRAINT FK_Parcial_CursoOfertado
        FOREIGN KEY (IdCursoOfertado)
        REFERENCES acad.CursoOfertado(IdCursoOfertado),

    CONSTRAINT UQ_Parcial_Curso_Orden
        UNIQUE (IdCursoOfertado, Orden),

    CONSTRAINT CK_Parcial_Orden
        CHECK (Orden > 0),

    CONSTRAINT CK_Parcial_PuntajeMaximo
        CHECK (PuntajeMaximo > 0)
);
GO


/* ------------------------------------------------------------
   Evaluación
   ------------------------------------------------------------ */
CREATE TABLE [eval].[Evaluacion]
(
    [IdEvaluacion]     INT IDENTITY(1,1) NOT NULL,
    [IdParcial]        INT NOT NULL,
    [IdTipoEvaluacion] INT NOT NULL,
    [Nombre]           VARCHAR(150) NOT NULL,
    [Descripcion]      VARCHAR(500) NULL,
    [PuntajeMaximo]    DECIMAL(5,2) NOT NULL,
    [EsObligatoria]    BIT NOT NULL
        CONSTRAINT [DF_Evaluacion_EsObligatoria] DEFAULT (0),
    [Orden]            TINYINT NOT NULL,
    [Semana]           TINYINT NULL,

    CONSTRAINT [PK_Evaluacion]
        PRIMARY KEY CLUSTERED
        ([IdEvaluacion]),

    CONSTRAINT [UQ_Evaluacion_Parcial_Orden]
        UNIQUE NONCLUSTERED
        ([IdParcial], [Orden]),

    CONSTRAINT [FK_Evaluacion_Parcial]
        FOREIGN KEY ([IdParcial])
        REFERENCES [eval].[Parcial] ([IdParcial]),

    CONSTRAINT [FK_Evaluacion_Tipo]
        FOREIGN KEY ([IdTipoEvaluacion])
        REFERENCES [cat].[TipoEvaluacion] ([IdTipoEvaluacion]),

    CONSTRAINT [CK_Evaluacion_Orden]
        CHECK ([Orden] > 0),

    CONSTRAINT [CK_Evaluacion_PuntajeMaximo]
        CHECK ([PuntajeMaximo] > 0)
)
GO


/* ------------------------------------------------------------
   Calificación
   ------------------------------------------------------------ */
CREATE TABLE eval.Calificacion
(
    IdCalificacion   INT IDENTITY(1,1) NOT NULL,
    IdEvaluacion     INT NOT NULL,
    IdInscripcion    INT NOT NULL,
    PuntajeObtenido  DECIMAL(5,2) NOT NULL,
    FechaRegistro    DATETIME2(0) NOT NULL
        CONSTRAINT DF_Calificacion_FechaRegistro
        DEFAULT (SYSDATETIME()),
    Observacion      VARCHAR(500) NULL,

    CONSTRAINT PK_Calificacion
        PRIMARY KEY (IdCalificacion),

    CONSTRAINT FK_Calificacion_Evaluacion
        FOREIGN KEY (IdEvaluacion)
        REFERENCES eval.Evaluacion(IdEvaluacion),

    CONSTRAINT FK_Calificacion_Inscripcion
        FOREIGN KEY (IdInscripcion)
        REFERENCES acad.InscripcionCurso(IdInscripcion),

    CONSTRAINT UQ_Calificacion_Evaluacion_Inscripcion
        UNIQUE (IdEvaluacion, IdInscripcion),

    CONSTRAINT CK_Calificacion_Puntaje
        CHECK (PuntajeObtenido >= 0)
);
GO