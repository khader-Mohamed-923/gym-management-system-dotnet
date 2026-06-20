---
name: kinetic-engine-gym
description: >
  Use this skill when building the Gym Management System UI using the Kinetic Engine
  design language. Trigger on any request to build a view, page, component, dashboard,
  or layout for the gym system. Also trigger when the user says "use the design system",
  "follow the design doc", or "industrial brutalist UI". Applies to HTML, CSS, Razor
  views (.cshtml), and all web frontend output. Always read this skill before writing
  a single line of code or CSS.
---

# Kinetic Engine — Gym Management System
## Design Skill · Industrial Brutalist · ASP.NET MVC Razor Views

**Creative North Star:** *"Iron Authority"* — heavy, dependable, structurally exposed, and built
for operators who need data at a glance. Inspired by locker-room signage, training logbooks,
and industrial wayfinding. Every view should feel like the inside of a serious training facility:
no decoration for its own sake, only structure that earns its place.

---

## Step 1 — Design Thinking Before Any Code

Before writing markup or styles, answer these four questions:

1. **What gym data is being displayed?** Members, check-ins, plan tiers, expiry dates, attendance counts?
2. **What is the primary action?** One CTA dominates. It gets `primary_container` + hard shadow.
   Examples: "Add Member", "Check In", "Renew Plan".
3. **What is the layout structure?** Identify the grid blocks. Every zone is a visible, stacked slab.
4. **Where is the typographic hierarchy?** The largest label (e.g. member count "248") must sit
   adjacent to the smallest (e.g. "ACTIVE MEMBERS" caption). Visual tension is mandatory.

**Commit to the aesthetic, then code it. Do not start generic and decorate — start with the grid.**

---

## Step 2 — Design Tokens (CSS Variables)

Always declare at `:root` in `_Layout.cshtml` or `site.css`. Never hardcode hex values inline.

```css
:root {
  /* Primary — Neon Signal Yellow (the gym's energy color) */
  --primary: #5b6400;
  --on-primary: #ffffff;
  --primary-container: #eaff00;       /* The neon yellow — used for the #1 CTA only */
  --on-primary-container: #1a1d00;

  /* Surface Hierarchy — paper-on-concrete depth */
  --surface: #f9f9f9;
  --surface-container-lowest: #ffffff;
  --surface-container-low: #f3f3f3;
  --surface-container: #eeeeee;       /* Page background — the "concrete floor" */
  --surface-container-high: #e8e8e8;
  --surface-container-highest: #e2e2e2;

  /* On-Surface — all text and strokes */
  --on-surface: #1b1b1b;
  --on-surface-variant: #44483d;
  --outline: #75796c;
  --outline-variant: rgba(27, 27, 27, 0.10); /* ghost border — manifest list ONLY */

  /* State / Status */
  --error: #ba1a1a;
  --on-error: #ffffff;
  --error-container: #ffdad6;

  /* Gym-specific status tokens */
  --status-active-bg:    #eaff00;     /* primary-container — active membership */
  --status-active-fg:    #1a1d00;
  --status-expiring-bg:  #fff3cd;     /* amber-tinted surface */
  --status-expiring-fg:  #6b4c00;
  --status-expired-bg:   var(--error-container);
  --status-expired-fg:   var(--error);
  --status-frozen-bg:    #d6eaff;
  --status-frozen-fg:    #003366;

  /* Typography */
  --font-display: 'Public Sans', sans-serif;
  --font-data:    'Space Grotesk', monospace;

  /* Spacing scale */
  --space-xs:  8px;
  --space-sm:  12px;
  --space-md:  16px;
  --space-lg:  24px;
  --space-xl:  32px;
  --space-2xl: 48px;
  --space-3xl: 64px;

  /* Strokes — minimum 2px, always */
  --stroke-structural: 2px solid var(--on-surface);
  --stroke-heavy:      4px solid var(--on-surface);

  /* Hard Shadows — blur always 0px */
  --shadow-hard:         4px 4px 0px 0px var(--on-surface-variant);
  --shadow-hard-primary: 4px 4px 0px 0px var(--primary);

  /* Radius — zero. Always. No exceptions. */
  --radius: 0px;
}
```

> **Rule:** No token = no use. If a color or size isn't in this map, it has no place in the UI.

---

## Step 3 — Typography

```html
<!-- Add to <head> in _Layout.cshtml -->
<link rel="preconnect" href="https://fonts.googleapis.com">
<link href="https://fonts.googleapis.com/css2?family=Public+Sans:wght@400;700;900&family=Space+Grotesk:wght@400;500;700&display=swap" rel="stylesheet">
```

### Type Scale

