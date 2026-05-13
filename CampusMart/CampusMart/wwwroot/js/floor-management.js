// ═══════════════════════════════════════════════════════════════
// CampusMart — Floor Management (Database-backed)
// ═══════════════════════════════════════════════════════════════

const API = '/Admin/FloorApi';
let floors = [];
let currentFloorId = null;
let stalls = [];

// ── Custom Alert / Confirm ──
function showToast(message, type = 'success') {
    let container = document.getElementById('toastContainer');
    if (!container) {
        container = document.createElement('div');
        container.id = 'toastContainer';
        document.body.appendChild(container);
    }
    const toast = document.createElement('div');
    toast.className = `custom-toast ${type}`;
    const icon = type === 'error' ? '<i class="bi bi-exclamation-triangle-fill"></i>' : '<i class="bi bi-check-circle-fill"></i>';
    toast.innerHTML = `<div style="font-size: 20px;">${icon}</div><div>${message}</div>`;
    container.appendChild(toast);
    
    // Trigger animation
    requestAnimationFrame(() => {
        toast.classList.add('show');
    });

    setTimeout(() => {
        toast.classList.remove('show');
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}

function showConfirm(message) {
    return new Promise((resolve) => {
        let overlay = document.getElementById('customConfirmOverlay');
        if (!overlay) {
            overlay = document.createElement('div');
            overlay.id = 'customConfirmOverlay';
            overlay.innerHTML = `
                <div class="confirm-dialog">
                    <div class="confirm-icon"><i class="bi bi-exclamation-circle" style="color: #ef4444;"></i></div>
                    <div class="confirm-title">Are you sure?</div>
                    <div class="confirm-message" id="confirmMsgText"></div>
                    <div class="confirm-actions">
                        <button class="confirm-btn cancel" id="confirmCancelBtn">Cancel</button>
                        <button class="confirm-btn danger" id="confirmOkBtn">Yes, Delete</button>
                    </div>
                </div>
            `;
            document.body.appendChild(overlay);
        }

        document.getElementById('confirmMsgText').textContent = message;
        overlay.classList.add('active');

        const btnOk = document.getElementById('confirmOkBtn');
        const btnCancel = document.getElementById('confirmCancelBtn');

        const cleanup = () => {
            overlay.classList.remove('active');
            btnOk.onclick = null;
            btnCancel.onclick = null;
        };

        btnOk.onclick = () => { cleanup(); resolve(true); };
        btnCancel.onclick = () => { cleanup(); resolve(false); };
    });
}

// ── API Helper ──

async function apiFetch(url, options = {}) {
    try {
        const res = await fetch(url, options);
        if (!res.ok) {
            const text = await res.text();
            console.error('API Error:', res.status, text);
            showToast('Error: ' + (text || res.statusText), 'error');
            return null;
        }
        if (res.status === 200) {
            const ct = res.headers.get('content-type');
            if (ct && ct.includes('json')) return res.json();
        }
        return true;
    } catch (err) {
        console.error('Network error:', err);
        showToast('Network error. Check console.', 'error');
        return null;
    }
}

// ── Time Utilities ──

function isStallOpen(openTime, closeTime) {
    if (!openTime || !closeTime) return false;
    const now = new Date();
    const current = now.getHours() * 60 + now.getMinutes();
    const [oh, om] = openTime.split(':').map(Number);
    const [ch, cm] = closeTime.split(':').map(Number);
    return current >= (oh * 60 + (om || 0)) && current < (ch * 60 + (cm || 0));
}

function formatTime(t) {
    if (!t) return '';
    const [h, m] = t.split(':').map(Number);
    const ampm = h >= 12 ? 'PM' : 'AM';
    const hour = h % 12 || 12;
    return `${hour}:${String(m || 0).padStart(2, '0')} ${ampm}`;
}

function categoryColor(cat) {
    const c = (cat || '').toLowerCase();
    return { food: '#ffdd67', electronics: '#7cc3ff', textbooks: '#9effb2', clothing: '#f0a0ff', stationery: '#ffb07c', services: '#80e0d0', other: '#ccc' }[c] || '#ccc';
}

// ══════════════════════════════════════
// FLOORS
// ══════════════════════════════════════

async function loadFloors() {
    floors = await apiFetch(`${API}/floors`) || [];
    renderFloorButtons();
    updateGlobalMetrics();

    if (floors.length > 0) {
        if (!currentFloorId || !floors.find(f => f.id === currentFloorId)) {
            currentFloorId = floors[0].id;
        }
        renderFloorButtons(); // re-render with correct active state
        await loadStalls();
    } else {
        currentFloorId = null;
        stalls = [];
        renderStalls();
        updateCapacity();
    }
}

function renderFloorButtons() {
    const container = document.getElementById('floorButtons');
    if (!container) return;

    container.innerHTML = floors.map(f => `
        <button class="floor-btn ${f.id === currentFloorId ? 'active' : ''}" data-id="${f.id}" style="word-break: break-word; white-space: normal; min-height: 40px; padding: 8px 12px;">
            Floor ${f.floorNumber}
        </button>
    `).join('');

    container.querySelectorAll('.floor-btn').forEach(btn => {
        btn.addEventListener('click', async () => {
            currentFloorId = Number(btn.dataset.id);
            renderFloorButtons();
            await loadStalls();
        });
    });
}

// ══════════════════════════════════════
// STALLS
// ══════════════════════════════════════

async function loadStalls() {
    if (!currentFloorId) { stalls = []; renderStalls(); return; }
    stalls = await apiFetch(`${API}/floors/${currentFloorId}/stalls`) || [];
    renderStalls();
    updateCapacity();
}

function renderStalls() {
    const grid = document.getElementById('stallGrid');
    const header = document.getElementById('stallsHeader');
    if (!grid) return;

    const floor = floors.find(f => f.id === currentFloorId);
    const floorNum = floor ? floor.floorNumber : '?';
    if (header) header.textContent = `Active Stalls — Floor ${floorNum}`;

    if (stalls.length === 0) {
        grid.innerHTML = `
            <div style="grid-column: 1/-1; text-align:center; padding: 60px 0; color: var(--text-muted);">
                <div style="font-size:48px; margin-bottom:12px; opacity: 0.2;"><i class="bi bi-shop"></i></div>
                <h3 style="font-weight:700; color:#fff;">No stalls yet</h3>
                <p>Click "Create Stall" to add the first stall to this floor.</p>
            </div>`;
        return;
    }

    grid.innerHTML = stalls.map(s => {
        const open = isStallOpen(s.openTime, s.closeTime);
        const statusClass = s.isActive ? (open ? 'status-open' : 'status-closed') : 'status-inactive';
        const statusText = s.isActive ? (open ? 'Open' : 'Closed') : 'Inactive';
        const catColor = categoryColor(s.category);

        return `
        <article class="stall-card">
            ${s.imageUrl ? `
                <div class="stall-card-img">
                    <img src="${s.imageUrl}" alt="${s.name}">
                </div>` : `
                <div class="stall-card-img d-flex align-items-center justify-content-center" style="font-size: 40px; color: rgba(255,255,255,0.1);">
                    <i class="bi bi-shop"></i>
                </div>`}
            <div class="stall-card-body">
                <div class="stall-header">
                    <h3 class="stall-name">${s.name}</h3>
                    <span class="status-badge ${statusClass}">${statusText}</span>
                </div>
                <div class="stall-id"><i class="bi bi-hash"></i> ${s.stallNumber}</div>
                <div class="stall-meta"><span><i class="bi bi-person"></i></span> Owner: <strong>${s.ownerName || '—'}</strong></div>
                <div class="stall-meta"><span><i class="bi bi-tag"></i></span> Category: <span class="pill" style="color:${catColor}; border-color:${catColor}44;">${s.category || 'N/A'}</span></div>
                <div class="stall-meta"><span><i class="bi bi-clock"></i></span> ${formatTime(s.openTime)} — ${formatTime(s.closeTime)}</div>
                
                <div class="stall-actions">
                    <button class="tool-btn" onclick="openEditStall(${s.id})" title="Edit Settings">
                        <i class="bi bi-pencil-square"></i>
                    </button>
                    <button class="tool-btn" onclick="openStallDetail(${s.id})" title="Manage Items">
                        <i class="bi bi-box-seam"></i>
                    </button>
                    <button class="tool-btn" onclick="toggleStall(${s.id})" title="${s.isActive ? 'Deactivate' : 'Activate'}">
                        <i class="bi ${s.isActive ? 'bi-slash-circle' : 'bi-check-circle'}"></i>
                    </button>
                    <button class="tool-btn" style="color: #ef4444;" onclick="deleteStall(${s.id})" title="Delete Stall">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </div>
        </article>`;
    }).join('');
}

function updateCapacity() {
    const floor = floors.find(f => f.id === currentFloorId);
    if (!floor) return;

    const active = stalls.filter(s => s.isActive).length;
    const cap = floor.capacity;
    const pct = cap > 0 ? Math.round((active / cap) * 100) : 0;

    const el = (id) => document.getElementById(id);
    if (el('capacityLabel')) el('capacityLabel').textContent = `Floor ${floor.floorNumber} Utilization`;
    if (el('capacityUsed')) el('capacityUsed').textContent = active;
    if (el('capacityTotal')) el('capacityTotal').textContent = `/ ${cap} stalls occupied`;
    if (el('capacityFill')) el('capacityFill').style.width = `${pct}%`;
    if (el('currentOccupancy')) el('currentOccupancy').textContent = `${pct}%`;
    if (el('occupancySub')) el('occupancySub').textContent = `${active} of ${cap} stalls used`;
}

function updateGlobalMetrics() {
    const total = floors.reduce((sum, f) => sum + (f.stallCount || 0), 0);
    const el = document.getElementById('totalActiveStalls');
    if (el) el.textContent = total;
    const sub = document.getElementById('totalStallsSub');
    if (sub) sub.innerHTML = `<i class="bi bi-shop"></i> across ${floors.length} levels`;
}

// ══════════════════════════════════════
// EVENT LISTENERS — DOMContentLoaded
// ══════════════════════════════════════

document.addEventListener('DOMContentLoaded', () => {
    // Load data
    loadFloors();

    // ── Add Floor Button ──
    const addFloorBtn = document.getElementById('addFloorBtn');
    if (addFloorBtn) {
        addFloorBtn.addEventListener('click', () => {
            document.getElementById('newFloorName').value = '';
            document.getElementById('newFloorNumber').value = floors.length + 1;
            document.getElementById('newFloorCapacity').value = 8;
            showModal('addFloorModal');
        });
    }

    // ── Save Floor Button ──
    const saveFloorBtn = document.getElementById('saveFloorBtn');
    if (saveFloorBtn) {
        saveFloorBtn.addEventListener('click', async () => {
            const name = document.getElementById('newFloorName').value.trim();
            const num = parseInt(document.getElementById('newFloorNumber').value) || 1;
            const cap = parseInt(document.getElementById('newFloorCapacity').value) || 8;

            if (!name) { showToast('Please enter a floor name.', 'error'); return; }

            saveFloorBtn.disabled = true;
            saveFloorBtn.textContent = 'Creating...';

            const result = await apiFetch(`${API}/floors`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ name, floorNumber: num, capacity: cap })
            });

            saveFloorBtn.disabled = false;
            saveFloorBtn.textContent = 'Create Floor';

            if (result) {
                showToast('Floor created successfully!', 'success');
                closeModal('addFloorModal');
                currentFloorId = result.id;
                await loadFloors();
            }
        });
    }

    // ── Add Stall Button ──
    const addStallBtn = document.getElementById('addStallBtn');
    if (addStallBtn) {
        addStallBtn.addEventListener('click', () => {
            if (!currentFloorId) { showToast('Please create a floor first.', 'error'); return; }
            document.getElementById('stallEditId').value = '';
            document.getElementById('stallModalTitle').textContent = 'Create Stall';
            document.getElementById('saveStallBtn').innerHTML = '<i class="bi bi-plus-lg"></i> Create Stall';
            document.getElementById('stallForm').reset();
            document.getElementById('stallImagePreview').innerHTML = '';
            document.getElementById('stallIsActive').value = 'true';
            showModal('stallModal');
        });
    }

    // ── Stall Form Submit ──
    const stallForm = document.getElementById('stallForm');
    if (stallForm) {
        stallForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const editId = document.getElementById('stallEditId').value;
            const submitBtn = document.getElementById('saveStallBtn');

            const fd = new FormData();
            fd.append('Name', document.getElementById('stallName').value);
            fd.append('StallNumber', document.getElementById('stallNumber').value);
            fd.append('OwnerName', document.getElementById('stallOwner').value);
            fd.append('Category', document.getElementById('stallCategory').value);
            fd.append('Description', document.getElementById('stallDesc').value);
            fd.append('OpenTime', document.getElementById('stallOpenTime').value);
            fd.append('CloseTime', document.getElementById('stallCloseTime').value);
            fd.append('IsActive', document.getElementById('stallIsActive').value === 'true');
            fd.append('FloorId', currentFloorId);

            const imgInput = document.getElementById('stallImage');
            if (imgInput.files.length > 0) fd.append('Image', imgInput.files[0]);

            submitBtn.disabled = true;
            submitBtn.innerHTML = '<i class="bi bi-hourglass-split spin"></i> Saving...';

            let url = `${API}/stalls`;
            let method = 'POST';
            if (editId) {
                url = `${API}/stalls/${editId}`;
                method = 'PUT';
            }

            const result = await apiFetch(url, { method, body: fd });

            submitBtn.disabled = false;
            submitBtn.innerHTML = editId ? '<i class="bi bi-save-fill"></i> Save Changes' : '<i class="bi bi-plus-lg"></i> Create Stall';

            if (result) {
                showToast(editId ? 'Stall updated!' : 'Stall created!', 'success');
                closeModal('stallModal');
                await loadFloors();
                await loadStalls();
            }
        });
    }

    // ── Add Item Form ──
    const addItemForm = document.getElementById('addItemForm');
    if (addItemForm) {
        addItemForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const stallId = document.getElementById('itemStallId').value;
            const submitBtn = addItemForm.querySelector('button[type="submit"]');

            const fd = new FormData();
            fd.append('Name', document.getElementById('itemName').value);
            fd.append('Price', document.getElementById('itemPrice').value || 0);
            fd.append('Description', document.getElementById('itemDesc').value);
            fd.append('Stock', document.getElementById('itemStock').value || 0);
            fd.append('CategoryId', document.getElementById('itemCategory').value);
            fd.append('StallId', stallId);

            const imgInput = document.getElementById('itemImage');
            if (imgInput.files.length > 0) fd.append('Image', imgInput.files[0]);

            submitBtn.disabled = true;
            const originalHtml = submitBtn.innerHTML;
            submitBtn.innerHTML = '<i class="bi bi-hourglass-split spin"></i> Uploading...';

            const result = await apiFetch(`${API}/products`, { method: 'POST', body: fd });
            
            submitBtn.disabled = false;
            submitBtn.innerHTML = originalHtml;

            if (result) {
                showToast('Product added!', 'success');
                addItemForm.reset();
                document.getElementById('itemStallId').value = stallId;
                document.getElementById('itemImagePreview').innerHTML = ''; // Clear preview
                await loadStallItems(stallId);
            }
        });
    }

    // ── Add Category Form ──
    const addCategoryForm = document.getElementById('addCategoryForm');
    if (addCategoryForm) {
        addCategoryForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const stallId = document.getElementById('itemStallId').value;
            const name = document.getElementById('newCatName').value;
            const icon = document.getElementById('newCatIcon').value;
            const submitBtn = addCategoryForm.querySelector('button[type="submit"]');

            submitBtn.disabled = true;

            const result = await apiFetch(`${API}/categories`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ name, icon, stallId: parseInt(stallId) })
            });

            submitBtn.disabled = false;

            if (result) {
                showToast('Category created!', 'success');
                closeModal('addCategoryModal');
                addCategoryForm.reset();
                await loadCategories(stallId);
            }
        });
    }

    // ── Image Preview Logic ──
    const setupPreview = (inputSelector, previewSelector) => {
        const input = document.getElementById(inputSelector);
        const preview = document.getElementById(previewSelector);
        if (input && preview) {
            input.addEventListener('change', function () {
                if (this.files.length > 0) {
                    const url = URL.createObjectURL(this.files[0]);
                    preview.innerHTML = `<img src="${url}" style="max-width:100%; max-height:120px; border-radius:12px; object-fit:cover; border: 2px solid var(--admin-primary); margin-top: 8px;">`;
                } else {
                    preview.innerHTML = '';
                }
            });
        }
    };

    setupPreview('stallImage', 'stallImagePreview');
    setupPreview('itemImage', 'itemImagePreview');
});

