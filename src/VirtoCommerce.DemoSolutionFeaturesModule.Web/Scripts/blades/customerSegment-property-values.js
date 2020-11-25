angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.controller('virtoCommerce.DemoSolutionFeaturesModule.customerSegmentPropertyValuesController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.dynamicProperties.dictionaryItemsApi', function ($scope, bladeNavigationService, dictionaryItemsApi) {
    var blade = $scope.blade;
    blade.toolbarCommands = [
        {
            name: "platform.commands.preview", icon: 'fa fa-eye',
            executeMethod: (currentBlade) => {
                let properties = currentBlade.currentEntities.reduce((propertiesAggregate, property) => {
                    propertiesAggregate[property.name] = property.values.map(propertyValue => propertyValue.value.name || propertyValue.value);
                    return propertiesAggregate;
                }, {});
                let previewBlade = {
                    id: 'customerSegmentsPreview',
                    controller: 'virtoCommerce.DemoSolutionFeaturesModule.customerSegmentsPreview',
                    template: 'Modules/$(virtoCommerce.DemoSolutionFeaturesModule)/Scripts/blades/customerSegments-preview.tpl.html',
                    currentEntity: blade.currentEntity,
                    properties: properties
                };
                bladeNavigationService.showBlade(previewBlade, currentBlade);
            },
            canExecuteMethod: () => true
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
