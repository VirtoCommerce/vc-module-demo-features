angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.controller('virtoCommerce.DemoSolutionFeaturesModule.customerSegmentsPreview',
    ['$scope', 'virtoCommerce.customerModule.members',
    'platformWebApp.bladeUtils', 'platformWebApp.uiGridHelper', 'platformWebApp.ui-grid.extension',
    'virtoCommerce.customerModule.memberTypesResolverService', 'virtoCommerce.DemoSolutionFeaturesModule.customerSearchCriteriaBuilder',
    function($scope, membersApi, bladeUtils, uiGridHelper, gridOptionExtension, memberTypesResolverService, customerSearchCriteriaBuilder) {
        $scope.uiGridConstants = uiGridHelper.uiGridConstants;

        var blade = $scope.blade;
        blade.title = 'demoSolutionFeaturesModule.blades.customer-segment-preview.title';
        blade.toolbarCommands = [
            {
                name: "platform.commands.refresh", icon: 'fa fa-refresh',
                executeMethod: () => blade.refresh(),
                canExecuteMethod: () => true
            }
        ];

        blade.currentEntity = {};

        function initializeBlade() {
            blade.currentEntity = angular.copy(blade.originalEntity);
        }

        blade.refresh = function() {
            blade.isLoading = true;

            let searchCriteria = getSearchCriteria();
            membersApi.search(searchCriteria, searchResult => {
                $scope.pageSettings.totalItems = searchResult.totalCount;
                let memberTypeDefinition;
                _.each(searchResult.results,
                    function(x) {
                        if (memberTypeDefinition = memberTypesResolverService.resolve(x.memberType)) {
                            x._memberTypeIcon = memberTypeDefinition.icon;
                        }
                    });
                $scope.customers = searchResult.results ? searchResult.results : [];
                blade.isLoading = false;
            });
        }

        var filter = $scope.filter = { };

        filter.criteriaChanged = function () {
            if ($scope.pageSettings.currentPage > 1) {
                $scope.pageSettings.currentPage = 1;
            } else {
                blade.refresh();
            }
        };

        // ui-grid
        $scope.setGridOptions = function (gridId, gridOptions) {
            $scope.gridOptions = gridOptions;
            gridOptionExtension.tryExtendGridOptions(gridId, gridOptions);

            gridOptions.onRegisterApi = function (gridApi) {
                $scope.gridApi = gridApi;
                gridApi.core.on.sortChanged($scope, function () {
                    if (!blade.isLoading) blade.refresh();
                });
            };

            bladeUtils.initializePagination($scope);
        };

        function getSearchCriteria() {
            let searchCriteria = customerSearchCriteriaBuilder.build(filter.keyword, blade.properties, blade.currentEntity.storeIds);
            searchCriteria.sort = uiGridHelper.getSortExpression($scope);
            searchCriteria.skip = ($scope.pageSettings.currentPage - 1) * $scope.pageSettings.itemsPerPageCount;
            searchCriteria.take = $scope.pageSettings.itemsPerPageCount
            return searchCriteria;
        }

        initializeBlade();
    }]);
