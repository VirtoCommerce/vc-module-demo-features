// Call this to register your module to main application
var moduleName = "virtoCommerce.DemoSolutionFeaturesModule";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .factory('virtoCommerce.demoFeatures.featureManager', ['$q', '$http', function($q, $http) {
        return {
            isFeatureEnabled: (featureName) => {
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
            }
        }
    }]);
