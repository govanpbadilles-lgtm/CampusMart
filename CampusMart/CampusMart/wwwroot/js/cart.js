document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.elite-qty-selector button').forEach(btn => {
        btn.addEventListener('click', async function () {
            const action = this.dataset.action;
            const itemElement = this.closest('.cart-item-elite');
            const cartItemId = itemElement.dataset.itemId;
            const qtyInput = itemElement.querySelector('input[type="number"]');
            
            let qty = parseInt(qtyInput.value || '1');
            
            if (action === 'increase') qty++;
            else if (action === 'decrease' && qty > 1) qty--;
            else return; // Don't allow qty < 1 through this button

            // Optimistic UI update
            qtyInput.value = qty;
            
            // Post update
            try {
                const response = await fetch('/Cart/UpdateQuantity', {
                    method: 'POST',
                    credentials: 'same-origin',
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                    body: `cartItemId=${cartItemId}&quantity=${qty}`
                });
                
                if (response.ok) {
                    // Force reload for accurate total and flash messages
                    window.location.reload();
                } else {
                    console.error('Failed to update quantity');
                }
            } catch(e) {
                console.error(e);
            }
        });
    });
});
