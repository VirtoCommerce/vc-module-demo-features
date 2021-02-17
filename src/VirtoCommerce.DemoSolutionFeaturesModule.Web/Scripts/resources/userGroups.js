angular.module('virtoCommerce.DemoSolutionFeaturesModule')
    .factory('virtoCommerce.DemoSolutionFeaturesModule.userGroupsApi', ['$resource', function ($resource) {
        return $resource('api/demo/members', {}, {
            get: { url: 'api/demo/members/tagged/:id', method: 'GET' },
            save: { url: 'api/demo/members/tagged', method: 'POST' },
            search: { url: 'api/demo/members/search/tagged', method: 'POST' },
        });
}]);
