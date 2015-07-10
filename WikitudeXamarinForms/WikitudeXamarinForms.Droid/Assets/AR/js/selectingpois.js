// implementation of AR-Experience (aka "World")
var World = {
	// you may request new data from server periodically, however: in this sample data is only requested once
	isRequestingData: false,

	// true once data was fetched
	initiallyLoadedData: false,

	// different POI-Marker assets
	markerDrawable_idle: null,
	markerDrawable_selected: null,
	markerDrawable_directionIndicator: null,

	markerListUnclustered: [],
	// list of AR.GeoObjects that are currently shown in the scene / World
	markerList: [],
	lat: 0,
    lon: 0,
	// The last selected marker
	currentMarker: null,

	// called to inject new POI data
	loadPoisFromJsonData: function loadPoisFromJsonDataFn(poiData) {
		// empty list of visible markers
	    World.markerList = [];
	    //World.updateStatusMessage(parseFloat(poiData[0].latitude) + " arrived");
		// Start loading marker assets:
		// Create an AR.ImageResource for the marker idle-image
		World.markerDrawable_idle = new AR.ImageResource("assets/marker_idle.png");
		// Create an AR.ImageResource for the marker selected-image
		World.markerDrawable_selected = new AR.ImageResource("assets/marker_selected.png");
		// Create an AR.ImageResource referencing the image that should be displayed for a direction indicator. 
		World.markerDrawable_directionIndicator = new AR.ImageResource("assets/indi.png");
		World.markerDrawable_distance = new AR.ImageResource("assets/distance.png");
		World.markerDrawable_house = new AR.ImageResource("assets/house-icon.png");
		World.markerDrawable_museum = new AR.ImageResource("assets/museum-icon.png");

		// loop through POI-information and create an AR.GeoObject (=Marker) per POI
		for (var currentPlaceNr = 0; currentPlaceNr < poiData.length; currentPlaceNr++) {

			var singlePoi = {
				"id": poiData[currentPlaceNr].id,
				"latitude": parseFloat(poiData[currentPlaceNr].latitude),
				"longitude": parseFloat(poiData[currentPlaceNr].longitude),
				"altitude": parseFloat(poiData[currentPlaceNr].altitude),
				//"altitude": altitude,
				"title": poiData[currentPlaceNr].name,
				"description": poiData[currentPlaceNr].description,
				"distance": poiData[currentPlaceNr].distance,
				"type": poiData[currentPlaceNr].type
			};

			/*
				To be able to deselect a marker while the user taps on the empty screen, 
				the World object holds an array that contains each marker.
			*/
			World.markerList.push(new Marker(singlePoi));
			//World.markerListUnclustered.push(singlePoi);
		}
/*
		console.log(World.markerListUnclustered.length + " Unclustered places loaded");

		World.placeGeoObjects = ClusterHelper.createClusteredPlaces(20, World.lat, World.lon, World.markerListUnclustered);
		console.log(World.placeGeoObjects.length + " Clustered placeGeoObjects");

	    //go through all clusters
		for (var i = 0; i < World.placeGeoObjects.length; i++) {
		    //go through all items in each cluster
		    for (var j = 0; j < World.placeGeoObjects[i].places.length; j++) {

		        singlePoi = World.placeGeoObjects[i].places[j];
		        // the singlePoi altitude is originally 0 
		        // it will be increased by 250 for each item remaining in the cluster
		        singlePoi.altitude = j * 250;
		        // Add pois to World
		        World.markerList.push(new Marker(singlePoi));
		    }
		}*/

		console.log(World.markerList.length + " places loaded");
		World.updateStatusMessage(currentPlaceNr + ' places loaded');
	},

	// updates status message shon in small "i"-button aligned bottom center
	updateStatusMessage: function updateStatusMessageFn(message, isWarning) {
	    return;
		var themeToUse = isWarning ? "e" : "c";
		var iconToUse = isWarning ? "alert" : "info";

		$("#status-message").html(message);
		$("#popupInfoButton").buttonMarkup({
			theme: themeToUse
		});
		$("#popupInfoButton").buttonMarkup({
			icon: iconToUse
		});
	},

	// location updates, fired every time you call architectView.setLocation() in native environment
	locationChanged: function locationChangedFn(lat, lon, alt, acc) {

	    World.lat = lat;
	    World.lon = lon;
	    console.log("Current pos=" + lat + ", " + lon);
		/*
			The custom function World.onLocationChanged checks with the flag World.initiallyLoadedData if the function was already called. With the first call of World.onLocationChanged an object that contains geo information will be created which will be later used to create a marker using the World.loadPoisFromJsonData function.
		*/
		if (!World.initiallyLoadedData) {
			/* 
				requestDataFromLocal with the geo information as parameters (latitude, longitude) creates different poi data to a random location in the user's vicinity.
			*/
			//World.requestDataFromLocal(lat, lon);
			World.initiallyLoadedData = true;
		}
	},

	// fired when user pressed maker in cam
	onMarkerSelected: function onMarkerSelectedFn(marker) {
	    if (World.currentMarker) {
	        World.currentMarker.setDeselected(World.currentMarker);
	    }

	        marker.setSelected(marker);
	        World.currentMarker = marker;
	    
	        marker.markerObject.renderingOrder = 1000;
	},

	onMarkerDeselected: function onMarkerDeselectedFn(marker) {
	    // deselect previous marker
	    marker.markerObject.renderingOrder = 0;
	    if (World.currentMarker) {
	        if (World.currentMarker.poiData.id == marker.poiData.id) {
//	            marker.moveDown();
	            World.currentMarker = marker;
	            var locationUrl = 'architectsdk://markerselected?id=' + encodeURIComponent(marker.poiData.id) + '&type=' + encodeURIComponent(marker.poiData.type);
	            document.location.href = locationUrl;
	            marker.setDeselected(marker);
	            return;
	        }
	    }
	},
	// screen was clicked but no geo-object was hit
	onScreenClick: function onScreenClickFn() {
		if (World.currentMarker) {
			World.currentMarker.setDeselected(World.currentMarker);
		}
	},

	// request POI data
	requestDataFromLocal: function requestDataFromLocalFn(centerPointLatitude, centerPointLongitude) {
		var poisToCreate = 20;
		var poiData = [];

		for (var i = 0; i < poisToCreate; i++) {
			poiData.push({
				"id": (i + 1),
				"longitude": (centerPointLongitude + (Math.random() / 5 - 0.1)),
				"latitude": (centerPointLatitude + (Math.random() / 5 - 0.1)),
				"description": ("This is the description of POI#" + (i + 1)),
				// use this value to ignore altitude information in general - marker will always be on user-level
				"altitude": 0,
				"name": ("POI#" + (i + 1))
			});
		}
		World.loadPoisFromJsonData(poiData);
	}

};

/* 
	Set a custom function where location changes are forwarded to. There is also a possibility to set AR.context.onLocationChanged to null. In this case the function will not be called anymore and no further location updates will be received. 
*/
AR.context.onLocationChanged = World.locationChanged;

/*
	To detect clicks where no drawable was hit set a custom function on AR.context.onScreenClick where the currently selected marker is deselected.
*/
AR.context.onScreenClick = World.onScreenClick;