# Scripts de SQL Server

Se ejecutan **en orden numérico**, por carpeta.

```text
01_esquema/        crear base, esquemas y tablas
02_datos/          catálogos y datos de ejemplo
03_diagnostico/    validación en SSMS (la API no lo usa)
04_reportes/       procedimientos de consulta
05_cambios/        ALTER sobre una base que ya existe
```

## Orden

```text
01_esquema/01_Creacion.sql
01_esquema/02_CreacionTablas.sql

02_datos/03_InsertarCatalogos.sql
02_datos/04_DatosAcademicos.sql
02_datos/05_Estudiantes.sql
02_datos/06_Inscripciones.sql
02_datos/07_Clases.sql
02_datos/08_Asistencia.sql
02_datos/09_Evaluaciones.sql
02_datos/10_Calificaciones.sql

03_diagnostico/11_DiagnosticoAcademico.sql

04_reportes/12_sp_ReporteAsistenciaEstudiante.sql
04_reportes/13_sp_ReporteAsistenciaSesion.sql
04_reportes/14_sp_ReporteAsistenciaSemanal.sql
04_reportes/15_sp_ReporteCalificaciones.sql
04_reportes/16_sp_ReporteSituacionAcademica.sql

05_cambios/17_AlterGrupoIdCarrera.sql   ← solo si la base ya existía sin IdCarrera
```

La API no crea ni migra la base. Usa:

```text
DESKTOP-INGRIDT / GestionAcademica
```

`acad.Grupo` ahora tiene `IdCarrera` (FK a `acad.Carrera`). El unique `(IdPeriodo, CodigoGrupo)` no cambia. `CursoOfertado` no se toca.

- Base **ya creada**: ejecutar `05_cambios/17_AlterGrupoIdCarrera.sql` en SSMS. Idempotente. Rellena los grupos actuales con Ingeniería en Sistemas.
- Instalación nueva: la columna ya está en `02_CreacionTablas.sql`; el 17 no hace falta (si lo corres, no vuelve a agregar la columna).

Los reportes están escritos como `CREATE OR ALTER`. Si cambias un SP, vuelve a ejecutarlo en SSMS; no hace falta recrear la base.

En el repo está versionado `15_sp_ReporteCalificaciones.sql`. Los de asistencia (`12`–`14`) y situación académica (`16`) viven hoy en la base; si los cambias, extrae el `CREATE OR ALTER` a `04_reportes/` para no perder el contrato.

Los scripts de `02_datos/` usan identidades, correos, carnets y notas de ejemplo. No corresponden a personas reales.

## Reportes y API

| Script | Procedimiento | GET |
|---|---|---|
| `12` | `acad.sp_ReporteAsistenciaEstudiante` | `/api/cursos-ofertados/{id}/reportes/asistencia-estudiante` |
| `13` | `acad.sp_ReporteAsistenciaSesion` | `.../asistencia-sesion` |
| `14` | `acad.sp_ReporteAsistenciaSemanal` | `.../asistencia-semanal` |
| `15` | `eval.sp_ReporteCalificaciones` | `.../calificaciones` |
| `16` | `eval.sp_ReporteSituacionAcademica` | `.../situacion-academica` |

Los porcentajes y el resultado académico **no se guardan en tablas**. El SP los calcula.

Los nombres de procedimiento que usa C# están en `src/Api/Infraestructura/Reportes/NombresProcedimientos.cs`.

## Calificaciones (`15`)

Script versionado: `04_reportes/15_sp_ReporteCalificaciones.sql`. Parámetro: `@IdCursoOfertado`. Curso inexistente → `50100`. Curso sin evaluaciones → `50101`.

No filtra inscripciones: salen **Activa**, **Pendiente** y **Retirada**. El front oculta retirados en pantalla; el Excel usa la respuesta completa.

Campos fijos (nombres estables; no van al `STRING_AGG` de evaluaciones):

```text
Carnet
Nombre Completo
EstadoInscripcion     ← catálogo: 'Activa' | 'Pendiente' | 'Retirada'
IdEstudiante
IdInscripcion
…una columna por eval.Evaluacion.Nombre (orden: Parcial.Orden, Evaluacion.Orden)…
AcumuladoTotal
```

`EstadoInscripcion` es el mismo texto que inscripciones y situación académica: **`Retirada`**, no `Retirado`. `Retirado` es `ResultadoAcademico`.

Cada celda de evaluación es `VARCHAR`: `'0'`, `'sin evaluar'` o el puntaje convertido. `AcumuladoTotal` es `DECIMAL(5,2)`.

```text
TieneActividad = 0                         → '0' en todas las evaluaciones
PuntajeObtenido IS NULL (con actividad)    → 'sin evaluar'
Retirada y ninguna nota <> 0               → '0' en todas y AcumuladoTotal = 0
Retirada con nota real (ej. 7)             → se dejan las notas
```

`TieneActividad` es 1 si hay asistencia del curso o cualquier fila en `eval.Calificacion` de esa inscripción.

La regla de ceros del retirado está replicada en `ReglasReporteCalificaciones`. Si cambias el SP, cambia también esa clase.

Tras editar el script, ejecutarlo de nuevo en SSMS (`CREATE OR ALTER`). La API no hay que recompilarla: solo reenvía el result set.

## Situación académica (`16`)

Parámetro: `@IdCursoOfertado`. Si el curso no existe, lanza `50110`.

```text
NotaFinal = SUM(PuntajeObtenido) = Parcial I + Parcial II
```

`ParcialI` / `ParcialII` agrupan por `Orden` del parcial (1 y 2). La Tarea de Curso se identifica por `TipoEvaluacion.Nombre = 'Tarea de Curso'` y **ya forma parte del Parcial II**; el SP la expone aparte (`PuntajeTareaCurso`) solo para la regla de aprobación.

Prioridad de `ResultadoAcademico`:

```text
EstadoInscripcion = 'Retirada'   → Retirado
EvaluacionesPendientes > 0       → En curso
NotaFinal < 60                   → Reprobado
PuntajeTareaCurso IS NULL        → En curso
PuntajeTareaCurso = 0            → No aprobado - Tarea de Curso
else                             → Aprobado
```

Ese `CASE` es la fuente de verdad en producción. En C# está replicado en `ClasificadorResultadoAcademico` para pruebas unitarias. Si cambias una regla aquí, cambia también el clasificador.

Caso útil para una futura prueba de integración: una inscripción con estado `Retirada` debe salir `Retirado` aunque tenga notas o evaluaciones pendientes.
