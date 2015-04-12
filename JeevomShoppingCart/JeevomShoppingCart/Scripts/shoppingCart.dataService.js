/// <reference path="knockout-2.0.0.js" />

var shoppingCartDataService = function () {

    var init = function () {
        amplify.request.define('PlaceOrder', 'ajax', {
            url: appConfig.HostName + '/ShoppingCart/PlaceOrder',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            type: 'GET'
        });
        amplify.request.define('CancelCart', 'ajax', {
            url: appConfig.HostName + '/ShoppingCart/CancelCart',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            type: 'GET'
        });
    };

    var placeOrder = function (callBack) {
        return amplify.request({
            resourceId: "PlaceOrder",
            success: function (result) {
                callBack(result);
            }
        });
    };
   
    var CancelCart = function (callBack) {
        return amplify.request({
            resourceId: "CancelCart",
            success: function (result) {
                callBack(result);
            }
        });
    };

    init();

    return {
        placeOrder: placeOrder,
        CancelCart: CancelCart
    };
}();


