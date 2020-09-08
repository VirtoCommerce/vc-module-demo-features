angular.module('virtoCommerce.demoSolutionFeaturesModule')
    .controller('virtoCommerce.demoSolutionFeaturesModule.helloWorldController', ['$scope', 'virtoCommerce.demoSolutionFeaturesModule.webApi', function ($scope, api) {
        var blade = $scope.blade;
        blade.title = 'VirtoCommerce.DemoSolutionFeaturesModule';

        blade.refresh = function () {
            api.get(function (data) {
                blade.title = 'virtoCommerce.demoSolutionFeaturesModule.blades.hello-world.title';
                blade.data = data.result;
                blade.isLoading = false;
            });
        };

        blade.refresh();
    }]);
