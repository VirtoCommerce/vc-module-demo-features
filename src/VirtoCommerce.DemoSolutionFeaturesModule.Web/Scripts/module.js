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
    .run(['platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state',
        function (mainMenuService, widgetService, $state) {
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
        }
    ]);
