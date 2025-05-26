import { DeviceDetector } from './deviceDetector.js';

export const NavbarHelpers = {
    // Toggle the navbar programmatically
    toggleNavbar: function() {
        const navbarContent = document.querySelector('#navbarContent');
        if (!navbarContent) return;
        
        const bsCollapse = bootstrap.Collapse.getInstance(navbarContent) || 
                          new bootstrap.Collapse(navbarContent, { toggle: false });
        bsCollapse.toggle();
        
        // Update the aria-expanded attribute on the toggler
        const navbarToggler = document.querySelector('.navbar-toggler');
        if (navbarToggler) {
            const isExpanded = navbarToggler.getAttribute('aria-expanded') === 'true';
            navbarToggler.setAttribute('aria-expanded', (!isExpanded).toString());
        }
    },
    
    // Close the navbar programmatically
    closeNavbar: function() {
        const navbarContent = document.querySelector('#navbarContent');
        if (!navbarContent) return;
        
        const bsCollapse = bootstrap.Collapse.getInstance(navbarContent);
        if (bsCollapse) {
            bsCollapse.hide();
        }
        
        // Update the aria-expanded attribute on the toggler
        const navbarToggler = document.querySelector('.navbar-toggler');
        if (navbarToggler) {
            navbarToggler.setAttribute('aria-expanded', 'false');
        }
    },
    
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
