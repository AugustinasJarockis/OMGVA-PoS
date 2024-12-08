﻿function getTokenPayload(token: string) {
    const arrayToken = token.split('.');
    return JSON.parse(atob(arrayToken[1]));
}

function getTokenRole(token: string) {
    return getTokenPayload(token)['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
}
function getTokenBusinessId(token: string) {
    return getTokenPayload(token)['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid'];
}
function getTokenUserId(token: string) {
    return getTokenPayload(token)['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
}

//NOTE: Checks if name equals to the one in the token. Not if username is equal.
function getTokenUserName(token: string) {
    return getTokenPayload(token)['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
}

export { getTokenRole, getTokenBusinessId, getTokenUserId, getTokenUserName }