| Token | Font | Size | Weight | Letter-Spacing | Gym Usage |
|---|---|---|---|---|---|
| `display-lg` | Public Sans | 3.5rem | 900 | -0.02em | Page titles: "MEMBERS", "CHECK-INS" |
| `display-md` | Public Sans | 2.5rem | 900 | -0.02em | Hero KPI numbers: "248", "67" |
| `headline-lg` | Public Sans | 2rem | 700 | -0.01em | Section headers, member name on details |
| `headline-md` | Public Sans | 1.5rem | 700 | -0.01em | Plan names, card titles |
| `title-md` | Public Sans | 1.125rem | 700 | 0 | Sub-section labels |
| `body-md` | Public Sans | 1rem | 400 | 0 | Notes, descriptions |
| `label-md` | Space Grotesk | 0.875rem | 500 | 0.02em | Field labels, column headers |
| `label-sm` | Space Grotesk | 0.75rem | 400 | 0.04em | Timestamps, member IDs, join dates |
| `data-mono` | Space Grotesk | 0.875rem | 700 | 0.04em | Member ID codes, check-in counts |

```css
.display-lg  { font-family: var(--font-display); font-size: 3.5rem; font-weight: 900; letter-spacing: -0.02em; line-height: 1; }
.display-md  { font-family: var(--font-display); font-size: 2.5rem; font-weight: 900; letter-spacing: -0.02em; line-height: 1.1; }
.headline-lg { font-family: var(--font-display); font-size: 2rem;   font-weight: 700; letter-spacing: -0.01em; }
.headline-md { font-family: var(--font-display); font-size: 1.5rem; font-weight: 700; letter-spacing: -0.01em; }
.label-md    { font-family: var(--font-data); font-size: 0.875rem; font-weight: 500; letter-spacing: 0.02em; }
.label-sm    { font-family: var(--font-data); font-size: 0.75rem;  font-weight: 400; letter-spacing: 0.04em; }
.data-mono   { font-family: var(--font-data); font-size: 0.875rem; font-weight: 700; letter-spacing: 0.04em; }
```

### Typography Rules (Non-Negotiable)

- **Contrast is mandatory.** A `display-md` KPI number ("248") must sit directly above a `label-sm` caption ("ACTIVE MEMBERS"). This pairing creates the visual tension that defines the system.
- **Left-align everything.** No center-alignment on member lists, forms, or data tables. Center is only allowed on isolated CTAs or single-line hero elements.
- **Headlines can overflow.** If a heading like "MEMBERSHIP MANAGEMENT" feels too big, that is correct. Intentional editorial overscale is a feature, not a bug.
- **Uppercase labels** always use `letter-spacing: 0.08em` — mimics locker-room signage.
- **Negative letter-spacing on display sizes is required** (`-0.02em`). It mimics industrial stenciling.

---

## Step 4 — Layout & Grid

```css
body {
  background-color: var(--surface-container); /* The concrete floor */
  margin: 0;
  padding: 0;
  font-family: var(--font-display);
}

/* Global zero-radius mandate */
*, *::before, *::after {
  border-radius: 0 !important;
}

/* Primary layout wrapper */
.layout-root {
  display: grid;
  grid-template-columns: 240px 1fr; /* sidebar + content */
  min-height: 100vh;
  gap: 0; /* Blocks abut — spacing is internal padding, not gap */
}

/* Page panel */
.panel {
  background-color: var(--surface);
  padding: var(--space-xl);
}

/* White card (paper on concrete) */
.card {
  background-color: var(--surface-container-lowest);
  padding: var(--space-xl);
}
```

### Spacing Rules

- **Section separation:** `--space-2xl` (48px) or `--space-3xl` (64px) between major sections. Extreme whitespace signals industrial breathing room.
- **Card internal padding:** Minimum `--space-lg` (24px), preferred `--space-xl` (32px).
- **Inline element gaps:** `--space-sm` (12px) or `--space-md` (16px).
- **No `gap` between structural blocks.** Zone separation = surface color shift, not space.

---

## Step 5 — The No-Thin-Line Rule

**1px borders are a design error in this system.**

### How to Separate Zones (Priority Order)

1. **Surface Shift (preferred):** Change `background-color` using the surface scale. A white card on a grey slab needs no border.
2. **Structural Stroke:** Exactly `2px solid var(--on-surface)` or `4px solid var(--on-surface)`. Nothing thinner.
3. **Ghost Border (last resort, data lists only):** `1px solid var(--outline-variant)` inside the Roster List component only.

```css
/* DO */
.structural-divider { border-bottom: var(--stroke-structural); }
.heavy-divider      { border-bottom: var(--stroke-heavy); }

/* DON'T */
.bad { border: 1px solid #ccc; }  /* ❌ Too thin */
.bad { border-radius: 4px; }      /* ❌ Any radius forbidden */
```

---

## Step 6 — Elevation & Depth

### Surface Layering

```
Page canvas:   --surface-container        (#eeeeee)  ← concrete
Panels:        --surface                  (#f9f9f9)  ← slab
Cards:         --surface-container-lowest (#ffffff)  ← paper
Inputs:        --surface-container-highest(#e2e2e2)  ← form well
```

