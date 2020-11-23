angular.module('virtoCommerce.demoSolutionFeaturesModule')
    .controller('virtoCommerce.demoSolutionFeaturesModule.customerSegmentsDetailController',
        ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
            const blade = $scope.blade;
            blade.headIcon = 'fa-pie-chart';
            var formScope;

            blade.currentEntity = {};

            blade.isLoading = false;

            blade.activeBladeId = null;

            $scope.canSave = () => {
                return false;
            };

            $scope.setForm = (form) => { formScope = form };

            $scope.mainParameters = function () {
                const parametersBlade = {
                    id: "mainParameters",
                    title: "demoSolutionFeaturesModule.blades.customer-segments-parameters.title",
                    subtitle: 'demoSolutionFeaturesModule.blades.customer-segments-parameters.subtitle',
                    controller: 'virtoCommerce.demoSolutionFeaturesModule.customerSegmentsParametersController',
                    template: 'Modules/$(virtoCommerce.DemoSolutionFeaturesModule)/Scripts/blades/mainParameters.tpl.html',
                    originalEntity: blade.currentEntity,
                    onSelected: function (entity) {
                        blade.currentEntity = entity;
                    }
                };
                blade.activeBladeId = parametersBlade.id;
                bladeNavigationService.showBlade(parametersBlade, blade);
            };

            $scope.$watch('blade.currentEntity', (data) => {
                if (data) {
                    $scope.totalPropertiesCount = 4;
                    $scope.filledPropertiesCount = 0;

                    $scope.filledPropertiesCount += blade.currentEntity.isActive !== undefined ? 1 : 0;
                    $scope.filledPropertiesCount += blade.currentEntity.startDate ? 1 : 0;
                    $scope.filledPropertiesCount += blade.currentEntity.endDate ? 1 : 0;
                    $scope.filledPropertiesCount += blade.currentEntity.storeIds ? 1 : 0;
                }
            }, true);

        }]);
