var CommonHelper = function ($rootScope, $localstorage, $timeout, $q, $http) {
    let urlApi = "http://localhost:51582/api/";
    let service = {};

    service.ConfigUrl = urlApi + "Configs/";
    service.MenuUrl = urlApi + "Menus/";
    service.RoleUrl = urlApi + "Roles/";
    service.UserUrl = urlApi + "Users/";

    return service;
}
CommonHelper.$inject = ["$rootScope", "$localstorage", "$timeout", "$q", "$http"];