### Hard Shadows (Floating Elements Only)

Use **only** on: primary CTAs, active dropdowns, open modals, FABs.

```css
.floating { box-shadow: var(--shadow-hard); }          /* 4px 4px 0 var(--on-surface-variant) */
.primary-cta { box-shadow: var(--shadow-hard-primary); } /* 4px 4px 0 var(--primary) */
```

Rules: `blur: 0px` always. Offset always `4px 4px`. Only `--on-surface-variant` or `--primary` as shadow color.

### Glassmorphism (Modals & Top-Nav Only)

```css
.overlay-nav, .modal-overlay {
  background-color: rgba(249, 249, 249, 0.80);
  backdrop-filter: blur(20px);
  -webkit-backdrop-filter: blur(20px);
}
```

> This is the **only** place blurred effects are allowed. Use sparingly.

---

## Step 7 — Components

### 7.1 Buttons

```css
/* Primary CTA — neon yellow, hard shadow. One per view. */
.btn-primary {
  font-family: var(--font-display);
  font-size: 0.875rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  background-color: var(--primary-container);  /* #eaff00 */
  color: var(--on-primary-container);
  border: var(--stroke-structural);
  box-shadow: var(--shadow-hard-primary);
  padding: var(--space-sm) var(--space-lg);
  border-radius: 0;
  cursor: pointer;
  transition: transform 0.1s ease, box-shadow 0.1s ease;
}
.btn-primary:hover  { transform: translate(-1px, -1px); box-shadow: 5px 5px 0px 0px var(--primary); }
.btn-primary:active { transform: translate(2px, 2px);   box-shadow: 2px 2px 0px 0px var(--primary); }

/* Secondary — structural, no fill */
.btn-secondary {
  font-family: var(--font-display);
  font-size: 0.875rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  background-color: var(--surface);
  color: var(--on-surface);
  border: var(--stroke-structural);
  box-shadow: none;
  padding: var(--space-sm) var(--space-lg);
  border-radius: 0;
  cursor: pointer;
  transition: background-color 0.1s ease;
}
.btn-secondary:hover { background-color: var(--surface-container-low); }

/* Danger — for delete / remove actions */
.btn-danger {
  font-family: var(--font-display);
  font-size: 0.875rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  background-color: var(--error-container);
  color: var(--error);
  border: 2px solid var(--error);
  box-shadow: 4px 4px 0px 0px var(--error);
  padding: var(--space-sm) var(--space-lg);
  border-radius: 0;
  cursor: pointer;
  transition: transform 0.1s ease, box-shadow 0.1s ease;
}
.btn-danger:hover  { transform: translate(-1px, -1px); box-shadow: 5px 5px 0px 0px var(--error); }
.btn-danger:active { transform: translate(2px, 2px);   box-shadow: 2px 2px 0px 0px var(--error); }

/* Check-in — positive action, secondary weight */
.btn-checkin {
  font-family: var(--font-display);
  font-size: 0.875rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  background-color: var(--primary-container);
  color: var(--on-primary-container);
  border: var(--stroke-structural);
  box-shadow: var(--shadow-hard);
  padding: var(--space-xs) var(--space-md);
  border-radius: 0;
  cursor: pointer;
}
```

---

### 7.2 Input Fields

```css
.field-group {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.field-label {
  font-family: var(--font-data);
  font-size: 0.75rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: var(--on-surface-variant);
}

.field-input {
  font-family: var(--font-data);
  font-size: 0.875rem;
  background-color: var(--surface-container-highest);
  color: var(--on-surface);
  border: none;
  border-bottom: var(--stroke-structural);
  padding: var(--space-sm) var(--space-md);
  border-radius: 0;
  outline: none;
  transition: border-bottom-color 0.15s ease;
}
.field-input:focus {
  border-bottom: 4px solid var(--primary-container);
}
.field-input::placeholder {
  color: var(--outline);
  font-style: normal;
}

select.field-input {
  appearance: none;
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='12' fill='%2344483d' viewBox='0 0 16 16'%3E%3Cpath d='M7.247 11.14L2.451 5.658C1.885 5.013 2.345 4 3.204 4h9.592a1 1 0 0 1 .753 1.659l-4.796 5.48a1 1 0 0 1-1.506 0z'/%3E%3C/svg%3E");
  background-repeat: no-repeat;
  background-position: right 12px center;
  padding-right: 2rem;
  cursor: pointer;
}
```

