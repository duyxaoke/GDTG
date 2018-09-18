var lazyLoad = function ($timeout, $window) {
    return {
        restrict: 'A',
        scope: {
            fncallback: "&lazyLoad"
        },

        link: function (scope, element, attrs) {
            scope.IsLoaded = false;
            scope.raw = element[0];  
            angular.element($window).bind("scroll", function (e) {
                var IsVisible = $(scope.raw).is(':visible');
                if (!scope.IsLoaded && IsVisible) {
                    var PositionYofElement = $(scope.raw).position().top;
                    if (this.pageYOffset + this.innerHeight >= PositionYofElement) {
                        scope.fncallback();
                        scope.IsLoaded = true; 
                        scope.$apply();
                    }

                }
            });
        }
    };
};

lazyLoad.$inject = ["$timeout", "$window"];
 