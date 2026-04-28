<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Campus Elite | Shop</title>
    <link rel="stylesheet" href="globals.css">
    <style>
        /* DASHBOARD LAYOUT */
        body {
            display: flex;
            min-height: 100vh;
            overflow: hidden; /* Hide body scroll, handle scroll in main content */
            background-color: #0f0f11;
        }

        /* SIDEBAR */
        .sidebar {
            width: 260px;
            background: #121214; 
            border-right: 1px solid var(--border-subtle);
            display: flex;
            flex-direction: column;
            z-index: 100;
            transition: var(--transition-smooth);
        }

        .sidebar-header {
            padding: 24px;
            display: flex;
            align-items: center;
        }

        .sidebar-brand {
            font-size: 20px;
            font-weight: 800;
            color: var(--primary);
            text-decoration: none;
            letter-spacing: 0.5px;
            text-transform: uppercase;
        }

        .portal-title {
            padding: 0 24px 16px;
        }

        .portal-title h3 {
            font-size: 16px;
            color: var(--primary);
            margin-bottom: 2px;
        }

        .portal-title p {
            font-size: 11px;
            color: var(--text-muted);
            font-weight: 600;
        }

        .sidebar-nav {
            flex: 1;
            display: flex;
            flex-direction: column;
        }

        .nav-item {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 16px 24px;
            color: var(--text-muted);
            text-decoration: none;
            font-size: 14px;
            font-weight: 600;
            transition: var(--transition-smooth);
        }

        .nav-item.active {
            background: rgba(255, 215, 0, 0.1);
            color: var(--primary);
            border-left: 3px solid var(--primary);
        }
        
        .nav-item:not(.active):hover {
            background: rgba(255, 255, 255, 0.05);
            color: white;
        }

        .nav-icon {
            font-size: 18px;
            width: 24px;
            text-align: center;
        }

        .user-profile {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 20px 24px;
            border-top: 1px solid rgba(255,255,255,0.05);
            margin-top: auto;
        }

        .user-avatar {
            width: 36px;
            height: 36px;
            border-radius: 50%;
            background: #222;
            object-fit: cover;
        }

        .user-info h3 {
            font-size: 13px;
            margin-bottom: 2px;
            color: white;
        }

        .user-info p {
            font-size: 10px;
            color: var(--primary);
            font-weight: 800;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        /* MAIN CONTENT */
        .main-content {
            flex: 1;
            display: flex;
            flex-direction: column;
            overflow-y: auto;
            background-color: var(--bg-base);
            position: relative;
        }

        /* TOP NAVBAR */
        .top-navbar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 20px 40px;
            background: rgba(15, 15, 17, 0.9);
            backdrop-filter: blur(24px);
            -webkit-backdrop-filter: blur(24px);
            border-bottom: 1px solid var(--border-subtle);
            position: sticky;
            top: 0;
            z-index: 50;
        }

        .top-nav-links {
            display: flex;
            gap: 32px;
            align-items: center;
            margin-left: 40px;
        }

        .top-link {
            color: var(--text-muted);
            text-decoration: none;
            font-size: 14px;
            font-weight: 600;
            transition: var(--transition-smooth);
            padding-bottom: 4px;
            border-bottom: 2px solid transparent;
        }

        .top-link:hover {
            color: white;
        }

        .top-link.active {
            color: var(--primary);
            border-bottom-color: var(--primary);
        }

        .top-actions {
            display: flex;
            align-items: center;
            gap: 20px;
        }

        .search-bar {
            position: relative;
            width: 280px;
        }

        .search-bar input {
            width: 100%;
            background: rgba(255,255,255,0.05);
            border: 1px solid rgba(255,255,255,0.08);
            padding: 10px 16px 10px 40px;
            border-radius: 20px;
            color: white;
            font-size: 13px;
            transition: var(--transition-smooth);
            font-family: inherit;
        }

        .search-bar input:focus {
            background: rgba(255,255,255,0.08);
            border-color: rgba(255,255,255,0.2);
            outline: none;
        }

        .search-bar .search-icon {
            position: absolute;
            left: 14px;
            top: 50%;
            transform: translateY(-50%);
            color: var(--text-muted);
            font-size: 14px;
        }

        .icon-btn {
            background: transparent;
            border: none;
            color: var(--text-muted);
            font-size: 18px;
            cursor: pointer;
            transition: var(--transition-smooth);
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .icon-btn:hover {
            color: white;
        }

        /* DASHBOARD CONTAINER */
        .dashboard-container {
            padding: 40px;
            max-width: 1200px;
            margin: 0 auto;
            width: 100%;
        }

        /* HEADER SECTION */
        .marketplace-header {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            margin-bottom: 30px;
        }

        .header-left {
            max-width: 600px;
        }

        .section-title {
            font-size: 24px;
            color: white;
            font-weight: 700;
            margin-bottom: 12px;
        }

        .header-desc {
            color: var(--text-muted);
            font-size: 14px;
            line-height: 1.6;
        }

        .header-right {
            text-align: right;
        }
        
        .building-status-label {
            font-size: 10px;
            color: var(--primary);
            font-weight: 800;
            text-transform: uppercase;
            letter-spacing: 1px;
            margin-bottom: 4px;
        }
        
        .building-name {
            font-size: 20px;
            font-weight: 700;
            color: white;
        }

        /* FLOOR TABS */
        .floor-tabs {
            display: flex;
            gap: 16px;
            margin-bottom: 30px;
        }

        .floor-tab {
            flex: 1;
            background: rgba(255,255,255,0.03);
            border: 1px solid rgba(255,255,255,0.05);
            border-radius: 8px;
            padding: 16px;
            text-align: center;
            cursor: pointer;
            transition: var(--transition-smooth);
            display: flex;
            flex-direction: column;
            gap: 4px;
        }

        .floor-tab:hover {
            background: rgba(255,255,255,0.06);
        }

        .floor-tab.active {
            background: var(--primary);
            border-color: var(--primary);
        }

        .floor-label {
            font-size: 10px;
            color: var(--text-muted);
            text-transform: uppercase;
            letter-spacing: 1px;
            font-weight: 600;
        }

        .floor-tab.active .floor-label {
            color: rgba(0,0,0,0.6);
        }

        .floor-num {
            font-size: 18px;
            color: white;
            font-weight: 700;
        }

        .floor-tab.active .floor-num {
            color: #0f0f11;
        }

        /* MAP CARD */
        .map-card {
            background: #1a1a1e;
            border-radius: 16px;
            position: relative;
            border: 1px solid var(--border-subtle);
            overflow: hidden;
            margin-bottom: 40px;
            height: 240px;
        }

        .map-bg {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            object-fit: cover;
            opacity: 0.4;
            mix-blend-mode: screen;
        }

        .map-content {
            position: relative;
            z-index: 10;
            padding: 30px;
            height: 100%;
            display: flex;
            flex-direction: column;
        }

        .map-title {
            font-size: 28px;
            font-weight: 700;
            color: white;
            margin-bottom: 4px;
        }

        .map-sub {
            font-size: 14px;
            color: var(--text-muted);
        }

        .map-badges {
            margin-top: auto;
            display: flex;
            gap: 12px;
        }

        .map-badge {
            background: rgba(0,0,0,0.6);
            border: 1px solid rgba(255,255,255,0.1);
            padding: 6px 12px;
            border-radius: 20px;
            font-size: 11px;
            font-weight: 600;
        }

        .map-badge.yellow { color: var(--primary); }
        .map-badge.green { color: #4ade80; }

        /* STALLS SECTION */
        .stalls-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 24px;
        }

        .stalls-title {
            font-size: 20px;
            color: white;
            font-weight: 700;
        }

        .view-toggles {
            display: flex;
            gap: 8px;
        }

        .view-btn {
            background: transparent;
            border: 1px solid rgba(255,255,255,0.1);
            color: var(--text-muted);
            padding: 6px;
            border-radius: 6px;
            cursor: pointer;
            transition: var(--transition-smooth);
        }

        .view-btn.active, .view-btn:hover {
            background: rgba(255,255,255,0.1);
            color: white;
        }

        /* STALLS GRID */
        .stalls-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
            gap: 24px;
            margin-bottom: 60px;
        }

        .stall-card {
            background: #141417;
            border: 1px solid var(--border-subtle);
            border-radius: 12px;
            overflow: hidden;
            transition: var(--transition-smooth);
            display: flex;
            flex-direction: column;
        }

        .stall-card:hover {
            border-color: rgba(255,255,255,0.15);
            transform: translateY(-4px);
            box-shadow: 0 12px 30px rgba(0,0,0,0.5);
        }

        .stall-img-wrapper {
            position: relative;
            height: 180px;
            background: #1a1a1e;
        }

        .stall-img-wrapper img {
            width: 100%;
            height: 100%;
            object-fit: cover;
            opacity: 0.8;
            transition: var(--transition-smooth);
        }

        .stall-card:hover .stall-img-wrapper img {
            opacity: 1;
            transform: scale(1.05);
        }

        .stall-badge {
            position: absolute;
            top: 16px;
            left: 16px;
            background: rgba(0,0,0,0.8);
            color: var(--primary);
            padding: 4px 10px;
            border-radius: 12px;
            font-size: 10px;
            font-weight: 800;
            z-index: 2;
        }

        .stall-content {
            padding: 20px;
            display: flex;
            flex-direction: column;
            flex: 1;
        }

        .stall-header-row {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            margin-bottom: 8px;
        }

        .stall-name {
            font-size: 16px;
            font-weight: 600;
            color: white;
        }

        .stall-price {
            font-size: 14px;
            color: var(--primary);
            font-weight: 700;
        }

        .stall-desc {
            font-size: 12px;
            color: var(--text-muted);
            line-height: 1.5;
            margin-bottom: 20px;
            flex: 1;
        }

        .stall-footer {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .stall-rating {
            font-size: 12px;
            color: white;
            font-weight: 600;
            display: flex;
            align-items: center;
            gap: 4px;
        }

        .stall-rating span {
            color: var(--text-muted);
            font-weight: 400;
            font-size: 11px;
        }
        
        .star-icon {
            color: var(--primary);
            font-size: 14px;
        }

        .btn-enter {
            background: var(--primary);
            color: #0f0f11;
            border: none;
            padding: 8px 16px;
            border-radius: 8px;
            font-weight: 800;
            font-size: 11px;
            cursor: pointer;
            transition: var(--transition-smooth);
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .btn-enter:hover {
            background: var(--primary-hover);
            box-shadow: 0 4px 15px rgba(255,215,0,0.3);
            transform: translateY(-2px);
        }

        /* FAB */
        .fab {
            position: fixed;
            bottom: 40px;
            right: 40px;
            width: 56px;
            height: 56px;
            background: var(--primary);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            box-shadow: 0 8px 25px rgba(255,215,0,0.3);
            transition: var(--transition-smooth);
            z-index: 100;
            color: #0f0f11;
            font-size: 24px;
        }

        .fab:hover {
            transform: scale(1.1) rotate(10deg);
            background: var(--primary-hover);
        }

        /* PROFILE MODAL */
        .modal-overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.8);
            backdrop-filter: blur(5px);
            z-index: 1000;
            align-items: center;
            justify-content: center;
            opacity: 0;
            transition: opacity 0.3s ease;
        }
        .modal-overlay.active {
            display: flex;
            opacity: 1;
        }
        .profile-modal {
            background: #141417;
            border: 1px solid var(--border-subtle);
            border-radius: 20px;
            width: 100%;
            max-width: 450px;
            padding: 30px;
            transform: translateY(20px);
            transition: transform 0.3s ease;
            box-shadow: 0 20px 40px rgba(0,0,0,0.5);
        }
        .modal-overlay.active .profile-modal {
            transform: translateY(0);
        }
        .modal-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 24px;
        }
        .modal-title {
            font-size: 20px;
            color: white;
            font-weight: 700;
        }
        .close-modal {
            background: transparent;
            border: none;
            color: var(--text-muted);
            font-size: 24px;
            cursor: pointer;
            transition: var(--transition-smooth);
        }
        .close-modal:hover {
            color: white;
        }
        .profile-pic-edit {
            display: flex;
            flex-direction: column;
            align-items: center;
            margin-bottom: 24px;
            gap: 12px;
        }
        .profile-pic-preview {
            width: 100px;
            height: 100px;
            border-radius: 50%;
            object-fit: cover;
            border: 3px solid var(--primary);
        }
        .btn-outline {
            background: transparent;
            border: 1px solid var(--primary);
            color: var(--primary);
            padding: 8px 16px;
            border-radius: 8px;
            font-size: 12px;
            font-weight: 700;
            cursor: pointer;
            transition: var(--transition-smooth);
        }
        .btn-outline:hover {
            background: rgba(255, 215, 0, 0.1);
        }
        .modal-form-group {
            margin-bottom: 16px;
        }
        .modal-label {
            display: block;
            font-size: 12px;
            color: var(--text-muted);
            margin-bottom: 8px;
            font-weight: 600;
            text-transform: uppercase;
        }
        .modal-input {
            width: 100%;
            background: rgba(255,255,255,0.05);
            border: 1px solid rgba(255,255,255,0.1);
            padding: 12px 16px;
            border-radius: 10px;
            color: white;
            font-size: 14px;
            font-family: inherit;
        }
        .modal-input:focus {
            outline: none;
            border-color: var(--primary);
        }
        .modal-input[readonly] {
            background: rgba(0,0,0,0.2);
            color: rgba(255,255,255,0.4);
            cursor: not-allowed;
            border-color: transparent;
        }
        .btn-save {
            width: 100%;
            background: var(--primary);
            color: #0f0f11;
            border: none;
            padding: 14px;
            border-radius: 10px;
            font-size: 14px;
            font-weight: 800;
            cursor: pointer;
            margin-top: 8px;
            transition: var(--transition-smooth);
        }
        .btn-save:hover {
            background: var(--primary-hover);
        }
    </style>
