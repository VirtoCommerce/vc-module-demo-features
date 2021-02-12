angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.controller('virtoCommerce.DemoSolutionFeaturesModule.userGroupsWidgetController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.DemoSolutionFeaturesModule.userGroupsApi',
    function($scope, bladeNavigationService, userGroupsApi) {
        var blade = $scope.blade;
        $scope.assignedGroupsCount = 0;
        $scope.inheritedGroupsCount = 0;

        function recalculateGroupsCount() {
            userGroupsApi.taggedMember({ id: blade.currentEntityId },
                function (result) {
                    $scope.loading = false;
                    $scope.assignedGroupsCount = result.tags ? result.tags.length : 0;
                    $scope.inheritedGroupsCount = result.inheritedTags ? result.inheritedTags.length : 0;
                });
        }

        $scope.openBlade = function () {
            var newBlade = {
                id: "userGroupsList",
                memberId: blade.currentEntityId,
                title: 'demoSolutionFeaturesModule.blades.user-groups-list.title',
                subtitle: 'demoSolutionFeaturesModule.blades.user-groups-list.subtitle',
                controller: 'virtoCommerce.DemoSolutionFeaturesModule.userGroupsListController',
                template: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Scripts/blades/user-groups-list.tpl.html',
                onGroupsChanged: () => recalculateGroupsCount()
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };

        recalculateGroupsCount();
    }]);
