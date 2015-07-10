var kMarker_AnimationDuration_ChangeDrawable = 500;
var kMarker_AnimationDuration_Resize = 1000;

var currentMarkerOrder = 0;
function Marker(poiData) {

    this.poiData = poiData;
    this.isSelected = false;

    /*
        With AR.PropertyAnimations you are able to animate almost any property of ARchitect objects. This sample will animate the opacity of both background drawables so that one will fade out while the other one fades in. The scaling is animated too. The marker size changes over time so the labels need to be animated too in order to keep them relative to the background drawable. AR.AnimationGroups are used to synchronize all animations in parallel or sequentially.
    */

    this.animationGroup_idle = null;
    this.animationGroup_selected = null;


    // create the AR.GeoLocation from the poi data
    var markerLocation = new AR.GeoLocation(poiData.latitude, poiData.longitude, poiData.altitude);

    // create an AR.ImageDrawable for the marker in idle state
    this.markerDrawable_idle = new AR.ImageDrawable(World.markerDrawable_idle, 4.5, {
        zOrder: 0,
        opacity: 1.0,
        /*
            To react on user interaction, an onClick property can be set for each AR.Drawable. The property is a function which will be called each time the user taps on the drawable. The function called on each tap is returned from the following helper function defined in marker.js. The function returns a function which checks the selected state with the help of the variable isSelected and executes the appropriate function. The clicked marker is passed as an argument.
        */
        onClick: Marker.prototype.getOnClickTrigger(this)
    });

    // create an AR.ImageDrawable for the marker in selected state
    this.markerDrawable_selected = new AR.ImageDrawable(World.markerDrawable_selected, 4.5, {
        zOrder: 0,
        opacity: 0.0,
        onClick: null
    });

    // create an AR.ImageDrawable for the marker in selected state
    this.markerDrawable_distance = new AR.ImageDrawable(World.markerDrawable_distance, 0.3, {
        zOrder: 1,
        opacity: 1.0,
        offsetY: 0.9,
        offsetX: -0.6,
        onClick: null
    });

    // create an AR.ImageDrawable for the marker in selected state
    this.markerDrawable_icon = new AR.ImageDrawable(poiData.type==="House"?World.markerDrawable_house:World.markerDrawable_museum, 0.6, {
        zOrder: 1,
        opacity: 1.0,
        offsetY: 1.6,
        onClick: null
    });

    // create an AR.Label for the marker's title 
    this.title1Label = new AR.Label(poiData.title.trunc2(16), 0.6, {
        zOrder: 1,
        offsetY: 0.2,
        style: {
            textColor: '#333333'
        }
    });

    this.title2Label = new AR.Label(poiData.title.trunc3(16), 0.6, {
        zOrder: 1,
        offsetY: -0.8,
        style: {
            textColor: '#333333'
        }
    });

    // create an AR.Label for the marker's description
    this.distanceLabel = new AR.Label(poiData.distance, 0.6, {
        zOrder: 1,
        offsetY: 0.9,
        offsetX: 0.8,
        style: {
            textColor: '#ff6a5c',
            fontStyle: AR.CONST.FONT_STYLE.BOLD
        }
    });

    /*
        Create an AR.ImageDrawable using the AR.ImageResource for the direction indicator which was created in the World. Set options regarding the offset and anchor of the image so that it will be displayed correctly on the edge of the screen.
    */
    this.directionIndicatorDrawable = new AR.ImageDrawable(World.markerDrawable_directionIndicator, 0.1, {
        enabled: false,
        verticalAnchor: AR.CONST.VERTICAL_ANCHOR.TOP
    });

    /*
        Create the AR.GeoObject with the drawable objects and define the AR.ImageDrawable as an indicator target on the marker AR.GeoObject. The direction indicator is displayed automatically when necessary. AR.Drawable subclasses (e.g. AR.Circle) can be used as direction indicators.
    */
    this.markerObject = new AR.GeoObject(markerLocation, {
        drawables: {
            cam: [this.markerDrawable_idle, this.markerDrawable_distance, this.markerDrawable_icon, this.markerDrawable_selected,
                this.title1Label, this.title2Label, this.distanceLabel],
            indicator: this.directionIndicatorDrawable
        }
    });

    return this;
}

Marker.prototype.getOnClickTrigger = function(marker) {

    /*
        The setSelected and setDeselected functions are prototype Marker functions. 
        Both functions perform the same steps but inverted.
    */

    return function() {

        if (!Marker.prototype.isAnyAnimationRunning(marker)) {
            if (marker.isSelected) {
                Marker.prototype.setDeselected(marker);
                World.onMarkerDeselected(marker);
            } else {
                Marker.prototype.setSelected(marker);
                try {
                    World.onMarkerSelected(marker);
                } catch (err) {
                    alert(err);
                }

            }
        } else {
            AR.logger.debug('a animation is already running');
        }


        return true;
    };
};

