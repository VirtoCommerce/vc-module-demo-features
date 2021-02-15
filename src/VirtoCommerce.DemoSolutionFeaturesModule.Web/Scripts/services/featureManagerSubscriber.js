var moduleName = "virtoCommerce.DemoSolutionFeaturesModule";

angular.module(moduleName)
    .factory('virtoCommerce.demoFeatures.featureManagerSubscriber', ['$rootScope', 'virtoCommerce.demoFeatures.featureManager', function ($rootScope, featureManager) {
        var result = {};

        result.callbacksGroupedByFeatureName = [];
        result.subscribeToLoginAction = (featureName, callback) => {
            if (!result.callbacksGroupedByFeatureName[featureName]) {
                result.callbacksGroupedByFeatureName[featureName] = [];
            }
            if (typeof callback === 'function') {
                result.callbacksGroupedByFeatureName[featureName].push(callback);
            }
        };

        function initialize() {
            $rootScope.$on('loginStatusChanged',
                () => {
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
