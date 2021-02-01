angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.controller('virtoCommerce.DemoSolutionFeaturesModule.orderLineItemsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService',
    function($scope, bladeNavigationService) {
        var blade = $scope.widget.blade;

        $scope.$watch('widget.blade.customerOrder', function (operation) {
            $scope.operation = operation;
        });

        $scope.openBlade = function () {
            var newBlade = {
                id: "orderLineItemsList",
                currentEntity: $scope.operation,
                recalculateFn: blade.recalculate,
                controller: 'virtoCommerce.DemoSolutionFeaturesModule.orderLineItemsListController',
                template: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Scripts/blades/order-line-items-list.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };
    }]);
