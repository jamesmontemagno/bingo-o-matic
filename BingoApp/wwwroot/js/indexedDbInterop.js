// IndexedDB interop functions for Blazor

let db = null;
let currentStoreName = null;

/**
 * Initialize IndexedDB database
 * @param {string} dbName - Database name
 * @param {string} storeName - Object store name
 * @param {number} version - Database version
 */
export function initDb(dbName, storeName, version) {
    return new Promise((resolve, reject) => {
        // First try to open without version to get current version
        const checkRequest = indexedDB.open(dbName);
        
        checkRequest.onsuccess = event => {
            const existingDb = event.target.result;
            const currentVersion = existingDb.version;
            existingDb.close();

            // If store doesn't exist, increment version
            if (!existingDb.objectStoreNames.contains(storeName)) {
                version = currentVersion + 1;
            } else {
                version = currentVersion;
            }

            // Now open with correct version
            const request = indexedDB.open(dbName, version);
            
            request.onerror = event => {
                console.error("IndexedDB error:", event.target.error);
                reject("Error opening IndexedDB");
            };
            
            request.onupgradeneeded = event => {
                console.log(`Upgrading database from version ${event.oldVersion} to ${event.newVersion}`);
                const db = event.target.result;
                
                // Create object store if it doesn't exist
                if (!db.objectStoreNames.contains(storeName)) {
                    console.log(`Creating object store: ${storeName}`);
                    db.createObjectStore(storeName, { keyPath: "id" });
                }
            };
            
            request.onsuccess = event => {
                db = event.target.result;
                currentStoreName = storeName;
                console.log(`Successfully opened database ${dbName} version ${db.version}`);
                resolve();
            };
        };

        checkRequest.onerror = event => {
            console.error("Error checking database version:", event.target.error);
            reject("Error checking database version");
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
        if (!db || currentStoreName !== storeName) {
            reject("Database not initialized or wrong store. Call initDb first.");
            return;
        }

        try {
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
        } catch (error) {
            console.error("Transaction error:", error);
            reject(error.toString());
        }
    });
}

/**
 * Get all items from IndexedDB
 * @param {string} dbName - Database name
 * @param {string} storeName - Object store name
 */
export function getAllItems(dbName, storeName) {
    return new Promise((resolve, reject) => {
        if (!db || currentStoreName !== storeName) {
            reject("Database not initialized or wrong store. Call initDb first.");
            return;
        }

        try {
            const transaction = db.transaction(storeName, "readonly");
            const store = transaction.objectStore(storeName);
            const getAllRequest = store.getAll();
            
            getAllRequest.onsuccess = event => {
                const results = event.target.result;
                const items = {};
                
                if (results) {
                    results.forEach(item => {
                        items[item.id] = item.value;
                    });
                    resolve(JSON.stringify(items));
                } else {
                    resolve(JSON.stringify({}));
                }
            };
            
            getAllRequest.onerror = event => {
                console.error("Error reading data:", event.target.error);
                reject("Error reading data");
            };
        } catch (error) {
            console.error("Transaction error:", error);
            reject(error.toString());
        }
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
        if (!db || currentStoreName !== storeName) {
            reject("Database not initialized or wrong store. Call initDb first.");
            return;
        }

        try {
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
        } catch (error) {
            console.error("Transaction error:", error);
            reject(error.toString());
        }
    });
}

/**
 * Set multiple items in IndexedDB
 * @param {string} dbName - Database name
 * @param {string} storeName - Object store name
 * @param {Object} items - Dictionary of key-value pairs to store
 */
export function setItems(dbName, storeName, items) {
    return new Promise((resolve, reject) => {
        if (!db || currentStoreName !== storeName) {
            reject("Database not initialized or wrong store. Call initDb first.");
            return;
        }

        try {
            const transaction = db.transaction(storeName, "readwrite");
            const store = transaction.objectStore(storeName);
            
            const promises = Object.entries(items).map(([key, value]) => {
                return new Promise((itemResolve, itemReject) => {
                    const storeRequest = store.put({ id: key, value: value });
                    storeRequest.onsuccess = () => itemResolve();
                    storeRequest.onerror = event => {
                        console.error("Error storing data:", event.target.error);
                        itemReject("Error storing data");
                    };
                });
            });
            
            Promise.all(promises)
                .then(() => resolve())
                .catch(error => reject(error));
        } catch (error) {
            console.error("Transaction error:", error);
            reject(error.toString());
        }
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
        if (!db || currentStoreName !== storeName) {
            reject("Database not initialized or wrong store. Call initDb first.");
            return;
        }

        try {
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
        } catch (error) {
            console.error("Transaction error:", error);
            reject(error.toString());
        }
    });
}

/**
 * Clear all items from an IndexedDB store
 * @param {string} dbName - Database name
 * @param {string} storeName - Object store name
 */
export function clearStore(dbName, storeName) {
    return new Promise((resolve, reject) => {
        if (!db || currentStoreName !== storeName) {
            reject("Database not initialized or wrong store. Call initDb first.");
            return;
        }

        try {
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
        } catch (error) {
            console.error("Transaction error:", error);
            reject(error.toString());
        }
    });
}
