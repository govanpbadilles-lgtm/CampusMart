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
    const notifyBadge = document.getElementById('navNotifyBadge');

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
});