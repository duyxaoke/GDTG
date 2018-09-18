var DataFactory = function ($rootScope, $localstorage, $timeout, UtilFactory, $q, $http, ApiHelper, CommonHelper) {
    var service = {};
    service.CheckCacheExist = (CacheKeyClient) => {
        let version = DataCacheKey[CacheKeyClient];
        if (!version) {
            return false;
        }
        let storerage = $localstorage.getObject(CacheKeyClient);
        if (!storerage) {
            return false;
        }
        if (storerage.version != version) {
            return false;
        }
        if (!storerage.data) {
            return false;
        }
        return true;
    };
    service.GetCache = (CacheKeyClient) => {
        let storerage = $localstorage.getObject(CacheKeyClient);
        return storerage.data;
    };
    service.AddCache = (CacheKeyClient, data) => {
        let version = DataCacheKey[CacheKeyClient];
        if (!version) {
            //case này do view output có DataCacheKey ko đồng nhất khai báo với CacheKeyClient
            //vo set lai view cho đúng, hoặc xóa cache render ra
            return;
        }
        let storerage = {};
        storerage.version = version;
        storerage.data = data;

        $localstorage.remove(CacheKeyClient);
        $localstorage.setObject(CacheKeyClient, storerage);
    };

    service.Users_Get = () => {
        let defer = $q.defer();
        let strApiEndPoint = ApiEndPoint.UserResource;
        ApiHelper.GetMethod(strApiEndPoint)
            .then(function (response) {
                response.Data = response.Data.filter(x => x.Username && x.FullName);//.splice(0, 20);
                defer.resolve(response);
            })
            .catch(function (response) {
                defer.reject(response);
            });
        return defer.promise;
    };
    //#endregion
    
    //#region Ward
    service.Stores_Get = function () {
        let defer = $q.defer();
        let strApiEndPoint = ApiEndPoint.StoreResource;
        ApiHelper.GetMethod(strApiEndPoint)
            .then(function (response) {
                response.Data.forEach((x) => {
                    x.id = x.StoreID;
                    x.text = x.StoreName;
                    x.parent = "#";
                });
                defer.resolve(response);
            })
            .catch(function (response) {
                defer.reject(response);
            });
        return defer.promise;
    };

    //#endregion

    //#region Menu
    service.Menus_Get = function () {
        let defer = $q.defer();
        let strApiEndPoint = CommonHelper.MenuUrl + "list";
        ApiHelper.GetMethod(strApiEndPoint)
            .then(function (response) {
                response.Data.forEach((x) => {
                    if (x.ParentId || x.ParentId > 0) {
                        x.ParentName = response.Data.filter(c => c.Id == x.ParentId)[0].Name;
                    };
                });
                defer.resolve(response);
            })
            .catch(function (response) {
                defer.reject(response);
            });
        return defer.promise;
    };
    //#endregion

    //#region Room
    service.Rooms_Get = function () {
        let strApiEndPoint = CommonHelper.ServiceUrl + "GetInfoRooms";
        return ApiHelper.GetMethod(strApiEndPoint);
    };
    //#endregion

    //#region GetInfoChartsByRoom
    service.GetInfoChartsByRooms_Get = function (roomId) {
        let strApiEndPoint = CommonHelper.ServiceUrl + "GetInfoChartsByRoom?roomId=" + roomId;
        return ApiHelper.GetMethod(strApiEndPoint);
    };
    //#endregion

    return service;
};
DataFactory.$inject = ["$rootScope", "$localstorage", "$timeout", "UtilFactory", "$q", "$http", "ApiHelper", "CommonHelper"];