Form section example (Razor):
```html
<div class="card" style="max-width: 640px;">
  <h1 class="display-lg" style="color: var(--on-surface); margin-bottom: var(--space-2xl);">
    ADD MEMBER
  </h1>
  <div style="display: grid; grid-template-columns: 1fr 1fr; gap: var(--space-lg);">
    <div class="field-group">
      <label class="field-label" asp-for="FullName">Full Name</label>
      <input class="field-input" asp-for="FullName" placeholder="e.g. Ahmed Hassan" />
    </div>
    <div class="field-group">
      <label class="field-label" asp-for="Phone">Phone</label>
      <input class="field-input" asp-for="Phone" placeholder="+20 1XX XXX XXXX" />
    </div>
    <div class="field-group">
      <label class="field-label" asp-for="Email">Email</label>
      <input class="field-input" asp-for="Email" type="email" />
    </div>
    <div class="field-group">
      <label class="field-label" asp-for="PlanId">Membership Plan</label>
      <select class="field-input" asp-for="PlanId" asp-items="ViewBag.Plans"></select>
    </div>
    <div class="field-group">
      <label class="field-label" asp-for="StartDate">Start Date</label>
      <input class="field-input" asp-for="StartDate" type="date" />
    </div>
    <div class="field-group">
      <label class="field-label" asp-for="EndDate">End Date</label>
      <input class="field-input" asp-for="EndDate" type="date" />
    </div>
  </div>
  <!-- Full-width notes -->
  <div class="field-group" style="margin-top: var(--space-lg);">
    <label class="field-label" asp-for="Notes">Notes</label>
    <textarea class="field-input" asp-for="Notes" rows="3"></textarea>
  </div>
  <!-- Actions -->
  <div style="display: flex; gap: var(--space-md); margin-top: var(--space-2xl); border-top: var(--stroke-structural); padding-top: var(--space-lg);">
    <button type="submit" class="btn-primary">Save Member</button>
    <a asp-action="Index" class="btn-secondary">Cancel</a>
  </div>
</div>
```

---

### 7.3 Cards

```css
.card {
  background-color: var(--surface-container-lowest);
  padding: var(--space-xl);
  border-radius: 0;
}

.card-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: var(--space-lg);
  border-bottom: var(--stroke-heavy);
  padding-bottom: var(--space-md);
}

.card-section {
  background-color: var(--surface-container-high);
  padding: var(--space-md) var(--space-lg);
  margin-top: var(--space-md);
  /* Surface shift replaces a divider line */
}

/* KPI stat card */
.stat-card {
  background-color: var(--surface-container-lowest);
  padding: var(--space-xl);
  border-top: var(--stroke-heavy);
}
```

KPI stat card example:
```html
<div class="stat-card">
  <p class="label-sm" style="color: var(--on-surface-variant); text-transform: uppercase; letter-spacing: 0.08em; margin-bottom: var(--space-xs);">
    ACTIVE MEMBERS
  </p>
  <p class="display-md" style="color: var(--on-surface); margin: 0;">248</p>
</div>
```

> **Critical:** The `display-md` number and `label-sm` caption must be adjacent. This pair IS the design.

---

### 7.4 Membership Status Chip

```css
.status-chip {
  display: inline-flex;
  align-items: center;
  gap: var(--space-xs);
  font-family: var(--font-data);
  font-size: 0.75rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  padding: 4px var(--space-md);
  border-radius: 0;
}

.status-chip--active   { background-color: var(--status-active-bg);   color: var(--status-active-fg);   border: 2px solid var(--primary); }
.status-chip--expiring { background-color: var(--status-expiring-bg); color: var(--status-expiring-fg); border: 2px solid #c97a00; }
.status-chip--expired  { background-color: var(--status-expired-bg);  color: var(--status-expired-fg);  border: 2px solid var(--error); }
.status-chip--frozen   { background-color: var(--status-frozen-bg);   color: var(--status-frozen-fg);   border: 2px solid #003366; }
.status-chip--new      { background-color: var(--on-surface);         color: var(--surface);            border: 2px solid var(--on-surface); }
```

Usage:
```html
<span class="status-chip status-chip--active">Active</span>
<span class="status-chip status-chip--expiring">Expiring Soon</span>
<span class="status-chip status-chip--expired">Expired</span>
<span class="status-chip status-chip--frozen">Frozen</span>
<span class="status-chip status-chip--new">New Member</span>
```

Razor helper:
```csharp
@functions {
  string StatusChipClass(DateTime endDate, bool isFrozen) {
    if (isFrozen) return "status-chip--frozen";
    var days = (endDate - DateTime.Now).TotalDays;
    return days switch {
      < 0  => "status-chip--expired",
      < 7  => "status-chip--expiring",
      _    => "status-chip--active"
    };
  }
  string StatusChipLabel(DateTime endDate, bool isFrozen) {
    if (isFrozen) return "Frozen";
    var days = (endDate - DateTime.Now).TotalDays;
    return days switch {
      < 0  => "Expired",
      < 7  => "Expiring Soon",
      _    => "Active"
    };
  }
}
```

---

### 7.5 Plan Tier Chip

