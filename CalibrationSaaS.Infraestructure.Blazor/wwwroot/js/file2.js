
window.getOnline =  (parameter) => {
    return navigator.onLine;
};
window.closeModalWindow = (id) => {

    $('#' + id).modal('hide');
};
