var app = {

    // configs and initializing
    config: function () {
        var body = document.getElementsByTagName("body")[0];
        body.addEventListener( "load", this.collect_data(), false );
    },

    // getting data
    get_data_pack: function () {

        var pack = new Object();
        pack.Params = window.location.search;
        pack.PageTitle = document.title;
        pack.Browser = navigator.vendor + navigator.appCodeName + navigator.appName + navigator.userAgent;
        return pack;
    },

    // sending to api by AJAX
    collect_data: function () {
        var pack = this.get_data_pack();
        api.call(
        	pack, 
        	app_config.urls.api_customer_data_recept,
			"POST"        	
    	);
    }

}

// setup js app
app.config();