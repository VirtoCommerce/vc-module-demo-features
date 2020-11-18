angular.module('virtoCommerce.demoSolutionFeaturesModule')
    .controller('virtoCommerce.demoSolutionFeaturesModule.customerSegmentsDetailController',
        ['$scope', function ($scope) {
            const blade = $scope.blade;
            var formScope;

            blade.currentEntity = {};

            blade.isLoading = false;

            $scope.canSave = () => {
                return formScope && formScope.$valid;
            };

            $scope.setForm = (form) => { formScope = form };
        }]);
