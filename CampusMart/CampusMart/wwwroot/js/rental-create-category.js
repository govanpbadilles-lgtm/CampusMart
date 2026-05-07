// Rental Category Icon Picker
document.addEventListener('DOMContentLoaded', function () {
    const iconInput = document.getElementById('iconInput');
    const iconDisplay = document.getElementById('iconDisplay');
    const iconButtons = document.querySelectorAll('.icon-picker-btn');
    const iconPickerGrid = document.getElementById('iconPickerGrid');

    // Initialize - check if there's an existing icon value
    const currentIcon = iconInput.value && iconInput.value.trim() !== '' ? iconInput.value.trim() : null;

    if (currentIcon) {
        // If there's a saved icon, show it
        iconDisplay.textContent = currentIcon;
        iconDisplay.classList.remove('empty');
        iconButtons.forEach(btn => {
            if (btn.textContent.trim() === currentIcon) {
                btn.classList.add('selected');
            }
        });
    } else {
        // If no icon, show empty state
        iconDisplay.classList.add('empty');
        iconDisplay.textContent = '?';
        iconInput.value = '';
    }

    // Icon button click handler
    iconButtons.forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            const icon = this.textContent.trim();
            iconInput.value = icon;
            iconDisplay.textContent = icon;
            iconDisplay.classList.remove('empty');

            // Update selected state
            iconButtons.forEach(b => b.classList.remove('selected'));
            this.classList.add('selected');

            // Animate icon display
            iconDisplay.style.animation = 'none';
            setTimeout(() => {
                iconDisplay.style.animation = 'scale-pulse 0.3s ease';
            }, 10);
        });
    });

    // Click on icon display to focus grid
    iconDisplay.addEventListener('click', function () {
        iconPickerGrid.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
        const firstBtn = iconPickerGrid.querySelector('.icon-picker-btn');
        if (firstBtn) firstBtn.focus();
    });
});
