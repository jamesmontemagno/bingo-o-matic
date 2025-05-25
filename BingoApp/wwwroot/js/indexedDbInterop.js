// IndexedDB interop functions for Blazor

/**
 * Initialize IndexedDB database
 * @param {string} dbName - Database name
 * @param {string} storeName - Object store name
 * @param {number} version - Database version
 */
export function initDb(dbName, storeName, version) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName, version);
        
        request.onerror = event => {
            console.error("IndexedDB error:", event.target.error);
            reject("Error opening IndexedDB");
        };
        
        request.onsuccess = event => {
            resolve();
        };
        
        request.onupgradeneeded = event => {
            const db = event.target.result;
            
            // Create object store if it doesn't exist
            if (!db.objectStoreNames.contains(storeName)) {
                db.createObjectStore(storeName, { keyPath: "id" });
            }
        };
    });
}

/**
 * Get an item from IndexedDB
 * @param {string} dbName - Database name
 * @param {string} storeName - Object store name
 * @param {string} key - Item key
 */
export function getItem(dbName, storeName, key) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName);
        
        request.onerror = event => {
            console.error("IndexedDB error:", event.target.error);
            reject("Error opening IndexedDB");
        };
        
        request.onsuccess = event => {
            const db = event.target.result;
            const transaction = db.transaction(storeName, "readonly");
            const store = transaction.objectStore(storeName);
            const getRequest = store.get(key);
            
            getRequest.onsuccess = event => {
                const result = event.target.result;
                if (result) {
                    resolve(result.value);
                } else {
                    resolve(null);
                }
            };
            
            getRequest.onerror = event => {
                console.error("Error reading data:", event.target.error);
                reject("Error reading data");
            };
            
            transaction.oncomplete = () => {
                db.close();
            };
        };
    });
}

/**
 * Set an item in IndexedDB
 * @param {string} dbName - Database name
 * @param {string} storeName - Object store name
 * @param {string} key - Item key
 * @param {string} value - Item value (serialized JSON)
 */
export function setItem(dbName, storeName, key, value) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName);
        
        request.onerror = event => {
            console.error("IndexedDB error:", event.target.error);
            reject("Error opening IndexedDB");
        };
        
        request.onsuccess = event => {
            const db = event.target.result;
            const transaction = db.transaction(storeName, "readwrite");
            const store = transaction.objectStore(storeName);
            
            const storeRequest = store.put({ id: key, value: value });
            
            storeRequest.onsuccess = () => {
                resolve();
            };
            
            storeRequest.onerror = event => {
                console.error("Error storing data:", event.target.error);
                reject("Error storing data");
            };
            
            transaction.oncomplete = () => {
                db.close();
            };
        };
    });
}

/**
 * Remove an item from IndexedDB
 * @param {string} dbName - Database name
 * @param {string} storeName - Object store name
 * @param {string} key - Item key
 */
export function removeItem(dbName, storeName, key) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName);
        
        request.onerror = event => {
            console.error("IndexedDB error:", event.target.error);
            reject("Error opening IndexedDB");
        };
        
        request.onsuccess = event => {
            const db = event.target.result;
            const transaction = db.transaction(storeName, "readwrite");
            const store = transaction.objectStore(storeName);
            
            const deleteRequest = store.delete(key);
            
            deleteRequest.onsuccess = () => {
                resolve();
            };
            
            deleteRequest.onerror = event => {
                console.error("Error removing data:", event.target.error);
                reject("Error removing data");
            };
            
            transaction.oncomplete = () => {
                db.close();
            };
        };
    });
}

/**
 * Clear all items from an IndexedDB store
 * @param {string} dbName - Database name
 * @param {string} storeName - Object store name
 */
export function clearStore(dbName, storeName) {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName);
        
        request.onerror = event => {
            console.error("IndexedDB error:", event.target.error);
            reject("Error opening IndexedDB");
        };
        
        request.onsuccess = event => {
            const db = event.target.result;
            const transaction = db.transaction(storeName, "readwrite");
            const store = transaction.objectStore(storeName);
            
            const clearRequest = store.clear();
            
            clearRequest.onsuccess = () => {
                resolve();
            };
            
            clearRequest.onerror = event => {
                console.error("Error clearing store:", event.target.error);
                reject("Error clearing store");
            };
            
            transaction.oncomplete = () => {
                db.close();
            };
        };
    });
}