</head>
<body>

    <!-- SIDEBAR -->
    <aside class="sidebar">
        <div class="sidebar-header">
            <a href="#" class="sidebar-brand">CAMPUSELITE</a>
        </div>
        
        <div class="portal-title">
            <h3>Student Portal</h3>
            <p>Elite Concierge Service</p>
        </div>

        <nav class="sidebar-nav">
            <a href="#" class="nav-item active">
                <span class="nav-icon">🛒</span> Cart Items
            </a>
            <a href="#" class="nav-item">
                <span class="nav-icon">🔖</span> Saved for Later
            </a>
            <a href="#" class="nav-item">
                <span class="nav-icon">📖</span> Academic Services
            </a>
            <a href="#" class="nav-item">
                <span class="nav-icon">🕒</span> Order History
            </a>
        </nav>

        <div class="user-profile" id="userProfileBtn" style="cursor: pointer; transition: background 0.3s;" onmouseover="this.style.background='rgba(255,255,255,0.05)'" onmouseout="this.style.background='transparent'">
            <img src="https://ui-avatars.com/api/?name=Alex+Chen&background=222&color=fff" alt="Alex Chen" class="user-avatar" id="sidebarAvatar">
            <div class="user-info">
                <h3 id="sidebarName">Alex Chen</h3>
                <p>Premium Member</p>
            </div>
        </div>
    </aside>

    <!-- MAIN CONTENT -->
    <main class="main-content">
        <!-- TOP NAVBAR -->
        <header class="top-navbar">
            <div class="top-nav-links">
                <a href="student_dashboard.html" class="top-link active">Shop</a>
                <a href="student_renting.html" class="top-link">Rentals</a>
                <a href="student_sell.html" class="top-link">Sell</a>
            </div>

            <div class="top-actions">
                <div class="search-bar">
                    <span class="search-icon">🔍</span>
                    <input type="text" placeholder="Search stalls...">
                </div>
                <button class="icon-btn">🔔</button>
                <button class="icon-btn">👤</button>
            </div>
        </header>

        <!-- DASHBOARD CONTAINER -->
        <div class="dashboard-container">
            
            <div class="marketplace-header animate-fade-in">
                <div class="header-left">
                    <h2 class="section-title">Campus Marketplace</h2>
                    <p class="header-desc">
                        Browse exclusive student-run stalls organized by building floors. Each floor
                        features curated vendors with high-quality academics and essentials.
                    </p>
                </div>
                <div class="header-right">
                    <div class="building-status-label">Building Status</div>
                    <div class="building-name">Main Academic Hall</div>
                </div>
            </div>

            <div class="floor-tabs animate-fade-in delay-1">
                <div class="floor-tab active">
                    <span class="floor-label">Floor</span>
                    <span class="floor-num">5</span>
                </div>
                <div class="floor-tab">
                    <span class="floor-label">Floor</span>
                    <span class="floor-num">4</span>
                </div>
                <div class="floor-tab">
                    <span class="floor-label">Floor</span>
                    <span class="floor-num">3</span>
                </div>
                <div class="floor-tab">
                    <span class="floor-label">Floor</span>
                    <span class="floor-num">2</span>
                </div>
                <div class="floor-tab">
                    <span class="floor-label">Floor</span>
                    <span class="floor-num">1</span>
                </div>
            </div>

            <div class="map-card animate-fade-in delay-1">
                <!-- Fallback gradient if image fails, simulating the map -->
                <div style="position:absolute; inset:0; background: linear-gradient(90deg, #1f2022 0%, #2a2c30 50%, #1f2022 100%);"></div>
                <!-- Abstract map lines overlay -->
                <div style="position:absolute; inset:0; background-image: repeating-linear-gradient(90deg, transparent, transparent 40px, rgba(255,255,255,0.03) 40px, rgba(255,255,255,0.03) 41px), repeating-linear-gradient(0deg, transparent, transparent 40px, rgba(255,255,255,0.03) 40px, rgba(255,255,255,0.03) 41px);"></div>
                <div style="position:absolute; top:0; bottom:0; left:60%; width:2px; background: rgba(255,0,0,0.3);"></div>
                
                <div class="map-content">
                    <h2 class="map-title">5th Floor Map</h2>
                    <p class="map-sub">Current Occupancy: 6 of 8 stalls</p>
                    
                    <div class="map-badges">
                        <span class="map-badge yellow">Lounge Area Open</span>
                        <span class="map-badge green">2 Vacancies</span>
                    </div>
                </div>
            </div>

            <div class="stalls-header animate-fade-in delay-2">
                <h3 class="stalls-title">Active Stalls - Floor 5</h3>
                <div class="view-toggles">
                    <button class="view-btn active">⊞</button>
                    <button class="view-btn">≡</button>
                </div>
            </div>

            <div class="stalls-grid animate-fade-in delay-2">
                <!-- Stall 1 -->
                <div class="stall-card">
                    <div class="stall-img-wrapper">
                        <span class="stall-badge">STALL 1/8</span>
                        <div style="position:absolute; inset:0; background: radial-gradient(circle at center, #1a2a3a 0%, #0a101a 100%); display:flex; align-items:center; justify-content:center;">
                            <div style="width:40px; height:40px; border:2px solid #3b82f6; border-radius:50%; box-shadow: 0 0 20px #3b82f6;"></div>
                        </div>
                    </div>
                    <div class="stall-content">
                        <div class="stall-header-row">
                            <h4 class="stall-name">The Tech Hub</h4>
                            <span class="stall-price">$45+</span>
                        </div>
                        <p class="stall-desc">Premium gadgets and high-speed rental accessories for modern students.</p>
                        <div class="stall-footer">
                            <div class="stall-rating">
                                <span class="star-icon">★</span> 4.9 <span>(120)</span>
                            </div>
                            <button class="btn-enter">Enter Stall</button>
                        </div>
                    </div>
                </div>

                <!-- Stall 2 -->
                <div class="stall-card">
                    <div class="stall-img-wrapper">
                        <span class="stall-badge">STALL 2/8</span>
                        <img src="https://images.unsplash.com/photo-1481627834876-b7833e8f5570?q=80&w=2228&auto=format&fit=crop" alt="Archive Books">
                    </div>
                    <div class="stall-content">
                        <div class="stall-header-row">
                            <h4 class="stall-name">Archive Books</h4>
                            <span class="stall-price">$15+</span>
                        </div>
                        <p class="stall-desc">Rare editions and semester textbooks at discounted rates.</p>
                        <div class="stall-footer">
                            <div class="stall-rating">
                                <span class="star-icon">★</span> 4.8 <span>(85)</span>
                            </div>
                            <button class="btn-enter">Enter Stall</button>
                        </div>
                    </div>
                </div>

                <!-- Stall 3 -->
                <div class="stall-card">
                    <div class="stall-img-wrapper">
                        <span class="stall-badge">STALL 3/8</span>
                        <img src="https://images.unsplash.com/photo-1559496417-e7f25cb247f3?q=80&w=2000&auto=format&fit=crop" alt="Caffeine Lab">
                    </div>
                    <div class="stall-content">
                        <div class="stall-header-row">
                            <h4 class="stall-name">Caffeine Lab</h4>
                            <span class="stall-price">$5+</span>
                        </div>
                        <p class="stall-desc">Artisan brews and snack packs for late-night study sessions.</p>
                        <div class="stall-footer">
                            <div class="stall-rating">
                                <span class="star-icon">★</span> 5.0 <span>(210)</span>
                            </div>
                            <button class="btn-enter">Enter Stall</button>
                        </div>
                    </div>
                </div>

                <!-- Stall 4 -->
                <div class="stall-card">
                    <div class="stall-img-wrapper">
                        <span class="stall-badge">STALL 4/8</span>
                        <img src="https://images.unsplash.com/photo-1585036156171-384164a8c675?q=80&w=2000&auto=format&fit=crop" alt="Stationery Elite">
                    </div>
                    <div class="stall-content">
                        <div class="stall-header-row">
                            <h4 class="stall-name">Stationery Elite</h4>
                            <span class="stall-price">$12+</span>
                        </div>
                        <p class="stall-desc">Minimalist planners and premium writing instruments.</p>
                        <div class="stall-footer">
                            <div class="stall-rating">
                                <span class="star-icon">★</span> 4.7 <span>(42)</span>
                            </div>
                            <button class="btn-enter">Enter Stall</button>
                        </div>
                    </div>
                </div>

                <!-- Stall 5 -->
                <div class="stall-card">
                    <div class="stall-img-wrapper">
                        <span class="stall-badge">STALL 5/8</span>
                        <img src="https://images.unsplash.com/photo-1529333166437-7750a6dd5a70?q=80&w=2000&auto=format&fit=crop" alt="Merch Corner">
                    </div>
                    <div class="stall-content">
                        <div class="stall-header-row">
                            <h4 class="stall-name">Merch Corner</h4>
                            <span class="stall-price">$25+</span>
                        </div>
                        <p class="stall-desc">Custom campus apparel and exclusive drop collections.</p>
                        <div class="stall-footer">
                            <div class="stall-rating">
                                <span class="star-icon">★</span> 4.6 <span>(31)</span>
                            </div>
                            <button class="btn-enter">Enter Stall</button>
                        </div>
                    </div>
                </div>

                <!-- Stall 6 -->
                <div class="stall-card">
                    <div class="stall-img-wrapper">
                        <span class="stall-badge">STALL 6/8</span>
                        <img src="https://images.unsplash.com/photo-1518455027359-f3f8164ba6bd?q=80&w=2000&auto=format&fit=crop" alt="Artisan Desk">
                    </div>
                    <div class="stall-content">
                        <div class="stall-header-row">
                            <h4 class="stall-name">Artisan Desk</h4>
                            <span class="stall-price">$80+</span>
                        </div>
                        <p class="stall-desc">Bespoke wooden furniture and ergonomic setups.</p>
                        <div class="stall-footer">
                            <div class="stall-rating">
                                <span class="star-icon">★</span> 5.0 <span>(15)</span>
                            </div>
                            <button class="btn-enter">Enter Stall</button>
                        </div>
                    </div>
                </div>
            </div>
            
        </div>
    </main>

    <!-- FAB -->
    <div class="fab">
        <span>🏪</span>
    </div>

    <!-- PROFILE EDIT MODAL -->
    <div class="modal-overlay" id="profileModal">
        <div class="profile-modal">
            <div class="modal-header">
                <h2 class="modal-title">Edit Profile</h2>
                <button class="close-modal" id="closeProfileModal">&times;</button>
            </div>
            
            <div class="profile-pic-edit">
                <img src="https://ui-avatars.com/api/?name=Alex+Chen&background=222&color=fff" alt="Profile" class="profile-pic-preview" id="modalPicPreview">
                <button class="btn-outline" onclick="document.getElementById('profilePicInput').click()">Change Picture</button>
                <input type="file" id="profilePicInput" style="display: none;" accept="image/*">
            </div>

            <div class="modal-form-group">
                <label class="modal-label">Full Name</label>
                <input type="text" class="modal-input" id="profileNameInput" value="Alex Chen">
            </div>

            <div class="modal-form-group">
                <label class="modal-label">ID Number</label>
                <input type="text" class="modal-input" value="2021-00123" readonly>
            </div>

            <div class="modal-form-group">
                <label class="modal-label">Course</label>
                <input type="text" class="modal-input" value="BS Computer Science" readonly>
            </div>

            <button class="btn-save" id="saveProfileBtn">Save Changes</button>
        </div>
    </div>

    <script>
        // Sidebar Interactivity
        const navItems = document.querySelectorAll('.sidebar-nav .nav-item');
        navItems.forEach(item => {
            item.addEventListener('click', (e) => {
                e.preventDefault();
                navItems.forEach(nav => nav.classList.remove('active'));
                item.classList.add('active');
            });
        });

        // Profile Modal Logic
        const profileModal = document.getElementById('profileModal');
        const userProfileBtn = document.getElementById('userProfileBtn');
        const closeProfileModal = document.getElementById('closeProfileModal');
        const saveProfileBtn = document.getElementById('saveProfileBtn');
        const profilePicInput = document.getElementById('profilePicInput');
        const modalPicPreview = document.getElementById('modalPicPreview');
        const sidebarAvatar = document.getElementById('sidebarAvatar');
        const sidebarName = document.getElementById('sidebarName');
        const profileNameInput = document.getElementById('profileNameInput');

        // Open Modal
        userProfileBtn.addEventListener('click', () => {
            profileModal.classList.add('active');
        });

        // Close Modal
        closeProfileModal.addEventListener('click', () => {
            profileModal.classList.remove('active');
        });

        // Close when clicking outside
        profileModal.addEventListener('click', (e) => {
            if(e.target === profileModal) {
                profileModal.classList.remove('active');
            }
        });

        // Handle Image Upload Preview
        profilePicInput.addEventListener('change', function() {
            if (this.files && this.files[0]) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    modalPicPreview.src = e.target.result;
                }
                reader.readAsDataURL(this.files[0]);
            }
        });

        // Save Profile
        saveProfileBtn.addEventListener('click', () => {
            // Update Sidebar Name
            if(profileNameInput.value.trim() !== '') {
                sidebarName.textContent = profileNameInput.value;
            }
            // Update Sidebar Avatar
            sidebarAvatar.src = modalPicPreview.src;
            
            // Close modal
            profileModal.classList.remove('active');
            
            // Optional: Show a subtle toast or alert
            // alert('Profile updated successfully!');
        });
    </script>
