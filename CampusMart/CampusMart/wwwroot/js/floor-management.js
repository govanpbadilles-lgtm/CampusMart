const floors = [
    {
        floor: 1,
        capacity: 8,
        stalls: [
            { name: "Starter Cafe", stall: "Stall 101", owner: "Jamie Sy", category: "food" },
            { name: "Basic Prints", stall: "Stall 102", owner: "Nico Lee", category: "textbooks" }
        ]
    },
    {
        floor: 2,
        capacity: 8,
        stalls: [
            { name: "Tech Hub", stall: "Stall 201", owner: "Alex Mercer", category: "electronics" },
            { name: "Campus Books", stall: "Stall 202", owner: "Sarah Jenkins", category: "textbooks" },
            { name: "The Grind Cafe", stall: "Stall 203", owner: "David Chen", category: "food" },
            { name: "Study Stop", stall: "Stall 204", owner: "Mila Gomez", category: "textbooks" },
            { name: "Campus Gear", stall: "Stall 205", owner: "Victor Tan", category: "electronics" },
            { name: "Snack Nook", stall: "Stall 206", owner: "Ari Flores", category: "food" }
        ]
    },
    {
        floor: 3,
        capacity: 8,
        stalls: [
            { name: "Photo Lab", stall: "Stall 301", owner: "Renee Lim", category: "electronics" },
            { name: "Uniform Depot", stall: "Stall 302", owner: "Troy Ong", category: "textbooks" },
            { name: "Food Corner", stall: "Stall 303", owner: "Lia Cruz", category: "food" },
            { name: "Book Point", stall: "Stall 304", owner: "Nia Bello", category: "textbooks" }
        ]
    },
    {
        floor: 4,
        capacity: 8,
        stalls: [
            { name: "Music Spot", stall: "Stall 401", owner: "Ian Yu", category: "electronics" },
            { name: "Print Express", stall: "Stall 402", owner: "Bea Lim", category: "textbooks" },
            { name: "Bean Loft", stall: "Stall 403", owner: "Cara Diaz", category: "food" }
        ]
    },
    {
        floor: 5,
        capacity: 8,
        stalls: [
            { name: "Senior Cafe", stall: "Stall 501", owner: "Noel Sy", category: "food" }
        ]
    }
];

let currentFloor = 2;

function renderFloors() {
    const buttons = document.getElementById("floorButtons");
    if (!buttons) return;
    buttons.innerHTML = floors.map((item) => `
        <button class="floor-btn ${item.floor === currentFloor ? "active" : ""}" data-floor="${item.floor}">
            Floor ${item.floor}
        </button>
    `).join("");

    document.querySelectorAll(".floor-btn").forEach((button) => {
        button.addEventListener("click", () => {
            currentFloor = Number(button.dataset.floor);
            renderFloors();
            renderStalls();
            updateCapacity();
        });
    });
}

function renderStalls() {
    const floorData = floors.find((item) => item.floor === currentFloor);
    const grid = document.getElementById("stallGrid");
    if (!grid) return;
    
    document.getElementById("stallsHeader").textContent = `Active Stalls - Floor ${currentFloor}`;

    grid.innerHTML = floorData.stalls.map((stall, index) => `
        <article class="stall-card">
            <h3 class="stall-name">${stall.name}</h3>
            <div class="stall-id">${stall.stall}</div>
            <div class="stall-meta">Owner: ${stall.owner}</div>
            <div class="stall-meta">Category <span class="pill ${stall.category}">${stall.category}</span></div>
            <div class="stall-actions">
                <button class="btn" data-edit="${index}">Edit</button>
                <button class="btn warn" data-toggle="${index}">Deactivate</button>
            </div>
        </article>
    `).join("");

    let currentEditIndex = -1;

    grid.querySelectorAll("[data-edit]").forEach((btn) => {
        btn.addEventListener("click", () => {
            currentEditIndex = Number(btn.dataset.edit);
            const target = floorData.stalls[currentEditIndex];
            
            const modal = document.getElementById("editStallModal");
            if(modal) {
                document.getElementById("editStallName").value = target.name;
                document.getElementById("editStallOwner").value = target.owner;
                modal.style.display = "flex";
            } else {
                // Fallback if modal is missing
                const newName = prompt("Update stall name:", target.name);
                if (newName && newName.trim()) {
                    target.name = newName.trim();
                    renderStalls();
                }
            }
        });
    });

    const modal = document.getElementById("editStallModal");
    if(modal) {
        document.getElementById("closeEditModal").onclick = () => modal.style.display = "none";
        document.getElementById("cancelEditBtn").onclick = () => modal.style.display = "none";
        document.getElementById("saveStallBtn").onclick = () => {
            if(currentEditIndex >= 0) {
                const newName = document.getElementById("editStallName").value;
                const newOwner = document.getElementById("editStallOwner").value;
                if(newName && newName.trim()) {
                    floorData.stalls[currentEditIndex].name = newName.trim();
                    floorData.stalls[currentEditIndex].owner = newOwner.trim();
                    renderStalls();
                }
                modal.style.display = "none";
            }
        };
    }

    grid.querySelectorAll("[data-toggle]").forEach((btn) => {
        btn.addEventListener("click", () => {
            const index = Number(btn.dataset.toggle);
            floorData.stalls.splice(index, 1);
            renderStalls();
            updateCapacity();
            updateMetrics();
        });
    });
}

function updateCapacity() {
    const floorData = floors.find((item) => item.floor === currentFloor);
    if (!floorData) return;
    const used = floorData.stalls.length;
    const total = floorData.capacity;
    const percent = Math.round((used / total) * 100);

    const capLabel = document.getElementById("capacityLabel");
    if (capLabel) {
        capLabel.textContent = `Floor ${currentFloor} Capacity`;
        document.getElementById("capacityUsed").textContent = used;
        document.getElementById("capacityTotal").textContent = `/ ${total} occupied`;
        document.getElementById("capacityFill").style.width = `${percent}%`;
    }
}

function updateMetrics() {
    const activeStalls = floors.reduce((total, floor) => total + floor.stalls.length, 0);
    const totalEl = document.getElementById("totalActiveStalls");
    if (totalEl) totalEl.textContent = activeStalls;
    
    const dashEl = document.getElementById("dashStalls");
    if (dashEl) dashEl.textContent = activeStalls;
}

document.addEventListener('DOMContentLoaded', () => {
    if (document.getElementById("floorButtons")) {
        renderFloors();
        renderStalls();
        updateCapacity();
        updateMetrics();
    }
});
