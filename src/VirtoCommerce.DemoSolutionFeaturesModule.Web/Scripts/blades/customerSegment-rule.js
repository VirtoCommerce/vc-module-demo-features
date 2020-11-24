angular.module('virtoCommerce.demoSolutionFeaturesModule')
    .controller('virtoCommerce.demoSolutionFeaturesModule.ruleController', [
        '$scope',
        'platformWebApp.bladeNavigationService',
        'platformWebApp.dynamicProperties.api',
        function ($scope, bladeNavigationService, dynamicPropertiesApi) {
            var blade = $scope.blade;
            blade.isLoading = true;
            blade.activeBladeId = null;
            var allProperties = [];
            blade.propertiesCount = 0;
            blade.editedPropertiesCount = 0;
            blade.customersCount = 0;

            function initializeBlade() {
                dynamicPropertiesApi.search({
                    "objectType":"VirtoCommerce.CustomerModule.Core.Model.Contact",
                    "take":100}, response => {
                        _.each(response.results, function(prop) {
                            prop.group = 'All properties';
                            prop.values = [];
                        });
                        allProperties = response.results;
                        blade.propertiesCount = response.totalCount;
                    });

                blade.isLoading = false;
            }

            $scope.selectProperties = function () {
                var newBlade = {
                    id: 'propertiesSelector',
                    title: 'demoSolutionFeaturesModule.blades.customer-segments-rule-creation.select-properties-blade-title',
                    controller: 'virtoCommerce.demoSolutionFeaturesModule.propertiesController',
                    template: 'Modules/$(virtoCommerce.DemoSolutionFeaturesModule)/Scripts/blades/customerSegment-properties.tpl.html',
                    properties: allProperties,
                    includedProperties: blade.editedProperties,
                    onSelected: function (includedProperties) {
                        blade.editedProperties = includedProperties;
                        blade.editedPropertiesCount = blade.editedProperties.length;
                        $scope.editProperties();
                    }
                };
                blade.activeBladeId = newBlade.id;
                bladeNavigationService.showBlade(newBlade, blade);
            };

            $scope.editProperties = function () {
                var newBlade = {
                    id: 'propertiesEditor',
                    title: 'demoSolutionFeaturesModule.blades.customer-segments-rule-creation.select-properties-blade-title',
                    headIcon: 'fa-pie-chart',
                    controller: 'virtoCommerce.demoSolutionFeaturesModule.propertyValuesController',
                    template: 'Modules/$(virtoCommerce.DemoSolutionFeaturesModule)/Scripts/blades/customerSegment-property-values.tpl.html',
                    entities: blade.editedProperties,
                    onSelected: function (editedProps) {
                        blade.editedProperties = editedProps;
                    }
                };
                blade.activeBladeId = newBlade.id;
                bladeNavigationService.showBlade(newBlade, blade);
            };

            $scope.canSave = () => {
                return false;
            };

            $scope.saveRule = function() {
                if (blade.onSelected) {
                    blade.onSelected(blade.editedProperties);
                }
                $scope.bladeClose();
            };

            $scope.bladeClose = function() {
                blade.parentBlade.activeBladeId = null;
                bladeNavigationService.closeBlade(blade);
            };

            initializeBlade();

        }]);
