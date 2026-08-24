# Scripts de SQL Server

Se ejecutan **en orden numérico**, por carpeta.

```text
01_esquema/        crear base, esquemas y tablas
02_datos/          catálogos y datos reales
03_diagnostico/    validación en SSMS (la API no lo usa)
04_reportes/       procedimientos de consulta
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
```

La API no crea ni migra la base. Usa:

```text
DESKTOP-INGRIDT / GestionAcademica
```

Los reportes están escritos como `CREATE OR ALTER`. Si cambias un SP, vuelve a ejecutarlo en SSMS; no hace falta recrear la base.

Si el repositorio va a ser público, no subas `02_datos/05`–`10` (nombres, carnets, correos y notas reales).

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
