/*
 * Dependencies:
 * - jQuery
 */

app = {

    // configs and initializing
    config: function () {
        var body = document.getElementsByTagName("body")[0];
        body.addEventListener( "load", this.api_send(), false );
    },

    // getting browser date
    get_data: function () {

        var pack = new Object();

        pack.Params = window.location.search;
        pack.PageTitle = document.title;
        pack.Browser = navigator.vendor + navigator.appCodeName + navigator.appName + navigator.userAgent;

        return pack;
    },

    // sending to api by AJAX
    api_send: function () {
        var pack = this.get_data();
        var json_pack = JSON.stringify(pack);

        /* bloqued by CORS
         * */
        var ajax = new XMLHttpRequest();

        ajax.open("POST", "http://localhost:54468/api/navigation", true);
        ajax.setRequestHeader("Content-type", "application/json; charset=utf-8");
        ajax.setRequestHeader("Access-Control-Allow-Origin", "*");
        ajax.withCredentials = true;

        ajax.send(json_pack);

        console.log("sent...");

        ajax.onreadystatechange = function () {
            if (ajax.readyState == 4 && ajax.status == 200) {
                console.log("Data Sent... Status: " + ajax.status);
                console.log("Json Pack: " + json);
            }
        }
        

        /*
         * $.ajax({
                type: 'POST',
                url: '/Person/Index',
                dataType: 'json',
                contentType: dataType,
                data: data,
                success: function(result) {
                    console.log('Data received: ');
                    console.log(result);
                }
            });
         */

        /*
        $.ajax({
            type: "POST",

            // CORS
            dataType: "jsonp",
            crossDomain: true,

            // log to console
            beforeSend: function (request) {
                request.setRequestHeader("Access-Control-Allow-Origin", "*");
                console.log("Sending... Json Pack: " + json_pack);
            },

            url: "http://localhost:54468/api/values",
            data: json_pack,

        });
        */

    }

}

app.config();