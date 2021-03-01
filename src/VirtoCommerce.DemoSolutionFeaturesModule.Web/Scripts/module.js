// Call this to register your module to main application
var moduleName = 'virtoCommerce.DemoSolutionFeaturesModule';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['virtoCommerce.catalogModule.itemTypesResolverService', 'platformWebApp.widgetService', 'virtoCommerce.demoFeatures.featureManagerSubscriber',
        function (itemTypesResolverService, widgetService, featureManagerSubscriber) {
            const configurableProductType = 'Configurable';
            featureManagerSubscriber.onLoginStatusChanged('ConfigurableProduct', () => {
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

            featureManagerSubscriber.onLoginStatusChanged('UserGroupsInheritance', () => {
                widgetService.registerWidget({
                    isVisible: function (blade) { return blade.currentEntityId; },
                    controller: 'virtoCommerce.DemoSolutionFeaturesModule.userGroupsWidgetController',
                    template: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Scripts/widgets/userGroupsWidget.tpl.html',
                    size: [2,1]
                }, 'customerDetail2');

                widgetService.registerWidget({
                    isVisible: function (blade) { return blade.currentEntityId; },
                    controller: 'virtoCommerce.DemoSolutionFeaturesModule.userGroupsWidgetController',
                    template: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Scripts/widgets/userGroupsWidget.tpl.html',
                    size: [2,1]
                }, 'organizationDetail2');
            });

            const itemDetailWidgets = widgetService.widgetsMap['itemDetail'];
            const variationWidget = itemDetailWidgets.find(widget => widget.controller == 'virtoCommerce.catalogModule.itemVariationWidgetController');
            variationWidget.isVisible = blade => blade.id !== 'variationDetail' && blade.productType !== configurableProductType;
    }]);