// ══════════════════════════════════════
// GLOBAL STALL ACTIONS (called from onclick)
// ══════════════════════════════════════

async function toggleStall(id) {
    await apiFetch(`${API}/stalls/${id}/toggle`, { method: 'POST' });
    showToast('Status toggled.');
    await loadFloors();
    await loadStalls();
}

async function deleteStall(id) {
    const confirmed = await showConfirm('Are you sure you want to delete this stall and all its items?');
    if (!confirmed) return;
    
    const result = await apiFetch(`${API}/stalls/${id}`, { method: 'DELETE' });
    if (result) {
        showToast('Stall deleted.', 'success');
        await loadFloors();
        await loadStalls();
    }
}

async function deleteFloor() {
    if (!currentFloorId) {
        showToast('No floor selected.', 'error');
        return;
    }
    const confirmed = await showConfirm('Are you sure you want to delete this floor and all its stalls?');
    if (!confirmed) return;
    
    const result = await apiFetch(`${API}/floors/${currentFloorId}`, { method: 'DELETE' });
    if (result) {
        showToast('Floor deleted.', 'success');
        currentFloorId = null;
        await loadFloors();
    }
}

function openEditStall(id) {
    const stall = stalls.find(s => s.id === id);
    if (!stall) return;

    document.getElementById('stallEditId').value = id;
    document.getElementById('stallModalTitle').textContent = 'Edit Stall Settings';
    document.getElementById('saveStallBtn').innerHTML = '<i class="bi bi-save-fill"></i> Save Changes';
    document.getElementById('stallName').value = stall.name;
    document.getElementById('stallNumber').value = stall.stallNumber;
    document.getElementById('stallOwner').value = stall.ownerName;
    document.getElementById('stallCategory').value = stall.category;
    document.getElementById('stallDesc').value = stall.description || '';
    document.getElementById('stallOpenTime').value = stall.openTime || '08:00';
    document.getElementById('stallCloseTime').value = stall.closeTime || '20:00';
    document.getElementById('stallIsActive').value = stall.isActive ? 'true' : 'false';

    const preview = document.getElementById('stallImagePreview');
    if (stall.imageUrl) {
        preview.innerHTML = `<img src="${stall.imageUrl}" style="max-width:100%; max-height:120px; border-radius:12px; object-fit:cover; border: 2px solid var(--admin-primary); margin-top: 8px;">`;
    } else {
        preview.innerHTML = '';
    }

    showModal('stallModal');
}

