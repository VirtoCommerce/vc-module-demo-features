angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.factory('virtoCommerce.DemoSolutionFeaturesModule.productPartsApi', ['$resource', function ($resource) {
    return $resource('api/demo/catalog/product/parts', {}, {
        save: { method: 'POST', isArray: true },
        delete: { method: 'DELETE' },
        search: { method: 'POST', url: 'api/demo/catalog/product/parts/search' }
    });
}]);
