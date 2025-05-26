// Device detection utilities
export const DeviceDetector = {
    isMobileDevice: () => {
        return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini|Windows Phone/i.test(navigator.userAgent) 
            || (window.innerWidth <= 768);
    }
};
