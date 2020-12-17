// Call this to register your module to main application
var moduleName = 'virtoCommerce.DemoSolutionFeaturesModule';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['$compile', '$http', 'virtoCommerce.catalogModule.itemTypesResolverService', 'platformWebApp.widgetService', 'virtoCommerce.demoFeatures.featureManager',
        function ($compile, $http, itemTypesResolverService, widgetService, featureManager) {
            const configurableProductType = 'Configurable';
            featureManager.isFeatureEnabled('ConfigurableProduct').then(() => {
                itemTypesResolverService.registerType({
                    itemType: 'demoSolutionFeaturesModule.blades.categories-items-add.menu.configurable-product.title',
                    description: 'demoSolutionFeaturesModule.blades.categories-items-add.menu.configurable-product.description',
                    productType: configurableProductType,
                    icon: 'fa-cogs'
                });

                widgetService.registerWidget({
                    isVisible: function (blade) { return blade.productType === configurableProductType; },
                    controller: 'virtoCommerce.DemoSolutionFeaturesModule.productPartsWidgetController',
                    template: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Scripts/widgets/productPartsWidget.tpl.html'
                }, 'itemDetail');
            });

            $http.get('Modules/$(VirtoCommerce.Orders)/Scripts/widgets/dashboard/statistics-templates.html').then(function (response) {
                // compile the response, which will put stuff into the cache
                $compile(response.data);
            });
    }]);
