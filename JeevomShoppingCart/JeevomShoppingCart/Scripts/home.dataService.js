/// <reference path="knockout-2.0.0.js" />

var homeDataService = function () {

    var init = function () {
        amplify.request.define('getAllProducts', 'ajax', {
            url: appConfig.HostName + '/home/GetAllProducts',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            type: 'GET'
        });

        amplify.request.define('addToUserCart', 'ajax', {
            url: appConfig.HostName + '/home/AddProductToUserCart',
            contentType: 'application/json; charset=UTF-8',
            dataType: 'json',
            type: 'POST'
        });
    };

    var getAllProducts = function (callback) {
        return amplify.request({
            resourceId: "getAllProducts",
            success: function (result) {
                if (result) {
                    callback(result);
                }
            }
        });
    };
   
    var addToUserCart = function (product,callback) {
        var request = JSON.stringify(product);
        return amplify.request({
            resourceId: "addToUserCart",
            data:request,
            success: function (result) {
                if (result) {
                    callback(request);
                }
            }
        });
    };
    init();
    return {
        getAllProducts: getAllProducts,
        addToUserCart: addToUserCart
    };
}();


