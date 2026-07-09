# Vital Trek Platform — Backend

ASP.NET Core (.NET 10) modular monolith, DDD/CQRS via Cortex.Mediator, MySQL + EF Core.
Bounded contexts: `Iam`, `Profiles`, `Navigation`, `Monitoring`, `Iot`, `TourManagement`,
`Engagement`, `Support`, `Dashboard`, `Subscriptions`.

Swagger: `/swagger` (enabled in all environments, including production).

## Running locally

1. MySQL reachable at the connection string in `appsettings.Development.json`
   (`ConnectionStrings:DefaultConnection`). Migrations run automatically on startup
   (`context.Database.Migrate()` in `Program.cs`).
2. `dotnet run` from `NexumDevs.VitalTrek.Platform/`.

## Auth model (guest + authenticated mixed access)

All endpoints require a valid JWT **by default** (fallback authorization policy in
`Program.cs`). Only endpoints explicitly marked `[AllowAnonymous]` are public. A request
without a token to a protected endpoint gets a `401` with a JSON body so the frontend can
redirect to sign-in/sign-up:

```json
{ "status": 401, "title": "Authentication required", "detail": "Sign in or create an account to continue." }
```

### Public (no token required)

| Endpoint | Purpose |
|---|---|
| `POST /api/v1/authentication/sign-up` | Register |
| `POST /api/v1/authentication/sign-in` | Login, returns JWT |
| `GET /api/v1/tours` (optional `?agencyId=` or `?term=`) | Tour catalog / by agency / search — merged into one collection endpoint |
| `GET /api/v1/tours/{tourId}` | Tour detail |
| `GET /api/v1/expeditions` / `GET /api/v1/expeditions/{id}` | Expedition catalog |
| `GET /api/v1/subscriptions/mock-checkout/{sessionId}` and its `PATCH` | Mock payment gateway pages — not for frontend use |

### Protected (JWT required — everything else)

Notably: `POST /api/v1/tours/{tourId}/assignments` (booking/reservation), all of
`Profiles/*` (tourist/staff profiles, medical data, preferences), review submission and
loyalty endpoints under `Engagement/*`, and the new `Subscriptions/*` endpoints below.

### Subscriptions (new)

Payment gateway is currently a **self-contained mock** — no external provider, no
credentials needed. `POST checkout` returns a URL to our own `mock-checkout` page (instead of
a Stripe-hosted one); clicking "Pagar"/"Cancelar" there calls our own confirm/cancel endpoints,
which activate/fail the subscription exactly like a real gateway's webhook would. This was a
deliberate choice for today's deadline over setting up a real Stripe test account — swapping in
a real provider later only means implementing `IPaymentGatewayService` again and changing one
line in `Program.cs` (`AddScoped<IPaymentGatewayService, ...>`); no caller changes.

| Endpoint | Auth | Purpose |
|---|---|---|
| `POST /api/v1/subscriptions/checkout` | JWT | Body `{ "plan": "Monthly" \| "Annual" }` → `{ "checkoutUrl": "..." }`. Redirect the browser there. |
| `GET /api/v1/subscriptions/me` | JWT | Current user's subscription status |
| `PATCH /api/v1/subscriptions/me` | JWT | Body `{ "status": "Canceled" }` — cancels the active subscription |
| `GET /api/v1/subscriptions/mock-checkout/{sessionId}` | none | Mock hosted checkout page (HTML, Pagar/Cancelar buttons; they call the PATCH below via `fetch`) |
| `PATCH /api/v1/subscriptions/mock-checkout/{sessionId}` | none | Body `{ "outcome": "paid" \| "canceled" }` — settles the session; response has `{ "redirectUrl": "..." }` |

## Resource naming conventions

All routes follow the collection (`/things`) + element (`/things/{id}`) pattern, nested nouns
for parent/child relationships (`/expeditions/{id}/alerts`, `/tickets/{id}/replies`), and query
parameters instead of verb-like path segments for filtering (`/tours?agencyId=`). Actions that
change a resource's state use `PATCH` with a body describing the new state instead of a
verb-suffixed URL. Table of what changed today, for the frontend team migrating existing calls:

