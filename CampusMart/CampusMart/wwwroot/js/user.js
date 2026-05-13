// === ELITE USER SUITE JS ===
document.addEventListener('DOMContentLoaded', function () {
    // 1. Core Element Selectors
    const mainSidebar = document.getElementById('mainSidebar');
    const sidebarOverlay = document.getElementById('sidebarOverlay');
    const sidebarToggleBtn = document.getElementById('sidebarToggleBtn');

    const cartSidebar = document.getElementById('cartSidebar');
    const cartToggleBtn = document.getElementById('cartToggleBtn');
    const closeCartBtn = document.getElementById('closeCartBtn');

    const notificationSidebar = document.getElementById('notificationSidebar');
    const notificationToggleBtn = document.getElementById('notificationToggleBtn');
    const closeNotificationBtn = document.getElementById('closeNotificationBtn');

    const profileModal = document.getElementById('profileModal');
    const closeProfileModal = document.getElementById('closeProfileModal');
    const quickEditForm = document.getElementById('quickEditForm');

    // ── Helper: Close All Sidebars ──
    function closeAllPanels() {
        if (mainSidebar) mainSidebar.classList.remove('open');
        if (cartSidebar) cartSidebar.classList.remove('open');
        if (notificationSidebar) notificationSidebar.classList.remove('open');
        if (profileModal) profileModal.classList.remove('active');
        if (sidebarOverlay) sidebarOverlay.classList.remove('active');
        document.body.style.overflow = '';
        // Reset sidebar toggle icon to list
        if (sidebarToggleBtn) {
            const icon = sidebarToggleBtn.querySelector('i');
            if (icon) icon.className = 'bi bi-list';
        }
    }

    // 2. Mobile Sidebar Logic (Toggle)
    if (sidebarToggleBtn) {
        sidebarToggleBtn.addEventListener('click', () => {
            const isOpen = mainSidebar.classList.contains('open');
            if (isOpen) {
                closeAllPanels();
            } else {
                mainSidebar.classList.add('open');
                sidebarOverlay.classList.add('active');
                document.body.style.overflow = 'hidden';
                // Change icon to X
                const icon = sidebarToggleBtn.querySelector('i');
                if (icon) icon.className = 'bi bi-x-lg';
            }
        });
    }

    // 2b. Close sidebar button (mobile)
    const closeSidebarBtn = document.getElementById('closeSidebarBtn');
    if (closeSidebarBtn) {
        closeSidebarBtn.addEventListener('click', closeAllPanels);
    }

    // 3. Cart Sidebar Logic
    if (cartToggleBtn && cartSidebar) {
        cartToggleBtn.addEventListener('click', (e) => {
            e.preventDefault();
            cartSidebar.classList.add('open');
            sidebarOverlay.classList.add('active');
            document.body.style.overflow = 'hidden';
        });
    }

    if (closeCartBtn) {
        closeCartBtn.addEventListener('click', closeAllPanels);
    }

    // 4. Notification Sidebar Logic
    if (notificationToggleBtn && notificationSidebar) {
        notificationToggleBtn.addEventListener('click', (e) => {
            e.preventDefault();
            notificationSidebar.classList.add('open');
            sidebarOverlay.classList.add('active');
            document.body.style.overflow = 'hidden';
            loadUserNotifications();
        });
    }

    if (closeNotificationBtn) {
        closeNotificationBtn.addEventListener('click', closeAllPanels);
    }

    // Mark all as read button (user)
    const markAllReadBtnUser = document.getElementById('markAllReadBtnUser');
    if (markAllReadBtnUser) {
        markAllReadBtnUser.addEventListener('click', markAllUserNotificationsRead);
    }

    // 5. Global Overlay Close
    if (sidebarOverlay) {
        sidebarOverlay.addEventListener('click', closeAllPanels);
    }

    // 6. Profile Management Logic
    // Allow triggering profile modal from anywhere with .profile-trigger class
    document.querySelectorAll('.profile-trigger').forEach(trigger => {
        trigger.addEventListener('click', (e) => {
            e.preventDefault();
            if (profileModal) {
                profileModal.classList.add('active');
                sidebarOverlay.classList.add('active');
            }
        });
    });

    if (closeProfileModal) {
        closeProfileModal.addEventListener('click', closeAllPanels);
    }

    // Image Preview
    const profilePicInput = document.getElementById('profilePicInput');
    const modalPicPreview = document.getElementById('modalPicPreview');
    if (profilePicInput && modalPicPreview) {
        profilePicInput.addEventListener('change', function () {
            if (this.files && this.files[0]) {
                const reader = new FileReader();
                reader.onload = (e) => { modalPicPreview.src = e.target.result; };
                reader.readAsDataURL(this.files[0]);
            }
        });
    }

    // Quick Edit Form Handling
    if (quickEditForm) {
        quickEditForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const saveBtn = document.getElementById('saveProfileBtn');
            const nameInput = document.getElementById('profileNameInput');
            
            if (saveBtn) {
                saveBtn.disabled = true;
                saveBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Updating...';
            }

            const fd = new FormData(quickEditForm);
            try {
                const res = await fetch('/Profile/QuickUpdate', { method: 'POST', body: fd });
                if (res.ok) {
                    const data = await res.json();
                    const sidebarName = document.getElementById('sidebarName');
                    const sidebarAvatar = document.getElementById('sidebarAvatar');
                    
                    if (sidebarName) sidebarName.textContent = data.fullName;
                    if (sidebarAvatar && data.avatarUrl) sidebarAvatar.src = data.avatarUrl;
                    
                    showToast('Elite profile updated successfully!');
                    closeAllPanels();
                } else {
                    showToast('Failed to sync profile updates.', true);
                }
            } catch (err) {
                showToast('Network error during synchronization.', true);
            } finally {
                if (saveBtn) {
                    saveBtn.disabled = false;
                    saveBtn.textContent = 'Save Changes';
                }
            }
        });
    }

    // Escape Key Handler
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') closeAllPanels();
    });

    // 7. Load initial notification badge on page load
    loadUserNotifications();
});

