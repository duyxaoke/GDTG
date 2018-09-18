var IndexController = ($scope, $rootScope, $timeout, $filter, $compile, ApiHelper, CommonHelper, $q) => {

    //#region declare variable
    $scope.UpdateMode = false;
    $scope.Config = {};
    //#endregion

    //#region Datatable
    $(function () {
        var peopleList = $('#myTable').DataTable({
            "bProcessing": true,
            "bSort": true,
            "bServerSide": true,
            "language": {
                paginate: {
                    previous: '<i class="fa fa-lg fa-angle-left"></i>',
                    next: '<i class="fa fa-lg fa-angle-right"></i>'
                }
            },
            "sAjaxSource": "/Configs/Data",
            "aoColumnDefs": [
                {
                    "sName": "SystemEnable",
                    "sTitle": "System enable",
                    "className": "text-center",
                    "bSearchable": true,
                    "bSortable": true,
                    "aTargets": [0],
                    'mRender': function (data, type, row, meta) {
                        return data == 'True' ? '<label class="badge badge-success">Hoạt động</label>' : '<label class="badge badge-danger">Ngưng hoạt động</label>';
                    }
                },
                {
                    "sName": "Currency",
                    "sTitle": "Tiền mặc định",
                    "className": "text-center",
                    "bSearchable": true,
                    "bSortable": true,
                    "aTargets": [1]
                },
                {
                    "sName": "ReferalBonus",
                    "sTitle": "Tỷ lệ hoa hồng",
                    "className": "text-center",
                    "bSearchable": true,
                    "bSortable": true,
                    "aTargets": [2]
                },
                {
                    'mRender': function (data, type, row, meta) {
                        return `
                                    <button class="btn btn-outline-success btn-sm" ng-click="Update('` + data + `');" data-toggle="tooltip" data-placement="top" title="Cập nhật"><i class="fa fa-edit"></i></button>
    <button class="btn btn-outline-danger btn-sm" ng-click="Delete('` + data + `');" data-toggle="tooltip" data-placement="top" title="Xóa"><i class="fa fa-times"></i></button>`;
                    },
                    "sName": "Id",
                    "sTitle": "Thao tác",
                    "className": "text-center",
                    "bSortable": false,
                    "aTargets": [3]
                }
            ],
            createdRow: function (row, data, dataIndex) {
                $compile(angular.element(row).contents())($scope);
            }
        });
    });
    //#endregion

    //#region ReadById
    var ReadById = (Id) => {
        let defer = $q.defer();
        $rootScope.MasterPage.IsLoading = true;
        let strApiEndPoint = CommonHelper.ConfigUrl + Id;
        ApiHelper.GetMethod(strApiEndPoint)
            .then(function (response) {
                $rootScope.MasterPage.IsLoading = false;
                $scope.Config = response.Data;
                defer.resolve(response);
            })
            .catch(function (response) {
                sys.Alert(false, response.Message);
                defer.reject(response);
                $rootScope.MasterPage.IsLoading = false;
            });
        return defer.promise;
    };
    //#endregion

    //#region Create
    $scope.Create = function () {
        $scope.UpdateMode = false;
        $scope.Config = {};
        $('#PnModal').modal('show');
    };
    //#endregion

    //#region Update
    $scope.Update = function (Id) {
        $scope.UpdateMode = true;
        ReadById(Id);
        $('#PnModal').modal('show');
    }
    //#endregion

    //#region Delete
    $scope.Delete = function (Id) {
        swal({
            title: "Xác nhận xóa?",
            text: "Một khi đã xóa, bạn không thể khôi phục lại",
            icon: "warning",
            buttons: true,
            dangerMode: true,
        })
            .then((willDelete) => {
                if (willDelete) {
                    $rootScope.MasterPage.IsLoading = true;
                    let strApiEndPoint = CommonHelper.ConfigUrl + "?id=" + Id;
                    ApiHelper.DeleteMethod(strApiEndPoint)
                        .then(function (response) {
                            $rootScope.MasterPage.IsLoading = false;
                            $('#myTable').DataTable().ajax.reload(null, false);
                            sys.Alert(true, 'Xóa thành công!');
                        })
                        .catch(function (response) {
                            sys.Alert(false, response.Message);
                            defer.reject(response);
                            $rootScope.MasterPage.IsLoading = false;
                        });
                }
            });
    };
    //#endregion

    //#region Save
    $scope.Save = function () {
        if (!$scope.UpdateMode) {
            $rootScope.MasterPage.IsLoading = true;
            let strApiEndPoint = CommonHelper.ConfigUrl + "Create";
            ApiHelper.PostMethod(strApiEndPoint, $scope.Config)
                .then(function (response) {
                    $rootScope.MasterPage.IsLoading = false;
                    $('#myTable').DataTable().ajax.reload(null, false);
                    sys.Alert(true, 'Thêm thành công');
                })
                .catch(function (response) {
                    sys.Alert(false, response.Message);
                    defer.reject(response);
                    $rootScope.MasterPage.IsLoading = false;
                });
            $('#PnModal').modal('hide');
        }
        else {
            $rootScope.MasterPage.IsLoading = true;
            let strApiEndPoint = CommonHelper.ConfigUrl + "Update";
            ApiHelper.PutMethod(strApiEndPoint, $scope.Config)
                .then(function (response) {
                    $rootScope.MasterPage.IsLoading = false;
                    $('#myTable').DataTable().ajax.reload(null, false);
                    sys.Alert(true, 'Cập nhật thành công');
                })
                .catch(function (response) {
                    sys.Alert(false, response.Message);
                    defer.reject(response);
                    $rootScope.MasterPage.IsLoading = false;
                });
            $('#PnModal').modal('hide');
        }
    };
    //#endregion
}
IndexController.$inject = ["$scope", "$rootScope", "$timeout", "$filter", "$compile", "ApiHelper", "CommonHelper", "$q"];
addController("IndexController", IndexController);
