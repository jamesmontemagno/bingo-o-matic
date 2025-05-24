let dotNetHelper = null;

window.addSpacebarListener = (helper) => {
    dotNetHelper = helper;
    document.addEventListener('keydown', handleKeydown);
};

window.removeSpacebarListener = () => {
    document.removeEventListener('keydown', handleKeydown);
    dotNetHelper = null;
};

function handleKeydown(event) {
    if (event.code === 'Space' && event.target.tagName !== 'INPUT' && event.target.tagName !== 'TEXTAREA') {
        event.preventDefault();
        dotNetHelper.invokeMethodAsync('HandleSpacebar');
    }
}
