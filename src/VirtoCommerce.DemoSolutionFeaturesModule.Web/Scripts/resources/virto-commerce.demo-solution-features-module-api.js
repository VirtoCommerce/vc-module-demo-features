angular.module('virtoCommerce.demoSolutionFeaturesModule')
    .factory('virtoCommerce.demoSolutionFeaturesModule.webApi', ['$resource', function ($resource) {
        return $resource('api/VirtoCommerceDemoSolutionFeaturesModule');
}]);
