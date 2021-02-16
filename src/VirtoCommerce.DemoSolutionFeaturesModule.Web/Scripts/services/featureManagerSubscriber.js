var moduleName = "virtoCommerce.DemoSolutionFeaturesModule";

angular.module(moduleName)
    .factory('virtoCommerce.demoFeatures.featureManagerSubscriber', ['$rootScope', 'virtoCommerce.demoFeatures.featureManager', '$window', function ($rootScope, featureManager, $window) {
        var result = {};

        result.callbacksGroupedByFeatureName = [];
        result.onLoginStatusChanged = (featureName, callback) => {
            if (!result.callbacksGroupedByFeatureName[featureName]) {
                result.callbacksGroupedByFeatureName[featureName] = [];
            }
            if (typeof callback === 'function') {
                result.callbacksGroupedByFeatureName[featureName].push(callback);
            }
        };

        function initialize() {
            //This function runs only on 'login'/'sign out' event, but the features are registering only at the time of login,
            //when we sign out - we are just reloading
            $rootScope.$on('loginStatusChanged',
                (_, authContext) => {
                    if (!authContext.isAuthenticated) {
                        $window.location.reload();
                    }

                    for (const [featureName, callbacks] of Object.entries(result.callbacksGroupedByFeatureName)) {
                        featureManager.isFeatureEnabled(featureName).then(() => {
                            angular.forEach(callbacks, callback => {
                                callback();
                            })
                        })
                      }
                });
        }

        initialize();

        return result;
    }]);
