var myState = {
    pdf: null,
    currentPage: 1,
    zoom: 1
}

const url = 'http://127.0.0.1:8081/Home/ExportToPDF'
fetch(url, {})
    .then((response) => response.json())
    .then(data => {
        var byteArr = base64EncodeUnicode(data);
        //pdfjsLib.getDocument(byteArr).then(pdf => {
        pdfjsLib.getDocument('/files/4PointBuildWithCorners.pdf').then(pdf => {
//            console.log("el documento contiene " + pdf._pdfInfo.numPages + "Pages")
            // more code here
            myState.pdf = pdf
            render()

        });
    })

pdfjsLib.disableWorker = true


var canvas

function render() {
    myState.pdf.getPage(myState.currentPage).then((page) => {

        // more code here
        pdfjsLib.disableWorker = true
         canvas = document.getElementById("pdf_renderer");
        var ctx = canvas.getContext('2d');

        var viewport = page.getViewport(myState.zoom);
        canvas.width = viewport.width;
        canvas.height = viewport.height;
        canvas.
        page.render({
            canvasContext: ctx,
            viewport: viewport
        });

    });
}

function base64EncodeUnicode(str) {
    // First we escape the string using encodeURIComponent to get the UTF-8 encoding of the characters, 
    // then we convert the percent encodings into raw bytes, and finally feed it to btoa() function.
    utf8Bytes = encodeURIComponent(str).replace(/%([0-9A-F]{2})/g, function (match, p1) {
        return String.fromCharCode('0x' + p1);
    });

    return btoa(utf8Bytes);
}


document.getElementById('go_previous')
    .addEventListener('click', (e) => {
        if (myState.pdf == null ||
            myState.currentPage == 1) return;
        myState.currentPage -= 1;
        document.getElementById("current_page")
            .value = myState.currentPage;
        render();
    });
document.getElementById('go_next')
    .addEventListener('click', (e) => {
        if (myState.pdf == null ||
            myState.currentPage > myState.pdf
                ._pdfInfo.numPages)
            return;

        myState.currentPage += 1;
        document.getElementById("current_page")
            .value = myState.currentPage;
        render();
    });
document.getElementById('current_page')
    .addEventListener('keypress', (e) => {
        if (myState.pdf == null) return;

        // Get key code
        var code = (e.keyCode ? e.keyCode : e.which);

        // If key code matches that of the Enter key
        if (code == 13) {
            var desiredPage =
                document.getElementById('current_page')
                    .valueAsNumber;

            if (desiredPage >= 1 &&
                desiredPage <= myState.pdf
                    ._pdfInfo.numPages) {
                myState.currentPage = desiredPage;
                document.getElementById("current_page")
                    .value = desiredPage;
                render();
            }
        }
    });
document.getElementById('zoom_in')
    .addEventListener('click', (e) => {
        if (myState.pdf == null) return;
        myState.zoom += 0.5;
        render();
    });
document.getElementById('zoom_out')
    .addEventListener('click', (e) => {
        if (myState.pdf == null) return;
        myState.zoom -= 0.5;
        render();
    });
window.onwheel = function (e) {
    e.preventDefault();

    if (e.ctrlKey) {

        scale -= e.deltaY + 0.01;
    } else {
        posX -= e.deltaX * 2;
        posY -= e.deltaY * 2;
    }

    render();
};