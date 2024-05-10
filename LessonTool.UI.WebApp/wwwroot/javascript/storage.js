function getFromLocalStorage(storageKey) {
    return localStorage.getItem(storageKey);
}

function removeFromLocalStorage(storageKey) {
    localStorage.removeItem(storageKey);
}

function addToLocalStorage(storageKey, item) {
    localStorage.setItem(storageKey, item);
}

function getFromSessionStorage(storageKey) {
    return sessionStorage.getItem(storageKey);
}

function removeFromSessionStorage(storageKey) {
    sessionStorage.removeItem(storageKey);
}

function addToSessionStorage(storageKey, item) {
    sessionStorage.setItem(storageKey, item);
}