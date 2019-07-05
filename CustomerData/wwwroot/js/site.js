app = {

    // inicialização e configuração
    config: function () {
        var body = document.getElementsByTagName("body")[0];
        body.addEventListener( "load", this.api_send(), false );
    },

    // pegando os dados
    get_data: function () {

        var pack = new Object();

        pack.params = window.location.search;
        pack.page_title = document.title;
        pack.browser = navigator.vendor + navigator.appCodeName + navigator.appName + navigator.userAgent;

        return json = JSON.stringify(pack);
    },

    // enviando para a api
    api_send: function () {
        var json = this.get_data();
        var ajax = new XMLHttpRequest();

        ajax.open("POST", "minha-url-api", true);
        ajax.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
        ajax.send(json);

        ajax.onreadystatechange = function () {
            if (ajax.readyState == 4 && ajax.status == 200) {
                console.log("Dados enviados. Status: " + ajax.status);
            }
        }

    }

}

app.config();