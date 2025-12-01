const cacheNamePrefix = 'offline-cache-';
const cacheName = 'nombre dinamico';
export async function store(url, method, body = "", responseString) {
    let blazorSchoolCache = await openCacheStorage();
    let request = createRequest(url, method, body);
    let response = new Response(responseString);
    await blazorSchoolCache.put(request, response);
}

export async function get(url, method, body = "") {
    let blazorSchoolCache = await openCacheStorage();
    let request = createRequest(url, method, body);
    let response = await blazorSchoolCache.match(request);

    if (response == undefined) {
        return "";
    }

    let result = await response.text();

    return result;
}

export async function remove(url, method, body = "") {
    let blazorSchoolCache = await openCacheStorage();
    let request = createRequest(url, method, body);
    await blazorSchoolCache.delete(request);
}

export async function removeAll() {
    let blazorSchoolCache = await openCacheStorage();
    let requests = await blazorSchoolCache.keys();

    for (let i = 0; i < requests.length; i++) {

        const url = new URL(requests[i].url);

        var url1 = requests[i].url;

        var strurl = url1.replace(url.origin, '');

        console.log(strurl);

        await blazorSchoolCache.delete(requests[i]);
    }
}

async function openCacheStorage() {
    return await window.caches.open(cacheName);
}

function createRequest(url, method, body = "") {
    let requestInit =
    {
        method: method
    };

    if (body != "") {
        requestInit.body = body;
    }

    let request = new Request(url, requestInit);

    return request;
}

export async function removeByUrl(urlsource) {

    console.log(cacheName);

    let blazorSchoolCache = await openCacheStorage();
    let requests = await blazorSchoolCache.keys();

    for (let i = 0; i < requests.length; i++) {

        const url = new URL(requests[i].url);

        var url1 = requests[i].url;

        var strurl = url1.replace(url.origin, '');

        console.log(strurl);


        if (strurl == urlsource) {
            await blazorSchoolCache.delete(requests[i]);
        }

    }
}