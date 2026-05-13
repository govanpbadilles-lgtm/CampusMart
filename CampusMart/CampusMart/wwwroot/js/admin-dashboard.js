// Global Modal Management Functions
function showModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.add('active');
        document.body.style.overflow = 'hidden';
    }
}

function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.remove('active');
        // Wait for transition before hiding if using display:none/flex
        setTimeout(() => {
            if (!modal.classList.contains('active')) {
                document.body.style.overflow = '';
            }
        }, 400);
    }
}

// ── Admin Notification System ──
function loadNotifications() {
    const notificationList = document.getElementById('notificationList');
    const notifyBadge = document.getElementById('navNotifyBadge');

    if (!notificationList) return;

    // Show loading state
    notificationList.innerHTML = `
        <div class="notification-loading">
            <i class="bi bi-arrow-clockwise spin"></i> Loading...
        </div>`;

    fetch('/Admin/NotificationApi/GetAdminNotifications')
        .then(res => {
            if (!res.ok) throw new Error('Failed to load');
            return res.json();
        })
        .then(data => {
            const { notifications, unreadCount } = data;

            // Update badge
            if (notifyBadge) {
                if (unreadCount > 0) {
                    notifyBadge.textContent = unreadCount > 99 ? '99+' : unreadCount;
                    notifyBadge.style.display = 'flex';
                } else {
                    notifyBadge.style.display = 'none';
                }
            }

            // Render notifications
            if (!notifications || notifications.length === 0) {
                notificationList.innerHTML = `
                    <div class="notification-empty">
                        <i class="bi bi-bell-slash d-block"></i>
                        <p class="mt-3">No notifications yet</p>
                        <p class="small" style="color: var(--text-dim);">System alerts will appear here when there's activity.</p>
                    </div>`;
                return;
            }

            notificationList.innerHTML = notifications.map(n => `
                <div class="notification-item ${n.unread ? 'unread' : ''}" data-id="${n.id}">
                    <div class="notification-icon" style="background: ${n.iconBg}; color: ${n.iconColor};">
                        <i class="bi ${n.icon}"></i>
                    </div>
                    <div class="notification-content">
                        <p class="notification-heading">${n.title}</p>
                        <p class="notification-message">${n.message}</p>
                        <div class="notification-meta">
                            <i class="bi bi-clock"></i> ${n.time}
                            ${n.unread ? '<span class="notification-badge-dot"></span>' : ''}
                        </div>
                    </div>
                </div>
            `).join('');
        })
        .catch(err => {
            console.error('Notification load error:', err);
            notificationList.innerHTML = `
                <div class="notification-empty">
                    <i class="bi bi-wifi-off d-block"></i>
                    <p class="mt-3">Couldn't load notifications</p>
                    <button class="mark-read-btn mt-2" onclick="loadNotifications()">
                        <i class="bi bi-arrow-clockwise"></i> Retry
                    </button>
                </div>`;
        });
}

function markAllNotificationsRead() {
    const items = document.querySelectorAll('.notification-item.unread');
    items.forEach(item => item.classList.remove('unread'));

    const notifyBadge = document.getElementById('navNotifyBadge');
    if (notifyBadge) notifyBadge.style.display = 'none';

    if (typeof showToast === 'function') {
        showToast('All notifications marked as read', 'success');
    }
}

