
const endpoints = {
    getUsers: 'users/get?pageSize={rowsPerPage}&pageNumber={go-to-page}',
};


export function getUsers(pageSize, pageNumber) {
    return fetch(
        endpoints.getUsers.replace('{rowsPerPage}', pageSize).replace('{go-to-page}', pageNumber),
        {
            method: 'GET',
        }
    )
    .then((resp) => resp.json());
}