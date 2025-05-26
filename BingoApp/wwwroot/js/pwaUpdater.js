// PWA Updater JavaScript module
let registration;
let dotNetHelper;
let waitingWorker = null;

// Initialize the updater with a DotNet reference to the component
export function initialize(dotNetRef) {
    dotNetHelper = dotNetRef;

    // Check if service worker is supported
    if ('serviceWorker' in navigator) {
        // Try to get the service worker registration
        navigator.serviceWorker.getRegistration()
            .then(reg => {
                if (!reg) {
                    console.info('No service worker registration found.');
                    return;
                }

                registration = reg;
                
                // If there's already a waiting worker, notify the component
                if (registration.waiting) {
                    waitingWorker = registration.waiting;
                    dotNetHelper.invokeMethodAsync('OnUpdateAvailable');
                }

                // Add listeners for future updates
                registration.addEventListener('updatefound', () => {
                    trackInstalling(registration.installing);
                });
            })
            .catch(error => {
                console.error('Error during service worker registration:', error);
            });

        // Listen for controller change to reload the page
        navigator.serviceWorker.addEventListener('controllerchange', () => {
            window.location.reload();
        });
    }
}

// Track the installing worker and notify when it's ready
function trackInstalling(worker) {
    if (!worker) return;
    
    worker.addEventListener('statechange', () => {
        if (worker.state === 'installed' && navigator.serviceWorker.controller) {
            waitingWorker = worker;
            dotNetHelper.invokeMethodAsync('OnUpdateAvailable');
        }
    });
}

// Tell the service worker to skip waiting and activate the new version
export function updateApplication() {
    if (waitingWorker) {
        // Send message to the service worker to skip waiting
        waitingWorker.postMessage({ type: 'SKIP_WAITING' });
    }
}