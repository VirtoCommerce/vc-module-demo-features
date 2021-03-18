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

        blade.headIcon = 'fas fa-box-open';

        blade.currentEntity = angular.copy(blade.originalEntity);

        blade.toolbarCommands = [
            {
                name: "platform.commands.add", icon: 'fa fa-plus',
                executeMethod: () => {
                    $scope.addNewProduct();
                },
                canExecuteMethod: () => true
            },
            {
                name: "platform.commands.delete", icon: 'fa fa-trash-o',
                executeMethod: () => {
                    const selectedNodesId = _.pluck($scope.gridApi.selection.getSelectedRows(), "id");
                    if (_.some(selectedNodesId, (nodeId) => nodeId === blade.currentEntity.defaultItemId)) {
                        bladeNavigationService.showConfirmationIfNeeded(true, true, blade, () => { deleteProducts(selectedNodesId, true); }, () => {}, "demoSolutionFeaturesModule.dialogs.default-product-delete.title", "demoSolutionFeaturesModule.dialogs.default-product-delete.message");
                    } else {
                        bladeNavigationService.showConfirmationIfNeeded(true, true, blade, () => { deleteProducts(selectedNodesId, false); }, () => {}, "demoSolutionFeaturesModule.dialogs.part-product-delete.title", "demoSolutionFeaturesModule.dialogs.part-product-delete.message");
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
                        skip: 0,
                        take: blade.productIds.length
                    }, response => {
                        blade.isLoading = false;
                        $scope.pageSettings.totalItems = response.totalCount;
                        blade.currentEntities = _.map(response.items, item => {
                            return _.extend(item, _.findWhere(blade.currentEntity.partItems, {itemId: item.id}));
                        });
                        blade.currentEntities = _.sortBy(blade.currentEntities, 'priority');
                        return response.items;
                    });
            } else {
                $scope.pageSettings.totalItems = 0;
                blade.currentEntities = [];
                blade.isLoading = false;
            }
        };

        filter.criteriaChanged = () => {
            blade.refresh();
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
                bladeNavigationService.showConfirmationIfNeeded(true, true, blade, () => { deleteProducts([selectedNodeId], true); }, () => {}, "demoSolutionFeaturesModule.dialogs.default-product-delete.title", "demoSolutionFeaturesModule.dialogs.default-product-delete.message");
            } else {
                bladeNavigationService.showConfirmationIfNeeded(true, true, blade, () => { deleteProducts([selectedNodeId], false); }, () => {}, "demoSolutionFeaturesModule.dialogs.part-product-delete.title", "demoSolutionFeaturesModule.dialogs.part-product-delete.message");
            }
        };

        $scope.addNewProduct = function() {
            const options = {
                showCheckingMultiple: false,
                allowCheckingCategory: false,
                allowCheckingItem: true,
                selectedItemIds: [],
                checkItemFn: (listItem, isSelected) => {
                    if (isSelected) {
                        if (!_.find(options.selectedItemIds, (x) => x === listItem.id)) {
                            options.selectedItemIds.push(listItem.id);
                        }
                    }
                    else {
                        options.selectedItemIds = _.reject(options.selectedItemIds, (x) => x === listItem.id);
                    }
                },
                onItemsLoaded: (entries) => {
                    const filteredEntries = _.reject(entries, (x) => x.type === 'product' && x.productType === 'Configurable');
                    entries.splice(0, entries.length, ...filteredEntries);
                }
            };

            const newBlade = {
                id: "CatalogItemsSelect",
                controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
                template: 'Modules/$(virtoCommerce.catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
                title: 'catalog.selectors.blades.titles.select-categories',
                options: options,
                breadcrumbs: [],
                catalogId: blade.catalogId,
                toolbarCommands: [
                    {
                        name: "platform.commands.confirm", icon: 'fa fa-check',
                        executeMethod: function (pickingBlade) {
                            const newIds = _.difference(options.selectedItemIds, blade.productIds);
                            const oldPartItemsLength = blade.currentEntity.partItems.length;
                            newIds.forEach((id, index) => {
                                if (!blade.productIds.length && index === 0) {
                                    blade.currentEntity.defaultItemId = id;
                                }
                                blade.currentEntity.partItems.push({itemId: id, priority: oldPartItemsLength + (index + 1)});
                            });
                            bladeNavigationService.closeBlade(pickingBlade);
                            blade.refresh();
                        },
                        canExecuteMethod: () => _.any(options.selectedItemIds)
                    }]
            };
            bladeNavigationService.showBlade(newBlade, blade);

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
        };

        function deleteProducts(selectedNodesId, defaultDeleted) {
            _.each(selectedNodesId, (nodeId) => {
                blade.currentEntity.partItems = _.filter(blade.currentEntity.partItems, (part) => part.itemId !== nodeId);
            });
            if (defaultDeleted) {
                if (blade.currentEntity.partItems.length) {
                    blade.currentEntity.defaultItemId = _.min(blade.currentEntity.partItems, (item) => item.priority).itemId;
                } else {
                    blade.currentEntity.defaultItemId = '';
                }
            }
            bladeNavigationService.closeChildrenBlades(blade);
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
