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
        this.get_data();
        window.alert("thats ok!");
    }

}

app.config();