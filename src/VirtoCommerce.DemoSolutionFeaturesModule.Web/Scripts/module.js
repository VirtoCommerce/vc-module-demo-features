// Call this to register your module to main application
var moduleName = "virtoCommerce.demoSolutionFeaturesModule";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            $stateProvider
                .state('workspace.virtoCommerceDemoSolutionFeaturesModuleState', {
                    url: '/virtoCommerce.demoSolutionFeaturesModule',
                    templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
                    controller: [
                        '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                            var newBlade = {
                                id: 'blade1',
                                controller: 'virtoCommerce.demoSolutionFeaturesModule.helloWorldController',
                                template: 'Modules/$(virtoCommerce.demoSolutionFeaturesModule)/Scripts/blades/hello-world.html',
                                isClosingDisabled: true
                            };
                            bladeNavigationService.showBlade(newBlade);
                        }
                    ]
                });
        }
    ])
    .run(['platformWebApp.mainMenuService', 'virtoCommerce.coreModule.common.dynamicExpressionService', '$http', '$state', '$compile',
        function (mainMenuService, dynamicExpressionService, $http, $state, $compile) {
            //Register module in main menu
            var menuItem = {
                path: 'browse/virtoCommerce.demoSolutionFeaturesModule',
                icon: 'fa fa-cube',
                title: 'VirtoCommerce.DemoSolutionFeaturesModule',
                priority: 100,
                action: function () { $state.go('workspace.virtoCommerceDemoSolutionFeaturesModuleState'); },
                permission: 'virtoCommerceDemoSolutionFeaturesModule:access'
            };
            mainMenuService.addMenuItem(menuItem);

            //Register Customer Segments expressions
            dynamicExpressionService.registerExpression({
                id: 'DemoBlockCustomerSegmentCondition',
                newChildLabel: '+ add segment',
                getValidationError: function () {
                    return (this.children && this.children.length) ? undefined : 'Promotion requires at least one eligibility';
                }
            });
            dynamicExpressionService.registerExpression({
                id: 'DemoCustomerSegmentCondition',
                displayName: 'Segment is []'
            });
            $http.get('Modules/$(virtoCommerce.demoSolutionFeaturesModule)/Scripts/dynamicConditions/all-templates.html').then(function (response) {
                // compile the response, which will put stuff into the cache
                $compile(response.data);
            });
        }
    ]);
