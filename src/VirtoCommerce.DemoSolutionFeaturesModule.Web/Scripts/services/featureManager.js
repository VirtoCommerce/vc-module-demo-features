var moduleName = "virtoCommerce.DemoSolutionFeaturesModule";

angular.module(moduleName)
    .factory('virtoCommerce.demoFeatures.featureManager', ['$q', '$http', function ($q, $http) {
        var result = {};

        result.isFeatureEnabled = (featureName) => {
            var deferred = $q.defer();

            $http({ method: 'GET', url: `api/demo/features/${featureName}` })
                .then(
                    (response) => {
                        var res = response.data;
                        if (res) {
                            deferred.resolve();
                        } else {
                            deferred.reject();
                        }
                    }
                );

            return deferred.promise;
        };

        return result;
    }]);
