angular.module('virtoCommerce.demoSolutionFeaturesModule')
    .controller('virtoCommerce.demoSolutionFeaturesModule.customerSegmentsParametersController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.storeModule.stores', function ($scope, bladeNavigationService, stores) {
        var blade = $scope.blade;
        blade.isLoading = true;
        var formScope;
        $scope.setForm = (form) => { formScope = form; };

        $scope.isValid = function() {
            return formScope && formScope.$valid;
        };

        blade.currentEntity = {};

        blade.refresh = (parentRefresh) => {
            stores.query({}, response => {
                $scope.stores = response;
                if (parentRefresh) {
                    blade.parentBlade.refresh();
                }
                blade.isLoading = false;
            });

            blade.currentEntity = angular.copy(blade.originalEntity);
        };

        // datepicker
        $scope.datepickers = {
            str: false,
            end: false
        };

        $scope.open = function ($event, which) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.datepickers[which] = true;
        };

        $scope.cancelChanges = function() {
            blade.parentBlade.activeBladeId = null;
            bladeNavigationService.closeBlade(blade);
        };

        $scope.saveChanges = function() {
            blade.parentBlade.activeBladeId = null;

            if (blade.onSelected) {
                blade.onSelected(blade.currentEntity);
                bladeNavigationService.closeBlade(blade);
            }

            bladeNavigationService.closeBlade(blade);
        };

        $scope.bladeClose = function() {
            blade.parentBlade.activeBladeId = null;
            bladeNavigationService.closeBlade(blade);
        };

        blade.refresh(false);
    }]);