</body>
</html>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>CampusElite | Edit Profile</title>
    <link rel="stylesheet" href="globals.css">
    <style>
        body {
            display: flex;
            min-height: 100vh;
            overflow: hidden;
            background-color: #0f0f11;
        }

        /* ── SIDEBAR ── */
        .sidebar {
            width: 260px;
            background: #121214;
            border-right: 1px solid var(--border-subtle);
            display: flex;
            flex-direction: column;
            z-index: 100;
        }
        .sidebar-header { padding: 24px; display: flex; align-items: center; }
        .sidebar-brand {
            font-size: 20px; font-weight: 800; color: var(--primary);
            text-decoration: none; letter-spacing: 0.5px; text-transform: uppercase;
        }
        .portal-title { padding: 0 24px 16px; }
        .portal-title h3 { font-size: 16px; color: var(--primary); margin-bottom: 2px; }
        .portal-title p { font-size: 11px; color: var(--text-muted); font-weight: 600; }

        .sidebar-nav { flex: 1; display: flex; flex-direction: column; }
        .nav-item {
            display: flex; align-items: center; gap: 12px;
            padding: 16px 24px; color: var(--text-muted); text-decoration: none;
            font-size: 14px; font-weight: 600; transition: var(--transition-smooth);
        }
        .nav-item.active { background: rgba(255,215,0,0.1); color: var(--primary); border-left: 3px solid var(--primary); }
        .nav-item:not(.active):hover { background: rgba(255,255,255,0.05); color: white; }
        .nav-icon { font-size: 18px; width: 24px; text-align: center; }

        .user-profile {
            display: flex; align-items: center; gap: 12px;
            padding: 20px 24px; border-top: 1px solid rgba(255,255,255,0.05); margin-top: auto;
            cursor: pointer; transition: background 0.3s;
        }
        .user-profile:hover { background: rgba(255,255,255,0.05); }
        .user-avatar { width: 36px; height: 36px; border-radius: 50%; background: #222; object-fit: cover; }
        .user-info h3 { font-size: 13px; margin-bottom: 2px; color: white; }
        .user-info p { font-size: 10px; color: var(--primary); font-weight: 800; text-transform: uppercase; letter-spacing: 0.5px; }

        /* ── MAIN ── */
        .main-content { flex: 1; display: flex; flex-direction: column; overflow-y: auto; background-color: var(--bg-base); }

        /* ── TOP NAVBAR ── */
        .top-navbar {
            display: flex; justify-content: space-between; align-items: center;
            padding: 20px 40px; background: rgba(15,15,17,0.9);
            backdrop-filter: blur(24px); -webkit-backdrop-filter: blur(24px);
            border-bottom: 1px solid var(--border-subtle); position: sticky; top: 0; z-index: 50;
        }
        .top-nav-links { display: flex; gap: 32px; align-items: center; margin-left: 40px; }
        .top-link {
            color: var(--text-muted); text-decoration: none; font-size: 14px; font-weight: 600;
            transition: var(--transition-smooth); padding-bottom: 4px; border-bottom: 2px solid transparent;
        }
        .top-link:hover { color: white; }
        .top-link.active { color: var(--primary); border-bottom-color: var(--primary); }
        .top-actions { display: flex; align-items: center; gap: 20px; }
        .icon-btn {
            background: transparent; border: none; color: var(--text-muted);
            font-size: 18px; cursor: pointer; transition: var(--transition-smooth);
            display: flex; align-items: center; justify-content: center;
        }
        .icon-btn:hover { color: white; }

        /* ── PROFILE PAGE ── */
        .profile-page { padding: 40px; max-width: 900px; margin: 0 auto; width: 100%; }

        .page-header {
            display: flex; align-items: center; gap: 16px; margin-bottom: 36px;
        }
        .back-btn {
            width: 40px; height: 40px; border-radius: 12px; display: flex; align-items: center; justify-content: center;
            background: rgba(255,255,255,0.04); border: 1px solid var(--border-subtle);
            color: var(--text-muted); font-size: 20px; cursor: pointer; transition: var(--transition-smooth);
            text-decoration: none;
        }
        .back-btn:hover { background: rgba(255,255,255,0.08); color: white; border-color: rgba(255,255,255,0.15); }
        .page-title { font-size: 24px; color: white; font-weight: 700; }
        .page-subtitle { font-size: 13px; color: var(--text-muted); margin-top: 2px; }

        /* ── AVATAR SECTION ── */
        .avatar-section {
            display: flex; align-items: center; gap: 28px; padding: 28px;
            background: #141417; border: 1px solid var(--border-subtle); border-radius: 16px;
            margin-bottom: 28px; position: relative; overflow: hidden;
        }
        .avatar-section::before {
            content: ''; position: absolute; top: -50%; right: -10%; width: 300px; height: 300px;
            background: radial-gradient(circle, rgba(255,215,0,0.06) 0%, transparent 70%);
            pointer-events: none;
        }
        .avatar-wrapper { position: relative; flex-shrink: 0; }
        .avatar-large {
            width: 110px; height: 110px; border-radius: 50%; object-fit: cover;
            border: 3px solid var(--primary); box-shadow: 0 0 24px rgba(255,215,0,0.15);
            transition: var(--transition-smooth);
        }
        .avatar-edit-overlay {
            position: absolute; inset: 0; border-radius: 50%; background: rgba(0,0,0,0.55);
            display: flex; align-items: center; justify-content: center;
            opacity: 0; transition: opacity 0.3s; cursor: pointer; font-size: 28px;
        }
        .avatar-wrapper:hover .avatar-edit-overlay { opacity: 1; }
        .avatar-wrapper:hover .avatar-large { filter: brightness(0.7); }

        .avatar-details { position: relative; z-index: 1; }
        .avatar-details h2 { font-size: 22px; margin-bottom: 4px; }
        .avatar-details .role-badge {
            display: inline-block; padding: 4px 12px; border-radius: 20px; font-size: 10px;
            font-weight: 800; text-transform: uppercase; letter-spacing: 1px;
            background: rgba(255,215,0,0.12); color: var(--primary); margin-bottom: 10px;
        }
        .avatar-details .avatar-meta { font-size: 12px; color: var(--text-muted); line-height: 1.8; }
        .avatar-details .avatar-meta span { color: rgba(255,255,255,0.6); }

        .avatar-actions { margin-left: auto; display: flex; flex-direction: column; gap: 8px; position: relative; z-index: 1; }
        .btn-upload {
            background: var(--primary); color: #0f0f11; border: none; padding: 10px 20px;
            border-radius: 10px; font-weight: 800; font-size: 12px; cursor: pointer;
            transition: var(--transition-smooth); text-transform: uppercase; letter-spacing: 0.5px;
        }
        .btn-upload:hover { background: var(--primary-hover); box-shadow: 0 4px 15px rgba(255,215,0,0.3); transform: translateY(-2px); }
        .btn-remove {
            background: transparent; color: #f87171; border: 1px solid rgba(248,113,113,0.3);
            padding: 10px 20px; border-radius: 10px; font-weight: 700; font-size: 12px;
            cursor: pointer; transition: var(--transition-smooth); text-transform: uppercase; letter-spacing: 0.5px;
        }
        .btn-remove:hover { background: rgba(248,113,113,0.1); border-color: #f87171; }

        /* ── FORM SECTIONS ── */
        .form-section {
            background: #141417; border: 1px solid var(--border-subtle); border-radius: 16px;
            padding: 28px; margin-bottom: 28px;
        }
        .form-section-header {
            display: flex; align-items: center; gap: 10px; margin-bottom: 24px;
            padding-bottom: 16px; border-bottom: 1px solid rgba(255,255,255,0.05);
        }
        .form-section-icon { font-size: 20px; }
        .form-section-title { font-size: 16px; color: white; font-weight: 700; }
        .form-section-desc { font-size: 12px; color: var(--text-muted); margin-left: auto; }

        .form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 20px; }
        .form-grid .full-width { grid-column: 1 / -1; }

        .form-group { display: flex; flex-direction: column; gap: 8px; }
        .form-label {
            font-size: 12px; color: var(--text-muted); font-weight: 600;
            text-transform: uppercase; letter-spacing: 0.5px;
            display: flex; align-items: center; gap: 6px;
        }
        .form-label .lock-icon { font-size: 10px; color: rgba(255,255,255,0.2); }

        .form-input {
            width: 100%; background: rgba(255,255,255,0.04); border: 1px solid rgba(255,255,255,0.08);
            padding: 13px 16px; border-radius: 10px; color: white; font-size: 14px;
            font-family: inherit; transition: var(--transition-smooth); outline: none;
        }
        .form-input:focus { border-color: var(--primary); background: rgba(255,255,255,0.06); box-shadow: 0 0 0 3px rgba(255,215,0,0.1); }
        .form-input[readonly] {
            background: rgba(0,0,0,0.2); color: rgba(255,255,255,0.35);
            cursor: not-allowed; border-color: transparent;
        }
        .form-input[readonly]:focus { border-color: transparent; box-shadow: none; }

        .form-select {
            width: 100%; background: rgba(255,255,255,0.04); border: 1px solid rgba(255,255,255,0.08);
            padding: 13px 16px; border-radius: 10px; color: white; font-size: 14px;
            font-family: inherit; transition: var(--transition-smooth); outline: none;
            appearance: none; cursor: pointer;
            background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='12' fill='%239CA3AF' viewBox='0 0 16 16'%3E%3Cpath d='M8 11L3 6h10l-5 5z'/%3E%3C/svg%3E");
            background-repeat: no-repeat; background-position: right 16px center;
        }
        .form-select:focus { border-color: var(--primary); box-shadow: 0 0 0 3px rgba(255,215,0,0.1); }
        .form-select option { background: #1a1a1e; color: white; }

        .form-textarea {
            width: 100%; background: rgba(255,255,255,0.04); border: 1px solid rgba(255,255,255,0.08);
            padding: 13px 16px; border-radius: 10px; color: white; font-size: 14px;
            font-family: inherit; transition: var(--transition-smooth); outline: none;
            resize: vertical; min-height: 100px;
        }
        .form-textarea:focus { border-color: var(--primary); background: rgba(255,255,255,0.06); box-shadow: 0 0 0 3px rgba(255,215,0,0.1); }

        .char-count { font-size: 11px; color: var(--text-muted); text-align: right; margin-top: -4px; }

        /* ── ACTION BAR ── */
        .action-bar {
            display: flex; justify-content: flex-end; gap: 12px;
            padding: 24px 28px; background: #141417; border: 1px solid var(--border-subtle);
            border-radius: 16px; margin-bottom: 60px;
        }
        .btn-cancel {
            background: rgba(255,255,255,0.04); color: var(--text-muted); border: 1px solid rgba(255,255,255,0.08);
            padding: 12px 28px; border-radius: 10px; font-weight: 700; font-size: 13px;
            cursor: pointer; transition: var(--transition-smooth);
        }
        .btn-cancel:hover { background: rgba(255,255,255,0.08); color: white; border-color: rgba(255,255,255,0.15); }
        .btn-save-profile {
            background: var(--primary); color: #0f0f11; border: none;
            padding: 12px 36px; border-radius: 10px; font-weight: 800; font-size: 13px;
            cursor: pointer; transition: var(--transition-smooth); text-transform: uppercase; letter-spacing: 0.5px;
            position: relative; overflow: hidden;
        }
        .btn-save-profile::after {
            content: ''; position: absolute; top: 0; left: -100%; width: 100%; height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.4), transparent);
            transition: left 0.5s ease;
        }
        .btn-save-profile:hover { background: var(--primary-hover); box-shadow: 0 4px 20px rgba(255,215,0,0.35); transform: translateY(-2px); }
        .btn-save-profile:hover::after { left: 100%; }

        /* ── TOAST ── */
        .toast {
            position: fixed; bottom: 40px; right: 40px; padding: 14px 24px; border-radius: 12px;
            font-weight: 600; font-size: 14px; display: flex; align-items: center; gap: 10px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.5); transform: translateY(100px) scale(0.9); opacity: 0;
            transition: all 0.4s cubic-bezier(0.16,1,0.3,1); z-index: 2000;
            border: 1px solid rgba(255,255,255,0.15); backdrop-filter: blur(10px);
        }
        .toast.success { background: rgba(34,197,94,0.9); color: white; }
        .toast.show { transform: translateY(0) scale(1); opacity: 1; }

        /* ── ANIMATIONS ── */
        .animate-in { opacity: 0; transform: translateY(16px); animation: fadeIn 0.6s cubic-bezier(0.16,1,0.3,1) forwards; }
        .d1 { animation-delay: 0.05s; } .d2 { animation-delay: 0.1s; }
        .d3 { animation-delay: 0.15s; } .d4 { animation-delay: 0.2s; }
    </style>
