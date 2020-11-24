angular.module('virtoCommerce.demoSolutionFeaturesModule')
    .controller('virtoCommerce.demoSolutionFeaturesModule.propertiesController', ['$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
        var blade = $scope.blade;

        blade.isLoading = true;

        function initializeBlade() {
            var allProperties = angular.copy(blade.properties);
            allProperties = _.sortBy(allProperties, 'group', 'name');
            var selectedProperties = angular.copy(blade.includedProperties);
            selectedProperties = _.sortBy(selectedProperties, 'name');
            blade.allEntities = _.groupBy(allProperties, 'group');
            blade.selectedEntities = _.groupBy(selectedProperties, 'group');
            blade.isLoading = false;
        }

        $scope.selectAllInGroup = function (groupKey) {
            blade.selectedEntities[groupKey] = blade.allEntities[groupKey];
        }

        $scope.clearAllInGroup = function (groupKey) {
            blade.selectedEntities[groupKey] = [];
        }

        $scope.sortSelected = function (groupKey) {
            blade.selectedEntities[groupKey] = _.sortBy(blade.selectedEntities[groupKey], 'name');
        }

        $scope.cancelChanges = function () {
            blade.parentBlade.activeBladeId = null;
            bladeNavigationService.closeBlade(blade);
        }

        $scope.isValid = function () {
            return _.some(blade.selectedEntities, function (item) { return item.length; });
        }

        $scope.saveChanges = function () {
            blade.parentBlade.activeBladeId = null;
            var includedProperties = _.flatten(_.map(blade.selectedEntities, _.values));
            if (blade.onSelected) {
                blade.onSelected(includedProperties);
                bladeNavigationService.closeBlade(blade);
            }
        };

        $scope.bladeClose = function() {
            blade.parentBlade.activeBladeId = null;
            bladeNavigationService.closeBlade(blade);
        };

        initializeBlade();

    }]);