//Marker.prototype.moveUp = function (marker) {
//    marker.markerObject.location.altitude = 1000;
//}
//
//Marker.prototype.moveDown = function (marker) {
//    marker.markerObject.location.altitude = 100;
//}

/*
    Property Animations allow constant changes to a numeric value/property of an object, dependent on start-value, end-value and the duration of the animation. Animations can be seen as functions defining the progress of the change on the value. The Animation can be parametrized via easing curves.
*/
Marker.prototype.setSelected = function(marker) {

    marker.isSelected = true;
   
   // marker.zIndex = 100;
 
  //  marker.poiData.altitude = 400;
    /*
    for (i = 0; i < World.markerList.length; i++) {
        var newLocation2 = World.markerList[i].markerLocation;
        var distanceUpdate2 = (newLocation2.distanceToUser() > 999) ? ((newLocation2.distanceToUser() / 1000).toFixed(2) + " km") : (Math.round(newLocation2.distanceToUser()) + " m");
       // World.markerList[i].poiData.distance = distanceUpdate2;
        World.markerList[i].distanceLabel.text = distanceUpdate2;
    }
   */
   // console.log("prototype select current marker 1 " + marker.markerObject.drawables.cam[0].zOrder);
    //marker.zOrder = 10000;
    // New: 
    if (marker.animationGroup_selected === null) {

        // create AR.PropertyAnimation that animates the opacity to 0.0 in order to hide the idle-state-drawable
        var hideIdleDrawableAnimation = new AR.PropertyAnimation(marker.markerDrawable_idle, "opacity", null, 0.0, kMarker_AnimationDuration_ChangeDrawable);
        // create AR.PropertyAnimation that animates the opacity to 1.0 in order to show the selected-state-drawable
        var showSelectedDrawableAnimation = new AR.PropertyAnimation(marker.markerDrawable_selected, "opacity", null, 1.0, kMarker_AnimationDuration_ChangeDrawable);

        // create AR.PropertyAnimation that animates the scaling of the idle-state-drawable to 1.2
        var idleDrawableResizeAnimation = new AR.PropertyAnimation(marker.markerDrawable_idle, 'scaling', null, 1.2, kMarker_AnimationDuration_Resize, new AR.EasingCurve(AR.CONST.EASING_CURVE_TYPE.EASE_OUT_ELASTIC, {
            amplitude: 2.0
        }));
        // create AR.PropertyAnimation that animates the scaling of the selected-state-drawable to 1.2
        var selectedDrawableResizeAnimation = new AR.PropertyAnimation(marker.markerDrawable_selected, 'scaling', null, 1.2, kMarker_AnimationDuration_Resize, new AR.EasingCurve(AR.CONST.EASING_CURVE_TYPE.EASE_OUT_ELASTIC, {
            amplitude: 2.0
        }));
        // create AR.PropertyAnimation that animates the scaling of the title label to 1.2
//        var titleLabelResizeAnimation = new AR.PropertyAnimation(marker.titleLabel, 'scaling', null, 1.2, kMarker_AnimationDuration_Resize, new AR.EasingCurve(AR.CONST.EASING_CURVE_TYPE.EASE_OUT_ELASTIC, {
//            amplitude: 2.0
//        }));
//        // create AR.PropertyAnimation that animates the scaling of the description label to 1.2
//        var descriptionLabelResizeAnimation = new AR.PropertyAnimation(marker.descriptionLabel, 'scaling', null, 1.2, kMarker_AnimationDuration_Resize, new AR.EasingCurve(AR.CONST.EASING_CURVE_TYPE.EASE_OUT_ELASTIC, {
//            amplitude: 2.0
//        }));

        /*
            There are two types of AR.AnimationGroups. Parallel animations are running at the same time, sequentials are played one after another. This example uses a parallel AR.AnimationGroup.
        */
        marker.animationGroup_selected = new AR.AnimationGroup(AR.CONST.ANIMATION_GROUP_TYPE.PARALLEL, [hideIdleDrawableAnimation, showSelectedDrawableAnimation, idleDrawableResizeAnimation, selectedDrawableResizeAnimation]);
    }

    // removes function that is set on the onClick trigger of the idle-state marker
    marker.markerDrawable_idle.onClick = null;
//    // sets the click trigger function for the selected state marker
    marker.markerDrawable_selected.onClick = Marker.prototype.getOnClickTrigger(marker);
//
//    // enables the direction indicator drawable for the current marker
    marker.directionIndicatorDrawable.enabled = false;
//    // starts the selected-state animation
    marker.animationGroup_selected.start();
};