</head>
<body>

    <!-- SIDEBAR -->
    <aside class="sidebar">
        <div class="sidebar-header">
            <a href="index.html" class="sidebar-brand">CAMPUSELITE</a>
        </div>
        <div class="portal-title">
            <h3>Student Portal</h3>
            <p>Elite Concierge Service</p>
        </div>
        <nav class="sidebar-nav">
            <a href="student_dashboard.html" class="nav-item">
                <span class="nav-icon">🛒</span> Cart Items
            </a>
            <a href="#" class="nav-item">
                <span class="nav-icon">🔖</span> Saved for Later
            </a>
            <a href="#" class="nav-item">
                <span class="nav-icon">📖</span> Academic Services
            </a>
            <a href="#" class="nav-item">
                <span class="nav-icon">🕒</span> Order History
            </a>
        </nav>
        <div class="user-profile" id="userProfileBtn">
            <img src="https://ui-avatars.com/api/?name=Alex+Chen&background=222&color=fff" alt="Alex Chen" class="user-avatar" id="sidebarAvatar">
            <div class="user-info">
                <h3 id="sidebarName">Alex Chen</h3>
                <p>Premium Member</p>
            </div>
        </div>
    </aside>

    <!-- MAIN CONTENT -->
    <main class="main-content">
        <!-- TOP NAVBAR -->
        <header class="top-navbar">
            <div class="top-nav-links">
                <a href="student_dashboard.html" class="top-link">Shop</a>
                <a href="student_renting.html" class="top-link">Rentals</a>
                <a href="student_sell.html" class="top-link">Sell</a>
            </div>
            <div class="top-actions">
                <button class="icon-btn">🔔</button>
                <button class="icon-btn">👤</button>
            </div>
        </header>

        <!-- PROFILE PAGE -->
        <div class="profile-page">

            <!-- Page Header -->
            <div class="page-header animate-in">
                <a href="student_dashboard.html" class="back-btn" title="Back to Dashboard">←</a>
                <div>
                    <h1 class="page-title">Edit Profile</h1>
                    <p class="page-subtitle">Manage your personal information and preferences</p>
                </div>
            </div>

            <!-- Avatar Section -->
            <div class="avatar-section animate-in d1">
                <div class="avatar-wrapper" onclick="document.getElementById('avatarInput').click()">
                    <img src="https://ui-avatars.com/api/?name=Alex+Chen&background=222&color=fff&size=220" alt="Profile" class="avatar-large" id="avatarPreview">
                    <div class="avatar-edit-overlay">📷</div>
                </div>
                <input type="file" id="avatarInput" style="display:none;" accept="image/*">
                <div class="avatar-details">
                    <h2 id="displayName">Alex Chen</h2>
                    <span class="role-badge">Premium Member</span>
                    <div class="avatar-meta">
                        <span>ID:</span> 2021-00123 &nbsp;·&nbsp; <span>Course:</span> BS Computer Science<br>
                        <span>Joined:</span> September 2021
                    </div>
                </div>
                <div class="avatar-actions">
                    <button class="btn-upload" onclick="document.getElementById('avatarInput').click()">Upload Photo</button>
                    <button class="btn-remove" id="removeAvatarBtn">Remove</button>
                </div>
            </div>

            <!-- Personal Information -->
            <div class="form-section animate-in d2">
                <div class="form-section-header">
                    <span class="form-section-icon">👤</span>
                    <h3 class="form-section-title">Personal Information</h3>
                    <span class="form-section-desc">Fields with 🔒 cannot be changed</span>
                </div>
                <div class="form-grid">
                    <div class="form-group">
                        <label class="form-label">First Name</label>
                        <input type="text" class="form-input" id="firstName" value="Alex">
                    </div>
                    <div class="form-group">
                        <label class="form-label">Last Name</label>
                        <input type="text" class="form-input" id="lastName" value="Chen">
                    </div>
                    <div class="form-group">
                        <label class="form-label">Student ID <span class="lock-icon">🔒</span></label>
                        <input type="text" class="form-input" value="2021-00123" readonly>
                    </div>
                    <div class="form-group">
                        <label class="form-label">Course / Program <span class="lock-icon">🔒</span></label>
                        <input type="text" class="form-input" value="BS Computer Science" readonly>
                    </div>
                    <div class="form-group">
                        <label class="form-label">Year Level</label>
                        <select class="form-select" id="yearLevel">
                            <option>1st Year</option>
                            <option>2nd Year</option>
                            <option selected>3rd Year</option>
                            <option>4th Year</option>
                            <option>5th Year</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label class="form-label">Section</label>
                        <input type="text" class="form-input" id="section" value="3-A">
                    </div>
                </div>
            </div>

            <!-- Contact Information -->
            <div class="form-section animate-in d3">
                <div class="form-section-header">
                    <span class="form-section-icon">📧</span>
                    <h3 class="form-section-title">Contact Information</h3>
                </div>
                <div class="form-grid">
                    <div class="form-group">
                        <label class="form-label">Email Address <span class="lock-icon">🔒</span></label>
                        <input type="email" class="form-input" value="alex.chen@university.edu" readonly>
                    </div>
                    <div class="form-group">
                        <label class="form-label">Phone Number</label>
                        <input type="tel" class="form-input" id="phone" value="+63 912 345 6789" placeholder="+63 9XX XXX XXXX">
                    </div>
                    <div class="form-group full-width">
                        <label class="form-label">Address</label>
                        <input type="text" class="form-input" id="address" value="123 University Blvd, Campus City" placeholder="Enter your address">
                    </div>
                </div>
            </div>

            <!-- Bio / About -->
            <div class="form-section animate-in d4">
                <div class="form-section-header">
                    <span class="form-section-icon">✏️</span>
                    <h3 class="form-section-title">About You</h3>
                </div>
                <div class="form-grid">
                    <div class="form-group full-width">
                        <label class="form-label">Bio</label>
                        <textarea class="form-textarea" id="bioInput" maxlength="200" placeholder="Tell other students a bit about yourself...">CS student, tech enthusiast, and coffee addict. Always looking for good deals on campus!</textarea>
                    </div>
                    <div class="char-count full-width"><span id="charCount">0</span> / 200</div>
                </div>
            </div>

            <!-- Action Bar -->
            <div class="action-bar animate-in d4">
                <button class="btn-cancel" onclick="window.location.href='student_dashboard.html'">Cancel</button>
                <button class="btn-save-profile" id="saveBtn">Save Changes</button>
            </div>

        </div>
    </main>

    <!-- Toast Notification -->
    <div class="toast success" id="toast">✓ &nbsp;Profile updated successfully!</div>

    <script>
        // ── Avatar Upload Preview ──
        const avatarInput = document.getElementById('avatarInput');
        const avatarPreview = document.getElementById('avatarPreview');
        const sidebarAvatar = document.getElementById('sidebarAvatar');
        const removeAvatarBtn = document.getElementById('removeAvatarBtn');
        const defaultAvatar = 'https://ui-avatars.com/api/?name=Alex+Chen&background=222&color=fff&size=220';

        avatarInput.addEventListener('change', function () {
            if (this.files && this.files[0]) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    avatarPreview.src = e.target.result;
                    sidebarAvatar.src = e.target.result;
                };
                reader.readAsDataURL(this.files[0]);
            }
        });

        removeAvatarBtn.addEventListener('click', () => {
            avatarPreview.src = defaultAvatar;
            sidebarAvatar.src = defaultAvatar.replace('&size=220', '');
            avatarInput.value = '';
        });

        // ── Bio character count ──
        const bioInput = document.getElementById('bioInput');
        const charCount = document.getElementById('charCount');
        function updateCount() { charCount.textContent = bioInput.value.length; }
        bioInput.addEventListener('input', updateCount);
        updateCount();

        // ── Live name sync ──
        const firstName = document.getElementById('firstName');
        const lastName = document.getElementById('lastName');
        const displayName = document.getElementById('displayName');
        const sidebarName = document.getElementById('sidebarName');

        function syncName() {
            const full = (firstName.value + ' ' + lastName.value).trim();
            displayName.textContent = full || 'Your Name';
            sidebarName.textContent = full || 'Your Name';
        }
        firstName.addEventListener('input', syncName);
        lastName.addEventListener('input', syncName);

        // ── Save ──
        const saveBtn = document.getElementById('saveBtn');
        const toast = document.getElementById('toast');

        saveBtn.addEventListener('click', () => {
            // Animate button
            saveBtn.textContent = 'Saving...';
            saveBtn.style.pointerEvents = 'none';

            setTimeout(() => {
                saveBtn.textContent = 'Save Changes';
                saveBtn.style.pointerEvents = 'auto';

                // Show toast
                toast.classList.add('show');
                setTimeout(() => toast.classList.remove('show'), 3000);
            }, 800);
        });

        // ── Sidebar nav click ──
        document.querySelectorAll('.sidebar-nav .nav-item').forEach(item => {
            item.addEventListener('click', function (e) {
                if (this.getAttribute('href') === '#') e.preventDefault();
                document.querySelectorAll('.sidebar-nav .nav-item').forEach(n => n.classList.remove('active'));
                this.classList.add('active');
            });
        });
    </script>
