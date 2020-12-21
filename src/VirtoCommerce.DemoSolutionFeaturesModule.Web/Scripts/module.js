// Call this to register your module to main application
var moduleName = "virtoCommerce.DemoSolutionFeaturesModule";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['virtoCommerce.catalogModule.itemTypesResolverService', 'virtoCommerce.demoFeatures.featureManager',
        function (itemTypesResolverService, featureManager) {
            featureManager.isFeatureEnabled('ConfigurableProduct').then(() => {
                itemTypesResolverService.registerType({
                    itemType: 'demoSolutionFeaturesModule.blades.categories-items-add.menu.configurable-product.title',
                    description: 'demoSolutionFeaturesModule.blades.categories-items-add.menu.configurable-product.description',
                    productType: 'Configurable',
                    icon: 'fa-cogs'
                });
            });
    }]);
