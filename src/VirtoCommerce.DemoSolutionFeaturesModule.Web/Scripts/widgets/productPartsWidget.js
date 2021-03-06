angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.controller('virtoCommerce.DemoSolutionFeaturesModule.productPartsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService',
    function($scope, bladeNavigationService) {
        var blade = $scope.blade;

        $scope.openBlade = function () {
            var newBlade = {
                id: "productPartList",
                item: blade.item,
                itemId: blade.itemId,
                catalogId: blade.catalog.id,
                controller: 'virtoCommerce.DemoSolutionFeaturesModule.productPartListController',
                template: 'Modules/$(VirtoCommerce.DemoSolutionFeatures)/Scripts/blades/product-part-list.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };
    }]);
