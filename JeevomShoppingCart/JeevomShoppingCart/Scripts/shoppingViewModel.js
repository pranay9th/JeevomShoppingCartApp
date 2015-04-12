/// <reference path="knockout-2.0.0.js" />
/// <reference path="shoppingCart.dataService.js" />

var shoppingViewModel = (function () {
    var isCartCancled = ko.observable(false);

    var placeOrder = function () {

    };

    var cancelCart = function () {
        isCartCancled(true);
    };

    return {
        placeOrder: placeOrder,
        cancelCart: cancelCart,
        isCartCancled: isCartCancled
    };
})();



