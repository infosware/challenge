
const endpoints = {
    getUsers: 'users/get?pageSize={rowsPerPage}&pageNumber={goToPage}',
    requestGdpr: 'api/users/gdpr',
    requestGdprDelete: 'api/users/gdpr/delete',
};


export function getUsers(pageSize, pageNumber) {
    return fetch(
        endpoints.getUsers.replace('{rowsPerPage}', pageSize).replace('{goToPage}', pageNumber),
        {
            method: 'GET',
        })
        .then((resp) => resp.json());
}

export function requestGdpr(emails) {
    let formData = new FormData();
    formData.append('userEmailsJson', JSON.stringify(emails));

    return fetch(
        endpoints.requestGdpr ,
        {
            method: 'PUT',
            body: formData,
        })
        .then((resp) => resp.json());
}

export function requestGdprDelete(emails) {
    let formData = new FormData();
    formData.append('userEmailsJson', JSON.stringify(emails));

    return fetch(
        endpoints.requestGdprDelete,
        {
            method: 'PUT',
            body: formData,
        })
        .then((resp) => resp.json());
}