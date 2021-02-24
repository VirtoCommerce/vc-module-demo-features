angular.module('virtoCommerce.DemoSolutionFeaturesModule')
    .controller('virtoCommerce.DemoSolutionFeaturesModule.allUserGroupsListController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.DemoSolutionFeaturesModule.userGroupsApi',
        function ($scope, bladeNavigationService, userGroupsApi) {
            var blade = $scope.blade;
            blade.title = blade.type === 'assigned' ? 'demoSolutionFeaturesModule.blades.all-user-groups-list.title.assigned' : 'demoSolutionFeaturesModule.blades.all-user-groups-list.title.inherited';
            blade.isLoading = false;

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
                                currentTags: blade.currentEntity.tags,
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
                    canExecuteMethod: () => true,
                    permission: 'customer:update'
                },
                {
                    name: "platform.commands.delete",
                    icon: 'fas fa-trash-alt',
                    executeMethod: deleteChecked,
                    canExecuteMethod: isItemsChecked,
                    permission: 'customer:delete'
                }
            ];

            $scope.clearKeyword = () => {
                blade.searchKeyword = '';
            }

            $scope.navigateBack = () => {
                bladeNavigationService.closeBlade(blade, () => {
                    blade.onBackButtonClick();
                });
            }

            function isItemsChecked() {
                return _.any(blade.assignedTags, tag => tag.$selected);
            }

            function deleteChecked() {
                _.each(blade.assignedTags.slice(), tag => {
                    if (tag.$selected) {
                        blade.currentEntity.tags.splice(blade.currentEntity.tags.indexOf(tag.value), 1);
                    }
                });
                saveChanges();
            }

            function saveChanges() {
                blade.isLoading = true;
                blade.currentEntity.id = blade.memberId;

                userGroupsApi.save(blade.currentEntity,
                     (result) => {
                        blade.currentEntity = result;
                        blade.groupsChanged();
                        blade.isLoading = false;
                    }
                );
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
