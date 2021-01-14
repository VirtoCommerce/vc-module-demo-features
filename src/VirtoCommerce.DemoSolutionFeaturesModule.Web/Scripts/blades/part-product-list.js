angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.controller('virtoCommerce.DemoSolutionFeaturesModule.partProductListController', [
    '$scope',
    'uiGridConstants',
    'platformWebApp.uiGridHelper',
    'platformWebApp.bladeUtils',
    'virtoCommerce.DemoSolutionFeaturesModule.catalogProductApi',
    function($scope, uiGridConstants, uiGridHelper, bladeUtils, productApi) {
        $scope.uiGridConstants = uiGridConstants;
        var blade = $scope.blade;
        var filter = $scope.filter = blade.filter = {};
        const bladeNavigationService = bladeUtils.bladeNavigationService;

        blade.headIcon = 'fa-archive';

        blade.currentEntity = angular.copy(blade.originalEntity);

        blade.toolbarCommands = [
            {
                name: "platform.commands.add", icon: 'fa fa-plus',
                executeMethod: () => {},
                canExecuteMethod: () => true
            },
            {
                name: "platform.commands.delete", icon: 'fa fa-trash-o',
                executeMethod: () => {
                    const selectedNodesId = _.pluck($scope.gridApi.selection.getSelectedRows(), "id");
                    if (_.some(selectedNodesId, (nodeId) => nodeId === blade.currentEntity.defaultItemId)) {
                        bladeNavigationService.showConfirmationIfNeeded(true, true, blade, () => { deleteProducts(selectedNodesId); }, () => {}, "demoSolutionFeaturesModule.dialogs.default-product-delete.title", "demoSolutionFeaturesModule.dialogs.default-product-delete.message");
                    } else {
                        deleteProducts(selectedNodesId);
                    }
                },
                canExecuteMethod: isAnySelected
            },
            {
                name: "demoSolutionFeaturesModule.blades.part-product-list.toolbar-commands.default", icon: 'fa fa-check',
                executeMethod: () => {
                    const selectedNodeId = $scope.gridApi.selection.getSelectedRows()[0].id;
                    blade.currentEntity.defaultItemId = selectedNodeId;
                },
                canExecuteMethod: isOnlyOneSelected
            }
        ];

        blade.refresh = () => {
            blade.isLoading = true;
            blade.productIds = _.pluck(blade.currentEntity.partItems, "itemId");

            if (blade.productIds.length) {
                productApi.search(
                    {
                        objectIds: blade.productIds,
                        searchPhrase: filter.keyword ? filter.keyword : undefined,
                        sort: 'priority:asc',
                        skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                        take: $scope.pageSettings.itemsPerPageCount
                    }, response => {
                        blade.isLoading = false;
                        $scope.pageSettings.totalItems = response.totalCount;
                        blade.currentEntities = _.map(response.items, item => {
                            return _.extend(item, _.findWhere(blade.currentEntity.partItems, {itemId: item.id}));
                        });
                        return response.items;
                    });
            }

            blade.isLoading = false;
        };

        filter.criteriaChanged = () => {
            if ($scope.pageSettings.currentPage > 1) {
                $scope.pageSettings.currentPage = 1;
            } else {
                blade.refresh();
            }
        };

        $scope.canSave = () => isDirty();

        $scope.saveChanges = () => {
            blade.onConfirm(blade.currentEntity);
            $scope.closeBlade();
        };

        $scope.closeBlade = () => {
            bladeNavigationService.closeBlade(blade);
        };

        $scope.bladeClose = () => {
            if (!isDirty()) {
                $scope.closeBlade();
            }
            bladeNavigationService.showConfirmationIfNeeded(isDirty(), $scope.canSave, blade, $scope.saveChanges, $scope.closeBlade, "demoSolutionFeaturesModule.dialogs.add-products-to-part.title", "demoSolutionFeaturesModule.dialogs.add-products-to-part.message");
        }

        $scope.setDefault = (contextMenuEntity) => {
            const selectedNodeId = contextMenuEntity.id;
            blade.currentEntity.defaultItemId = selectedNodeId;
        };

        $scope.delete = (contextMenuEntity) => {
            const selectedNodeId = contextMenuEntity.id;
            if (selectedNodeId === blade.currentEntity.defaultItemId) {
                bladeNavigationService.showConfirmationIfNeeded(true, true, blade, () => { deleteProduct(selectedNodeId); }, () => {}, "demoSolutionFeaturesModule.dialogs.default-product-delete.title", "demoSolutionFeaturesModule.dialogs.default-product-delete.message");
            } else {
                deleteProduct(selectedNodeId);
            }
        };

        $scope.setGridOptions = (gridOptions) => {
            uiGridHelper.initialize($scope, gridOptions, (gridApi) => {
                gridApi.draggableRows.on.rowDropped($scope, () => {
                    blade.currentEntities.forEach((entity, index) => {
                        entity.priority = index + 1;
                    });
                    blade.currentEntity.partItems.forEach((item) => {
                        item.priority = _.findWhere(blade.currentEntities, {id: item.itemId}).priority;
                    });
                });
            });
            bladeUtils.initializePagination($scope);
            $scope.pageSettings.itemsPerPageCount = 10;
        };

        function deleteProducts(selectedNodesId) {
            _.each(selectedNodesId, (nodeId) => {
                blade.currentEntity.partItems = _.filter(blade.currentEntity.partItems, (part) => part.itemId !== nodeId);
            });
            blade.refresh();
        }

        function deleteProduct(selectedNodeId) {
            blade.currentEntity.partItems = _.filter(blade.currentEntity.partItems, (part) => part.itemId !== selectedNodeId);
            blade.refresh();
        }

        function isDirty() {
            return !angular.equals(blade.currentEntity, blade.originalEntity);
        }

        function isAnySelected() {
            return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
        }

        function isOnlyOneSelected() {
            return $scope.gridApi && $scope.gridApi.selection.getSelectedRows().length === 1;
        }
    }]);
