/// <reference path="knockout-2.0.0.js" />
/// <reference path="home.dataService.js" />
/// <reference path="shoppingCart.dataService.js" />

var product = function (item) {
    this.ProductID = item.ProductID;
    this.name = ko.observable(item.ProductName);
    this.price = ko.observable(item.Price);
    this.unitCount = ko.observable(item.UnitCount);
    this.totalPrice = ko.observable((item.Price * item.UnitCount).toFixed(2));
};

var homeViewModel = (function () {
    var products = ko.observableArray([]);
    var isCartCancled = ko.observable(false);
    init = function () {
        var koProducts = [];
        homeDataService.getAllProducts(function (allProducts) {
            $.each(allProducts, function (i, item) {
                koProducts.push(new product(item));
            });
            products(koProducts);
        });
    };

    var addProduct = function (item) {
        item.unitCount(item.unitCount()+1);
        updateTotalPrice(item);
    };

    var removeProduct = function (item) {
        item.unitCount(item.unitCount() - 1);
        updateTotalPrice(item);
    };

    var updateTotalPrice = function (item) {
        item.totalPrice((item.unitCount()*item.price()).toFixed(2));
    };

    var addToCart = function (item) {

        var selectedProduct = { ProductID: item.ProductID, ProductName: item.name(), Price: item.price(), UnitCount: item.unitCount() };

        homeDataService.addToUserCart(selectedProduct, function (data) {
            alert("added ProductInfo.  "+data);
        });
    };

    var placeOrder = function () {
        shoppingCartDataService.placeOrder(function(isSuccess) {
            if (isSuccess) {
                isCartCancled(true);
                alert("Ordered Placed");
            }
        });
        
    };

    var cancelCart = function () {
        shoppingCartDataService.CancelCart(function (isSuccess) {
            if (isSuccess) {
                isCartCancled(true);
                alert("Ordered cancled");
            }
        });
       
    };

    init();

    return {
        products: products,
        addProduct: addProduct,
        removeProduct: removeProduct,
        addToCart: addToCart,
        placeOrder: placeOrder,
        cancelCart: cancelCart,
        isCartCancled: isCartCancled
    };
   
})();



