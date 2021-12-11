
export function showLoadingOverlay(element) {
    element && element.length ? element.LoadingOverlay('show') : $.LoadingOverlay('show');
}

export function hideLoadingOverlay(element) {
    element && element.length ? element.LoadingOverlay('hide', true) : $.LoadingOverlay('hide', true);
}