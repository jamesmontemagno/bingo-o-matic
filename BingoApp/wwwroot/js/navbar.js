import { DeviceDetector } from './deviceDetector.js';

export const NavbarHelpers = {
    // Store event handler references for cleanup
    _handlers: {
        clickHandler: null,
        resizeHandler: null,
        navLinkHandlers: []
    },

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
        // Remove existing handler if any
        if (this._handlers.clickHandler) {
            document.removeEventListener('click', this._handlers.clickHandler);
        }

        this._handlers.clickHandler = function (event) {
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
        };
        
        document.addEventListener('click', this._handlers.clickHandler);
        
        // Remove existing nav link handlers
        this._handlers.navLinkHandlers.forEach(({element, handler}) => {
            element.removeEventListener('click', handler);
        });
        this._handlers.navLinkHandlers = [];
        
        // Close menu when clicking on a nav link
        const navLinks = document.querySelectorAll('.navbar-nav .nav-link');
        navLinks.forEach(function(link) {
            const handler = function() {
                const navbarMenu = document.getElementById('navbarContent');
                if (navbarMenu && window.innerWidth < 992) { // Only on mobile
                    const bsCollapse = bootstrap.Collapse.getInstance(navbarMenu);
                    if (bsCollapse) {
                        bsCollapse.hide();
                    }
                }
            };
            
            link.addEventListener('click', handler);
            NavbarHelpers._handlers.navLinkHandlers.push({element: link, handler: handler});
        });
    },
    
    // Watch for resize events to ensure proper mobile/desktop behavior
    setupResizeHandler: function() {
        // Remove existing handler if any
        if (this._handlers.resizeHandler) {
            window.removeEventListener('resize', this._handlers.resizeHandler);
        }

        this._handlers.resizeHandler = function() {
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
        };
        
        window.addEventListener('resize', this._handlers.resizeHandler);
    },

    // Cleanup all event listeners to prevent memory leaks
    cleanup: function() {
        // Remove document click handler
        if (this._handlers.clickHandler) {
            document.removeEventListener('click', this._handlers.clickHandler);
            this._handlers.clickHandler = null;
        }

        // Remove window resize handler
        if (this._handlers.resizeHandler) {
            window.removeEventListener('resize', this._handlers.resizeHandler);
            this._handlers.resizeHandler = null;
        }

        // Remove nav link handlers
        this._handlers.navLinkHandlers.forEach(({element, handler}) => {
            element.removeEventListener('click', handler);
        });
        this._handlers.navLinkHandlers = [];
    }
};
