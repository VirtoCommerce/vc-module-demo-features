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

        blade.headIcon = 'fa-cogs';
        blade.title = 'demoSolutionFeaturesModule.blades.product-part-list.title';
        blade.subtitle = 'demoSolutionFeaturesModule.blades.product-part-list.subtitle';

        blade.toolbarCommands = [
            {
                name: "platform.commands.add", icon: 'fa fa-plus',
                canExecuteMethod: () => true,
                permission: 'catalog:create'
            },
            {
                name: "platform.commands.edit", icon: 'fa fa-pencil',
                canExecuteMethod: isAnySelected,
                permission: 'catalog:update'
            },
            {
                name: "platform.commands.delete", icon: 'fa fa-trash-o',
                canExecuteMethod: isAnySelected,
                permission: 'catalog:delete'
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

        $scope.select = () => {};
        $scope.delete = () => {};

        filter.criteriaChanged = () => {
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
                        blade.isLoading = false;
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
    }]);