Marker.prototype.setDeselected = function(marker) {
    marker.markerObject.renderingOrder = 0;
    marker.isSelected = false;
    //alert("prototype setDeselected current marker " + marker.zOrder + " ? " + marker.zIndex);
   // marker.zIndex = 0;
   // marker.zOrder = 10000;
    // New: 

    if (marker.animationGroup_idle === null) {

        // create AR.PropertyAnimation that animates the opacity to 1.0 in order to show the idle-state-drawable
        var showIdleDrawableAnimation = new AR.PropertyAnimation(marker.markerDrawable_idle, "opacity", null, 1.0, kMarker_AnimationDuration_ChangeDrawable);
        // create AR.PropertyAnimation that animates the opacity to 0.0 in order to hide the selected-state-drawable
        var hideSelectedDrawableAnimation = new AR.PropertyAnimation(marker.markerDrawable_selected, "opacity", null, 0, kMarker_AnimationDuration_ChangeDrawable);
        // create AR.PropertyAnimation that animates the scaling of the idle-state-drawable to 1.0
        var idleDrawableResizeAnimation = new AR.PropertyAnimation(marker.markerDrawable_idle, 'scaling', null, 1.0, kMarker_AnimationDuration_Resize, new AR.EasingCurve(AR.CONST.EASING_CURVE_TYPE.EASE_OUT_ELASTIC, {
            amplitude: 2.0
        }));
        // create AR.PropertyAnimation that animates the scaling of the selected-state-drawable to 1.0
        var selectedDrawableResizeAnimation = new AR.PropertyAnimation(marker.markerDrawable_selected, 'scaling', null, 1.0, kMarker_AnimationDuration_Resize, new AR.EasingCurve(AR.CONST.EASING_CURVE_TYPE.EASE_OUT_ELASTIC, {
            amplitude: 2.0
        }));
        // create AR.PropertyAnimation that animates the scaling of the title label to 1.0
//        var titleLabelResizeAnimation = new AR.PropertyAnimation(marker.titleLabel, 'scaling', null, 1.0, kMarker_AnimationDuration_Resize, new AR.EasingCurve(AR.CONST.EASING_CURVE_TYPE.EASE_OUT_ELASTIC, {
//            amplitude: 2.0
//        }));
        // create AR.PropertyAnimation that animates the scaling of the description label to 1.0
//        var descriptionLabelResizeAnimation = new AR.PropertyAnimation(marker.descriptionLabel, 'scaling', null, 1.0, kMarker_AnimationDuration_Resize, new AR.EasingCurve(AR.CONST.EASING_CURVE_TYPE.EASE_OUT_ELASTIC, {
//            amplitude: 2.0
//        }));

        /*
            There are two types of AR.AnimationGroups. Parallel animations are running at the same time, sequentials are played one after another. This example uses a parallel AR.AnimationGroup.
        */
        marker.animationGroup_idle = new AR.AnimationGroup(AR.CONST.ANIMATION_GROUP_TYPE.PARALLEL, [showIdleDrawableAnimation, hideSelectedDrawableAnimation, idleDrawableResizeAnimation, selectedDrawableResizeAnimation]);
    }

    // sets the click trigger function for the idle state marker
    marker.markerDrawable_idle.onClick = Marker.prototype.getOnClickTrigger(marker);
//    // removes function that is set on the onClick trigger of the selected-state marker
    marker.markerDrawable_selected.onClick = null;
//
//    // disables the direction indicator drawable for the current marker
    marker.directionIndicatorDrawable.enabled = false;
//    // starts the idle-state animation
    marker.animationGroup_idle.start();
};

Marker.prototype.isAnyAnimationRunning = function(marker) {

    if (marker.animationGroup_idle === null || marker.animationGroup_selected === null) {
        return false;
    } else {
        if ((marker.animationGroup_idle.isRunning() === true) || (marker.animationGroup_selected.isRunning() === true)) {
            return true;
        } else {
            return false;
        }
    }
};

// will truncate all strings longer than given max-length "n". e.g. "foobar".trunc(3) -> "foo..."
String.prototype.trunc = function (n) {
    return this.substr(0, n - 1) + (this.length > n ? '...' : '');
};
String.prototype.trunc2 = function (n) {
    var i = this.indexOf(" ");
    if (i === -1) return this;
    var res = "";
    while (i < n) {
        res = this.substr(0, i);
        i = this.indexOf(" ", i + 1);
        if (i === -1) return res;
    }
    return res;
};
String.prototype.trunc3 = function (n) {
    var l = this.trunc2(n).length;
    if (l === this.length)
        return "";
    return this.substr(l + 1, this.length - 1 - l).trunc(n-2);
};