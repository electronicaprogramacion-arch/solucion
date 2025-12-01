export function mountAndInitializeDb() {

    try {
        window.Module.FS.mkdir('/database');
    } catch (error) {
        // Directory already exists or error creating it
    }


    window.Module.FS.mount(window.Module.FS.filesystems.IDBFS, {}, '/database');
    return syncDatabase(true);
};
export function getOnline(parameter) {
    // Function implementation
};
export function syncDatabase(populate) {

    return new Promise((resolve, reject) => {
        window.Module.FS.syncfs(populate, (err) => {
            if (err) {
                reject(err);
            }
            else {
                resolve();
            }
        });
    });
};

export function deleteDb() {
    try {
        FS.unmount('/database');

        // for modern browsers, this works:
        //const dbs = await window.indexedDB.databases()
        //dbs.forEach(db => { window.indexedDB.deleteDatabase(db.name) })
    } catch (error) {
        // Error deleting database
    }
};