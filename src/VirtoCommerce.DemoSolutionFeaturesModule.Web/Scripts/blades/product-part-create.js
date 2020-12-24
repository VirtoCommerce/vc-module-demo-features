angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.controller('virtoCommerce.DemoSolutionFeaturesModule.createProductPartController',
    ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.DemoSolutionFeaturesModule.productPartsApi',
    function ($scope, bladeNavigationService, productPartsApi) {
        const blade = $scope.blade;
        blade.headIcon = 'fa-cogs';

        blade.refresh = (parentRefresh) => {
            if (blade.isNew) {
                blade.originalEntity = {
                    configuredProductId: blade.configuredProductId,
                    priority: blade.partsLength + 1,
                    maxQuantity: 0,
                    minQuantity: 0
                };
                blade.currentEntity = angular.copy(blade.originalEntity);
                blade.isLoading = false;
            } else {
                blade.currentEntity = angular.copy(blade.originalEntity);
                blade.isLoading = false;
            }

            if (parentRefresh) {
                blade.parentBlade.refresh();
            }
        };

        blade.toolbarCommands = [
            {
                name: "platform.commands.create", icon: 'fa fa-check',
                executeMethod: () => $scope.saveChanges(),
                canExecuteMethod: () => blade.isNew && $scope.canSave(),
                permission: 'catalog:create'
            },
            {
                name: "platform.commands.update", icon: 'fa fa-save',
                executeMethod: () => $scope.saveChanges(),
                canExecuteMethod: () => !blade.isNew && $scope.canSave(),
                permission: 'catalog:update'
            }
        ];

        blade.onClose = (closeCallback) => {
            bladeNavigationService.showConfirmationIfNeeded(isDirty() && !blade.isNew && !isPriorityChanged(), $scope.isValid(), blade, $scope.saveChanges, closeCallback, "demoSolutionFeaturesModule.dialogs.product-part-create.title", "demoSolutionFeaturesModule.dialogs.product-part-create.message");
        };

        var formScope;
        $scope.setForm = (form) => { formScope = form; };

        $scope.isValid = () => formScope && formScope.$valid;

        $scope.canSave = () => isDirty() && $scope.isValid();

        function isDirty() {
            return !angular.equals(blade.currentEntity, blade.originalEntity);
        }

        function isPriorityChanged() {
            return !angular.equals(blade.currentEntity.priority, blade.originalEntity.priority);
        }

        $scope.saveChanges = () => {
            productPartsApi.save({}, [blade.currentEntity], () => {
                if (blade.isNew) {
                    blade.isNew = false;
                }
                blade.refresh(true);
                $scope.closeBlade();
            });
        }

        $scope.closeBlade = () => {
            bladeNavigationService.closeBlade(blade);
        };

        blade.refresh();
    }]);