// ── User Notification System ──
function loadUserNotifications() {
    const notificationList = document.getElementById('userNotificationList');
    const notifyDot = document.getElementById('userNotifyDot');

    if (!notificationList) return;

    // Show loading state
    notificationList.innerHTML = `
        <div class="notification-loading-user">
            <i class="bi bi-arrow-clockwise spin-user"></i> Loading...
        </div>`;

    fetch('/NotificationApi/GetUserNotifications')
        .then(res => {
            if (!res.ok) throw new Error('Failed to load');
            return res.json();
        })
        .then(data => {
            const { notifications, unreadCount } = data;

            // Update notification dot
            if (notifyDot) {
                notifyDot.style.display = unreadCount > 0 ? 'block' : 'none';
            }

            // Render notifications
            if (!notifications || notifications.length === 0) {
                notificationList.innerHTML = `
                    <div class="notification-empty-user">
                        <i class="bi bi-bell-slash"></i>
                        <p>No notifications yet</p>
                        <p class="sub">Alerts will appear here when there's activity.</p>
                    </div>`;
                return;
            }

            notificationList.innerHTML = notifications.map(n => `
                <div class="notification-item-user ${n.unread ? 'unread' : ''}" data-id="${n.id}">
                    <div class="notification-icon-user" style="background: ${n.iconBg}; color: ${n.iconColor};">
                        <i class="bi ${n.icon}"></i>
                    </div>
                    <div class="notification-content-user">
                        <p class="notification-heading-user">${n.title}</p>
                        <p class="notification-message-user">${n.message}</p>
                        ${n.time ? `<div class="notification-meta-user">
                            <i class="bi bi-clock"></i> ${n.time}
                            ${n.unread ? '<span class="notification-dot-inline"></span>' : ''}
                        </div>` : ''}
                    </div>
                </div>
            `).join('');
        })
        .catch(err => {
            console.error('User notification load error:', err);
            notificationList.innerHTML = `
                <div class="notification-empty-user">
                    <i class="bi bi-wifi-off"></i>
                    <p>Couldn't load notifications</p>
                    <button class="retry-btn-user" onclick="loadUserNotifications()">
                        <i class="bi bi-arrow-clockwise"></i> Retry
                    </button>
                </div>`;
        });
}

function markAllUserNotificationsRead() {
    const items = document.querySelectorAll('.notification-item-user.unread');
    items.forEach(item => item.classList.remove('unread'));

    const notifyDot = document.getElementById('userNotifyDot');
    if (notifyDot) notifyDot.style.display = 'none';

    showToast('All notifications marked as read');
}

// Toast notification
function showToast(message, isError = false) {
    let toast = document.getElementById('userToast');
    if (!toast) {
        toast = document.createElement('div');
        toast.id = 'userToast';
        toast.className = 'toast-notify';
        document.body.appendChild(toast);
    }
    
    toast.innerHTML = isError ? `<i class="bi bi-x-circle-fill me-2"></i> ${message}` : `<i class="bi bi-check-circle-fill me-2"></i> ${message}`;
    toast.classList.toggle('toast-error', isError);
    
    void toast.offsetWidth; // Trigger reflow
    toast.classList.add('show');
    setTimeout(() => toast.classList.remove('show'), 3500);
}
