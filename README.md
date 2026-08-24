# Gestión Académica

Sistema para docentes: asistencia, evaluación y reportes.

Arquitectura: **monolito modular** en ASP.NET Core 6. Un solo proyecto de API, tres capas por módulo (`Api` → `Aplicacion` → `Datos`) y una base SQL Server. Los porcentajes y el resultado académico **no se persisten**: se calculan al consultar.

```text
GestionAcademica.sln
├── database/                         ← scripts SQL (esquema, datos, reportes)
├── src/Api/                          ← Web API + Swagger
├── src/Web/                          ← PWA (pendiente)
└── tests/GestionAcademica.Api.Tests/ ← xUnit + Moq
```

Hoy se usa Swagger. No hay autenticación: el esquema `sec` queda para más adelante.

## Requisitos

- .NET SDK 6
- SQL Server (`MSSQLSERVER`) en ejecución
- Base `GestionAcademica` creada con los scripts de `database/` (la API no migra)

Cadena de conexión: `src/Api/appsettings.json` → `localhost` / `GestionAcademica`, autenticación de Windows. Cambia `localhost` por el nombre de tu instancia de SQL Server si hace falta.

## Arranque

```powershell
dotnet run --project src/Api
```

Swagger: `https://localhost:7040/swagger`

Si `GET /api/salud` muestra 36 estudiantes, la API está leyendo los datos de ejemplo.

```powershell
dotnet test tests/GestionAcademica.Api.Tests/GestionAcademica.Api.Tests.csproj
```

## API

El hub es `CursoOfertado`. La inscripción (`InscripcionCurso`) une estudiante y curso (`EsOficial`, estado, `FechaRetiro`).

| Área | Método | Ruta |
|---|---|---|
| Salud | GET | `/api/salud` |
| Catálogos | GET | `/api/catalogos` |
| Académico | GET | `/api/academico` |
| Cursos | GET | `/api/cursos-ofertados` |
| Clases | GET | `/api/cursos-ofertados/{id}/clases` |
| Asistencia | GET, PUT | `/api/clases/{id}/asistencias` |
| Parciales | GET | `/api/cursos-ofertados/{id}/parciales` |
| Evaluaciones del parcial | GET, POST | `/api/parciales/{id}/evaluaciones` |
| Evaluaciones | GET | `/api/evaluaciones` |
| Evaluación | GET, PUT | `/api/evaluaciones/{id}` |
| Calificaciones | GET, PUT | `/api/evaluaciones/{id}/calificaciones` |
| Reportes | GET | `/api/cursos-ofertados/{id}/reportes/...` |

Reportes (todos delegan a stored procedures; ver `database/README.md`):

```text
.../reportes/asistencia-estudiante
.../reportes/asistencia-sesion
.../reportes/asistencia-semanal
.../reportes/calificaciones
.../reportes/situacion-academica
```

No hay `DELETE` de evaluaciones. La anulación lógica (`Activo`) está pendiente.

### Errores (contrato para React)

Los fallos no usan `{ "mensaje": "..." }`. Un middleware global (`ManejadorExcepcionesMiddleware`) y la validación de `[ApiController]` responden **siempre** `application/problem+json` (RFC 7807):

```json
{
  "type": "https://httpstatuses.com/400",
  "title": "Regla de negocio",
  "status": 400,
  "detail": "La suma de puntajes (65.00) supera el máximo del parcial (50.00).",
  "instance": "/api/evaluaciones/12",
  "traceId": "..."
}
```

En el front lee **`detail`**.

| Excepción / caso | HTTP | `title` |
|---|---|---|
| `ReglaNegocioException` | 400 | Regla de negocio |
| Modelo inválido (`[ApiController]`) | 400 | Datos inválidos |
| `KeyNotFoundException` | 404 | Recurso no encontrado |
| `SqlException` / `DbException` | 503 | Servicio no disponible |
| Cualquier otra | 500 | Error interno |

La validación 400 incluye además `errors` (campo → mensajes). `GET /api/salud` en éxito sigue siendo `SaludDto`; si no hay SQL, también 503 con `ProblemDetails` (`detail` explicando la caída).

Los controladores no llevan `try/catch` de HTTP: lanzan la excepción y el middleware la traduce.

### Asistencia

- Ausente ≠ sin registro (fila inexistente) ≠ retirado.
- El PUT hace upsert. No inventa ausencias.
- Rechaza retirados e inscripciones de otro curso (`ReglaNegocioException` → 400).
- Después de `FechaRetiro` no se espera asistencia (`Fecha < FechaRetiro`).

### Evaluación y calificaciones

- Semestre = 100 puntos: Parcial I = 50, Parcial II = 50.
- `null` en una calificación = sin fila = sin evaluar. `0` es un cero.
- PUT de evaluación: la suma de puntajes no puede pasar el tope del parcial; el orden es único; no se baja `PuntajeMaximo` por debajo de notas ya cargadas.

### Situación académica

Pregunta: *¿cómo va cada estudiante?*

En **producción** el GET ejecuta `eval.sp_ReporteSituacionAcademica`. El `CASE` del SP es la fuente de verdad.

En **pruebas unitarias** las mismas reglas viven en `ClasificadorResultadoAcademico` (C#), para no depender de SQL Server. Si se cambia el `CASE` del SP, hay que actualizar el clasificador (y al revés). Más adelante conviene una prueba de integración contra la API + la base.

Nota del semestre:

```text
NotaFinal = Parcial I + Parcial II
          = máximo 100
```

La **Tarea de Curso** (tipo `Tarea de Curso`, suele valer 20) **ya está dentro del Parcial II**. No se suma otra vez. Solo sirve para la condición obligatoria de aprobación.

Prioridad del resultado:

```text
Retirada                         → Retirado
Evaluaciones pendientes > 0      → En curso
NotaFinal < 60                   → Reprobado
Tarea de Curso sin calificar     → En curso
Tarea de Curso = 0               → No aprobado - Tarea de Curso
cualquier otro caso              → Aprobado
```

Aprobar exige nota ≥ 60 **y** Tarea de Curso calificada con puntaje > 0.

## Estado

| Bloque | Estado |
|---|---|
| Asistencia | Cerrado |
| Evaluación y calificaciones | Cerrado (falta anulación lógica) |
| Reportes | Cerrado a nivel funcional |
| Situación académica | Cerrado a nivel funcional |
| Contrato global de errores (`ProblemDetails`) | Cerrado |
| Revisión Swagger / `201 Created` en POST evaluaciones | Pendiente |
| Anulación lógica de evaluaciones | Pendiente |
| Prueba de integración API + SP | Pendiente |
| Autenticación / `sec` | Futura |
| PWA (`src/Web`) | Futura |

No se agregan más endpoints académicos. Siguiente paso: revisión de controladores/Swagger (incluido devolver `201` al crear una evaluación) y después React.
