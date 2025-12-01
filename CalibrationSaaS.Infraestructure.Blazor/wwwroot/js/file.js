export function mountAndInitializeDb() {
    FS.mkdir('/database');
    FS.mount(IDBFS, {}, '/database');
    return syncDatabase(true);
};
export function getOnline(parameter) {
    // Function implementation
};
export function syncDatabase(populate) {

    return new Promise((resolve, reject) => {
        FS.syncfs(populate, (err) => {
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
    FS.unlink('/database/app.db');
};

