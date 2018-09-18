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