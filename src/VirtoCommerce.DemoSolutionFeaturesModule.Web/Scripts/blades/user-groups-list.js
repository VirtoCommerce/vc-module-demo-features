angular.module('virtoCommerce.DemoSolutionFeaturesModule')
    .controller('virtoCommerce.DemoSolutionFeaturesModule.userGroupsListController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.DemoSolutionFeaturesModule.userGroupsApi',
        function ($scope, bladeNavigationService, userGroupsApi) {
            var blade = $scope.blade;
            blade.currentEntity = undefined;

            userGroupsApi.taggedMember({ id: blade.memberId },
                function(result) {
                    blade.currentEntity = result || {};
                    blade.currentEntity.tags = blade.currentEntity.tags || [];
                    blade.isLoading = false;
            });

            blade.toolbarCommands = [
                {
                    name: "platform.commands.add",
                    icon: 'fa fa-plus',
                    executeMethod: () => {
                        bladeNavigationService.closeChildrenBlades(blade, () => {
                            var newBlade = {
                                id: 'assignUserGroup',
                                title: 'demoSolutionFeaturesModule.blades.assign-user-group.title',
                                subtitle: 'demoSolutionFeaturesModule.blades.assign-user-group.subtitle',
                                controller: 'virtoCommerce.DemoSolutionFeaturesModule.assignUserGroupController',
                                template: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Scripts/blades/assign-user-group.tpl.html',
                                onGroupsAdded: (entity) => {
                                    const newTags = _.difference(entity.tags, blade.currentEntity.tags);
                                    blade.currentEntity.tags = [...blade.currentEntity.tags, ...newTags];
                                    bladeNavigationService.closeChildrenBlades(blade);
                                    saveChanges();
                                }
                            };
                            bladeNavigationService.showBlade(newBlade, blade);
                        });
                    },
                    canExecuteMethod: () => true
                },
                {
                    name: "platform.commands.delete",
                    icon: 'fas fa-trash-alt',
                    executeMethod: deleteChecked,
                    canExecuteMethod: isItemsChecked
                }
            ];

            $scope.showAllGroups = (groupType) => {
                const newBlade = {
                    id: 'allGroups',
                    type: groupType,
                    memberId: blade.memberId,
                    currentEntity: blade.currentEntity,
                    onBackButtonClick: () => {
                        bladeNavigationService.showBlade(blade, blade.parentBlade);
                    },
                    groupsChanged: () => blade.onGroupsChanged(),
                    controller: 'virtoCommerce.DemoSolutionFeaturesModule.allUserGroupsListController',
                    template: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Scripts/blades/all-user-groups-list.tpl.html',

                }
                bladeNavigationService.closeBlade(blade, () => {
                    bladeNavigationService.showBlade(newBlade, blade.parentBlade);
                });
            }

            function isItemsChecked() {
                return _.any(blade.assignedTags, tag => tag.$selected);
            }

            function saveChanges() {
                blade.isLoading = true;
                blade.currentEntity.id = blade.memberId;

                userGroupsApi.save(blade.currentEntity,
                     (result) => {
                        blade.currentEntity = result;
                        blade.onGroupsChanged();
                        blade.isLoading = false;
                    }
                );
            }

            function deleteChecked() {
                _.each(blade.assignedTags.slice(), tag => {
                    if (tag.$selected) {
                        blade.currentEntity.tags.splice(blade.currentEntity.tags.indexOf(tag.value), 1);
                    }
                });
                saveChanges();
            }

            $scope.$watchCollection('blade.currentEntity.tags', tags => {
                if (!tags) {
                    blade.assignedTags = [];
                }
                else {
                    blade.assignedTags = _.map(tags, tag => { return { value: tag }; });
                }
            });
        }]);