</body>
</html>


<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Campus Elite | Student Portal</title>
    <link rel="stylesheet" href="globals.css">
    <style>
        /* DASHBOARD LAYOUT */
        body {
            display: flex;
            min-height: 100vh;
            overflow: hidden; /* Hide body scroll, handle scroll in main content */
            background-color: #0a0a0c; /* Slightly darker than base for contrast */
        }

        /* SIDEBAR */
        .sidebar {
            width: 260px;
            background: #121414; /* Match the dark gray of the sidebar in image */
            border-right: 1px solid var(--border-subtle);
            display: flex;
            flex-direction: column;
            z-index: 100;
            transition: var(--transition-smooth);
        }

        .sidebar-header {
            padding: 24px;
            display: flex;
            align-items: center;
        }

        .sidebar-brand {
            font-size: 20px;
            font-weight: 800;
            color: var(--primary);
            text-decoration: none;
            letter-spacing: 0.5px;
            text-transform: uppercase;
        }

        .user-profile {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 16px 24px;
            border-bottom: 1px solid rgba(255,255,255,0.05);
            margin-bottom: 16px;
        }

        .user-avatar {
            width: 40px;
            height: 40px;
            border-radius: 50%;
            background: #fff;
            object-fit: cover;
            border: 2px solid var(--primary);
        }

        .user-info h3 {
            font-size: 14px;
            margin-bottom: 2px;
            color: var(--primary);
        }

        .user-info p {
            font-size: 11px;
            color: var(--text-muted);
            font-weight: 600;
        }

        .sidebar-nav {
            flex: 1;
            padding: 0 16px;
            display: flex;
            flex-direction: column;
            gap: 4px;
        }

        .nav-item {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 12px 16px;
            color: var(--text-muted);
            text-decoration: none;
            font-size: 14px;
            font-weight: 600;
            border-radius: 12px;
            transition: var(--transition-smooth);
        }

        .nav-item:hover, .nav-item.active {
            background: rgba(255, 255, 255, 0.05);
            color: white;
        }

        .nav-icon {
            font-size: 18px;
            width: 24px;
            text-align: center;
        }

        .sidebar-footer {
            padding: 24px 16px;
            display: flex;
            flex-direction: column;
            gap: 12px;
        }

        .btn-post {
            background: var(--primary);
            color: #0f0f11;
            border: none;
            padding: 12px;
            border-radius: 10px;
            font-weight: 800;
            font-size: 14px;
            cursor: pointer;
            transition: var(--transition-smooth);
            text-align: center;
            text-decoration: none;
            width: 100%;
            margin-bottom: 12px;
        }

        .btn-post:hover {
            background: var(--primary-hover);
            transform: translateY(-2px);
            box-shadow: 0 8px 20px rgba(255, 215, 0, 0.2);
        }

        .footer-link {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 8px 16px;
            color: var(--text-muted);
            text-decoration: none;
            font-size: 13px;
            font-weight: 600;
            transition: var(--transition-smooth);
        }

        .footer-link:hover {
            color: white;
        }

        /* MAIN CONTENT */
        .main-content {
            flex: 1;
            display: flex;
            flex-direction: column;
            overflow-y: auto;
            background-color: var(--bg-base);
            background-image: var(--bg-radial);
        }

        /* TOP NAVBAR */
        .top-navbar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 20px 40px;
            background: rgba(15, 15, 17, 0.8);
            backdrop-filter: blur(24px);
            -webkit-backdrop-filter: blur(24px);
            border-bottom: 1px solid var(--border-subtle);
            position: sticky;
            top: 0;
            z-index: 50;
        }

        .top-nav-links {
            display: flex;
            gap: 32px;
            align-items: center;
        }

        .top-link {
            color: var(--text-muted);
            text-decoration: none;
            font-size: 15px;
            font-weight: 700;
            transition: var(--transition-smooth);
            padding-bottom: 4px;
            border-bottom: 2px solid transparent;
        }

        .top-link:hover {
            color: white;
        }

        .top-link.active {
            color: white;
            border-bottom-color: var(--primary);
        }

        .top-actions {
            display: flex;
            align-items: center;
            gap: 20px;
        }

        .search-bar {
            position: relative;
            width: 280px;
        }

        .search-bar input {
            width: 100%;
            background: rgba(255,255,255,0.05);
            border: 1px solid rgba(255,255,255,0.1);
            padding: 10px 16px 10px 40px;
            border-radius: 20px;
            color: white;
            font-size: 13px;
            transition: var(--transition-smooth);
            font-family: inherit;
        }

        .search-bar input:focus {
            background: rgba(255,255,255,0.08);
            border-color: rgba(255,255,255,0.2);
            outline: none;
            box-shadow: 0 0 10px rgba(255,255,255,0.05);
        }

        .search-bar .search-icon {
            position: absolute;
            left: 14px;
            top: 50%;
            transform: translateY(-50%);
            color: var(--text-muted);
            font-size: 14px;
        }

        .icon-btn {
            background: transparent;
            border: none;
            color: var(--primary);
            font-size: 18px;
            cursor: pointer;
            transition: var(--transition-smooth);
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
        }

        .icon-btn:hover {
            transform: scale(1.1);
            filter: drop-shadow(0 0 8px var(--primary-glow));
        }

        /* DASHBOARD CONTENT */
        .dashboard-container {
            padding: 40px;
            max-width: 1400px;
            margin: 0 auto;
            width: 100%;
        }

        .welcome-text {
            color: var(--text-muted);
            font-size: 16px;
            line-height: 1.6;
            max-width: 800px;
            margin-bottom: 40px;
        }

        .section-header {
            display: flex;
            justify-content: space-between;
            align-items: flex-end;
            margin-bottom: 24px;
        }

        .section-title {
            font-size: 24px;
            color: var(--primary);
            font-weight: 800;
        }

        .view-all {
            color: var(--primary);
            text-transform: uppercase;
            font-size: 11px;
            font-weight: 800;
            letter-spacing: 1px;
            text-decoration: none;
            transition: var(--transition-smooth);
        }

        .view-all:hover {
            text-shadow: 0 0 8px var(--primary-glow);
        }

        /* RENTALS WIDGETS */
        .rentals-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
            gap: 20px;
            margin-bottom: 40px;
        }

        .rental-card {
            background: #1a1a1e;
            border-radius: 16px;
            padding: 24px;
            position: relative;
            border: 1px solid var(--border-subtle);
            transition: var(--transition-smooth);
            cursor: pointer;
            display: flex;
            flex-direction: column;
            gap: 16px;
        }

        .rental-card:hover {
            transform: translateY(-4px);
            border-color: var(--border-accent);
            box-shadow: 0 12px 30px rgba(0,0,0,0.5);
        }

        .rental-badge {
            background: rgba(255, 215, 0, 0.1);
            color: var(--primary);
            padding: 4px 10px;
            border-radius: 6px;
            font-size: 10px;
            font-weight: 800;
            text-transform: uppercase;
            width: fit-content;
        }

        .rental-title {
            font-size: 16px;
            color: white;
            font-weight: 700;
            margin-bottom: 4px;
        }

        .rental-sub {
            font-size: 13px;
            color: var(--text-muted);
        }

        .rental-meta {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-top: auto;
            padding-top: 16px;
        }

        .access-type {
            display: flex;
            align-items: center;
            gap: 8px;
            font-size: 13px;
            color: white;
            font-weight: 600;
        }

        .access-icon {
            width: 32px;
            height: 32px;
            background: rgba(255,255,255,0.05);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 10px;
            color: var(--text-muted);
        }

        .progress-bar {
            width: 100%;
            height: 4px;
            background: rgba(255,255,255,0.1);
            border-radius: 2px;
            overflow: hidden;
            margin-top: auto;
        }

        .progress-fill {
            height: 100%;
            background: var(--primary);
            width: 60%;
            border-radius: 2px;
            box-shadow: 0 0 10px var(--primary-glow);
        }

        .new-booking-card {
            background: rgba(255, 215, 0, 0.05);
            border: 1px dashed var(--border-accent);
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            text-align: center;
            gap: 12px;
            transition: var(--transition-smooth);
        }

        .new-booking-card:hover {
            background: rgba(255, 215, 0, 0.08);
            border-style: solid;
        }

        .new-icon {
            font-size: 28px;
            color: var(--primary);
        }

        .new-title {
            font-size: 16px;
            font-weight: 700;
            color: white;
        }

        .new-sub {
            font-size: 12px;
            color: var(--text-muted);
        }

        /* EQUIPMENT GRID */
        .filter-pill {
            background: var(--primary);
            color: #0f0f11;
            padding: 8px 20px;
            border-radius: 20px;
            font-weight: 800;
            font-size: 13px;
            display: inline-block;
            margin-bottom: 24px;
            cursor: pointer;
            box-shadow: 0 0 15px rgba(255,215,0,0.2);
        }

        .equipment-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));
            gap: 24px;
        }

        .eq-card {
            background: var(--card-bg);
            border: 1px solid var(--border-subtle);
            border-radius: 16px;
            overflow: hidden;
            display: flex;
            flex-direction: column;
            transition: var(--transition-smooth);
        }

        .eq-card:hover {
            transform: translateY(-5px);
            border-color: var(--border-accent);
            box-shadow: 0 15px 35px rgba(0,0,0,0.6);
        }

        .eq-img-container {
            height: 180px;
            width: 100%;
            position: relative;
            background: #222;
        }

        .eq-img-container img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

        .eq-badge {
            position: absolute;
            bottom: 12px;
            left: 12px;
            background: rgba(0,0,0,0.8);
            color: var(--primary);
            padding: 4px 10px;
            border-radius: 6px;
            font-size: 10px;
            font-weight: 800;
            border: 1px solid var(--border-accent);
            text-transform: uppercase;
        }

        .eq-content {
            padding: 20px;
            display: flex;
            flex-direction: column;
            gap: 10px;
            flex: 1;
        }

        .eq-header {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
        }

        .eq-title {
            font-size: 16px;
            font-weight: 700;
            color: white;
            line-height: 1.3;
        }

        .eq-price {
            font-size: 14px;
            color: var(--primary);
            font-weight: 800;
            text-align: right;
        }
        
        .eq-price span {
            font-size: 11px;
            color: var(--text-muted);
            font-weight: 600;
        }

        .eq-desc {
            font-size: 13px;
            color: var(--text-muted);
            line-height: 1.5;
            flex: 1;
        }

        .btn-rent {
            width: 100%;
            background: var(--primary);
            color: #0f0f11;
            border: none;
            padding: 12px;
            border-radius: 10px;
            font-weight: 800;
            font-size: 13px;
            cursor: pointer;
            transition: var(--transition-smooth);
            margin-top: 8px;
        }

        .btn-rent:hover {
            background: var(--primary-hover);
            box-shadow: 0 0 15px rgba(255,215,0,0.3);
        }

        /* MORE OPTIONS ICON */
        .more-icon {
            position: absolute;
            top: 20px;
            right: 20px;
            color: var(--text-muted);
            cursor: pointer;
            font-size: 18px;
            display: flex;
            flex-direction: column;
            gap: 3px;
            padding: 4px;
        }
        .more-dot {
            width: 3px;
            height: 3px;
            background: currentColor;
            border-radius: 50%;
        }
        
        @media(max-width: 992px) {
            .sidebar {
                position: fixed;
                left: -260px;
                height: 100%;
            }
            .sidebar.open {
                left: 0;
            }
        }

        /* PROFILE MODAL */
        .modal-overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.8);
            backdrop-filter: blur(5px);
            z-index: 1000;
            align-items: center;
            justify-content: center;
            opacity: 0;
            transition: opacity 0.3s ease;
        }
        .modal-overlay.active {
            display: flex;
            opacity: 1;
        }
        .profile-modal {
            background: #141417;
            border: 1px solid var(--border-subtle);
            border-radius: 20px;
            width: 100%;
            max-width: 450px;
            padding: 30px;
            transform: translateY(20px);
            transition: transform 0.3s ease;
            box-shadow: 0 20px 40px rgba(0,0,0,0.5);
        }
        .modal-overlay.active .profile-modal {
            transform: translateY(0);
        }
        .modal-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 24px;
        }
        .modal-title {
            font-size: 20px;
            color: white;
            font-weight: 700;
        }
        .close-modal {
            background: transparent;
            border: none;
            color: var(--text-muted);
            font-size: 24px;
            cursor: pointer;
            transition: var(--transition-smooth);
        }
        .close-modal:hover {
            color: white;
        }
        .profile-pic-edit {
            display: flex;
            flex-direction: column;
            align-items: center;
            margin-bottom: 24px;
            gap: 12px;
        }
        .profile-pic-preview {
            width: 100px;
            height: 100px;
            border-radius: 50%;
            object-fit: cover;
            border: 3px solid var(--primary);
        }
        .btn-outline {
            background: transparent;
            border: 1px solid var(--primary);
            color: var(--primary);
            padding: 8px 16px;
            border-radius: 8px;
            font-size: 12px;
            font-weight: 700;
            cursor: pointer;
            transition: var(--transition-smooth);
        }
        .btn-outline:hover {
            background: rgba(255, 215, 0, 0.1);
        }
        .modal-form-group {
            margin-bottom: 16px;
        }
        .modal-label {
            display: block;
            font-size: 12px;
            color: var(--text-muted);
            margin-bottom: 8px;
            font-weight: 600;
            text-transform: uppercase;
        }
        .modal-input {
            width: 100%;
            background: rgba(255,255,255,0.05);
            border: 1px solid rgba(255,255,255,0.1);
            padding: 12px 16px;
            border-radius: 10px;
            color: white;
            font-size: 14px;
            font-family: inherit;
        }
        .modal-input:focus {
            outline: none;
            border-color: var(--primary);
        }
        .modal-input[readonly] {
            background: rgba(0,0,0,0.2);
            color: rgba(255,255,255,0.4);
            cursor: not-allowed;
            border-color: transparent;
        }
        .btn-save {
            width: 100%;
            background: var(--primary);
            color: #0f0f11;
            border: none;
            padding: 14px;
            border-radius: 10px;
            font-size: 14px;
            font-weight: 800;
            cursor: pointer;
            margin-top: 8px;
            transition: var(--transition-smooth);
        }
        .btn-save:hover {
            background: var(--primary-hover);
        }
    </style>
