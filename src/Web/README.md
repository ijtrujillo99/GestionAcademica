# Front (PWA)

Pendiente. Aquí irá la aplicación web (React + Vite) cuando la API esté revisada y estable.

La API ya deja CORS abierto para `http://localhost:5173`.

No empezar pantallas todavía. El siguiente paso del proyecto es la revisión técnica de `src/Api` (rutas, DTOs, códigos HTTP, errores, Swagger). Después de eso sí tiene sentido montar el front contra:

```text
https://localhost:7040
```

El contrato de recursos está en el `README.md` de la raíz.

Los errores de la API son siempre `ProblemDetails`. En TypeScript conviene leer **`detail`**:

```ts
type ErrorApi = {
  type: string;
  title: string;
  status: number;
  detail: string;
};

// fetch / axios: si !response.ok → JSON.parse(body) como ErrorApi
// mostrarError(error.detail)
```

Validación 400 puede traer además `errors` (diccionario por campo). No esperes `{ mensaje }`.
