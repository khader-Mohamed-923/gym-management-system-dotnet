







function dismissAlert(button) {
    const alert = button.closest('.alert-dismissible');
    if (alert) {
        alert.classList.add('fade-out');
        setTimeout(() => {
            alert.remove();
        }, 500);
    }
}


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




function switchTab(tabId, evt) {
    evt = evt || window.event;
    
    
    document.querySelectorAll('.tab-content').forEach(content => {
        content.style.display = 'none';
    });
    
    
    document.querySelectorAll('.tab-btn').forEach(btn => {
        btn.classList.remove('active');
        btn.style.color = 'var(--text-secondary)';
        btn.style.borderBottom = '2px solid transparent';
    });
    
    
    const tabContent = document.getElementById(tabId);
    if (tabContent) {
        tabContent.style.display = 'block';
    }
    
    
    if (evt && evt.target) {
        evt.target.classList.add('active');
        evt.target.style.color = 'var(--text-primary)';
        evt.target.style.borderBottom = '2px solid var(--fire)';
    }
}



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