// ── Stall Detail & Items ──

async function loadCategories(stallId) {
    const cats = await apiFetch(`${API}/categories?stallId=${stallId}`) || [];
    const select = document.getElementById('itemCategory');
    if (select) {
        select.innerHTML = cats.map(c => `<option value="${c.id}">${c.name}</option>`).join('');
    }
}

async function openStallDetail(id) {
    const stall = stalls.find(s => s.id === id);
    if (!stall) return;

    document.getElementById('stallDetailTitle').textContent = stall.name;
    document.getElementById('itemStallId').value = id;

    const open = isStallOpen(stall.openTime, stall.closeTime);
    const statusColor = stall.isActive ? (open ? '#79e7a3' : '#ff8d8d') : '#666';
    const statusText = stall.isActive ? (open ? 'Open' : 'Closed') : 'Inactive';

    document.getElementById('stallDetailContent').innerHTML = `
        ${stall.imageUrl ? `<img src="${stall.imageUrl}" style="width:100%; max-height:220px; object-fit:cover; border-radius:16px; margin-bottom:20px; border: 1px solid var(--admin-border);">` : ''}
        <div style="display:grid; grid-template-columns:repeat(auto-fit, minmax(120px, 1fr)); gap:16px; background:rgba(255,255,255,0.02); border:1px solid var(--admin-border); border-radius:16px; padding:24px;">
            <div><span style="color:var(--text-muted); font-size:11px; text-transform:uppercase; font-weight:800; letter-spacing:1px;">STALL ID</span><br><strong style="font-size:16px;">${stall.stallNumber}</strong></div>
            <div><span style="color:var(--text-muted); font-size:11px; text-transform:uppercase; font-weight:800; letter-spacing:1px;">OWNER</span><br><strong style="font-size:16px;">${stall.ownerName || '—'}</strong></div>
            <div><span style="color:var(--text-muted); font-size:11px; text-transform:uppercase; font-weight:800; letter-spacing:1px;">CATEGORY</span><br><span class="pill" style="color:${categoryColor(stall.category)}; border-color:${categoryColor(stall.category)}44; margin-left:0; font-size:10px;">${stall.category || 'N/A'}</span></div>
            <div><span style="color:var(--text-muted); font-size:11px; text-transform:uppercase; font-weight:800; letter-spacing:1px;">STATUS</span><br><span style="color:${statusColor}; font-weight:800; font-size:16px;">${statusText}</span></div>
        </div>
        <div style="margin-top:16px; font-size:14px; color:var(--text-muted); padding: 0 10px;">
            <i class="bi bi-clock-history"></i> Operating Hours: <strong style="color:#fff;">${formatTime(stall.openTime)} — ${formatTime(stall.closeTime)}</strong>
        </div>`;


    showModal('stallDetailModal');
    await loadCategories(id);
    await loadStallItems(id);
}

