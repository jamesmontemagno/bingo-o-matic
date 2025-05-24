/**
 * Helper functions for Navbar Bootstrap integration
 */
window.navbarHelpers = {
    // Close the navbar when clicking outside
    setupNavbarAutoClose: function () {
        document.addEventListener('click', function (event) {
            // Only run if navbar is expanded
            const navbarToggler = document.querySelector('.navbar-toggler');
            if (!navbarToggler || navbarToggler.getAttribute('aria-expanded') !== 'true') {
                return;
            }
            
            const navbarMenu = document.getElementById('navbarContent');
            if (!navbarMenu) return;
            
            // Check if click is outside the navbar
            if (!navbarMenu.contains(event.target) && !navbarToggler.contains(event.target)) {
                // Using Bootstrap's collapse API
                const bsCollapse = bootstrap.Collapse.getInstance(navbarMenu) || new bootstrap.Collapse(navbarMenu);
                bsCollapse.hide();
            }
        });
        
        // Close menu when clicking on a nav link
        const navLinks = document.querySelectorAll('.navbar-nav .nav-link');
        navLinks.forEach(function(link) {
            link.addEventListener('click', function() {
                const navbarMenu = document.getElementById('navbarContent');
                if (navbarMenu && window.innerWidth < 992) { // Only on mobile
                    const bsCollapse = bootstrap.Collapse.getInstance(navbarMenu);
                    if (bsCollapse) {
                        bsCollapse.hide();
                    }
                }
            });
        });
    },
    
    // Watch for resize events to ensure proper mobile/desktop behavior
    setupResizeHandler: function() {
        window.addEventListener('resize', function() {
            // Reset expanded state when switching between mobile and desktop
            const navbarToggler = document.querySelector('.navbar-toggler');
            if (window.innerWidth >= 992 && navbarToggler && navbarToggler.getAttribute('aria-expanded') === 'true') {
                const navbarMenu = document.getElementById('navbarContent');
                const bsCollapse = bootstrap.Collapse.getInstance(navbarMenu);
                if (bsCollapse) {
                    bsCollapse.hide();
                }
                navbarToggler.setAttribute('aria-expanded', 'false');
            }
        });
    }
};
