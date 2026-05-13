# CampusMart Project Overview

CampusMart is a comprehensive campus marketplace and management system designed for university environments. It facilitates buying and selling among students, managing campus stalls, and providing academic resources.

## 🚀 Tech Stack

- **Framework:** ASP.NET Core 10.0 MVC
- **Database:** Microsoft SQL Server with Entity Framework Core
- **Security:** ASP.NET Core Identity
- **Frontend:** HTML5, CSS3 (Custom Dark Theme), JavaScript (ES6+), jQuery, Bootstrap 5
- **Design:** Modern glassmorphism aesthetic with "Plus Jakarta Sans" and "Caveat" fonts.

## 📂 Project Structure

- `CampusMart/Areas/Admin`: Contains administrative management controllers and views.
    - `FloorApi`: RESTful management of campus floors and stalls.
- `CampusMart/Controllers`: Main application controllers (Shop, Cart, Order, Account, etc.).
- `CampusMart/Models/Entities`: Database models (ApplicationUser, Product, Stall, Floor, Order, etc.).
- `CampusMart/Models/ViewModels`: Data transfer objects for views.
- `CampusMart/wwwroot`: Static assets (CSS, JS, Images, Libs).

## 🎨 UI/UX Standards

- **Theme:** Dark mode by default (`--bg-base: #0f0f11`).
- **Typography:**
    - Main: `Plus Jakarta Sans`
    - Accent: `Caveat` (for highlights)
- **CSS Variables:** Defined in `wwwroot/css/site.css` for consistent colors and transitions.
- **Custom Components:** Prefix classes with `cm-` (e.g., `cm-btn-primary`, `cm-card`).
- **Responsiveness:** Mobile-first approach using CSS Grid and Flexbox. Auth pages are optimized for large-scale viewing (75% zoom).

## 🛠 Development Guidelines

1. **Database Access:** Use `AppDbContext` via Dependency Injection.
2. **Authentication:**
    - Uses `StudentId` as the primary identifier (UserName).
    - Password reset requires `StudentId` and `Email` verification.
3. **Admin Management:**
    - Floor management is handled via `FloorApiController` and `floor-management.js`.
    - Always ensure `isActive` flags are respected in queries.
4. **Naming Conventions:**
    - Controllers: `NameController`
    - Views: PascalCase matching action names.
    - Entities: Singular nouns.

## 📌 Maintenance Notes

- **CSS:** Maintain separate files for specific modules (`admin.css`, `auth.css`, `user-pages.css`) to prevent bloat in `site.css`.
- **Validation:** Use DataAnnotations in ViewModels for server-side validation and jQuery Validation for client-side feedback.
- **Images:** Uploads are stored in `wwwroot/uploads`. Ensure proper cleanup if entities are deleted.
