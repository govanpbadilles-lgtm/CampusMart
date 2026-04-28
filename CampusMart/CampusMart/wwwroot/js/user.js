// === USER DASHBOARD JS ===
document.addEventListener('DOMContentLoaded', function () {
    // Mobile Sidebar & Cart Toggle
    const sidebarToggleBtn = document.getElementById('sidebarToggleBtn');
    const mainSidebar = document.getElementById('mainSidebar');
    const sidebarOverlay = document.getElementById('sidebarOverlay');
    
    const cartToggleBtn = document.getElementById('cartToggleBtn');
    const cartSidebar = document.getElementById('cartSidebar');
    const closeCartBtn = document.getElementById('closeCartBtn');

    const notificationToggleBtn = document.getElementById('notificationToggleBtn');
    const notificationSidebar = document.getElementById('notificationSidebar');
    const closeNotificationBtn = document.getElementById('closeNotificationBtn');

    if (sidebarToggleBtn && mainSidebar && sidebarOverlay) {
        sidebarToggleBtn.addEventListener('click', () => {
            mainSidebar.classList.add('open');
            sidebarOverlay.classList.add('active');
        });
    }

    if (cartToggleBtn && cartSidebar && sidebarOverlay) {
        cartToggleBtn.addEventListener('click', (e) => {
            e.preventDefault();
            cartSidebar.classList.add('open');
            if (notificationSidebar) notificationSidebar.classList.remove('open');
            sidebarOverlay.classList.add('active');
        });
        
        if (closeCartBtn) {
            closeCartBtn.addEventListener('click', () => {
                cartSidebar.classList.remove('open');
                sidebarOverlay.classList.remove('active');
            });
        }
    }

    if (notificationToggleBtn && notificationSidebar && sidebarOverlay) {
        notificationToggleBtn.addEventListener('click', (e) => {
            e.preventDefault();
            notificationSidebar.classList.add('open');
            if (cartSidebar) cartSidebar.classList.remove('open');
            sidebarOverlay.classList.add('active');
        });
        
        if (closeNotificationBtn) {
            closeNotificationBtn.addEventListener('click', () => {
                notificationSidebar.classList.remove('open');
                sidebarOverlay.classList.remove('active');
            });
        }
    }
        
    if (sidebarOverlay) {
        sidebarOverlay.addEventListener('click', () => {
            if (mainSidebar) mainSidebar.classList.remove('open');
            if (cartSidebar) cartSidebar.classList.remove('open');
            if (notificationSidebar) notificationSidebar.classList.remove('open');
            sidebarOverlay.classList.remove('active');
        });
    }

    // Profile Modal
    const profileModal = document.getElementById('profileModal');
    const userProfileBtn = document.getElementById('userProfileBtn');
    const closeProfileModal = document.getElementById('closeProfileModal');
    const saveProfileBtn = document.getElementById('saveProfileBtn');
    const profilePicInput = document.getElementById('profilePicInput');
    const modalPicPreview = document.getElementById('modalPicPreview');
    const sidebarAvatar = document.getElementById('sidebarAvatar');
    const sidebarName = document.getElementById('sidebarName');
    const profileNameInput = document.getElementById('profileNameInput');

    if (userProfileBtn && profileModal) {
        userProfileBtn.addEventListener('click', () => profileModal.classList.add('active'));
    }
    if (closeProfileModal && profileModal) {
        closeProfileModal.addEventListener('click', () => profileModal.classList.remove('active'));
    }
    if (profileModal) {
        profileModal.addEventListener('click', (e) => {
            if (e.target === profileModal) profileModal.classList.remove('active');
        });
    }
    if (profilePicInput && modalPicPreview) {
        profilePicInput.addEventListener('change', function () {
            if (this.files && this.files[0]) {
                const reader = new FileReader();
                reader.onload = (e) => { modalPicPreview.src = e.target.result; };
                reader.readAsDataURL(this.files[0]);
            }
        });
    }
    if (saveProfileBtn) {
        saveProfileBtn.addEventListener('click', () => {
            if (profileNameInput && profileNameInput.value.trim() !== '' && sidebarName) {
                sidebarName.textContent = profileNameInput.value;
            }
            if (modalPicPreview && sidebarAvatar) {
                sidebarAvatar.src = modalPicPreview.src;
            }
            if (profileModal) profileModal.classList.remove('active');
            showToast('Profile updated successfully!');
        });
    }

    // Floor tabs
    document.querySelectorAll('.floor-tab').forEach(tab => {
        tab.addEventListener('click', function () {
            document.querySelectorAll('.floor-tab').forEach(t => t.classList.remove('active'));
            this.classList.add('active');
            const floor = this.querySelector('.floor-num')?.textContent;
            const mapTitle = document.querySelector('.map-title');
            const stallsTitle = document.querySelector('.stalls-title');
            if (mapTitle) mapTitle.textContent = floor + 'th Floor Map';
            if (stallsTitle) stallsTitle.textContent = 'Active Stalls - Floor ' + floor;
        });
    });

    // View toggles
    document.querySelectorAll('.view-btn').forEach(btn => {
        btn.addEventListener('click', function () {
            document.querySelectorAll('.view-btn').forEach(b => b.classList.remove('active'));
            this.classList.add('active');
        });
    });

    // Profile page: avatar upload
    const avatarInput = document.getElementById('avatarInput');
    const avatarPreview = document.getElementById('avatarPreview');
    const removeAvatarBtn = document.getElementById('removeAvatarBtn');
    const displayName = document.getElementById('displayName');
    const defaultAvatar = sidebarAvatar ? sidebarAvatar.src : '';

    if (avatarInput && avatarPreview) {
        avatarInput.addEventListener('change', function () {
            if (this.files && this.files[0]) {
                const reader = new FileReader();
                reader.onload = (e) => {
                    avatarPreview.src = e.target.result;
                    if (sidebarAvatar) sidebarAvatar.src = e.target.result;
                };
                reader.readAsDataURL(this.files[0]);
            }
        });
    }
    if (removeAvatarBtn && avatarPreview) {
        removeAvatarBtn.addEventListener('click', () => {
            avatarPreview.src = defaultAvatar;
            if (sidebarAvatar) sidebarAvatar.src = defaultAvatar;
            if (avatarInput) avatarInput.value = '';
        });
    }

    // Bio char count
    const bioInput = document.getElementById('bioInput');
    const charCount = document.getElementById('charCount');
    if (bioInput && charCount) {
        function updateCount() { charCount.textContent = bioInput.value.length; }
        bioInput.addEventListener('input', updateCount);
        updateCount();
    }

    // Live name sync on profile page
    const firstName = document.getElementById('firstName');
    const lastName = document.getElementById('lastName');
    if (firstName && lastName) {
        function syncName() {
            const full = (firstName.value + ' ' + lastName.value).trim();
            if (displayName) displayName.textContent = full || 'Your Name';
            if (sidebarName) sidebarName.textContent = full || 'Your Name';
        }
        firstName.addEventListener('input', syncName);
        lastName.addEventListener('input', syncName);
    }

    // Save profile page
    const saveBtn = document.getElementById('saveBtn');
    if (saveBtn) {
        saveBtn.addEventListener('click', () => {
            saveBtn.textContent = 'Saving...';
            saveBtn.style.pointerEvents = 'none';
            setTimeout(() => {
                saveBtn.textContent = 'Save Changes';
                saveBtn.style.pointerEvents = 'auto';
                showToast('Profile updated successfully!');
            }, 800);
        });
    }
});

// Toast notification
function showToast(message, isError = false) {
    let toast = document.getElementById('userToast');
    if (!toast) {
        toast = document.createElement('div');
        toast.id = 'userToast';
        toast.className = 'toast-notify';
        document.body.appendChild(toast);
    }
    
    if (isError) {
        toast.classList.add('toast-error');
        toast.textContent = '✖  ' + message;
    } else {
        toast.classList.remove('toast-error');
        toast.textContent = '✓  ' + message;
    }
    
    // Trigger reflow
    void toast.offsetWidth;
    
    toast.classList.add('show');
    setTimeout(() => toast.classList.remove('show'), 3500);
}
