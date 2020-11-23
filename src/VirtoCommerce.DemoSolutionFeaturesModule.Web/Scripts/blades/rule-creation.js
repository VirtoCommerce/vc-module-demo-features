angular.module('virtoCommerce.demoSolutionFeaturesModule')
    .controller('virtoCommerce.demoSolutionFeaturesModule.ruleCreationController', [
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
            };

            $scope.selectProperties = function () {
                var newBlade = {
                    id: 'propertiesSelector',
                    title: 'demoSolutionFeaturesModule.blades.customer-segments-rule-creation.select-properties-blade-title',
                    controller: 'virtoCommerce.demoSolutionFeaturesModule.selectPropertiesController',
                    template: 'Modules/$(virtoCommerce.DemoSolutionFeaturesModule)/Scripts/blades/select-properties.tpl.html',
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
                    controller: 'virtoCommerce.demoSolutionFeaturesModule.editPropertiesController',
                    template: 'Modules/$(virtoCommerce.DemoSolutionFeaturesModule)/Scripts/blades/edit-properties.tpl.html',
                    entities: blade.editedProperties,
                    onSelected: function (editedProps) {
                        blade.editedProperties = editedProps;
                    },
                    toolbarCommands: [
                        {
                            name: "platform.commands.preview", icon: 'fa fa-eye',
                            // TODO: Preview functionality - not implemented
                            // executeMethod: (pickingBlade) => {
                            //     var viewerBlade = {
                            //         id: 'propertiesSelector',
                            //         controller: 'virtoCommerce.demoSolutionFeaturesModule.dynamicAssociationViewerController',
                            //         template: 'Modules/$(virtoCommerce.demoSolutionFeaturesModule)/Scripts/blades/dynamicAssociation-viewer.tpl.html',
                            //         properties: pickingBlade.currentEntities
                            //     };
                            //     bladeNavigationService.showBlade(viewerBlade, pickingBlade);
                            // },
                            //canExecuteMethod: () => true
                        }]
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
