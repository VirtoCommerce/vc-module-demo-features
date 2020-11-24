angular.module('virtoCommerce.demoSolutionFeaturesModule')
.controller('virtoCommerce.demoSolutionFeaturesModule.propertyValuesController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dynamicProperties.dictionaryItemsApi', function ($scope, bladeNavigationService, dictionaryItemsApi) {
    var blade = $scope.blade;
    blade.toolbarCommands = [
        {
            name: "platform.commands.preview", icon: 'fa fa-eye'
        }
    ];

    blade.refresh = function () {
        blade.currentEntities = angular.copy(blade.entities);
        blade.isLoading = false;
    };

    $scope.cancelChanges = function () {
        $scope.bladeClose();
    };

    $scope.saveChanges = function () {
        if (blade.onSelected) {
            blade.onSelected(blade.currentEntities);
        }
        $scope.bladeClose();
    };

    $scope.getDictionaryValues = function (property, callback) {
        dictionaryItemsApi.query({ id: property.objectType, propertyId: property.id }, callback);
    };

    $scope.bladeClose = function() {
        blade.parentBlade.activeBladeId = null;
        bladeNavigationService.closeBlade(blade);
    };

    blade.refresh();
}]);
