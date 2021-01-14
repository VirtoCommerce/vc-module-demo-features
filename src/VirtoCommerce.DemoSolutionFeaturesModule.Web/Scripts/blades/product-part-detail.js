angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.controller('virtoCommerce.DemoSolutionFeaturesModule.productPartDetailController',
    ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.DemoSolutionFeaturesModule.productPartsApi',
    function ($scope, bladeNavigationService, productPartsApi) {
        const blade = $scope.blade;
        blade.headIcon = 'fa-cogs';
        blade.toolbarCommands = [];

        if (blade.isNew) {
            blade.toolbarCommands.push({
                name: "platform.commands.create", icon: 'fa fa-check',
                executeMethod: () => $scope.saveChanges(),
                canExecuteMethod: () => blade.isNew && $scope.canSave(),
                permission: 'catalog:create'
            });
        } else {
            blade.toolbarCommands.push({
                name: "platform.commands.save", icon: 'fa fa-save',
                executeMethod: () => $scope.saveChanges(),
                canExecuteMethod: () => !blade.isNew && $scope.canSave(),
                permission: 'catalog:update'
            });
        }

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

        $scope.openProductList = () => {
            var newBlade = {
                id: 'partProductList',
                title: 'demoSolutionFeaturesModule.blades.part-product-list.title',
                subtitle: 'demoSolutionFeaturesModule.blades.part-product-list.subtitle',
                originalEntity: blade.currentEntity,
                controller: 'virtoCommerce.DemoSolutionFeaturesModule.partProductListController',
                template: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Scripts/blades/part-product-list.tpl.html',
                onConfirm: (entity) => {
                    blade.currentEntity = entity;
                }
            };
            bladeNavigationService.showBlade(newBlade, blade);

        };

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