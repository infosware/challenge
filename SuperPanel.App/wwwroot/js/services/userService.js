
const endpoints = {
    getUsers: 'users/get?pageSize={rowsPerPage}&pageNumber={goToPage}',
};


export function getUsers(pageSize, pageNumber) {
    return fetch(
        endpoints.getUsers.replace('{rowsPerPage}', pageSize).replace('{goToPage}', pageNumber),
        {
            method: 'GET',
        }
    )
    .then((resp) => resp.json());
}