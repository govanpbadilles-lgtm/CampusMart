// Seller page JS - Product listing
document.addEventListener('DOMContentLoaded', function () {
    // Upload zone drag & drop
    const uploadZone = document.querySelector('.upload-zone');
    const fileInput = document.getElementById('listingImages');
    let uploadedFiles = [];

    if (uploadZone && fileInput) {
        // Click to upload
        uploadZone.addEventListener('click', () => fileInput.click());

        // Drag and drop
        uploadZone.addEventListener('dragover', (e) => {
            e.preventDefault();
            uploadZone.classList.add('dragover');
        });

        uploadZone.addEventListener('dragleave', () => {
            uploadZone.classList.remove('dragover');
        });

        uploadZone.addEventListener('drop', (e) => {
            e.preventDefault();
            uploadZone.classList.remove('dragover');
            if (e.dataTransfer.files.length > 0) {
                handleFileSelection(e.dataTransfer.files);
            }
        });

        // File input change
        fileInput.addEventListener('change', function () {
            if (this.files.length > 0) {
                handleFileSelection(this.files);
            }
        });
    }

    function handleFileSelection(files) {
        const maxFiles = 5;
        const maxSize = 5 * 1024 * 1024; // 5MB
        const allowedTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];

        let validFiles = [];
        for (let file of files) {
            if (validFiles.length >= maxFiles) break;

            if (!allowedTypes.includes(file.type)) {
                showNotification('Invalid file type: ' + file.name + '. Only JPG, PNG, GIF, and WebP allowed.', 'error');
                continue;
            }

            if (file.size > maxSize) {
                showNotification('File too large: ' + file.name + '. Max 5MB per file.', 'error');
                continue;
            }

            validFiles.push(file);
        }

        if (validFiles.length === 0) return;

        uploadedFiles = validFiles;
        updateUploadedFile(fileInput);
        previewImages(validFiles);
    }

    function updateUploadedFile(input) {
        const dt = new DataTransfer();
        uploadedFiles.forEach(file => dt.items.add(file));
        input.files = dt.files;
    }

    function previewImages(files) {
        let container = document.querySelector('.upload-previews');
        if (!container) {
            container = document.createElement('div');
            container.className = 'upload-previews';
            container.style.cssText = 'display: grid; grid-template-columns: repeat(auto-fill, minmax(80px, 1fr)); gap: 12px; margin-top: 12px;';
            uploadZone.parentNode.insertBefore(container, uploadZone.nextSibling);
        }
        container.innerHTML = '';
        Array.from(files).forEach((file, idx) => {
            const reader = new FileReader();
            reader.onload = (e) => {
                const imgWrapper = document.createElement('div');
                imgWrapper.style.cssText = 'position: relative; border-radius: 8px; overflow: hidden; border: 2px solid var(--border-subtle);';
                const img = document.createElement('img');
                img.src = e.target.result;
                img.style.cssText = 'width: 100%; height: 80px; object-fit: cover; display: block;';
                imgWrapper.appendChild(img);

                const removeBtn = document.createElement('button');
                removeBtn.type = 'button';
                removeBtn.textContent = '✕';
                removeBtn.style.cssText = 'position: absolute; top: 2px; right: 2px; background: rgba(255,0,0,0.7); color: white; border: none; border-radius: 50%; width: 20px; height: 20px; padding: 0; cursor: pointer; font-size: 14px; font-weight: bold;';
                removeBtn.addEventListener('click', (e) => {
                    e.preventDefault();
                    uploadedFiles.splice(idx, 1);
                    updateUploadedFile(fileInput);
                    previewImages(uploadedFiles);
                });

                imgWrapper.appendChild(removeBtn);
                container.appendChild(imgWrapper);
            };
            reader.readAsDataURL(file);
        });
    }

    // Form validation and submission
    const forms = document.querySelectorAll('form[asp-controller="Product"][asp-action="Sell"]');
    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            const titleInput = form.querySelector('input[name="Title"]');
            const priceInput = form.querySelector('input[name="Price"]');
            const categoryInput = form.querySelector('select[name="CategoryId"]');

            if (!titleInput || !titleInput.value.trim()) {
                e.preventDefault();
                showNotification('Please enter an item title.', 'error');
                return false;
            }

            if (!priceInput || !priceInput.value || parseFloat(priceInput.value) <= 0) {
                e.preventDefault();
                showNotification('Please enter a valid price.', 'error');
                return false;
            }

            if (!categoryInput || !categoryInput.value) {
                e.preventDefault();
                showNotification('Please select a category.', 'error');
                return false;
            }

            return true;
        });
    });

    // Show notification function
    function showNotification(message, type = 'info') {
        let container = document.getElementById('notificationContainer');
        if (!container) {
            container = document.createElement('div');
            container.id = 'notificationContainer';
            container.style.cssText = 'position: fixed; top: 20px; right: 20px; z-index: 9999; max-width: 400px;';
            document.body.appendChild(container);
        }

        const notification = document.createElement('div');
        notification.style.cssText = `
            background: ${type === 'error' ? 'rgba(255, 107, 107, 0.1)' : 'rgba(121, 231, 163, 0.1)'};
            border: 1px solid ${type === 'error' ? 'rgba(255, 107, 107, 0.3)' : 'rgba(121, 231, 163, 0.3)'};
            color: ${type === 'error' ? '#ff6b6b' : '#79e7a3'};
            padding: 12px 16px;
            border-radius: 8px;
            margin-bottom: 8px;
            font-size: 14px;
            animation: slideIn 0.3s ease;
        `;
        notification.textContent = message;
        container.appendChild(notification);

        setTimeout(() => {
            notification.style.animation = 'slideOut 0.3s ease';
            setTimeout(() => notification.remove(), 300);
        }, 4000);
    }

    // Add CSS animations
    const style = document.createElement('style');
    style.textContent = `
        @keyframes slideIn {
            from { transform: translateX(400px); opacity: 0; }
            to { transform: translateX(0); opacity: 1; }
        }
        @keyframes slideOut {
            from { transform: translateX(0); opacity: 1; }
            to { transform: translateX(400px); opacity: 0; }
        }
        .upload-zone.dragover {
            background: rgba(255, 213, 79, 0.1) !important;
            border-color: var(--primary) !important;
        }
    `;
    document.head.appendChild(style);
});
