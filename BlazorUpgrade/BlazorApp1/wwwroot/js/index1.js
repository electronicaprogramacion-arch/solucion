
    if (window.location.hostname === 'bit-staging-calibration-cdn.azureedge.net') {
        (function (w, d, s) {
            var a = d.getElementsByTagName('head')[0];
            var r = d.createElement('script');
            r.async = 1;
            r.src = s;
            r.setAttribute('id', 'usetifulScript');
            r.dataset.token = "97b72d29ea44723f57a87f13480620bb";
            a.appendChild(r);
        })(window, document, "https://www.usetiful.com/dist/usetiful.js");
        }

function Nav_ScrollIntoView(navId) {

    //setTimeout(function () {

    var elem = document.getElementById(navId);
    if (elem === null) {
        return
    };


    var a = elem.scrollTop;
    var b = elem.scrollHeight - elem.clientHeight;
    var c = a / b;

    //alert(a);
    var scrollTop = window.pageYOffset || document.documentElement.scrollTop;

    //alert(scrollTop);
    //alert(getScroll());
    if (scrollTop > 400) {
        //elem.scrollIntoView({ behavior: 'smooth' });
        elem.scrollIntoView(false);
    }


    //}, 20);





}
    window.startupParams = function () {

            const url = new URL(
    window.location.href,
    );


    var str = url.toJSON();

    console.log(str);

    var onl = navigator.onLine.toString();

    return [str, onl, 'third'];
        };

        window.blazorCulture = {
            get: () => window.localStorage['BlazorCulture'],
            set: (value) => window.localStorage['BlazorCulture'] = value
        };
   
window.currentUser =
{
    get: () => window.localStorage['currentUser'],
    set: (value) => window.localStorage['currentUser'] = value
};

window.isOffline =
{
    get: () => window.localStorage['isOffline'],
    set: (value) => window.localStorage['isOffline'] = value
};


function HideLoading()
{

        try{
            document.getElementById('wrappercde').style.display = 'none';
        } catch (e)
        {
            console.error("Error hiding wrappercde:", e);
        }
   

}
 
        //Blazor.start({
        //    applicationCulture: 'en-US'
        //});


window.executeMethod = (id) => {

    setTimeout(function () {

        $('#' + id).click();

    }, 200);



    //document.getElementById(id).style.display = 'none';
};
function uuidv4() {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
        (+c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> +c / 4).toString(16)
    );
};

async function backupbase() {
    {
        const cache = await caches.open('SqliteWasmHelper');


        const resp = await cache.match('/data/cache/things.db');


        const blob = await resp.blob();

        const backupPath = 'backup_' + uuidv4();



        parent = document.getElementById('bklinkdownload');

        if (parent != undefined) {
            const a = document.createElement("a");
            a.href = URL.createObjectURL(blob);
            a.download = backupPath;
            a.target = "_self";
            a.innerText = `Download Database`;
            a.style = "color:white";
            parent.innerHTML = '';
            parent.appendChild(a);
            console.log('backupbase');
        }


    }
};




