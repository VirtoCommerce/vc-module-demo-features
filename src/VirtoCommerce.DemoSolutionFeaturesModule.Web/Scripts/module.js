// Call this to register your module to main application
var moduleName = "virtoCommerce.DemoSolutionFeaturesModule";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .run(['virtoCommerce.marketingModule.marketingMenuItemService',
        function (marketingMenuItemService) {
            marketingMenuItemService.register({
                id: 'customerSegments',
                name: 'Customer segments',
                entityName: 'customerSegment',
                icon: 'fa fa-pie-chart',
                controller: 'virtoCommerce.DemoSolutionFeaturesModule.customerSegmentDetailController',
                template: 'Modules/$(virtoCommerce.DemoSolutionFeaturesModule)/Scripts/blades/customerSegment-detail.tpl.html'
        });
        }
    ]);
