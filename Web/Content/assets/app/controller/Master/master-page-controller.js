var MasterPageController = function ($scope, $rootScope, $timeout, $filter, $localstorage) {
    $rootScope.MasterPage = { IsLoading: false };
}
MasterPageController.$inject = ["$scope", "$rootScope", "$timeout", "$filter", "$localstorage"];