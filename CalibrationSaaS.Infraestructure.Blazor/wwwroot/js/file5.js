export function mountAndInitializeDb() {

    try {
        window.Module.FS.mkdir('/database');
    } catch (error) {
//        console.log(error);
    }
    

    window.Module.FS.mount(window.Module.FS.filesystems.IDBFS, {}, '/database');
    return syncDatabase(true);
};
export function getOnline(parameter) {

//    console.log('que lio');
    
};
export function syncDatabase(populate) {
    
    return new Promise((resolve, reject) => {
        window.Module.FS.syncfs(populate, (err) => {
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
        FS.unmount('/database');
        
        // for modern browsers, this works:
        //const dbs = await window.indexedDB.databases()
        //dbs.forEach(db => { window.indexedDB.deleteDatabase(db.name) })
//        console.log("Delete File successfully.");
        //return syncDatabase(function (err) {
        //    console.log("otro error horrible....................................................");

        //});
    } catch (error) {
//        console.log('error delete----------------------------------------------------');
//        console.log(error);
    }
};