# Product Requirements Document (PRD)  
**Product:** Bingo-o-matic  
**Doc Version:** 1.2  
**Owner:** jamesmontemagno  
**Last Updated:** 2025-05-24  

---

## 1. Purpose  
Bingo-o-matic is a completely client-side web tool that helps bingo callers run engaging, flexible bingo games with zero setup, zero accounts, and full offline support. The product targets community event hosts, office-party organizers, teachers, and anyone who needs to lead a bingo session quickly and professionally.

---

## 2. Scope  
This PRD covers the MVP (“launchable” v1) and outlines a backlog of future ideas.  
• 100 % static delivery (no server code).  
• **No authentication, user profiles, or cloud storage.**  
• Only browser-local data (IndexedDB/`localStorage`) for custom word lists and current board state.

---

## 3. Goals & Non-Goals  

| Goals (v1) | Non-Goals (v1) |
|------------|----------------|
| • Polished landing page funneling users to the app via a single **Launch** button. | • Multiplayer sync across devices |
| • Standard 5 × 5 B-I-N-G-O board with number calling & animation. | • Real-time leaderboards or card distribution |
| • Custom Bingo Mode with multiple reveal styles. | • Any login/auth mechanism |
| • Local persistence of custom lists & current board only. | • Cloud backups or cross-device sync |
| • PWA-style offline support. | • Extensive user-configurable settings |

---

## 4. Success Metrics  

1. **Activation:** ≥ 70 % of landing-page visitors click **Launch App**.  
2. **Engagement:** Median session length ≥ 10 min.  
3. **Retention:** ≥ 40 % of browsers revisit within 30 days (tracked via anonymous local UUID).  
4. **Bug-Free Sessions:** < 1 % JS errors per 1000 sessions.  
5. **Offline Usage:** ≥ 25 % of sessions run without network after first load.

---

## 5. Personas  

1. **Carol the Community Host** – wants instant, reliable number calling.  
2. **Mr. Lopez the Teacher** – needs quick custom word lists, no sign-ups.  
3. **Dana the Designer** – values visually slick animations but no tech fuss.

---

## 6. User Stories (MVP)  

| Priority | As a… | I want… | So that… |
|----------|-------|---------|----------|
| P0 | Host | to see a clean landing page | I can quickly understand and start |
| P0 | Host | to generate a classic bingo board | I can call numbers |
| P0 | Host | to click “Next Number” and see it animate large on screen | Everyone can follow |
| P0 | Host | to paste a custom list of words | I can run themed bingo |
| P0 | Host | to pick a reveal style (roulette, fade, list) | Games feel fresh |
| P1 | Host | my custom lists to auto-save locally | I don’t re-type next session |
| P1 | Host | the app to keep working offline once loaded | Spotty Wi-Fi doesn’t block me |

---

## 7. Functional Requirements  

### 7.1 Landing Page  
• Hero tagline, short feature blurbs, looping GIF demo.  
• Single primary CTA: **Launch Bingo-o-matic**.  
• Lightweight CSS animations (no large JS libs).  
• Footer: MIT License link, GitHub repo.

### 7.2 Standard Bingo Mode  
• 5 × 5 grid with B-I-N-G-O headers & free center square.  
• Randomised pool of numbers (1–75).  
• Large “Next Number” overlay animation.  
• Called numbers greyed/highlighted on board.  
• Reset & shuffle controls.

### 7.3 Custom Bingo Mode  
• Textarea (CSV / one-per-line) import.  
• Validation (≥ 25 unique items recommended).  
• Reveal styles:  
 1. Roulette wheel (SVG/CSS spin).  
 2. Card flip fade-in.  
 3. Classic list ticker.  
• Style switchable between calls.  
• Called items appear highlighted.

### 7.4 Local Persistence  
• Use `IndexedDB` (via `Microsoft.JSInterop`) to store:  
 – Named custom lists.  
 – Current in-progress board (optional).  
• Single **Clear All Data** button (no extra settings UI).

### 7.5 Offline Support  
• Blazor WebAssembly PWA with Service Worker (offline cache of app shell, CSS, fonts, images).  
• Lightweight loader & progress bar while WASM downloads.

---

## 8. Non-Functional Requirements  

1. **Performance:** Uncompressed WASM ≤ 700 kB; first paint < 2 s on 3G with caching.  
2. **Accessibility:** WCAG 2.1 AA compliance.  
3. **Internationalisation Ready:** Strings in resx; English default.  
4. **Tech Stack (minimal dependencies):**  
 • Runtime: **Blazor WebAssembly (.NET 9)**  
 • Styling: Plain CSS + CSS custom properties; optional PostCSS build (no Tailwind).  
 • Animations: Pure CSS/SVG, no third-party JS animation libs.  
 • Build: `dotnet publish` → static assets.

---

## 9. Future Ideas & Backlog  

1. Printable player card generator (PDF).  
2. QR code for audience to mirror live board.  
3. Chromecast / AirPlay casting support.  
4. Themed board skins (seasonal, corporate).  
5. Sound effects & TTS callouts.  
6. Auto-call timer mode.  
7. Multilingual UI.  
8. Dark mode / high-contrast toggle.  
9. Template gallery for community custom lists.  
10. Real-time remote player mode (WebRTC).  
11. Voice command control for accessibility.  
12. Analytics dashboard for number frequency.  
13. Admin “script mode” to pre-plan calls.

---

## 10. Risks & Mitigations  

| Risk | Impact | Likelihood | Mitigation |
|------|--------|-----------|------------|
| WASM size too large for slow mobile | Medium | Medium | IL trimming & AOT; Brotli compression |
| Animation performance on low-end devices | Low | Medium | Use transform/opacity CSS only |
| Service worker cache staleness after deploy | Medium | Medium | Versioned cache & “Refresh for latest” toast |
| Browser storage quota exceeded | Low | Low | Cap total saved lists < 2 MB |

---

## 11. Milestones  

1. **M0 – Design (1 wk):** Wireframes & CSS animation prototypes.  
2. **M1 – Core Engine (2 wks):** Bingo logic, components.  
3. **M2 – Custom Mode (2 wks):** List import & reveal styles.  
4. **M3 – PWA/Offline (1 wk):** Service worker, manifest.  
5. **M4 – Landing Page (0.5 wk).**  
6. **M5 – QA & Accessibility (1 wk).**  
7. **M6 – Launch (0.5 wk).**

---

## 12. Dependencies  
• .NET 9 SDK & Blazor WebAssembly runtime (MIT).  
• **No external UI/component libraries.**  
• Optional: tiny open-source roulette SVG (≤ 2 kB, MIT).

---

## 13. Open Questions  

1. Which lightweight SVG technique gives smooth roulette spin while keeping bundle small?  
2. Minimum browser support matrix (Edge 110+, Safari 15+, Chrome 108+)?

---

## 14. Glossary  

• **Caller / Host:** Person running the bingo session.  
• **Board:** 5 × 5 bingo grid.  
• **Reveal Style:** Animation used to show next item.

---
