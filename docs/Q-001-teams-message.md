# Q-001 — Teams Message to Rodrigo Alarcón

**To:** Rodrigo Alarcón  
**Subject:** Why the `/Login` endpoint must be called before `/Item/Entry`

---

Hi Rodrigo,

Thank you for raising this question. I'd like to explain what is happening under the hood so the behaviour makes complete sense.

## What causes the error

The application uses an **in-memory cache** (a Singleton called `MemoryContext`) to hold working data during the lifetime of the server process. Think of it as a temporary whiteboard that starts completely empty every time the server boots up.

The **`POST /api/Login`** endpoint is responsible for filling that whiteboard. Specifically, it runs two database queries and stores their results in the cache under the keys `"warehouses"` and `"facilities"`:

```plaintext
Login  →  SELECT * FROM tnwrhse  →  saved in memory as "warehouses"
       →  SELECT * FROM tnfclty  →  saved in memory as "facilities"
```

The **`POST /Item/Entry`** endpoint (which increases an item's quantity) then reads the `"facilities"` data directly from that in-memory cache:

```plaintext
Item/Entry  →  reads "facilities" from memory  →  processes the entry
```

If `Login` has **never** been called, the cache is still empty — the key `"facilities"` simply does not exist yet. At that point the application throws a `KeyNotFoundException`, which is the error you are seeing.

## Why it works after calling Login

Once `Login` is called, `"facilities"` (and `"warehouses"`) are loaded into memory and remain available for the rest of the server session. Any subsequent call to `/Item/Entry` finds the data it needs and completes successfully.

## Summary

| Step | What happens |
|------|-------------|
| Server starts | In-memory cache is empty |
| `POST /api/Login` | Cache is populated with warehouses & facilities data |
| `POST /Item/Entry` (before Login) | Cache is empty → `KeyNotFoundException` → error |
| `POST /Item/Entry` (after Login) | Cache has the data → success |

The current design requires `Login` to be the first call because it acts as the **data-loading step** for the whole session. No code change is needed to address this; it is the intended workflow.

Please let me know if you have any further questions!

Best regards,  
Development Team
