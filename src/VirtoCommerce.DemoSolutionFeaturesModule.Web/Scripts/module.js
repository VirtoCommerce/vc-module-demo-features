// Call this to register your module to main application
var moduleName = 'virtoCommerce.DemoSolutionFeaturesModule';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['$rootScope', '$window', 'virtoCommerce.catalogModule.itemTypesResolverService', 'platformWebApp.widgetService', 'virtoCommerce.demoFeatures.featureManager',
        function ($rootScope, $window, itemTypesResolverService, widgetService, featureManager) {
            $rootScope.$on('loginStatusChanged', (_, authContext) => {
                if (!authContext.isAuthenticated) {
                    $window.location.reload();
                }
            });

            featureManager.subscribeToLoginAction('ConfigurableProduct', () => {
                const configurableProductType = 'Configurable';
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
