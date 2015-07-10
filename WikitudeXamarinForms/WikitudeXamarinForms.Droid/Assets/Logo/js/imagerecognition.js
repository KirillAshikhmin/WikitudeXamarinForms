var World = {
	init: function initFn() {
	    AR.context.services.sensors = false;
	    console.log("init recognize      ");
	    alert(init);
		
	},
	loadPoisFromJsonData: function initWRWithFile(file) {
	    alert(file);
	    this.createOverlays(file);
	},
	createOverlays: function createOverlaysFn(file) {
	    alert(file);
		this.tracker = new AR.Tracker("assets/logos.wtc", {
		//this.tracker = new AR.Tracker(file, {
			onLoaded: this.worldLoaded
		});
	    console.log("loadfile      " + file);
		var logos = ["698px-Samsung_Logo.svg_", "7651d831ed73796498b872af4a9f76e5.600x", "kfc-funny2_20140503191959", "Free-Google-Font-Logo-Catull-BQ-Download", "starbucks-logo"];
	    var trackables = [];
        for (var i = 0; i < logos.length; i++) {
            trackables[i] = new AR.Trackable2DObject(this.tracker, logos[i], {
                onEnterFieldOfVision: function () {
                    //alert(this.targetName);
                    var architectSdkUrl = "architectsdk://logofound?id=" + this.targetName;
                    document.location = architectSdkUrl;
                }
            });
        }
	},

	worldLoaded: function worldLoadedFn() {
		setTimeout(function() {
			var e = document.getElementById('loadingMessage');
			e.parentElement.removeChild(e);
			this.worldLoaded = true;
		}, 10000);
	}
};

World.init();