</head>
<body>

    <!-- SIDEBAR -->
    <aside class="sidebar">
        <div class="sidebar-header">
            <a href="#" class="sidebar-brand">CAMPUS ELITE</a>
        </div>
        
        <div class="user-profile" id="userProfileBtn" style="cursor: pointer; transition: background 0.3s;" onmouseover="this.style.background='rgba(255,255,255,0.05)'" onmouseout="this.style.background='transparent'">
            <img src="https://ui-avatars.com/api/?name=Alex+Chen&background=FFD700&color=0f0f11&bold=true" alt="User" class="user-avatar" id="sidebarAvatar">
            <div class="user-info">
                <h3 id="sidebarName">Alex Chen</h3>
                <p>Elite Member</p>
            </div>
        </div>

        <nav class="sidebar-nav">
            <a href="#" class="nav-item"><span class="nav-icon">🛍️</span> Cart</a>
            <a href="#" class="nav-item"><span class="nav-icon">🔖</span> Saved</a>
            <a href="#" class="nav-item"><span class="nav-icon">🎓</span> Academic</a>
            <a href="#" class="nav-item"><span class="nav-icon">🕒</span> History</a>
        </nav>

        <div class="sidebar-footer">
            <a href="#" class="btn-post">Post Listing</a>
            <a href="#" class="footer-link"><span class="nav-icon">⚙️</span> Settings</a>
            <a href="#" class="footer-link"><span class="nav-icon">❓</span> Support</a>
        </div>
    </aside>

    <!-- MAIN CONTENT -->
    <main class="main-content">
        <!-- TOP NAVBAR -->
        <header class="top-navbar">
            <div class="top-nav-links">
                <a href="student_dashboard.html" class="top-link">Shop</a>
                <a href="student_renting.html" class="top-link active">Rentals</a>
                <a href="student_sell.html" class="top-link">Sell</a>
            </div>

            <div class="top-actions">
                <div class="search-bar">
                    <span class="search-icon">🔍</span>
                    <input type="text" placeholder="Search equipment...">
                </div>
                <button class="icon-btn">🔔</button>
                <button class="icon-btn">🛒</button>
            </div>
        </header>

        <!-- DASHBOARD CONTAINER -->
        <div class="dashboard-container">
            <p class="welcome-text animate-fade-in">
                Premium access to essential academic tools, high-end laboratory equipment, and exclusive campus spaces.
            </p>

            <div class="section-header animate-fade-in delay-1">
                <h2 class="section-title">My Active Rentals</h2>
                <a href="#" class="view-all">VIEW ALL STATUS</a>
            </div>

            <div class="rentals-grid animate-fade-in delay-1">
                <!-- Active Rental 1 -->
                <div class="rental-card">
                    <div class="more-icon">
                        <div class="more-dot"></div><div class="more-dot"></div><div class="more-dot"></div>
                    </div>
                    <span class="rental-badge">DUE IN 3 DAYS</span>
                    <div>
                        <h3 class="rental-title">Premium Locker #402</h3>
                        <p class="rental-sub">Engineering Hall, Level 2</p>
                    </div>
                    <div class="rental-meta">
                        <div class="access-type">
                            <div class="access-icon">Access</div>
                            Digital Key
                        </div>
                    </div>
                </div>

                <!-- Active Rental 2 -->
                <div class="rental-card">
                    <div class="more-icon">
                        <div class="more-dot"></div><div class="more-dot"></div><div class="more-dot"></div>
                    </div>
                    <span class="rental-badge" style="color:var(--text-muted); background:rgba(255,255,255,0.05); border:1px solid var(--border-subtle)">SESSION ENDS 2:00 PM</span>
                    <div>
                        <h3 class="rental-title">Mass Spectrometer</h3>
                        <p class="rental-sub">Chemistry Lab B</p>
                    </div>
                    <div class="progress-bar">
                        <div class="progress-fill"></div>
                    </div>
                </div>

                <!-- New Booking -->
                <div class="rental-card new-booking-card">
                    <div class="new-icon">⊕</div>
                    <div>
                        <h3 class="new-title">New Booking</h3>
                        <p class="new-sub">Explore available spaces</p>
                    </div>
                </div>
            </div>

            <div class="animate-fade-in delay-2">
                <span class="filter-pill">All Equipment</span>
            </div>

            <div class="equipment-grid animate-fade-in delay-2">
                <!-- Equipment 1 -->
                <div class="eq-card">
                    <div class="eq-img-container">
                        <img src="https://images.unsplash.com/photo-1581093458791-9f3c3900df4b?q=80&w=2070&auto=format&fit=crop" alt="Microscope">
                        <span class="eq-badge">SCIENCE</span>
                    </div>
                    <div class="eq-content">
                        <div class="eq-header">
                            <h3 class="eq-title">Nikon ECLIPSE<br>Ti2</h3>
                            <div class="eq-price">$45<span>/hr</span></div>
                        </div>
                        <p class="eq-desc">High-performance inverted research microscope system.</p>
                        <button class="btn-rent">Rent Now</button>
                    </div>
                </div>

                <!-- Equipment 2 -->
                <div class="eq-card">
                    <div class="eq-img-container">
                        <img src="https://images.unsplash.com/photo-1497366216548-37526070297c?q=80&w=2069&auto=format&fit=crop" alt="Meeting Room">
                        <span class="eq-badge">SPACES</span>
                    </div>
                    <div class="eq-content">
                        <div class="eq-header">
                            <h3 class="eq-title">Skyline Suite<br>4A</h3>
                            <div class="eq-price">$120<span>/day</span></div>
                        </div>
                        <p class="eq-desc">Private study lounge with panoramic campus views.</p>
                        <button class="btn-rent">Rent Now</button>
                    </div>
                </div>

                <!-- Equipment 3 -->
                <div class="eq-card">
                    <div class="eq-img-container">
                        <img src="https://images.unsplash.com/photo-1580584126903-c17d41830450?q=80&w=1939&auto=format&fit=crop" alt="Lockers">
                        <span class="eq-badge">STORAGE</span>
                    </div>
                    <div class="eq-content">
                        <div class="eq-header">
                            <h3 class="eq-title">Elite Storage<br>Pod</h3>
                            <div class="eq-price">$15<span>/mo</span></div>
                        </div>
                        <p class="eq-desc">Climate controlled, biometric access, and 24/7 security.</p>
                        <button class="btn-rent">Rent Now</button>
                    </div>
                </div>

                <!-- Equipment 4 -->
                <div class="eq-card">
                    <div class="eq-img-container">
                        <img src="https://images.unsplash.com/photo-1600861194942-f883de0dfe96?q=80&w=2069&auto=format&fit=crop" alt="Workstation">
                        <span class="eq-badge">MEDIA</span>
                    </div>
                    <div class="eq-content">
                        <div class="eq-header">
                            <h3 class="eq-title">VFX<br>Workstation</h3>
                            <div class="eq-price">$30<span>/hr</span></div>
                        </div>
                        <p class="eq-desc">RTX 4090 equipped studio rig for 3D rendering.</p>
                        <button class="btn-rent">Rent Now</button>
                    </div>
                </div>
            </div>

        </div>
    </main>

    <!-- PROFILE EDIT MODAL -->
    <div class="modal-overlay" id="profileModal">
        <div class="profile-modal">
            <div class="modal-header">
                <h2 class="modal-title">Edit Profile</h2>
                <button class="close-modal" id="closeProfileModal">&times;</button>
            </div>
            
            <div class="profile-pic-edit">
                <img src="https://ui-avatars.com/api/?name=Alex+Chen&background=FFD700&color=0f0f11&bold=true" alt="Profile" class="profile-pic-preview" id="modalPicPreview">
                <button class="btn-outline" onclick="document.getElementById('profilePicInput').click()">Change Picture</button>
                <input type="file" id="profilePicInput" style="display: none;" accept="image/*">
            </div>

            <div class="modal-form-group">
                <label class="modal-label">Full Name</label>
                <input type="text" class="modal-input" id="profileNameInput" value="Alex Chen">
            </div>

            <div class="modal-form-group">
                <label class="modal-label">ID Number</label>
                <input type="text" class="modal-input" value="2021-00123" readonly>
            </div>

            <div class="modal-form-group">
                <label class="modal-label">Course</label>
                <input type="text" class="modal-input" value="BS Computer Science" readonly>
            </div>

            <button class="btn-save" id="saveProfileBtn">Save Changes</button>
        </div>
    </div>

    <script>
        // Sidebar Interactivity
        const navItems = document.querySelectorAll('.sidebar-nav .nav-item');
        navItems.forEach(item => {
            item.addEventListener('click', (e) => {
                e.preventDefault();
                navItems.forEach(nav => nav.classList.remove('active'));
                item.classList.add('active');
            });
        });

        // Profile Modal Logic
        const profileModal = document.getElementById('profileModal');
        const userProfileBtn = document.getElementById('userProfileBtn');
        const closeProfileModal = document.getElementById('closeProfileModal');
        const saveProfileBtn = document.getElementById('saveProfileBtn');
        const profilePicInput = document.getElementById('profilePicInput');
        const modalPicPreview = document.getElementById('modalPicPreview');
        const sidebarAvatar = document.getElementById('sidebarAvatar');
        const sidebarName = document.getElementById('sidebarName');
        const profileNameInput = document.getElementById('profileNameInput');

        // Open Modal
        userProfileBtn.addEventListener('click', () => {
            profileModal.classList.add('active');
        });

        // Close Modal
        closeProfileModal.addEventListener('click', () => {
            profileModal.classList.remove('active');
        });

        // Close when clicking outside
        profileModal.addEventListener('click', (e) => {
            if(e.target === profileModal) {
                profileModal.classList.remove('active');
            }
        });

        // Handle Image Upload Preview
        profilePicInput.addEventListener('change', function() {
            if (this.files && this.files[0]) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    modalPicPreview.src = e.target.result;
                }
                reader.readAsDataURL(this.files[0]);
            }
        });

        // Save Profile
        saveProfileBtn.addEventListener('click', () => {
            if(profileNameInput.value.trim() !== '') {
                sidebarName.textContent = profileNameInput.value;
            }
            sidebarAvatar.src = modalPicPreview.src;
            profileModal.classList.remove('active');
        });
    </script>
