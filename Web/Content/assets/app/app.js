var lstDependency = [];
lstDependency.push("ngRoute");


var MyApp = angular.module("MyApp", lstDependency);
MyApp.value('$', $);
////#region Khai báo Factories

var addFactory = function (name, factory) {
    try {
        MyApp.factory(name, factory);
    } catch (e) {
        console.log(JSON.stringify(e));
    }
}
//#region EWorking
addFactory("$localstorage", $localstorage);
addFactory("ApiHelper", ApiHelper);
addFactory("CommonHelper", CommonHelper);
addFactory("UtilFactory", UtilFactory);
addFactory("DataFactory", DataFactory);

//#endregion

//#endregion

//#region Khai báo Controllers

var addController = function (name, controller) {
    try {
        MyApp.controller(name, controller);
    } catch (e) {
        console.log(JSON.stringify(e));
    }
}

//#region Index
addController("MasterPageController", MasterPageController);
//#endregion

//#region Khai báo Directives

var addDirective = function (name, directive) {
    try {
        MyApp.directive(name, directive);
    } catch (e) {
        console.log(JSON.stringify(e));
    }
}
addDirective("compile", compile);
addDirective("formatMoney", formatMoney);
addDirective("getWidth", getWidth);
addDirective("inputFormat", inputFormat);
addDirective("lazyLoad", lazyLoad);
addDirective("noInput", noInput);
addDirective("whenEnter", whenEnter);

var addService = function (name, service) {
    try {
        MyApp.service(name, service);
    } catch (e) {
        console.log(JSON.stringify(e));
    }
}