| Old route | New route |
|---|---|
| `GET /tours/agency/{agencyId}` | `GET /tours?agencyId={agencyId}` |
| `GET /tours/search?term=` | `GET /tours?term=` |
| `POST /tours/{id}/duplicate` | `POST /tours/{id}/copies` |
| `GET /alerts/expedition/{id}` | `GET /expeditions/{id}/alerts` |
| `PUT /alerts/{id}/acknowledge` | `PATCH /alerts/{id}` body `{ "status": "ACKNOWLEDGED", "userId": <int> }` |
| `PUT /alerts/{id}/dismiss` | `PATCH /alerts/{id}` body `{ "status": "DISMISSED" }` |
| `GET /location-readings/expedition/{id}` | `GET /expeditions/{id}/location-readings` |
| `GET /vital-sign-readings/expedition/{id}` | `GET /expeditions/{id}/vital-sign-readings` |
| `GET /binnacle-readings/expedition/{id}` | `GET /expeditions/{id}/binnacle-readings` |
| `GET /sensor-readings/device/{id}` | `GET /devices/{id}/sensor-readings` |
| `POST /subscriptions/me/cancel` | `PATCH /subscriptions/me` body `{ "status": "Canceled" }` |
| `POST /subscriptions/mock-checkout/{id}/confirm` \| `/cancel` | `PATCH /subscriptions/mock-checkout/{id}` body `{ "outcome": "paid" \| "canceled" }` |
| `GET /dashboard/admin/summary` | `GET /dashboard/summary` |
| `GET /dashboard/admin/alerts/distribution` | `GET /dashboard/alerts/distribution` |
| `GET /dashboard/admin/alerts/attention` | `GET /dashboard/alerts/pending` |
| `GET /dashboard/admin/expeditions/timeseries` | `GET /dashboard/expeditions/timeseries` |
| `GET /dashboard/admin/expeditions/active` | `GET /dashboard/expeditions/active` |
| `POST /support-ticket-replies` body incl. `ticketId` | `POST /tickets/{ticketId}/replies` body without `ticketId` |
| `GET /support-ticket-replies?ticketId=` | `GET /tickets/{ticketId}/replies` |

Everything else (`IoTDevicesController`, `SensorReadingsController`'s root `GET`, all of
`Engagement/*`, `Profiles/*`, `Support/SupportTicketsController`, `TourManagement/TourAssignmentsController`,
`Iam/*`) already followed the collection+element pattern and needed no changes.
`POST sign-in` / `POST sign-up` stay as-is — verb-shaped auth endpoints are a universally
accepted exception (matches Auth0, Firebase, GitHub OAuth, etc.), not a resource CRUD action.

## Environment variables (Railway)

| Variable | Notes |
|---|---|
| `ConnectionStrings__DefaultConnection` | MySQL connection string |
| `TokenSettings__Secret` | JWT signing secret |
| `Payments__SuccessUrl` / `Payments__CancelUrl` | Frontend routes the mock checkout redirects to after Pagar/Cancelar |

No payment-gateway credentials are required — the mock gateway has none. If/when a real
provider (Stripe, etc.) replaces it, that provider's own env vars (secret key, webhook secret)
would be added then.

## Known gaps / TODOs (left intentionally for today's deadline)

- `Engagement/ProgramController` and `ProfileController` (loyalty) take `agencyId`/`touristId`
  as path parameters instead of deriving them from the JWT — pre-existing, documented inline.
- Payment gateway is a mock, not a real processor — see the Subscriptions section above. The
  `IPaymentGatewayService` port is already gateway-agnostic so swapping it in later is a
  single-file change plus one `Program.cs` line.
- Subscription pricing is a hardcoded two-plan catalog (`PlanCatalog.cs`), not configurable.
- `Monitoring/Infrastructure/Services/ThresholdAnomalyDetectionService.cs`: `DetectRouteDeviationAsync`
  and `DetectCommunicationLossAsync` are still empty `TODO` stubs (never called from anywhere
  either). `DetectVitalAnomalyAsync` is fully implemented and now wired in — see the bug-fix
  notes below.