```css
.plan-chip {
  display: inline-block;
  font-family: var(--font-data);
  font-size: 0.72rem;
  font-weight: 700;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  padding: 3px 10px;
  border-radius: 0;
  border: var(--stroke-structural);
}

.plan-chip--basic    { background-color: var(--surface-container-high); color: var(--on-surface-variant); }
.plan-chip--standard { background-color: var(--surface-container-lowest); color: var(--on-surface); border-color: var(--on-surface); }
.plan-chip--premium  { background-color: var(--on-surface-variant); color: var(--surface); }
.plan-chip--elite    { background-color: var(--primary-container); color: var(--on-primary-container); border-color: var(--primary); }
```

Usage:
```html
<span class="plan-chip plan-chip--elite">Elite</span>
<span class="plan-chip plan-chip--premium">Premium</span>
<span class="plan-chip plan-chip--standard">Standard</span>
<span class="plan-chip plan-chip--basic">Basic</span>
```

---

### 7.6 The Roster List (Member Table)

The gym's version of the Kinetic Engine Manifest List — used for Members Index and Check-ins.

```css
.roster-list {
  display: flex;
  flex-direction: column;
  background-color: var(--surface-container-lowest);
}

.roster-header {
  display: grid;
  grid-template-columns: 48px 1fr 140px 120px 130px 100px;
  gap: var(--space-lg);
  padding: var(--space-sm) var(--space-xl);
  background-color: var(--surface-container-high);
  border-bottom: var(--stroke-heavy);
}

.roster-header-cell {
  font-family: var(--font-data);
  font-size: 0.72rem;
  font-weight: 700;
  letter-spacing: 0.08em;
  text-transform: uppercase;
  color: var(--on-surface-variant);
}

.roster-row {
  display: grid;
  grid-template-columns: 48px 1fr 140px 120px 130px 100px;
  gap: var(--space-lg);
  padding: var(--space-md) var(--space-xl);
  align-items: center;
}

.roster-row + .roster-row {
  border-top: 1px solid var(--outline-variant); /* Ghost border — only here */
}

.roster-row:hover {
  background-color: var(--surface-container-low);
}

/* Member ID — monospace */
.member-id {
  font-family: var(--font-data);
  font-size: 0.8rem;
  font-weight: 700;
  letter-spacing: 0.04em;
  color: var(--on-surface);
}

/* Member name */
.member-name {
  font-family: var(--font-display);
  font-size: 0.95rem;
  font-weight: 700;
  color: var(--on-surface);
  line-height: 1.2;
}

/* Sub-text (email, join date) */
.member-meta {
  font-family: var(--font-data);
  font-size: 0.75rem;
  font-weight: 400;
  color: var(--on-surface-variant);
  letter-spacing: 0.02em;
}

/* Action links */
.row-action {
  font-family: var(--font-data);
  font-size: 0.75rem;
  font-weight: 700;
  letter-spacing: 0.06em;
  text-transform: uppercase;
  color: var(--on-surface-variant);
  text-decoration: none;
  padding: 2px 6px;
  border-bottom: 2px solid transparent;
  transition: color 0.1s, border-color 0.1s;
}
.row-action:hover {
  color: var(--on-surface);
  border-bottom-color: var(--on-surface);
}
.row-action--danger:hover {
  color: var(--error);
  border-bottom-color: var(--error);
}
```

Roster row HTML (Razor):
```html
<div class="roster-row">
  <div>
    <!-- Initials block -->
    <div style="width:36px; height:36px; background-color:var(--surface-container-high);
                border: var(--stroke-structural); display:flex; align-items:center;
                justify-content:center; font-family:var(--font-data); font-size:0.75rem;
                font-weight:700; color:var(--on-surface);">
      @MemberInitials(member.FullName)
    </div>
  </div>
  <div>
    <div class="member-name">@member.FullName</div>
    <div class="member-meta">@member.Email</div>
  </div>
  <div><span class="plan-chip plan-chip--@member.Plan.Name.ToLower()">@member.Plan.Name</span></div>
  <div><span class="status-chip @StatusChipClass(member.EndDate, member.IsFrozen)">@StatusChipLabel(member.EndDate, member.IsFrozen)</span></div>
  <div class="member-meta">@member.EndDate.ToString("dd MMM yyyy")</div>
  <div style="display:flex; gap:var(--space-md);">
    <a asp-action="Details" asp-route-id="@member.Id" class="row-action">View</a>
    <a asp-action="Edit"    asp-route-id="@member.Id" class="row-action">Edit</a>
    <a asp-action="Delete"  asp-route-id="@member.Id" class="row-action row-action--danger">Delete</a>
  </div>
</div>
```

---

### 7.7 Sidebar Navigation

