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
                canExecuteMethod: function () {
                    return true;
                },
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

        blade.refresh = function () {
            blade.isLoading = true;

            productPartsApi.search({
                searchPhrase: filter.keyword ? filter.keyword : undefined,
                sort: 'priority:asc',
                skip: ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount,
                take: $scope.pageSettings.itemsPerPageCount
            }, function (response) {
                blade.isLoading = false;
                $scope.pageSettings.totalItems = response.totalCount;
                blade.currentEntities = response.results;
                return response.results;
            });
        };

        $scope.select = function () {};
        $scope.delete = function() {};

        filter.criteriaChanged = function () {
            if ($scope.pageSettings.currentPage > 1) {
                $scope.pageSettings.currentPage = 1;
            } else {
                blade.refresh();
            }
        };

        $scope.setGridOptions = function (gridOptions) {
            uiGridHelper.initialize($scope, gridOptions);
            bladeUtils.initializePagination($scope);
        };

        function isAnySelected() {
            return $scope.gridApi && _.any($scope.gridApi.selection.getSelectedRows());
        }
    }]);