## Bug-fix pass (same session, after the resource-naming work)

Found and fixed while testing the naming changes, a few unrelated real bugs:

- **EF Core Guid-key footgun (the recurring one)**: several aggregates add a client-generated-Guid
  child entity (`Guid.NewGuid()` in its own constructor) into an *already-tracked* parent
  collection (`Ticket.Replies`, `Tour.Checkpoints`/`Assignments`, `TouristProfile.EmergencyContacts`).
  Without `.Property(x => x.Id).ValueGeneratedNever()`, EF's default Guid-key convention makes it
  ambiguous whether a non-default Id means "new" or "existing" — it emitted an `UPDATE` instead
  of an `INSERT` for the brand-new child, affecting 0 rows and throwing
  `DbUpdateConcurrencyException`. Fixed by adding `ValueGeneratedNever()` to all four affected
  entities' EF configuration (no migration needed, it's metadata-only).
- **IDOR on Support tickets**: `GetTicketById`, `UpdateTicket`, and both ticket-reply endpoints had
  no ownership check — any authenticated user could read/modify any other user's ticket. Now
  mirrors the check `GetTickets` already had (Tourist callers restricted to their own tickets).
- **Dashboard tourist summary had no `[Authorize]`** at all on that one action (every sibling
  action in the controller has one) — added.
- **`Weather` never persisted `ExpeditionId`** — every weather record was silently orphaned
  (`ExpeditionId = 0`) regardless of what the client sent. One-line constructor fix.
- **Reassigning a tourist after unassigning them threw an unhandled 500** (unique index on
  `(TourId, TouristId)` collided with the old cancelled row) — `Tour.AssignTourist` now reuses
  the existing cancelled row instead of inserting a new one.
- **Invalid `Difficulty` on tour creation threw an unhandled 500** (`Enum.Parse` uncaught) —
  now a clean `400 InvalidDifficulty`.
- **Alerts/LocationReadings/VitalSignReadings hardcoded a single status code** for every failure
  reason (e.g. a DB outage on alert acknowledgment returned `404 "not found"`) — now routed
  through a shared `MonitoringActionResultAssembler.ToActionResultFromResult` that maps each
  `MonitoringError` to its real status code.
- **`SensorReadings` accepted readings for a nonexistent device**, only failing via a generic DB
  FK-constraint error — now checks device existence first and returns a clean `404`.
- **Subscriptions**: `AlreadyActive` was defined but never thrown — a user could start a second
  checkout while already subscribed. The mock-checkout settle endpoint also had no idempotency
  guard — replaying it could silently downgrade an already-`Active` subscription to
  `PaymentFailed`. Both fixed (checked before checkout; both transitions are now no-ops unless
  the subscription is still `PendingPayment`).
- **Points could be double-awarded**: `SourceId` (the only idempotency key for a points event)
  was optional; the duplicate check silently skipped whenever it was omitted. Now `[Required]`.
- **Race condition on reward stock and points balance**: two concurrent redemptions could both
  read the same balance/stock, both pass validation, and both commit — overselling stock or
  overspending points. `Reward.Stock` and `GamificationProfile.TotalPoints` are now EF
  concurrency tokens; a losing concurrent request gets a clean `409 ConcurrentModification`
  instead of silently corrupting the balance.
- **Critical vital-sign readings never raised an alert**: `ThresholdAnomalyDetectionService.DetectVitalAnomalyAsync`
  was fully implemented but never called from anywhere. Now wired into
  `VitalSignReadingCommandService` — a critical heart rate/SpO2/temperature reading raises a
  `VITAL_SIGNS_CRITICAL` alert automatically.

## Migration history note

The migration history was reset once (`InitialMigration` regenerated from scratch) because
most bounded contexts (`Iam`, `Profiles`, `Support`, most of `Engagement`) had been coded and
wired into `AppDbContext` without ever having a real EF migration generated for them — the
committed model snapshot had been hand-edited to match instead. If you have a pre-existing
local or Railway database from before this fix, drop and recreate it (or drop all tables)
before deploying, since the old partial migration history no longer matches.