document.addEventListener('DOMContentLoaded', function () {
    // 1. Sidebar Toggle Logic
    const sidebar = document.querySelector('.admin-sidebar');
    const sidebarToggle = document.getElementById('adminSidebarToggle');
    const sidebarOverlay = document.getElementById('sidebarOverlay');

    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener('click', () => {
            const isOpen = sidebar.classList.toggle('open');
            if (sidebarOverlay) {
                sidebarOverlay.classList.toggle('active');
            }
            
            // Change icon
            const icon = sidebarToggle.querySelector('i');
            if (icon) {
                icon.className = isOpen ? 'bi bi-x-lg' : 'bi bi-list';
            }
        });

        if (sidebarOverlay) {
            sidebarOverlay.addEventListener('click', () => {
                sidebar.classList.remove('open');
                sidebarOverlay.classList.remove('active');
                const icon = sidebarToggle.querySelector('i');
                if (icon) icon.className = 'bi bi-list';
            });
        }

        document.addEventListener('click', (e) => {
            if (window.innerWidth <= 992 &&
                !sidebar.contains(e.target) &&
                !sidebarToggle.contains(e.target) &&
                sidebar.classList.contains('open')) {
                sidebar.classList.remove('open');
                if (sidebarOverlay) sidebarOverlay.classList.remove('active');
                const icon = sidebarToggle.querySelector('i');
                if (icon) icon.className = 'bi bi-list';
            }
        });
    }

    // 2. Notification Sidebar Logic
    const notificationBtn = document.getElementById('notificationBtn');
    const notificationSidebar = document.getElementById('notificationSidebar');
    const notificationOverlay = document.getElementById('notificationOverlay');
    const closeNotificationSidebar = document.getElementById('closeNotificationSidebar');

    if (notificationBtn && notificationSidebar) {
        notificationBtn.addEventListener('click', () => {
            notificationSidebar.classList.add('active');
            if (notificationOverlay) notificationOverlay.classList.add('active');
            document.body.style.overflow = 'hidden';
            loadNotifications();
        });
    }

    if (closeNotificationSidebar) {
        closeNotificationSidebar.addEventListener('click', () => {
            notificationSidebar.classList.remove('active');
            if (notificationOverlay) notificationOverlay.classList.remove('active');
            document.body.style.overflow = '';
        });
    }

    if (notificationOverlay) {
        notificationOverlay.addEventListener('click', () => {
            notificationSidebar.classList.remove('active');
            notificationOverlay.classList.remove('active');
            document.body.style.overflow = '';
        });
    }

    // Mark all as read button
    const markAllReadBtn = document.getElementById('markAllReadBtn');
    if (markAllReadBtn) {
        markAllReadBtn.addEventListener('click', markAllNotificationsRead);
    }

    // 3. Global Modal Click-to-Close
    document.querySelectorAll('.modal-overlay').forEach(modal => {
        modal.addEventListener('click', (e) => {
            if (e.target === modal) {
                closeModal(modal.id);
            }
        });
    });

    // 4. Profile Modal Logic
    const profileModal = document.getElementById('profileModal');
    if (profileModal) {
        const profileTriggers = document.querySelectorAll('.profile-trigger');
        const closeProfileModal = document.getElementById('closeProfileModal');

        profileTriggers.forEach(trigger => {
            trigger.addEventListener('click', () => showModal('profileModal'));
        });

        if (closeProfileModal) {
            closeProfileModal.addEventListener('click', () => closeModal('profileModal'));
        }
        
        // Pic Preview Logic
        const profilePicInput = document.getElementById('profilePicInput');
        const modalPicPreview = document.getElementById('modalPicPreview');
        const changeProfilePicBtn = document.getElementById('changeProfilePicBtn');

        if (changeProfilePicBtn && profilePicInput) {
            changeProfilePicBtn.addEventListener('click', () => profilePicInput.click());
        }

        if (profilePicInput && modalPicPreview) {
            profilePicInput.addEventListener('change', function () {
                if (this.files && this.files[0]) {
                    const reader = new FileReader();
                    reader.onload = (e) => { 
                        modalPicPreview.src = e.target.result; 
                        const smallPreview = document.getElementById('adminSmallPreview');
                        if (smallPreview) {
                            smallPreview.src = e.target.result;
                            smallPreview.style.display = 'block';
                        }
                    };
                    reader.readAsDataURL(this.files[0]);
                }
            });
        }

        const adminQuickEditForm = document.getElementById('adminQuickEditForm');
        if (adminQuickEditForm) {
            adminQuickEditForm.addEventListener('submit', async (e) => {
                e.preventDefault();
                const saveBtn = document.getElementById('saveProfileBtn');
                if (saveBtn) {
                    saveBtn.disabled = true;
                    saveBtn.textContent = 'Updating...';
                }

                const fd = new FormData(adminQuickEditForm);
                try {
                    const res = await fetch('/Profile/QuickUpdate', { method: 'POST', body: fd });
                    if (res.ok) {
                        const data = await res.json();
                        const sidebarName = document.getElementById('sidebarName');
                        const sidebarAvatar = document.getElementById('sidebarAvatar');
                        
                        if (sidebarName) sidebarName.textContent = data.fullName;
                        if (sidebarAvatar && data.avatarUrl) sidebarAvatar.src = data.avatarUrl;

                        showToast('Profile updated successfully!', 'success');
                        closeModal('profileModal');
                    } else {
                        showToast('Failed to update profile.', 'error');
                    }
                } catch (err) {
                    showToast('Network error.', 'error');
                } finally {
                    if (saveBtn) {
                        saveBtn.disabled = false;
                        saveBtn.textContent = 'Save Changes';
                    }
                }
            });
        }
    }

    // 5. Load initial notification badge count on page load
    loadNotifications();
});