async function loadStallItems(stallId) {
    const items = await apiFetch(`${API}/stalls/${stallId}/products`) || [];
    const container = document.getElementById('stallItemsList');

    if (items.length === 0) {
        container.innerHTML = `<div style="text-align:center; padding:40px; color:var(--text-muted); background: rgba(0,0,0,0.1); border-radius: 12px; border: 1px dashed var(--admin-border);">
            <i class="bi bi-box-seam fs-2 d-block mb-2"></i> No items added yet.
        </div>`;
        return;
    }

    container.innerHTML = items.map(item => `
        <div style="display:flex; align-items:center; gap:16px; padding:16px; border:1px solid var(--admin-border); border-radius:14px; margin-bottom:12px; background:rgba(255,255,255,0.03); transition: 0.2s;">
            ${item.imageUrl
                ? `<img src="${item.imageUrl}" style="width:56px; height:56px; border-radius:10px; object-fit:cover; border: 1px solid var(--admin-border);">`
                : `<div style="width:56px; height:56px; border-radius:10px; background:rgba(255,255,255,0.05); display:flex; align-items:center; justify-content:center; font-size:24px; color: rgba(255,255,255,0.1);"><i class="bi bi-box"></i></div>`
            }
            <div style="flex:1;">
                <div style="font-weight:700; color: #fff;">${item.name}</div>
                <div style="font-size:12px; color:var(--text-muted);">
                    <span class="badge bg-secondary" style="font-size: 10px; opacity: 0.8;">${item.categoryName || 'General'}</span>
                    ${item.description || 'No description provided.'}
                </div>
            </div>
            <div style="text-align:right;">
                <div style="font-weight:800; color:var(--admin-primary); font-size: 16px;">₱${Number(item.price).toFixed(2)}</div>
                <div style="font-size:11px; color:var(--text-muted); font-weight: 700;">Stock: ${item.stock}</div>
            </div>
            <button class="tool-btn" style="color: #ef4444; width: 32px; height: 32px; font-size: 14px;" onclick="deleteStallItem(${item.id}, ${stallId})">
                <i class="bi bi-x-lg"></i>
            </button>
        </div>
    `).join('');
}

async function deleteStallItem(itemId, stallId) {
    const confirmed = await showConfirm('Are you sure you want to delete this item?');
    if (!confirmed) return;
    await apiFetch(`${API}/products/${itemId}`, { method: 'DELETE' });
    showToast('Product deleted.', 'success');
    await loadStallItems(stallId);
}
