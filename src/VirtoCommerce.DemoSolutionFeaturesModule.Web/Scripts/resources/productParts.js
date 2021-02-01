angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.factory('virtoCommerce.DemoSolutionFeaturesModule.productPartsApi', ['$resource', function ($resource) {
    return $resource('api/demo/catalog/product/parts', {}, {
        save: { method: 'POST', isArray: true },
        delete: { method: 'DELETE', params: { ids: '@ids' } },
        search: { method: 'POST', url: 'api/demo/catalog/product/parts/search' }
    });
}]);