</body>
</html>


<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Campus Elite | Sell</title>
    <link rel="stylesheet" href="globals.css">
    <style>
        /* DASHBOARD LAYOUT */
        body {
            display: flex;
            min-height: 100vh;
            overflow: hidden;
            background-color: #0f0f11;
        }

        /* SIDEBAR (Match new Shop page) */
        .sidebar {
            width: 260px;
            background: #121214; 
            border-right: 1px solid var(--border-subtle);
            display: flex;
            flex-direction: column;
            z-index: 100;
            transition: var(--transition-smooth);
        }

        .sidebar-header {
            padding: 24px;
            display: flex;
            align-items: center;
        }

        .sidebar-brand {
            font-size: 20px;
            font-weight: 800;
            color: var(--primary);
            text-decoration: none;
            letter-spacing: 0.5px;
            text-transform: uppercase;
        }

        .portal-title {
            padding: 0 24px 16px;
        }

        .portal-title h3 {
            font-size: 16px;
            color: var(--primary);
            margin-bottom: 2px;
        }

        .portal-title p {
            font-size: 11px;
            color: var(--text-muted);
            font-weight: 600;
        }

        .sidebar-nav {
            flex: 1;
            display: flex;
            flex-direction: column;
        }

        .nav-item {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 16px 24px;
            color: var(--text-muted);
            text-decoration: none;
            font-size: 14px;
            font-weight: 600;
            transition: var(--transition-smooth);
        }
        
        .nav-item:hover {
            background: rgba(255, 255, 255, 0.05);
            color: white;
        }

        .nav-icon {
            font-size: 18px;
            width: 24px;
            text-align: center;
        }

        .user-profile {
            display: flex;
            align-items: center;
            gap: 12px;
            padding: 20px 24px;
            border-top: 1px solid rgba(255,255,255,0.05);
            margin-top: auto;
        }

        .user-avatar {
            width: 36px;
            height: 36px;
            border-radius: 50%;
            background: #222;
            object-fit: cover;
        }

        .user-info h3 {
            font-size: 13px;
            margin-bottom: 2px;
            color: white;
        }

        .user-info p {
            font-size: 10px;
            color: var(--primary);
            font-weight: 800;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        /* MAIN CONTENT */
        .main-content {
            flex: 1;
            display: flex;
            flex-direction: column;
            overflow-y: auto;
            background-color: var(--bg-base);
            position: relative;
            background-image: var(--bg-radial);
        }

        /* TOP NAVBAR */
        .top-navbar {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 20px 40px;
            background: rgba(15, 15, 17, 0.9);
            backdrop-filter: blur(24px);
            -webkit-backdrop-filter: blur(24px);
            border-bottom: 1px solid var(--border-subtle);
            position: sticky;
            top: 0;
            z-index: 50;
        }

        .top-nav-links {
            display: flex;
            gap: 32px;
            align-items: center;
            margin-left: 40px;
        }

        .top-link {
            color: var(--text-muted);
            text-decoration: none;
            font-size: 14px;
            font-weight: 600;
            transition: var(--transition-smooth);
            padding-bottom: 4px;
            border-bottom: 2px solid transparent;
        }

        .top-link:hover {
            color: white;
        }

        .top-link.active {
            color: var(--primary);
            border-bottom-color: var(--primary);
        }

        .top-actions {
            display: flex;
            align-items: center;
            gap: 20px;
        }

        .icon-btn {
            background: transparent;
            border: none;
            color: var(--text-muted);
            font-size: 18px;
            cursor: pointer;
            transition: var(--transition-smooth);
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .icon-btn:hover {
            color: white;
        }

        /* SELL CONTAINER */
        .dashboard-container {
            padding: 40px;
            max-width: 900px; /* Thinner for forms */
            margin: 0 auto;
            width: 100%;
        }

        .form-header {
            text-align: center;
            margin-bottom: 40px;
        }

        .form-title {
            font-size: 32px;
            color: white;
            font-weight: 800;
            margin-bottom: 12px;
        }

        .form-subtitle {
            font-size: 15px;
            color: var(--text-muted);
            max-width: 600px;
            margin: 0 auto;
        }

        /* FORM STYLING */
        .sell-form {
            background: var(--card-bg);
            border: 1px solid var(--border-subtle);
            border-radius: 20px;
            padding: 40px;
            backdrop-filter: blur(12px);
        }

        .form-group {
            margin-bottom: 24px;
        }

        .form-row {
            display: flex;
            gap: 24px;
            margin-bottom: 24px;
        }

        .form-row .form-group {
            flex: 1;
            margin-bottom: 0;
        }

        .form-label {
            display: block;
            font-size: 13px;
            color: var(--text-muted);
            font-weight: 700;
            margin-bottom: 8px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .form-input, .form-select, .form-textarea {
            width: 100%;
            background: rgba(0, 0, 0, 0.4);
            border: 1px solid rgba(255, 255, 255, 0.1);
            padding: 14px 18px;
            border-radius: 12px;
            color: white;
            font-size: 15px;
            font-family: inherit;
            transition: var(--transition-smooth);
        }

        .form-input:focus, .form-select:focus, .form-textarea:focus {
            outline: none;
            border-color: var(--primary);
            background: rgba(0, 0, 0, 0.6);
            box-shadow: 0 0 0 3px rgba(255, 215, 0, 0.1);
        }

        .form-select {
            appearance: none;
            background-image: url("data:image/svg+xml;charset=UTF-8,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='%239CA3AF' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'%3e%3cpolyline points='6 9 12 15 18 9'%3e%3c/polyline%3e%3c/svg%3e");
            background-repeat: no-repeat;
            background-position: right 16px center;
            background-size: 16px;
        }
        
        .form-select option {
            background: #121214;
            color: white;
        }

        .form-textarea {
            resize: vertical;
            min-height: 120px;
        }

        /* UPLOAD ZONE */
        .upload-zone {
            border: 2px dashed rgba(255, 255, 255, 0.15);
            border-radius: 16px;
            padding: 40px;
            text-align: center;
            background: rgba(0, 0, 0, 0.2);
            transition: var(--transition-smooth);
            cursor: pointer;
            margin-bottom: 32px;
        }

        .upload-zone:hover {
            border-color: var(--primary);
            background: rgba(255, 215, 0, 0.05);
        }

        .upload-icon {
            font-size: 40px;
            color: var(--primary);
            margin-bottom: 16px;
            display: inline-block;
        }

        .upload-text {
            color: white;
            font-size: 16px;
            font-weight: 600;
            margin-bottom: 8px;
        }

        .upload-subtext {
            color: var(--text-muted);
            font-size: 13px;
        }

        .submit-btn {
            width: 100%;
            background: var(--primary);
            color: #0f0f11;
            font-size: 16px;
            font-weight: 800;
            padding: 16px;
            border: none;
            border-radius: 12px;
            cursor: pointer;
            transition: var(--transition-smooth);
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        .submit-btn:hover {
            transform: translateY(-2px);
            background: var(--primary-hover);
            box-shadow: 0 10px 25px rgba(255, 215, 0, 0.3);
        }

        /* PROFILE MODAL */
        .modal-overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.8);
            backdrop-filter: blur(5px);
            z-index: 1000;
            align-items: center;
            justify-content: center;
            opacity: 0;
            transition: opacity 0.3s ease;
        }
        .modal-overlay.active {
            display: flex;
            opacity: 1;
        }
        .profile-modal {
            background: #141417;
            border: 1px solid var(--border-subtle);
            border-radius: 20px;
            width: 100%;
            max-width: 450px;
            padding: 30px;
            transform: translateY(20px);
            transition: transform 0.3s ease;
            box-shadow: 0 20px 40px rgba(0,0,0,0.5);
        }
        .modal-overlay.active .profile-modal {
            transform: translateY(0);
        }
        .modal-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 24px;
        }
        .modal-title {
            font-size: 20px;
            color: white;
            font-weight: 700;
        }
        .close-modal {
            background: transparent;
            border: none;
            color: var(--text-muted);
            font-size: 24px;
            cursor: pointer;
            transition: var(--transition-smooth);
        }
        .close-modal:hover {
            color: white;
        }
        .profile-pic-edit {
            display: flex;
            flex-direction: column;
            align-items: center;
            margin-bottom: 24px;
            gap: 12px;
        }
        .profile-pic-preview {
            width: 100px;
            height: 100px;
            border-radius: 50%;
            object-fit: cover;
            border: 3px solid var(--primary);
        }
        .btn-outline {
            background: transparent;
            border: 1px solid var(--primary);
            color: var(--primary);
            padding: 8px 16px;
            border-radius: 8px;
            font-size: 12px;
            font-weight: 700;
            cursor: pointer;
            transition: var(--transition-smooth);
        }
        .btn-outline:hover {
            background: rgba(255, 215, 0, 0.1);
        }
        .modal-form-group {
            margin-bottom: 16px;
        }
        .modal-label {
            display: block;
            font-size: 12px;
            color: var(--text-muted);
            margin-bottom: 8px;
            font-weight: 600;
            text-transform: uppercase;
        }
        .modal-input {
            width: 100%;
            background: rgba(255,255,255,0.05);
            border: 1px solid rgba(255,255,255,0.1);
            padding: 12px 16px;
            border-radius: 10px;
            color: white;
            font-size: 14px;
            font-family: inherit;
        }
        .modal-input:focus {
            outline: none;
            border-color: var(--primary);
        }
        .modal-input[readonly] {
            background: rgba(0,0,0,0.2);
            color: rgba(255,255,255,0.4);
            cursor: not-allowed;
            border-color: transparent;
        }
        .btn-save {
            width: 100%;
            background: var(--primary);
            color: #0f0f11;
            border: none;
            padding: 14px;
            border-radius: 10px;
            font-size: 14px;
            font-weight: 800;
            cursor: pointer;
            margin-top: 8px;
            transition: var(--transition-smooth);
        }
        .btn-save:hover {
            background: var(--primary-hover);
        }

        /* HISTORY LIST */
        .history-list {
            display: flex;
            flex-direction: column;
            gap: 16px;
        }
        .history-card {
            background: rgba(255,255,255,0.03);
            border: 1px solid var(--border-subtle);
            border-radius: 12px;
            padding: 20px;
            display: flex;
            align-items: center;
            gap: 20px;
            transition: var(--transition-smooth);
        }
        .history-card:hover {
            background: rgba(255,255,255,0.06);
            border-color: rgba(255,255,255,0.15);
        }
        .history-img {
            width: 80px;
            height: 80px;
            border-radius: 8px;
            object-fit: cover;
            background: #222;
        }
        .history-details {
            flex: 1;
        }
        .history-title {
            font-size: 16px;
            font-weight: 700;
            color: white;
            margin-bottom: 4px;
        }
        .history-meta {
            font-size: 13px;
            color: var(--text-muted);
            display: flex;
            gap: 16px;
        }
        .history-price {
            font-size: 16px;
            font-weight: 800;
            color: var(--primary);
        }
        .status-badge {
            padding: 4px 10px;
            border-radius: 20px;
            font-size: 11px;
            font-weight: 800;
            text-transform: uppercase;
        }
        .status-active {
            background: rgba(74, 222, 128, 0.1);
            color: #4ade80;
            border: 1px solid rgba(74, 222, 128, 0.2);
        }
        .status-sold {
            background: rgba(255, 215, 0, 0.1);
            color: var(--primary);
            border: 1px solid rgba(255, 215, 0, 0.2);
        }
        .history-actions {
            display: flex;
            gap: 8px;
        }
        .btn-small {
            background: transparent;
            border: 1px solid rgba(255,255,255,0.2);
            color: white;
            padding: 6px 12px;
            border-radius: 6px;
            font-size: 12px;
            font-weight: 600;
            cursor: pointer;
            transition: var(--transition-smooth);
        }
        .btn-small:hover {
            background: rgba(255,255,255,0.1);
        }
        .btn-small.danger:hover {
            background: rgba(239, 68, 68, 0.1);
            color: #ef4444;
            border-color: #ef4444;
        }
    </style>
</head>
<body>

    <!-- SIDEBAR -->
    <aside class="sidebar">
        <div class="sidebar-header">
            <a href="#" class="sidebar-brand">CAMPUSELITE</a>
        </div>
        
        <div class="portal-title">
            <h3>Student Portal</h3>
            <p>Elite Concierge Service</p>
        </div>

        <nav class="sidebar-nav" id="sidebarNav">
            <a href="#" class="nav-item active" data-target="postListingSection">
                <span class="nav-icon">➕</span> Post Listing
            </a>
            <a href="#" class="nav-item" data-target="activeListingsSection">
                <span class="nav-icon">📦</span> My Active Listings
            </a>
            <a href="#" class="nav-item">
                <span class="nav-icon">📈</span> Analytics
            </a>
            <a href="#" class="nav-item">
                <span class="nav-icon">💳</span> Payouts
            </a>
        </nav>

        <div class="user-profile" id="userProfileBtn" style="cursor: pointer; transition: background 0.3s;" onmouseover="this.style.background='rgba(255,255,255,0.05)'" onmouseout="this.style.background='transparent'">
            <img src="https://ui-avatars.com/api/?name=Alex+Chen&background=222&color=fff" alt="Alex Chen" class="user-avatar" id="sidebarAvatar">
            <div class="user-info">
                <h3 id="sidebarName">Alex Chen</h3>
                <p>Premium Member</p>
            </div>
        </div>
    </aside>

    <!-- MAIN CONTENT -->
    <main class="main-content">
        <!-- TOP NAVBAR -->
        <header class="top-navbar">
            <div class="top-nav-links">
                <a href="student_dashboard.html" class="top-link">Shop</a>
                <a href="student_renting.html" class="top-link">Rentals</a>
                <a href="student_sell.html" class="top-link active">Sell</a>
            </div>

            <div class="top-actions">
                <button class="icon-btn">🔔</button>
                <button class="icon-btn">👤</button>
            </div>
        </header>

        <!-- DASHBOARD CONTAINER -->
        <div class="dashboard-container">
            
            <!-- POST LISTING SECTION -->
            <div id="postListingSection">
                <div class="form-header animate-fade-in">
                    <h1 class="form-title">Post a New Listing</h1>
                    <p class="form-subtitle">Turn your unused textbooks, gadgets, and campus essentials into cash. Reach thousands of students securely.</p>
                </div>

            <div class="sell-form animate-fade-in delay-1">
                
                <div class="upload-zone">
                    <span class="upload-icon">📸</span>
                    <h3 class="upload-text">Drop images here or click to upload</h3>
                    <p class="upload-subtext">Add up to 5 high-quality photos (JPG, PNG). Max 5MB per file.</p>
                </div>

                <div class="form-group">
                    <label class="form-label">Item Title</label>
                    <input type="text" class="form-input" placeholder="e.g. Calculus Early Transcendentals 9th Edition">
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label class="form-label">Price ($)</label>
                        <input type="number" class="form-input" placeholder="0.00">
                    </div>
                    <div class="form-group">
                        <label class="form-label">Category</label>
                        <select class="form-select">
                            <option>Select a category...</option>
                            <option>Textbooks & Academics</option>
                            <option>Electronics & Gadgets</option>
                            <option>Dorm Essentials</option>
                            <option>Clothing & Apparel</option>
                            <option>Other</option>
                        </select>
                    </div>
                </div>

                <div class="form-group">
                    <label class="form-label">Condition</label>
                    <select class="form-select">
                        <option>Brand New (Sealed)</option>
                        <option>Like New (No signs of wear)</option>
                        <option>Good (Minor wear, fully functional)</option>
                        <option>Fair (Visible wear, works fine)</option>
                    </select>
                </div>

                <div class="form-group">
                    <label class="form-label">Description</label>
                    <textarea class="form-textarea" placeholder="Describe the item, note any flaws, and mention what is included..."></textarea>
                </div>

                <button class="submit-btn">Publish Listing</button>

            </div>
            </div> <!-- End Post Listing Section -->

            <!-- ACTIVE LISTINGS SECTION -->
            <div id="activeListingsSection" style="display: none;">
                <div class="form-header animate-fade-in">
                    <h1 class="form-title">My Active Listings</h1>
                    <p class="form-subtitle">Manage your current listings and view past sales.</p>
                </div>

                <div class="history-list animate-fade-in delay-1">
                    <div class="history-card">
                        <img src="https://images.unsplash.com/photo-1544947950-fa07a98d237f?q=80&w=2000&auto=format&fit=crop" alt="Book" class="history-img">
                        <div class="history-details">
                            <h3 class="history-title">Calculus Early Transcendentals 9th Ed</h3>
                            <div class="history-meta">
                                <span>Listed: Oct 12, 2023</span>
                                <span>Views: 45</span>
                            </div>
                        </div>
                        <div class="history-price">$85.00</div>
                        <span class="status-badge status-active">Active</span>
                        <div class="history-actions">
                            <button class="btn-small">Edit</button>
                            <button class="btn-small danger">Del</button>
                        </div>
                    </div>

                    <div class="history-card">
                        <img src="https://images.unsplash.com/photo-1525547719571-a2d4ac8945e2?q=80&w=2000&auto=format&fit=crop" alt="Laptop" class="history-img">
                        <div class="history-details">
                            <h3 class="history-title">MacBook Pro M1 2020</h3>
                            <div class="history-meta">
                                <span>Sold: Sep 05, 2023</span>
                                <span>Buyer: Sarah J.</span>
                            </div>
                        </div>
                        <div class="history-price">$800.00</div>
                        <span class="status-badge status-sold">Sold</span>
                        <div class="history-actions">
                            <button class="btn-small">View Receipt</button>
                        </div>
                    </div>
                </div>
            </div> <!-- End Active Listings Section -->

        </div>
    </main>

    <!-- PROFILE EDIT MODAL -->
    <div class="modal-overlay" id="profileModal">
        <div class="profile-modal">
            <div class="modal-header">
                <h2 class="modal-title">Edit Profile</h2>
                <button class="close-modal" id="closeProfileModal">&times;</button>
            </div>
            
            <div class="profile-pic-edit">
                <img src="https://ui-avatars.com/api/?name=Alex+Chen&background=222&color=fff" alt="Profile" class="profile-pic-preview" id="modalPicPreview">
                <button class="btn-outline" onclick="document.getElementById('profilePicInput').click()">Change Picture</button>
                <input type="file" id="profilePicInput" style="display: none;" accept="image/*">
            </div>

            <div class="modal-form-group">
                <label class="modal-label">Full Name</label>
                <input type="text" class="modal-input" id="profileNameInput" value="Alex Chen">
            </div>

            <div class="modal-form-group">
                <label class="modal-label">ID Number</label>
                <input type="text" class="modal-input" value="2021-00123" readonly>
            </div>

            <div class="modal-form-group">
                <label class="modal-label">Course</label>
                <input type="text" class="modal-input" value="BS Computer Science" readonly>
            </div>

            <button class="btn-save" id="saveProfileBtn">Save Changes</button>
        </div>
    </div>

    <script>
        // Sidebar Navigation
        const navItems = document.querySelectorAll('#sidebarNav .nav-item');
        const sections = {
            'postListingSection': document.getElementById('postListingSection'),
            'activeListingsSection': document.getElementById('activeListingsSection')
        };

        navItems.forEach(item => {
            item.addEventListener('click', (e) => {
                e.preventDefault();
                
                // Remove active class
                navItems.forEach(nav => nav.classList.remove('active'));
                item.classList.add('active');

                // Show target section if it exists
                const targetId = item.getAttribute('data-target');
                if (targetId && sections[targetId]) {
                    Object.values(sections).forEach(sec => {
                        if(sec) sec.style.display = 'none';
                    });
                    sections[targetId].style.display = 'block';
                }
            });
        });

        // Profile Modal Logic
        const profileModal = document.getElementById('profileModal');
        const userProfileBtn = document.getElementById('userProfileBtn');
        const closeProfileModal = document.getElementById('closeProfileModal');
        const saveProfileBtn = document.getElementById('saveProfileBtn');
        const profilePicInput = document.getElementById('profilePicInput');
        const modalPicPreview = document.getElementById('modalPicPreview');
        const sidebarAvatar = document.getElementById('sidebarAvatar');
        const sidebarName = document.getElementById('sidebarName');
        const profileNameInput = document.getElementById('profileNameInput');

        // Open Modal
        userProfileBtn.addEventListener('click', () => {
            profileModal.classList.add('active');
        });

        // Close Modal
        closeProfileModal.addEventListener('click', () => {
            profileModal.classList.remove('active');
        });

        // Close when clicking outside
        profileModal.addEventListener('click', (e) => {
            if(e.target === profileModal) {
                profileModal.classList.remove('active');
            }
        });

        // Handle Image Upload Preview
        profilePicInput.addEventListener('change', function() {
            if (this.files && this.files[0]) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    modalPicPreview.src = e.target.result;
                }
                reader.readAsDataURL(this.files[0]);
            }
        });

        // Save Profile
        saveProfileBtn.addEventListener('click', () => {
            if(profileNameInput.value.trim() !== '') {
                sidebarName.textContent = profileNameInput.value;
            }
            sidebarAvatar.src = modalPicPreview.src;
            profileModal.classList.remove('active');
        });
    </script>
</body>
</html>


Can uou make all of this code turn into my project like when you register a new account and after you create you login
you will be direct in the dashboard and you can see all the features that you have in the dashboard like posting a listing and all the other features that you have in the dashboard and also you can edit your profile and all the other features that you have in the dashboard
make the html i based make it working all of it even the sidebar working also, the profile editing and all the features that you have in the dashboard like posting a listing and all the other features that you have in the dashboard
shop just don't add only the admin can add the stall and the admin can add the item inside the stall and except the student selling can sell the item want.

Make the Styles and js put in the www.root you can make another css or js files so that it will be organized and clean
working all of the User like add to cart, checkout, and all the other features that you have in the dashboard like posting a listing and all the other features that you have in the dashboard as a Users
make the html use TAG HELPERS and if you want add another file like views for the html, sell, renting you can make it if you want to make it readable and cleanliness