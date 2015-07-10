var World = {
    loaded: false,

    init: function initFn() {
        alert("START!");
        AR.context.services.sensors = false;
        // console.log("init recognize      ");
        this.createOverlays();
    },
    createOverlays: function createOverlaysFn() {
        this.tracker = new AR.Tracker("assets/logos.wtc", {
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
                    alert(this.targetName);
                    document.location = architectSdkUrl;
                }
            });
        }
    },

    worldLoaded: function worldLoadedFn() {
        alert("WTF?!");
        setTimeout(function () {
            var e = document.getElementById('loadingMessage');
            e.parentElement.removeChild(e);
            this.worldLoaded = true;
        }, 10000);
    }
};

World.init();