angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.controller('virtoCommerce.DemoSolutionFeaturesModule.productPartListController', [
    '$scope',
    'uiGridConstants',
    'platformWebApp.uiGridHelper',
    'platformWebApp.bladeUtils',
    'virtoCommerce.DemoSolutionFeaturesModule.productPartsApi',
    function($scope, uiGridConstants, uiGridHelper, bladeUtils, productPartsApi) {
        $scope.uiGridConstants = uiGridConstants;
        var blade = $scope.blade;
        var filter = $scope.filter = {};
        const bladeNavigationService = bladeUtils.bladeNavigationService;

        blade.headIcon = 'fas fa-pencil-ruler';
        blade.title = 'demoSolutionFeaturesModule.blades.product-part-list.title';
        blade.subtitle = 'demoSolutionFeaturesModule.blades.product-part-list.subtitle';

        blade.toolbarCommands = [
            {
                name: "platform.commands.add", icon: 'fa fa-plus',
                executeMethod: () => {
                    bladeNavigationService.closeChildrenBlades(blade, function () {
                        var newBlade = {
                            id: 'createProductPart',
                            title: 'demoSolutionFeaturesModule.blades.product-part-detail.title',
                            subtitle: 'demoSolutionFeaturesModule.blades.product-part-detail.subtitle-create',
                            catalogId: blade.catalogId,
                            isNew: true,
                            partsLength: $scope.pageSettings.totalItems,
                            configuredProduct: blade.item,
                            configuredProductId: blade.itemId,
                            controller: 'virtoCommerce.DemoSolutionFeaturesModule.productPartDetailController',
                            template: 'Modules/$(VirtoCommerce.DemoSolutionFeatures)/Scripts/blades/product-part-detail.tpl.html'
                        };
                        bladeNavigationService.showBlade(newBlade, blade);
                    });
                },
                canExecuteMethod: () => true,
                permission: 'catalog:create'
            },
            {
                name: "platform.commands.edit", icon: 'fa fa-pencil',
                executeMethod: () => {
                    var selectedNode = $scope.gridApi.selection.getSelectedRows()[0];
                    bladeNavigationService.closeChildrenBlades(blade, function () {
                        var newBlade = {
                            id: 'editProductPart',
                            title: 'demoSolutionFeaturesModule.blades.product-part-detail.title',
                            subtitle: 'demoSolutionFeaturesModule.blades.product-part-detail.subtitle-edit',
                            configuredProduct: blade.item,
                            originalEntity: selectedNode,
                            controller: 'virtoCommerce.DemoSolutionFeaturesModule.productPartDetailController',
                            template: 'Modules/$(VirtoCommerce.DemoSolutionFeatures)/Scripts/blades/product-part-detail.tpl.html'
                        };
                        bladeNavigationService.showBlade(newBlade, blade);
                    });
                },
                canExecuteMethod: isOnlyOneSelected,
                permission: 'catalog:update'
            },
            {
                name: "platform.commands.delete", icon: 'fa fa-trash-o',
                canExecuteMethod: isAnySelected,
                permission: 'catalog:delete',
                executeMethod: () => {
                    const selectedIds = _.pluck($scope.gridApi.selection.getSelectedRows(), 'id');
                    productPartsApi.delete({ ids: selectedIds}, () => {
                        $scope.gridApi.selection.clearSelectedRows();
                        blade.refresh();
                    });
                }
            }
        ];

        blade.refresh = () => {
            blade.isLoading = true;

            productPartsApi.search({
                searchPhrase: filter.keyword ? filter.keyword : undefined,
                configuredProductId: blade.itemId,
                sort: 'priority:asc',
                skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                take: $scope.pageSettings.itemsPerPageCount
            }, response => {
                blade.isLoading = false;
                $scope.pageSettings.totalItems = response.totalCount;
                blade.currentEntities = response.results;
                return response.results;
            });
        };

        $scope.select = (node) => {
            $scope.selectedNodeId = node.id;

            var newBlade = {
                id: 'editProductPart',
                title: 'demoSolutionFeaturesModule.blades.product-part-detail.title',
                subtitle: 'demoSolutionFeaturesModule.blades.product-part-detail.subtitle-edit',
                configuredProduct: blade.item,
                originalEntity: node,
                catalogId: blade.catalogId,
                controller: 'virtoCommerce.DemoSolutionFeaturesModule.productPartDetailController',
                template: 'Modules/$(VirtoCommerce.DemoSolutionFeatures)/Scripts/blades/product-part-detail.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);

        };

        $scope.delete = () => {};

        filter.changeKeyword = ($event) => {
            const enterKeyCode = 13;
            if ($event.which === enterKeyCode) {
                filter.update();
            }
        }

        filter.clearKeyword = () => {
            filter.keyword = null;
            filter.update();
        }

        filter.update = () => {
            if ($scope.pageSettings.currentPage > 1) {
                $scope.pageSettings.currentPage = 1;
            } else {
                blade.refresh();
            }
        };

        $scope.setGridOptions = (gridOptions) => {
            uiGridHelper.initialize($scope, gridOptions, (gridApi) => {
                gridApi.draggableRows.on.rowDropped($scope, () => {
                    blade.isLoading = true;
                    blade.currentEntities.forEach((entity, index) => {
                        entity.priority = index + 1;
                    })
                    productPartsApi.save(blade.currentEntities, () => {
                        bladeNavigationService.closeChildrenBlades(blade, () => {
                            blade.isLoading = false;
                        });
                    }, () => {
                        blade.refresh();
                    });
                });
            });
            bladeUtils.initializePagination($scope);
            $scope.pageSettings.itemsPerPageCount = 10;
        };

        function isAnySelected() {
            return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
        }

        function isOnlyOneSelected() {
            return $scope.gridApi && $scope.gridApi.selection.getSelectedRows().length === 1;
        }
    }]);
