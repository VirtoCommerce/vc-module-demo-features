var moduleName = "virtoCommerce.DemoSolutionFeaturesModule";

angular.module(moduleName)
    .factory('virtoCommerce.demoFeatures.featureManager', ['$q', '$http', '$rootScope', function ($q, $http, $rootScope) {
        var result = {};

        result.isFeatureEnabled = (featureName) => {
            var deferred = $q.defer();

            $http({ method: 'GET', url: `api/demo/features/${featureName}` })
                .then(
                    (response) => {
                        var result = response.data;
                        if (result) {
                            deferred.resolve();
                        } else {
                            deferred.reject();
                        }
                    }
                );

            return deferred.promise;
        };

        //TODO: Add callback to the collection and iterate it when login action event fired
        //TODO: Move subscription to separate service wrapper
        result.subscribeToLoginAction = (featureName, callback) => {
            $rootScope.$on('loginStatusChanged',
                (_, authContext) => {
                    result.isFeatureEnabled(featureName).then(callback);
                });
        };

        return result;
    }]);