function fixedDecimal(navId, ndecimal) {
    $("#" + navId).change(function () {
        $(this).val(parseFloat($(this).val()).toFixed(ndecimal));
    });

}

        window.showProcessModal = (vbackdrop, vkeyboard, vstatic) => {
            //$('#pageProcessModal').modal('show');
            //$('#pageProcessModal').modal({ backdrop: vbackdrop, keyboard: vkeyboard, show: true });
            //$('#pageProcessModal').data('bs.modal').options.backdrop = vstatic;
            // console.log(vstatic);
            // console.log(vkeyboard);
            //$('#pageProcessModal').modal({ backdrop: vstatic.toString(), keyboard: vkeyboard, show: true });

            $('#pageProcessModal').modal('show', { backdrop: vstatic.toString(), keyboard: vkeyboard });
        };

        window.isinstalled = () => {
            try {
                var fg = false;
                //alert('isinstalled');
                // For iOS
                if (window.navigator.standalone) {
                    fg = true;
                }

                // For Android
                if (window.matchMedia('(display-mode: standalone)').matches) {
                    fg = true;
                }
                //alert(fg);
                // If neither is true, it's not installed
                return fg
            } catch (error) {
                console.log("error standalone");
                console.log(error);
                return false;
            }

        }

        function closeModal(id) {
            console.log('lego');
            $('#' + id).modal('hide');
        }

        $("#dvModalCreateCustomer").on('click', '#cmdNextProcess', function () {
            //$("#dvModalCreateCustomer").on('click', "a[ data-window='external']", function () {
            console.log('olw');
            $('#pageProcessModal').modal('toggle');
        });

        window.closeProcessModal = () => {
            $('#pageProcessModal').modal('hide');
        };


        window.closeModalWindow = (id) => {

            $('#' + id).modal('hide');
        };

        window.showModal = (id) => {

            $('#' + id).on('hide.bs.modal', function (e) {
                console.log('cerrando')
                e.preventDefault();
                e.stopPropagation();
                return false;
            });
            //$('#modalWindow').modal('show')
            setTimeout(function () {

                $('#' + id).on('hide.bs.modal', function (e) {
                    console.log('cerrando')
                    e.preventDefault();
                    e.stopPropagation();
                    return false;
                });


                $('#' + id).modal('show', { backdrop: 'static', keyboard: false })

            }, 100);
        };


        window.removeValidClass = () => {
            $(".form-control").removeClass("valid");
            console.log('hello');
        };

        window.removeClass = (id) => {

            //$('#modalWindow').modal('show')
            setTimeout(function () {

                $('#' + id).modal('hide');
                $('.' + id).remove()
                $('body').removeClass('modal-open')


            }, 200);
        };

        var cuz = '';
        window.hideElement = (id) => {

            //$('#modalWindow').modal('show')
            setTimeout(function () {

                $('#' + id).css('opacity', '0.7');
                cuz = $('#' + id).css('z-index');
                $('#' + id).css('z-index', '1');
                //$('#' + id).removeClass('show');
                //$('#' + id).css('display','none');


            }, 200);
        };


        var cuz = '';
        window.hideElement2 = (id) => {

            //$('#modalWindow').modal('show')
            setTimeout(function () {

                $('#' + id).css('opacity', '0.7');
                cuz = $('#' + id).css('z-index');
                $('#' + id).css('z-index', '1');
                $('#' + id).removeClass('show');
                $('#' + id).css('display', 'none');


            }, 200);
        };

        window.showElement = (id) => {

            //$('#modalWindow').modal('show')
            setTimeout(function () {



                //$('#' + id).addClass('show');
                //$('#' + id).css('display', 'block');
                $('#' + id).css('opacity', '1');
                //cuz = $('#' + id).css('z-index');
                $('#' + id).css('z-index', cuz);


            }, 200);
        };

       
        window.showElement2 = (id) => {

            
            //$('#modalWindow').modal('show')
            setTimeout(function ()
            {   //$('#' + id).addClass('show');
                $('#' + id).css('display', 'flex');
                $('#' + id).css('opacity', '1');
            }, 10);
        };

        window.showP = () =>
        {

            executeMethod("CloseButtonModal");
            showProgressBar("progressBar");
            showElement2("waitingring");
           
        };




        function isNumeric(n) {
            return !isNaN(parseFloat(n)) && isFinite(n);
        }


        window.getDimensions = function () {

            var scroll2 = 0;
            var scroll1 = window.pageYOffset || document.documentElement.scrollTop;


            if (isNumeric(scroll1)) {
                scroll2 = Math.round(scroll1);

            }


            return {
                width: window.innerWidth,
                height: window.innerHeight,
                online: navigator.onLine,
                scroll: scroll2,
                install: isinstalled()

            };
        };



        window.getOnline = function () {
            alert(navigator.onLine)
            return navigator.onLine


        };



        window.hideProgress = (id) => {
            try {

                setFocus();
                closeProgressBar(id);
            }
            catch (e) {

            }

            //$('#modalWindow').modal('show')
            setTimeout(function () {
                try {

                    $('#' + id).hide();

                    jsRemoveWindowLoad();


                }

                catch (e) {

                }
            }, 300);





        };

        function setFocus() {
            try {

                if (focused == undefined || focused == null || focused.length == 0) {
                    return;
                }

                $(findNextTabStop(focused, hor)).focus().select();
                setTimeout(function () {
                    if (nextelemnt == undefined || nextelemnt == null) {
                        return;
                    }
                    //$(findTabStop(nextelemnt)).focus().select();
                    /*$(findTabStop(nextelemnt)).focus().select();*/
                    console.log('tabb')
                    //alert(focused);
                    //focused = undefined;
                    //nextelemnt = undefined;
                }, 100)
            }
            catch (err) {
                log('setFocus')
                log(err.message)
            }
        }

        //window.showToast = (id) => {
            //    console.log(id.toString());
            //    $('#'+id.toString()).toast('show');
            //};

            //progressBar shared componente
            //var inheritwod = false;
            window.showProgressBar = (id, focus) => {

                setTimeout(function () {
                    try {

                        
                        $('#' + id).show();
                        $('#' + id + "ActionBar").hide();

                        
                        //jsShowWindowLoad('Loading', focus);
                    } catch (e) {

                    }
                }, 20);
            };
