angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.controller('virtoCommerce.DemoSolutionFeaturesModule.customerSegmentPropertiesController',
    ['$scope', 'platformWebApp.bladeNavigationService',
    function ($scope, bladeNavigationService) {
        var blade = $scope.blade;
        blade.isLoading = true;
        blade.currentEntity = {};

        function initializeBlade() {
            if (blade.selectedProperties && blade.selectedProperties.length) {
                let selectedPropertyNames = blade.selectedProperties.map(property => property.name);
                let notSelectedProperties = _.filter(blade.properties,
                    property => !_.contains(selectedPropertyNames, property.name));
                blade.availableProperties = _.union(blade.selectedProperties, notSelectedProperties);
            } else {
                blade.availableProperties = blade.properties;
            }
            blade.currentEntity = angular.copy(blade.originalEntity);
            blade.isLoading = false;
        }

        $scope.selectAll = function () {
            blade.selectedProperties = angular.copy(blade.properties);
        }

        $scope.clearAll = function () {
            blade.selectedProperties = [];
        }

        $scope.sortSelected = function ($item, $model) {
            blade.selectedProperties = _.sortBy(blade.selectedProperties, 'name');
        }

        $scope.isValid = function () {
            return blade.selectedProperties.length;
        }

        $scope.cancelChanges = function () {
            blade.parentBlade.activeBladeId = null;
            bladeNavigationService.closeBlade(blade);
        }

        $scope.saveChanges = function () {
            blade.parentBlade.activeBladeId = null;
            if (blade.onSelected) {
                blade.onSelected(blade.currentEntity, blade.selectedProperties);
                bladeNavigationService.closeBlade(blade);
            }
        };

        $scope.bladeClose = function() {
            blade.parentBlade.activeBladeId = null;
            bladeNavigationService.closeBlade(blade);
        };

        initializeBlade();

    }]);