```css
.sidebar {
  background-color: var(--on-surface);   /* Dark slab */
  padding: 0;
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.sidebar-logo {
  padding: var(--space-xl);
  border-bottom: var(--stroke-heavy);
  border-bottom-color: rgba(255,255,255,0.15);
}

.sidebar-logo-text {
  font-family: var(--font-display);
  font-size: 1.5rem;
  font-weight: 900;
  letter-spacing: -0.01em;
  color: var(--primary-container); /* Neon yellow on dark */
}

.sidebar-section-label {
  font-family: var(--font-data);
  font-size: 0.65rem;
  font-weight: 700;
  letter-spacing: 0.12em;
  text-transform: uppercase;
  color: rgba(255,255,255,0.3);
  padding: var(--space-lg) var(--space-xl) var(--space-xs);
}

.sidebar-link {
  display: flex;
  align-items: center;
  gap: var(--space-md);
  padding: var(--space-sm) var(--space-xl);
  font-family: var(--font-display);
  font-size: 0.875rem;
  font-weight: 700;
  letter-spacing: 0.04em;
  text-transform: uppercase;
  color: rgba(255, 255, 255, 0.6);
  text-decoration: none;
  transition: color 0.15s, background-color 0.15s;
}

.sidebar-link:hover {
  color: var(--surface);
  background-color: rgba(255,255,255,0.07);
}

.sidebar-link--active {
  color: var(--on-primary-container);
  background-color: var(--primary-container);  /* Neon yellow active state */
  border-left: var(--stroke-heavy);
  border-left-color: var(--primary);
}
```

Sidebar HTML:
```html
<aside class="sidebar">
  <div class="sidebar-logo">
    <span class="sidebar-logo-text">IRONCORE</span>
  </div>
  <p class="sidebar-section-label">Management</p>
  <a asp-controller="Home"     asp-action="Index"  class="sidebar-link">Dashboard</a>
  <a asp-controller="Members"  asp-action="Index"  class="sidebar-link sidebar-link--active">Members</a>
  <a asp-controller="Plans"    asp-action="Index"  class="sidebar-link">Plans</a>
  <a asp-controller="CheckIns" asp-action="Index"  class="sidebar-link">Check-ins</a>
  <p class="sidebar-section-label">Settings</p>
  <a asp-controller="Staff"    asp-action="Index"  class="sidebar-link">Staff</a>
</aside>
```

---

### 7.8 Signature Textures & Details

1. **Hero Gradient** — apply to large header blocks only:
   ```css
   .hero-header {
     background: linear-gradient(135deg, var(--primary) 0%, var(--primary-container) 100%);
     padding: var(--space-2xl) var(--space-xl);
   }
   ```

2. **Watermark Typography** — used in hero zones as a background element:
   ```css
   .hero-watermark {
     font-family: var(--font-display);
     font-size: clamp(4rem, 15vw, 12rem);
     font-weight: 900;
     color: transparent;
     -webkit-text-stroke: 2px var(--outline);
     pointer-events: none;
     position: absolute;
     opacity: 0.12;
     letter-spacing: -0.03em;
     user-select: none;
   }
   ```
   Example in dashboard hero: "MEMBERS" as a large watermark behind the KPI numbers.

3. **Heavy Border Accent** on section headers:
   ```css
   .section-header { border-top: var(--stroke-heavy); padding-top: var(--space-lg); }
   ```

---

## Step 8 — Page-by-Page Layout Guide

### 8.1 Dashboard (`Home/Index.cshtml`)

```
┌──────────────────────────────────────────────────────────────────┐
│  SIDEBAR (dark slab)  │  CONTENT PANEL (surface)                 │
│                       │                                          │
│  IRONCORE             │  [HERO BLOCK — hero-gradient]            │
│  ─────────────────    │   IRONCORE GYM          ← watermark bg  │
│  Dashboard   ← active │   Good morning, Admin                   │
│  Members              │   Today · 06 Jun 2026                   │
│  Plans                │                                          │
│  Check-ins            ├──────────────────────────────────────────│
│                       │  KPI STRIP (4 stat-cards in a row)       │
│  SETTINGS             │  [Active Members] [Check-ins] [Expiring] │
│  Staff                │  [Monthly Revenue]                       │
│                       ├──────────────────────────────────────────│
│                       │  ┌──────────────────┐ ┌───────────────┐ │
│                       │  │  Recent Check-ins │ │ Expiring Soon │ │
│                       │  │  roster-list      │ │ roster-list   │ │
│                       │  └──────────────────┘ └───────────────┘ │
└──────────────────────────────────────────────────────────────────┘
```

---

### 8.2 Members Index (`Members/Index.cshtml`)

