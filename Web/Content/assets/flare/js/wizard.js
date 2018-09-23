(function ($) {
    'use strict';
    var verticalForm = $("#frmTransaction");
    verticalForm.children("div").steps({
        headerTag: "h3",
        bodyTag: "section",
        transitionEffect: "slideLeft",
        stepsOrientation: "vertical",
        onFinished: function (event, currentIndex) {
            alert("Submitted!");
        }
    });
})(jQuery);