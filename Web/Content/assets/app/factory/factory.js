var $localstorage = function ($window) {
    return {
        set: function (key, value) {
            $window.localStorage[key] = value;
        },
        get: function (key, defaultValue) { return $window.localStorage[key] || defaultValue; },
        setObject: function (key, value) {
            $window.localStorage[key] = JSON.stringify(value);
        },
        getObject: function (key) {
            try {
                var temp = $window.localStorage[key];
                if (temp) {
                    return JSON.parse(temp || "{}");
                }
            } catch (e) {
                return JSON.parse("{}");
            }
        },
        remove: function (key) {
            $window.localStorage.removeItem(key);
        },
        clearAll: function () {
            $window.localStorage.clear();
        }
    };
};

$localstorage.$inject = ["$window"];

var ApiHelper = function ($rootScope, $localstorage, $timeout, $q, $http) {
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

    service.CodeStep = {
        Status: "",
        StatusCode: 0,
        ErrorStep: "",
        Message: "",
        ErrorMessage: "",
        Data: ""
    };

    service.JsonStatusCode = {
        Success: "Success",
        Error: "Error",
        Warning: "Warning",
        Info: "Info"
    };

    service.Status = {
        CreateSuccess: "Tạo thành công!",
        CreateFail: "Tạo thất bại!",
        UpdateSuccess: "Cập nhật thành công!",
        UpdateFail: "Cập nhật thất bại!",
        DeleteSuccess: "Xóa thành công!",
        DeleteFail: "Xóa thất bại!"
    };

    service.GetMethod = function (url, data) {
        let defer = $q.defer();
        let codeStep = jQuery.extend({}, ApiHelper.CodeStep);
        var req = {
            method: 'GET',
            url: url,
            data: data
        }
        $http(req).then(function (jqXHR) {
            if (jqXHR.status == 204) {
                codeStep = service.SetErrorAPI(jqXHR, url, data);
                defer.reject(codeStep);
            } else {
                codeStep.Status = service.JsonStatusCode.Success;
                codeStep.Data = jqXHR.data;
                defer.resolve(codeStep);
            }
        }, function (jqXHR) {
            codeStep = service.SetErrorAPI(jqXHR, url, data);
            defer.reject(codeStep);
        });
        return defer.promise;
    };

    service.PostMethod = function (url, data) {
        let codeStep = jQuery.extend({}, ApiHelper.CodeStep);
        let defer = $q.defer();

        var req = {
            method: 'POST',
            url: url,
            data: data
        }
        $http(req).then(function (jqXHR) {
            if (jqXHR.status == 204) {
                codeStep = service.SetErrorAPI(jqXHR, url, data);
                defer.reject(codeStep);
            } else {
                codeStep.Status = service.JsonStatusCode.Success;
                codeStep.Data = jqXHR.data;
                defer.resolve(codeStep);
            }
        }, function (jqXHR) {
            codeStep = service.SetErrorAPI(jqXHR, url, data);
            defer.reject(codeStep);
        });
        return defer.promise;
    };

    service.PutMethod = function (url, data) {
        let codeStep = jQuery.extend({}, ApiHelper.CodeStep);
        let defer = $q.defer();

        var req = {
            method: 'PUT',
            url: url,
            data: data
        }
        $http(req).then(function (jqXHR) {
            if (jqXHR.status == 204) {
                codeStep = service.SetErrorAPI(jqXHR, url, data);
                defer.reject(codeStep);
            } else {
                codeStep.Status = service.JsonStatusCode.Success;
                codeStep.Data = jqXHR.data;
                defer.resolve(codeStep);
            }
        }, function (jqXHR) {
            codeStep = service.SetErrorAPI(jqXHR, url, data);
            defer.reject(codeStep);
        });
        return defer.promise;
    };

    service.DeleteMethod = function (url, data) {
        let codeStep = jQuery.extend({}, ApiHelper.CodeStep);
        let defer = $q.defer();

        var req = {
            method: 'DELETE',
            url: url,
            data: data
        }
        $http(req).then(function (jqXHR) {
            if (jqXHR.status == 204) {
                codeStep = service.SetErrorAPI(jqXHR, url, data);
                defer.reject(codeStep);
            } else {
                codeStep.Status = service.JsonStatusCode.Success;
                codeStep.Data = jqXHR.data;
                defer.resolve(codeStep);
            }
        }, function (jqXHR) {
            codeStep = service.SetErrorAPI(jqXHR, url, data);
            defer.reject(codeStep);
        });
        return defer.promise;
    };

    service.SetErrorAPI = function (jqXHR, ApiEndPoint) {
        var codeStep = jQuery.extend({}, service.CodeStep);
        if (jqXHR.status == 200 || jqXHR.status == 201) return;
        codeStep.Status = service.JsonStatusCode.Error;
        codeStep.StatusCode = jqXHR.status;
        codeStep.ErrorStep = "API error " + jqXHR.status + ", ApiEndPoint:" + ApiEndPoint;
        switch (jqXHR.status) {
            case 406:
                var errorLst = jqXHR.data;
                codeStep.Status = service.JsonStatusCode.Warning;
                codeStep.Message = errorLst;
                if (jQuery.type(errorLst) == "array") {
                    codeStep.Message = errorLst.join("</br>");
                }
                break;
            case 500:
                //var errorLst = jqXHR.data;
                codeStep.ErrorMessage = jqXHR.data;
                codeStep.Message = service.StatusCodeMessage(jqXHR.status);
                break;
            case 204:
                codeStep.Message = "Không có dữ liệu hoặc bạn không có quyền xem dữ liệu";
                codeStep.Status = service.JsonStatusCode.Warning;
                break;
            default:
                codeStep.Message = service.StatusCodeMessage(jqXHR.status);
                break;
        }
        return codeStep;
    }

    service.StatusCodeMessage = function (status) {
        var strMessage = '';
        switch (status) {
            case 400:
                strMessage = 'Lỗi dữ liệu không hợp lệ';
                break;
            case 401:
                strMessage = 'Phiên làm việc đã hết hạn, vui lòng đăng nhập lại.';
                break;
            case 403:
                strMessage = 'Bạn không có quyền thực hiện thao tác này.';
                break;
            case 404:
                strMessage = 'URL action không chính xác';
                break;
            case 405:
                strMessage = 'Phương thức không được chấp nhận';
                break;
            case 429:
                strMessage = 'Thao tác quá nhanh';
                break;
            case 500:
                strMessage = 'Lỗi hệ thống';
                break;
            case 502:
                strMessage = 'Đường truyền kém';
                break;
            case 503:
                strMessage = 'Dịch vụ không hợp lệ';
                break;
            case 504:
                strMessage = 'Hết thời gian chờ';
                break;
            case 440:
                strMessage = 'Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại';
                break;
            default:
                strMessage = 'Lỗi chưa xác định';
                break;
        }
        return strMessage;
    };

    service.ConfirmRedirectLogin = function () {
        if ($rootScope.IsShowConfirmRedirectLogin) {
            return;
        }
        $rootScope.IsShowConfirmRedirectLogin = true;
        bootbox.alert({
            title: "Thông báo",
            message: "Phiên làm việc đã hết hạn, vui lòng đăng nhập lại…",
            callback: function (result) {
                $rootScope.IsShowConfirmRedirectLogin = false;
                window.location.href = "/Home/Logout";
            }
        })
    }

    service.NotPermission = function () {
        bootbox.alert({
            title: "Thông báo",
            message: "Bạn không có quyền thực hiện thao tác này…",
            callback: function () {
            }
        })
    }

    return service;
};
ApiHelper.$inject = ["$rootScope", "$localstorage", "$timeout", "$q", "$http"];
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
var UtilFactory = function ($rootScope, $timeout, $q) {
    var service = {};

    service.WaitingLoadDirective = function (arrar) {
        clearInterval(service.myTimer);
        let defer = $q.defer();
        service.myTimer = setInterval(() => {
            let objNotReady = _.find(arrar, (x) => x.IsReady == undefined || x.IsReady == false);
            if (!objNotReady) {
                clearInterval(service.myTimer);
                defer.resolve();
            }
        }, 100);
        return defer.promise;
    };

    service.InitArrayNoIndex = function (number) {
        var arr = new Array();
        for (var i = 1; i < number; i++) {
            arr.push(i);
        }
        return arr;
    };
    service.String = {};
    service.String.IsNullOrEmpty = function (str) {
        if (!str || str == null) {
            return true;
        }
        return false;
    };
    service.String.IsContain = function (strRoot, strRequest) {
        if (service.String.IsNullOrEmpty(strRoot)) {
            return false;
        }
        if (service.String.IsNullOrEmpty(strRequest)) {
            return true;
        }
        if (strRoot.indexOf(strRequest) < 0) {
            return false;
        }
        return true;
    };

    service.Alert = {};
    service.Alert.RequestError = function (e) {
        console.log(e);

        var strMessage = '';
        switch (e.status) {
            case 400:
                strMessage = 'Lỗi dữ liệu không hợp lệ';
                break;
            case 401:
                strMessage = 'Phiên làm việc đã hết hạn, vui lòng đăng nhập lại.';
                break;
            case 403:
                strMessage = 'Bạn không có quyền thực hiện thao tác này.';
                break;
            case 404:
                strMessage = 'URL action không chính xác';
                break;
            case 405:
                strMessage = 'Phương thức không được chấp nhận';
                break;
            case 500:
                strMessage = 'Lỗi hệ thống';
                break;
            case 502:
                strMessage = 'Đường truyền kém';
                break;
            case 503:
                strMessage = 'Dịch vụ không hợp lệ';
                break;
            case 504:
                strMessage = 'Hết thời gian chờ';
                break;
            case 440:
                strMessage = 'Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại';
                break;
            default:
                strMessage = 'Lỗi chưa xác định';
                break;
        }
        sys.Alert(false, strMessage);
    };
    return service;
};

UtilFactory.$inject = ["$rootScope", "$timeout", "$q"];