```
┌────────────────────────────────────────────────────────────────┐
│  MEMBERS                          [+ ADD MEMBER] ← btn-primary │
├────────────────────────────────────────────────────────────────┤
│  [Total: 312]  [Active: 248]  [Expiring: 14]  [Frozen: 5]      │  stat-cards row
├────────────────────────────────────────────────────────────────┤
│  [All ▾]  [Status ▾]  [Plan ▾]    [Search input]              │  filter bar
├────────────────────────────────────────────────────────────────┤
│  ROSTER TABLE  (card wrapper)                                  │
│  ┌────┬──────────────────┬──────────┬──────────┬──────┬──────┐ │
│  │    │ NAME / EMAIL     │ PLAN     │ STATUS   │EXPIRY│ACTS  │ │
│  ├────┼──────────────────┼──────────┼──────────┼──────┼──────┤ │
│  │ AH │ Ahmed Hassan     │ [ELITE]  │ [Active] │12/26 │V E D │ │
│  │ SK │ Sara Kamel       │ [BASIC]  │[Expiring]│06/26 │V E D │ │
│  └────┴──────────────────┴──────────┴──────────┴──────┴──────┘ │
└────────────────────────────────────────────────────────────────┘
```

---

### 8.3 Member Details (`Members/Details.cshtml`)

```
┌───────────────────────────────────────────────────────────────┐
│  ← BACK TO MEMBERS                                            │
├───────────────────────────────────────────────────────────────┤
│  [HERO BLOCK — hero-gradient]                                 │
│  [AH initials block, 64×64]                                   │
│  AHMED HASSAN               ← display-lg                      │
│  [ELITE] [Active]                                             │
│  MBR-00124                  ← data-mono ID                    │
├────────────────────────┬──────────────────────────────────────┤
│  card-section          │  card-section                        │
│  CONTACT               │  MEMBERSHIP                          │
│  📧 ahmed@email.com    │  Plan: ELITE                         │
│  📞 +20 100 000 0000   │  Start: 01 Jan 2026                  │
│                        │  Expires: 31 Dec 2026                │
│                        │  Days left: 208                      │
├────────────────────────┴──────────────────────────────────────┤
│  CHECK-IN HISTORY  (roster-list, last 10)                     │
├───────────────────────────────────────────────────────────────┤
│  [Edit Member]  [Check In Now]  [Delete Member ← btn-danger]  │
└───────────────────────────────────────────────────────────────┘
```

---

### 8.4 Plans Index (`Plans/Index.cshtml`)

```
┌───────────────────────────────────────────────────────────────┐
│  MEMBERSHIP PLANS                    [+ NEW PLAN] ← btn-primary│
├───────────────────────────────────────────────────────────────┤
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐ ┌─────────┐ │
│  │  [BASIC]    │ │ [STANDARD]  │ │  [PREMIUM]  │ │ [ELITE] │ │
│  │  card       │ │  card       │ │  card       │ │  card   │ │
│  │             │ │             │ │  ← hero-    │ │ ← neon  │ │
│  │  EGP 150/mo │ │  EGP 250/mo │ │  gradient   │ │ yellow  │ │
│  │             │ │             │ │  EGP 350/mo │ │ header  │ │
│  │  • Feature  │ │  • Feature  │ │  • Feature  │ │ EGP 500 │ │
│  │  • Feature  │ │  • Feature  │ │  • Feature  │ │         │ │
│  │  X members  │ │  X members  │ │  X members  │ │         │ │
│  │  [Edit]     │ │  [Edit]     │ │  [Edit]     │ │ [Edit]  │ │
│  └─────────────┘ └─────────────┘ └─────────────┘ └─────────┘ │
└───────────────────────────────────────────────────────────────┘
```

---

### 8.5 Check-ins (`CheckIns/Index.cshtml`)

```
┌───────────────────────────────────────────────────────────────┐
│  CHECK-INS                     [+ MANUAL CHECK-IN]            │
├───────────────────────────────────────────────────────────────┤
│  [Today: 67]   [This Week: 312]   [Avg/Day: 44]               │  stat-cards
├───────────────────────────────────────────────────────────────┤
│  [Date ▾]   [Member search]                                   │  filters
├───────────────────────────────────────────────────────────────┤
│  ROSTER  (check-in variant — TIME column instead of EXPIRY)   │
│  ┌────┬──────────────────┬──────────┬──────────┬────────────┐ │
│  │    │ NAME             │ PLAN     │ STATUS   │ TIME       │ │
│  ├────┼──────────────────┼──────────┼──────────┼────────────┤ │
│  │ AH │ Ahmed Hassan     │ [ELITE]  │ [Active] │ 09:14 AM   │ │
│  └────┴──────────────────┴──────────┴──────────┴────────────┘ │
└───────────────────────────────────────────────────────────────┘
```

---

### 8.6 Delete Confirmation (`Members/Delete.cshtml`)

```
┌──────────────────────────────────────────────┐
│  ← BACK                                      │
│                                              │
│  REMOVE MEMBER           ← headline-lg       │
│                                              │
│  ┌──────────────────────────────────────┐    │
│  │  card — error-container background   │    │
│  │  border-top: 4px solid var(--error)  │    │
│  │                                      │    │
│  │  You are about to permanently remove │    │
│  │  Ahmed Hassan from the system.       │    │
│  │  Their check-in history and plan     │    │
│  │  records will be deleted.            │    │
│  │                                      │    │
│  │  This action cannot be undone.       │    │
│  │                                      │    │
│  │  [Confirm Remove]  [Cancel]          │    │
│  │   btn-danger        btn-secondary    │    │
│  └──────────────────────────────────────┘    │
└──────────────────────────────────────────────┘
```

