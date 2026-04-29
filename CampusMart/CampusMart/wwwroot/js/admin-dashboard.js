// === ADMIN DASHBOARD JS ===
// Keeps admin profile modal UX consistent and separated from markup.
document.addEventListener('DOMContentLoaded', function () {
    const profileModal = document.getElementById('profileModal');
    if (!profileModal) return;

    const profileTriggers = document.querySelectorAll('.profile-trigger');
    const closeProfileModal = document.getElementById('closeProfileModal');
    const saveProfileBtn = document.getElementById('saveProfileBtn');

    const changeProfilePicBtn = document.getElementById('changeProfilePicBtn');
    const profilePicInput = document.getElementById('profilePicInput');
    const modalPicPreview = document.getElementById('modalPicPreview');

    const sidebarAvatar = document.getElementById('sidebarAvatar');
    const sidebarName = document.getElementById('sidebarName');
    const profileNameInput = document.getElementById('profileNameInput');

    function openProfileModal() {
        profileModal.classList.add('active');
        document.body.style.overflow = 'hidden';
    }

    function closeProfileEditor() {
        profileModal.classList.remove('active');
        document.body.style.overflow = '';
    }

    profileTriggers.forEach(trigger => {
        trigger.addEventListener('click', openProfileModal);
    });

    if (closeProfileModal) {
        closeProfileModal.addEventListener('click', closeProfileEditor);
    }

    // Close when clicking outside the modal card.
    profileModal.addEventListener('click', (e) => {
        if (e.target === profileModal) closeProfileEditor();
    });

    // Escape key closes the modal.
    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape' && profileModal.classList.contains('active')) {
            closeProfileEditor();
        }
    });

    // "Change Picture" button opens the hidden file input.
    if (changeProfilePicBtn && profilePicInput) {
        changeProfilePicBtn.addEventListener('click', () => profilePicInput.click());
    }

    // Image preview for the profile picture.
    if (profilePicInput && modalPicPreview) {
        profilePicInput.addEventListener('change', function () {
            if (this.files && this.files[0]) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    modalPicPreview.src = e.target.result;
                };
                reader.readAsDataURL(this.files[0]);
            }
        });
    }

    // Save profile (UI only; no backend persistence in this prototype).
    if (saveProfileBtn) {
        saveProfileBtn.addEventListener('click', () => {
            const trimmedName = profileNameInput ? profileNameInput.value.trim() : '';
            if (sidebarName && trimmedName) {
                sidebarName.textContent = trimmedName;
            }

            if (sidebarAvatar && modalPicPreview) {
                sidebarAvatar.src = modalPicPreview.src;
            }

            closeProfileEditor();
        });
    }
});

