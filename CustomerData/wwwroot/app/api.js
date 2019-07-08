// api helper
var api = {

    // calling apis
    call : function(pack, api_url, method) {
        // pack in json
        var json_pack = JSON.stringify(pack);
        
        // ajax setup
        var ajax = new XMLHttpRequest();
        ajax.open(method /*"POST"*/, api_url/*"http://localhost:54468/api/navigation"*/, true);
        ajax.setRequestHeader("Content-Type", "application/json; charset=utf-8");

        // send call
        ajax.send(json_pack);

        console.log("Call api_url...");

        ajax.onreadystatechange = function () {
            if (ajax.readyState == 4 && ajax.status == 200) {
                console.log("Data Sent... Status: " + ajax.status);
                console.log("Json Pack: " + ajax.responseText);
            }
        }        
    }
}
