var moduleName = "virtoCommerce.DemoSolutionFeaturesModule";

angular.module(moduleName)
    .factory('virtoCommerce.demoFeatures.featureManagerSubscriber', ['$rootScope', 'virtoCommerce.demoFeatures.featureManager', function ($rootScope, featureManager) {
        var result = {};

        result.callbackCollection = [];
        result.subscribeToLoginAction = (featureName, callback) => {
            if (!result.callbackCollection[featureName]) {
                result.callbackCollection[featureName] = [];
            }
            if (typeof callback === 'function') {
                result.callbackCollection[featureName].push(callback);
            }
        };

        function initialize() {
            $rootScope.$on('loginStatusChanged',
                () => {
                    for (const [featureName, callbacks] of Object.entries(result.callbackCollection)) {
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
