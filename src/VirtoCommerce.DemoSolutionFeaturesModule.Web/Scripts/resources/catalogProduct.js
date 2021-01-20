angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.factory('virtoCommerce.DemoSolutionFeaturesModule.catalogProductApi', ['$resource', function ($resource) {
    return $resource('api/catalog/search/products', {}, {
        search: { method: 'POST' }
    });
}]);
