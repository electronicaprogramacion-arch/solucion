export function mountAndInitializeDb() {
    FS.mkdir('/database');
    FS.mount(IDBFS, {}, '/database');
    return syncDatabase(true);
};
export function getOnline(parameter) {

//    console.log('que lio');
    
};
export function syncDatabase(populate) {
    
    return new Promise((resolve, reject) => {
        FS.syncfs(populate, (err) => {
            if (err) {
//                console.log('syncfs failed. Error:', err);
                reject(err);
            }
            else {
//                console.log('synced successfull.');
                resolve();
            }
        });
    });
};

export function deleteDb() {
    try {
        FS.unlinkSync('/database/app.db');
//        console.log("Delete File successfully.");
    } catch (error) {
//        console.log('error delete----------------------------------------------------');
//        console.log(error);
    }
};