window.closeProgressBar = (id) => {
    setTimeout(function () {
    try {
        $('#' + id).hide();
        $('#' + id + "ActionBar").show();

    } catch(e) {

    }
    }, 20);
            
        };


        var Base64 = '';

        window.SetReport = (id) => {

            Base64 = id;

        };

        window.OpenWindow = (id) => {
            alert(id)
            window.open(id, "_blank")

        };
        var focused;
        var hor;
        window.SetControlFocus = (id, _hor) => {

            focused = id;
        hor = _hor;

        };

        var nextelemnt;
        function findTabStop(el) {
            var universe = document.querySelectorAll("input[id*='Texbox_']"); //document.querySelectorAll('input, button, select, textarea, a[href]');
        var list = Array.prototype.filter.call(universe, function (item) { return item.tabIndex >= "0" });
        var index = list.indexOf(el);
        nextelemnt = list[index + 1];
        return list[index];// || list[0];
        }
        function findNextTabStop(control, _hor) {
            var el = document.getElementById(control);

        var filter = 'Texbox_';

        if (_hor == true) {
            filter = control.split("_")[0];
            }
        //input[id*='Texbox_']
        //alert(filter);
        //alert(hor);
        var universe = document.querySelectorAll("input[id*=" + filter + "]");
        var list = Array.prototype.filter.call(universe, function (item) { return item.tabIndex >= "0" });
        var index = list.indexOf(el);

        return list[index + 1];// || list[0];
        }

        window.Collapse = (id) => {

            $('#' + id).removeClass('show')

        };



        window.BlazorControls = {
            assemblyname: 'Blazor.Controls',
            setFocus: (element) => {
                if (!element)
        return;

        element.focus();
            },
            preventEnter: (element, disabled) => {
                if (!element) {
            console.log("Error: preventEnter() element not found");
        return;
                }

        if (disabled)
        element.addEventListener('keydown', BlazorControls.preventEnterHandler);
        else
        element.removeEventListener('keydown', BlazorControls.preventEnterHandler);
            },
            preventEnterHandler: (event) => {
                const key = event.key;

        if (key === "Enter") {
            event.preventDefault();
                }
            }

        };


        window.topaccordion = function () {

            setTimeout(function () {
                try {
                    accordion();
                } catch (ex) {

                }

            }, 1000);

        }


        function accordion() {

            $(document).ready(function () {


                if ($('#accordion') == undefined || $('#accordion').length == 0) {

                    return;
                }

                var size = $('#accordion').height();
                console.log(size)

                //$('.blazored-modal-content').css('max-height', size * 3)
                //$('.blazored-modal-content').css('height', size * 3)

                $('.contenido').css('max-height', size * 1.5)

                $('.contenido').css('height', (size * 1.5) + 2)

                $('.contenido').css('overflow', 'scroll')

                $('.contenido').css('padding-top', '1px')



                //$('.contenido').animate({ height: size * 3 }, 100);

                $('.collapse').on('shown.bs.collapse', function (e) {
                    var $card = $(this).closest('.card');
                    console.log($card)
                    //$('.blazored-modal-content').animate({ height: size * 2 }, 100);
                    console.log($card.offset().top)
                    console.log($card.height())
                    var enca = $('#headermodal').height() + $('#toolbarmodal').height()
                    console.log(enca)
                    $('.contenido').animate({
                        scrollTop: $card.offset().top - (enca + 50 * 2)//180 cargeader
                    }, 500);
                });
            })
        }

  

        window.BackUp = async (id) => {

            await indexDBBackup();


        };

        async function indexDBBackup() {
            try {

                let db = new Dexie('CalibrationSaaSOfflineDB');
                if (!(await Dexie.exists(db.name))) {
                    console.log("Db does not exist");
                    alert("Db does not exist");
                    //db.version(1).stores({ });
                }
    await db.open();
    //let db = new Dexie('exportSample');
    const blob = await db.export({prettyJson: true, progressCallback });
    download(blob, "CalibrationSaaSOfflineDB" + getHourDate() + ".json", "application/json");
            } catch (error) {
        console.error('' + error);
            }

    db.close();
        }

    function getHourDate() {
            var currentdate = new Date();
    var datetime = currentdate.getDate() + "_"
    + (currentdate.getMonth() + 1) + "_"
    + currentdate.getFullYear() + "_"
    + currentdate.getHours() + "_"
    + currentdate.getMinutes() + "_"
    + currentdate.getSeconds();

    return datetime;
        }

    function progressCallback({totalRows, completedRows}) {
        console.log(`Progress: ${completedRows} of ${totalRows} rows completed`);
        }

    function deleteDatabase() {
            if (confirm('delete offline database')) {
                //window.indexedDB.deleteDatabase('CalibrationSaaSOfflineDB')
                var db = new Dexie('CalibrationSaaSOfflineDB');
    db.delete().then(function () {
        console.log('Database successfully deleted: ', name);
                }).catch(function (err) {
        console.error('Could not delete database: ', name, err);
                }).finally(function () {
        console.log('Done. Now executing callback if passed.');
    if (typeof cb === 'function') {
        cb();
                    }

                });
            }
        }
    function cb() {

    }


    $(".mat-input").focus(function () {
        $(this).parent().addClass("is-active is-completed");
        });

    $(".mat-input").focusout(function () {
            if ($(this).val() === "")
    $(this).parent().removeClass("is-completed");
    $(this).parent().removeClass("is-active");
        })

    window.jsToDotNetSamples = {
        dotNetReference: null,
    setDotNetReference: function (dotNetReference) {
        this.dotNetReference = dotNetReference;
            },
    printPersonFromDotNet: function () {
                var person = this.dotNetReference.invokeMethod("GetPerson");
    alert(JSON.stringify(person));
            }
        };


    function inicoordcontrol() {


            // Escuchar cambios en los campos y actualizar los valores decimales
            const gradosInputs = document.querySelectorAll('.grados');
    const minutosInputs = document.querySelectorAll('.minutos');
    const segundosInputs = document.querySelectorAll('.segundos');



            gradosInputs.forEach(input => input.addEventListener('input', actualizarValorDecimal));
            minutosInputs.forEach(input => input.addEventListener('input', actualizarValorDecimal));
            segundosInputs.forEach(input => input.addEventListener('input', actualizarValorDecimal));

            // Escuchar cambios en el campo valorDecimal
            //const valorDecimalInputs = document.querySelectorAll('.valorDecimal');
            //valorDecimalInputs.forEach(input => input.addEventListener('input', actualizarCoordenadas));


        }

    function actualizarValorDecimal2(event) {
            const indice = event.target.dataset.indice;
    const grados = parseInt(document.querySelector(`.grados[data-indice="${indice}"]`).value);
    const minutos = parseInt(document.querySelector(`.minutos[data-indice="${indice}"]`).value);
    const segundos = parseFloat(document.querySelector(`.segundos[data-indice="${indice}"]`).value);

    const valorDecimalInput = document.querySelector(`.valorDecimal[data-indice="${indice}"]`);

    // Validar grados
    if (grados < 0 || grados >= 360) {
        valorDecimalInput.value = '0';
    return;
            }

    // Validar minutos
    if (minutos < 0 || minutos >= 60) {
        valorDecimalInput.value = '0';
    return;
            }

    // Realizar la conversión a valor decimal
    const valorDecimal = grados + (minutos / 60) + (segundos / 3600);

    // Mostrar el valor decimal en el campo de entrada
    valorDecimalInput.value = valorDecimal;

    const buttonvalorDecimalInput = document.querySelector(`.buttonvalorDecimal[data-indice="${indice}"]`);

    buttonvalorDecimalInput.click();


        }



    function actualizarValorDecimal(event) {
            const indice = event.target.dataset.indice;

    //7727

    var gradosinput = document.querySelector(`.grados[data-indice="${indice}"]`);

    var minutosinput = document.querySelector(`.minutos[data-indice="${indice}"]`);

    var segundosinput = document.querySelector(`.segundos[data-indice="${indice}"]`);

    const grados = parseInt(gradosinput.value);
    const minutos = parseInt(minutosinput.value);
    const segundos = parseFloat(segundosinput.value);

    const valorDecimalInput = document.querySelector(`.valorDecimal[data-indice="${indice}"]`);

    const valordecimaltmp = parseFloat(valorDecimalInput.value);

    // Validar grados
    if (grados < 0 || grados >= 360) {
        valorDecimalInput.value = '0';
    return;
            }

    // Validar minutos
    if (minutos < 0 || minutos >= 60) {
        valorDecimalInput.value = '0';
    return;
            }




    // Realizar la conversión a valor decimal
    const valorDecimal = grados + (minutos / 60) + (segundos / 3600);



    // Mostrar el valor decimal en el campo de entrada
    valorDecimalInput.value = valorDecimal;

    const buttonvalorDecimalInput = document.querySelector(`.buttonvalorDecimal[data-indice="${indice}"]`);

    buttonvalorDecimalInput.click();
        }

        let observer = new MutationObserver(mutationRecords => {
            //console.log(mutationRecords); // console.log(los cambios)

            for (const record of mutationRecords) {
        //log.textContent = `Target of mutation: ${record.target.id}`;
        console.log('MutationObserver');
    console.log(record.target.id);
    actualizarCoordenadas2(record.target.id);

            }
        });

    // observa todo exceptuando atributos
    var config = {attributes: true, childList: true, characterData: true };

    function observefield(indice) {
            const valorDecimalInput = document.querySelector(`.valorDecimal[data-indice="${indice}"]`);

    if (valorDecimalInput == null) {
                return;
            }

    actualizarCoordenadas2(indice);

    observer.observe(valorDecimalInput, config);
        }

    function actualizarCoordenadas(indice) {

        console.log('actualizarCoordenadas ' + indice);
    //const indice = event.target.dataset.indice;
    //const valorDecimal = parseFloat(event.target.value);
    const valorDecimalInput = document.querySelector(`.valorDecimal[data-indice="${indice}"]`);

    const gradosInput = document.querySelector(`.grados[data-indice="${indice}"]`);
    const minutosInput = document.querySelector(`.minutos[data-indice="${indice}"]`);
    const segundosInput = document.querySelector(`.segundos[data-indice="${indice}"]`);

    if (valorDecimalInput == null || gradosInput == null || minutosInput == null || segundosInput == null) {
                return;
            }



            //$(valorDecimalInput).bind("propertychange change keyup paste input", function () {
        //    // do stuff;

        //    console.log('bindigconobjeto');

        //});
        //valorDecimalInput.addEventListener('input', actualizarCoord2);

        gradosInput.addEventListener('change', actualizarValorDecimal);

    minutosInput.addEventListener('change', actualizarValorDecimal);

    segundosInput.addEventListener('change', actualizarValorDecimal);



    if (valorDecimalInput.value == 0 || valorDecimalInput.value == '') {

        gradosInput.value = 0;
    minutosInput.value = 0;
    segundosInput.value = 0;
    return;
            }

    const valorDecimal = parseFloat(valorDecimalInput.value);
    console.log(valorDecimal);
    // Validar el valor decimal
    if (isNaN(valorDecimal)) {
        gradosInput.value = 0;
    minutosInput.value = 0;
    segundosInput.value = 0;
    return;
            }


    // Calcular grados, minutos y segundos
    const grados = Math.floor(valorDecimal);
    const minutosDecimal = (valorDecimal - grados) * 60;
    const minutos = Math.floor(minutosDecimal);
    const segundos = (minutosDecimal - minutos) * 60;

    // Mostrar las coordenadas en los campos de entrada
    gradosInput.value = grados;
    minutosInput.value = minutos;
    segundosInput.value = segundos.toFixed(4); // Limitar los segundos a 4 decimales
        }


    function actualizarCoordenadas2(indice) {

        console.log('actualizarCoordenadas ' + indice);
    //const indice = event.target.dataset.indice;
    //const valorDecimal = parseFloat(event.target.value);
    const valorDecimalInput = document.querySelector(`.valorDecimal[data-indice="${indice}"]`);

    const gradosInput = document.querySelector(`.grados[data-indice="${indice}"]`);
    const minutosInput = document.querySelector(`.minutos[data-indice="${indice}"]`);
    const segundosInput = document.querySelector(`.segundos[data-indice="${indice}"]`);

    if (valorDecimalInput == null || gradosInput == null || minutosInput == null || segundosInput == null) {
                return;
            }



            //$(valorDecimalInput).bind("propertychange change keyup paste input", function () {
            //    // do stuff;

            //    console.log('bindigconobjeto');

            //});
            //valorDecimalInput.addEventListener('input', actualizarCoord2);

            //gradosInput.addEventListener('change', actualizarValorDecimal);

            //minutosInput.addEventListener('change', actualizarValorDecimal);

            //segundosInput.addEventListener('change', actualizarValorDecimal);



            if (valorDecimalInput.value == 0 || valorDecimalInput.value == '') {

        gradosInput.value = 0;
    minutosInput.value = 0;
    segundosInput.value = 0;
    return;
            }

    const valorDecimal = parseFloat(valorDecimalInput.value);
    console.log(valorDecimal);
    // Validar el valor decimal
    if (isNaN(valorDecimal)) {
        gradosInput.value = 0;
    minutosInput.value = 0;
    segundosInput.value = 0;
    return;
            }


    // Calcular grados, minutos y segundos
    const grados = Math.floor(valorDecimal);
    const minutosDecimal = (valorDecimal - grados) * 60;
    const minutos = Math.floor(minutosDecimal);
    const segundos = (minutosDecimal - minutos) * 60;

    // Mostrar las coordenadas en los campos de entrada
    gradosInput.value = grados;
    minutosInput.value = minutos;
    segundosInput.value = segundos.toFixed(4); // Limitar los segundos a 4 decimales
        }




        window.getSelectedStart = (element) => {

            //var a=document.getElementById('textbox_id').value
            var indice = element.id;
            //const gradosInput = document.querySelector(`.grados[data-indice="${indice}"]`);
            //const minutosInput = document.querySelector(`.minutos[data-indice="${indice}"]`);
            //const segundosInput = document.querySelector(`.segundos[data-indice="${indice}"]`);
            //const valorDecimal = parseFloat(element.value);
            //console.log(valorDecimal);
            //// Validar el valor decimal
            //if (isNaN(valorDecimal)) {
        //    gradosInput.value = 0;
        //    minutosInput.value = 0;
        //    segundosInput.value = 0;
        //    return;
        //}


        //// Calcular grados, minutos y segundos
        //const grados = Math.floor(valorDecimal);
        //const minutosDecimal = (valorDecimal - grados) * 60;
        //const minutos = Math.floor(minutosDecimal);
        //const segundos = (minutosDecimal - minutos) * 60;

        //// Mostrar las coordenadas en los campos de entrada
        //gradosInput.value = grados;
        //minutosInput.value = minutos;
        //segundosInput.value = segundos.toFixed(4); // Limitar los segundos a 4 decimales



        console.log('getSelectedStart' + indice);
    console.log(element.value);
    return element.value;



        }




        //window.allowConsole = false;
    //if (process.env.NODE_ENV === "development")

    var enablelog = true;
    window.setLog = function () {
        //alert('xxxxxxxxx');
        /*enablelog = false;*/
        (function () {
            if (1 == 1) {
                var method;
                var noop = function noop() { };
                var methods = [
                    'assert', 'clear', 'count', 'debug', 'dir', 'dirxml', 'error',
                    'exception', 'group', 'groupCollapsed', 'groupEnd', 'info', 'log',
                    'markTimeline', 'profile', 'profileEnd', 'table', 'time', 'timeEnd',
                    'timeStamp', 'trace', 'warn'
                ];
                var length = methods.length;
                var console = (window.console = window.console || {});

                while (length--) {
                    method = methods[length];
                    console[method] = noop;
                }
            }

        }());
        };

        //if (1 == 1) {
        //    console.log = function () { };
        //}






   
    