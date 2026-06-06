// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// ===== Iron UI Alert System =====

/**
 * Dismiss an alert with fade-out animation
 * @param {HTMLElement} button - The close button element
 */
function dismissAlert(button) {
    const alert = button.closest('.alert-dismissible');
    if (alert) {
        alert.classList.add('fade-out');
        setTimeout(() => {
            alert.remove();
        }, 500);
    }
}

/**
 * Auto-dismiss alerts after specified timeout
 */
document.addEventListener('DOMContentLoaded', function() {
    const alerts = document.querySelectorAll('.alert-dismissible[data-auto-dismiss]');
    
    alerts.forEach(alert => {
        const timeout = parseInt(alert.dataset.autoDismiss) || 5000;
        
        setTimeout(() => {
            if (alert && alert.parentNode) {
                alert.classList.add('fade-out');
                setTimeout(() => {
                    if (alert && alert.parentNode) {
                        alert.remove();
                    }
                }, 500);
            }
        }, timeout);
    });
});

// ===== Tab Navigation System =====

/**
 * Switch between tab content panels
 * @param {string} tabId - The ID of the tab content to show
 * @param {Event} evt - The click event (optional, uses window.event if not provided)
 */
function switchTab(tabId, evt) {
    evt = evt || window.event;
    
    // Hide all tab contents
    document.querySelectorAll('.tab-content').forEach(content => {
        content.style.display = 'none';
    });
    
    // Remove active class from all tab buttons
    document.querySelectorAll('.tab-btn').forEach(btn => {
        btn.classList.remove('active');
        btn.style.color = 'var(--text-secondary)';
        btn.style.borderBottom = '2px solid transparent';
    });
    
    // Show selected tab content
    const tabContent = document.getElementById(tabId);
    if (tabContent) {
        tabContent.style.display = 'block';
    }
    
    // Add active class to clicked button
    if (evt && evt.target) {
        evt.target.classList.add('active');
        evt.target.style.color = 'var(--text-primary)';
        evt.target.style.borderBottom = '2px solid var(--fire)';
    }
}

// ===== Active Navigation Highlighting =====

document.addEventListener('DOMContentLoaded', function() {
    const currentPath = window.location.pathname.toLowerCase();
    const navLinks = document.querySelectorAll('.nav-link-iron');
    
    navLinks.forEach(link => {
        link.classList.remove('active');
        const href = link.getAttribute('href');
        if (href && href !== '#') {
            if (currentPath.includes(href.toLowerCase()) || 
                (currentPath === '/' && href.includes('Home'))) {
                link.classList.add('active');
            }
        }
    });
});