---

## Step 9 — Razor Helpers

```csharp
@functions {

  /* --- Status chip --- */
  string StatusChipClass(DateTime endDate, bool isFrozen) {
    if (isFrozen) return "status-chip--frozen";
    var days = (endDate - DateTime.Now).TotalDays;
    return days switch { < 0 => "status-chip--expired", < 7 => "status-chip--expiring", _ => "status-chip--active" };
  }
  string StatusChipLabel(DateTime endDate, bool isFrozen) {
    if (isFrozen) return "Frozen";
    var days = (endDate - DateTime.Now).TotalDays;
    return days switch { < 0 => "Expired", < 7 => "Expiring Soon", _ => "Active" };
  }

  /* --- Plan chip --- */
  string PlanChipClass(string planName) => planName.ToLower() switch {
    "elite"    => "plan-chip plan-chip--elite",
    "premium"  => "plan-chip plan-chip--premium",
    "standard" => "plan-chip plan-chip--standard",
    _          => "plan-chip plan-chip--basic"
  };

  /* --- Member initials --- */
  string MemberInitials(string fullName) {
    var parts = fullName.Trim().Split(' ');
    return parts.Length >= 2
      ? $"{parts[0][0]}{parts[^1][0]}".ToUpper()
      : fullName[..Math.Min(2, fullName.Length)].ToUpper();
  }

  /* --- Days remaining display --- */
  string DaysRemainingLabel(DateTime endDate) {
    var days = (int)(endDate - DateTime.Now).TotalDays;
    return days switch {
      < 0  => "Expired",
      0    => "Expires today",
      1    => "1 day left",
      <= 7 => $"{days} days left",
      _    => $"{days} days remaining"
    };
  }

  /* --- Member ID formatter --- */
  string FormatMemberId(int id) => $"MBR-{id:D5}";
}
```

---

## Step 10 — Hard Rules (Violations Break the System)

| Rule | Requirement |
|---|---|
| **Zero radius** | `border-radius: 0` on ALL elements — badges, buttons, inputs, cards, images |
| **No thin lines** | No `1px` border except `outline-variant` inside Roster List only |
| **No blurred shadows** | `blur: 0px` on all `box-shadow` — hard-edge only |
| **No off-token colors** | Any color not in the `:root` token map must be explicitly justified |
| **No center-align on data** | Member lists, forms, tables — flush-left only |
| **No soft drop shadows** | If it isn't `4px 4px 0px` hard-edge, remove it |
| **Structural stroke minimum** | `2px` when a border is used. Nothing thinner |
| **Typography must contrast** | No two adjacent elements share the same type size/weight |
| **Font discipline** | Public Sans for all display/headline, Space Grotesk for all data/labels |
| **One primary CTA** | Only one `btn-primary` (neon yellow) per view |
| **Status chips are text + color** | Never convey status by color alone — always include the text label |

---

## Step 11 — Output Checklist

Before delivering any view or component, verify:

- [ ] CSS tokens declared at `:root`, no inline hex values
- [ ] `border-radius: 0 !important` applied globally
- [ ] All borders are `2px` or `4px` using `var(--on-surface)` or `var(--on-surface-variant)`
- [ ] All `box-shadow` uses `blur: 0px` (hard shadow only)
- [ ] Surface hierarchy used for depth (not shadows on static elements)
- [ ] Display/headline in Public Sans, all data/labels in Space Grotesk
- [ ] At least one `display-md` stat number sits above a `label-sm` caption
- [ ] All lists, tables, and forms are left-aligned
- [ ] Status chips use correct token colors, 0px radius, text label present
- [ ] Plan chips use correct tier class
- [ ] Only one `btn-primary` (neon yellow) per page
- [ ] Razor helpers used for status, plan, initials, and days-remaining logic
- [ ] No glassmorphism outside modal/nav
- [ ] Hero gradient restricted to large header blocks only

---

## Quick Reference: What Signals "This Is Kinetic Engine Gym"

1. A neon signal yellow (`#eaff00`) "ADD MEMBER" or "CHECK IN" button with a hard black shadow
2. A massive `display-lg` page title "MEMBERS" — oversized, tight, left-aligned
3. White roster cards (`surface-container-lowest`) on a grey concrete floor (`surface-container`)
4. Member IDs in Space Grotesk: `MBR-00124`
5. Hard 2px or 4px black strokes — no rounded anything, anywhere
6. Status chip in neon yellow `[Active]` or deep red `[Expired]` — both at 0px radius
7. Dark sidebar with neon-yellow active state — pure dark/light inversion
