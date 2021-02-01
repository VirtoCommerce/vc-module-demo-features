// Call this to register your module to main application
var moduleName = 'virtoCommerce.DemoSolutionFeaturesModule';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['$rootScope', '$window', 'virtoCommerce.catalogModule.itemTypesResolverService', 'platformWebApp.widgetService', 'virtoCommerce.demoFeatures.featureManager',
        function ($rootScope, $window, itemTypesResolverService, widgetService, featureManager) {
            // Refresh page & reinitialize application after login/logout.
            // We need this workaround to toggle features depending on current user permissions
            // It doesn't work by default because blades, widgets registration occurs during app initialization
            let isFreshPageLoad = true;
            $rootScope.$on('loginStatusChanged', function(_, authContext) {
                if (!isFreshPageLoad && authContext.isAuthenticated) {
                    $window.location.reload();
                }
                isFreshPageLoad = false;
            });

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

                widgetService.registerWidget({
                    isVisible: function (blade) { return blade.controller === 'virtoCommerce.orderModule.operationDetailController'; },
                    controller: 'virtoCommerce.DemoSolutionFeaturesModule.orderLineItemsWidgetController',
                    template: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Scripts/widgets/orderLineItemsWidget.tpl.html'
                }, 'customerOrderDetailWidgets');
            });

            const itemDetailWidgets = widgetService.widgetsMap['itemDetail'];
            const variationWidget = itemDetailWidgets.find(widget => widget.controller == 'virtoCommerce.catalogModule.itemVariationWidgetController');
            variationWidget.isVisible = blade => blade.id !== 'variationDetail' && blade.productType !== configurableProductType;
    }]);
