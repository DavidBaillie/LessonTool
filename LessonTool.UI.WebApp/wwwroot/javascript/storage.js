function getFromStorage(storageKey) {
    console.log("Reading key: " + storageKey)
    return localStorage.getItem(storageKey);
}

function removeFromStorage(storageKey) {
    console.log("Deleting key: " + storageKey)
    localStorage.removeItem(storageKey);
}

function addToStorage(storageKey, item) {
    console.log("Saving key: " + storageKey)
    localStorage.setItem(storageKey, item);
}