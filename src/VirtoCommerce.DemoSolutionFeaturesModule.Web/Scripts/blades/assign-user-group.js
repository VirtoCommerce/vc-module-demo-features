angular.module('virtoCommerce.DemoSolutionFeaturesModule')
    .controller('virtoCommerce.DemoSolutionFeaturesModule.assignUserGroupController', ['$scope', 'platformWebApp.bladeNavigationService', 'platformWebApp.settings',
        function ($scope, bladeNavigationService, settings) {
            var blade = $scope.blade;
            blade.tagsDictionary = [];
            blade.origEntity = undefined;
            blade.currentEntity = undefined;

            const settingKey = 'Customer.MemberGroups';

            settings.getValues({ id: settingKey }, tagsDictionary => {
                blade.tagsDictionary = _.difference(tagsDictionary, blade.currentTags);
                blade.currentEntity = {};
                blade.currentEntity.tags = blade.currentEntity.tags || [];
                blade.origEntity = angular.copy(blade.currentEntity);
                blade.isLoading = false;
            });

            blade.toolbarCommands = [
                {
                    name: "platform.commands.save",
                    icon: 'fas fa-save',
                    executeMethod: saveChanges,
                    canExecuteMethod: canSave
                },
                {
                    name: "platform.commands.delete",
                    icon: 'fas fa-trash-alt',
                    executeMethod: deleteChecked,
                    canExecuteMethod: isItemsChecked
                }
            ];

            $scope.editTagsDictionary = () => {
                const editTagsDictionaryBlade = {
                    id: "settingDetailChild",
                    currentEntityId: settingKey,
                    isApiSave: true,
                    controller: 'platformWebApp.settingDictionaryController',
                    template: '$(Platform)/Scripts/app/settings/blades/setting-dictionary.tpl.html',
                    onClose: (doCloseBlade) => {
                        doCloseBlade();
                        blade.isLoading = true;
                        settings.getValues({ id: settingKey }, tagsDictionary => {
                            blade.tagsDictionary = _.difference(tagsDictionary, blade.currentTags);
                            blade.availableTags = _.filter(blade.tagsDictionary, tag => {
                                return _.all(blade.currentEntity.tags, curr => curr !== tag);
                            });
                            blade.isLoading = false;
                        });
                    }
                };
                bladeNavigationService.showBlade(editTagsDictionaryBlade, blade);
            }

            $scope.assignTag = (selectedTag) => {
                blade.currentEntity.tags.push(selectedTag);
                blade.selectedTag = undefined;
            }

            function saveChanges() {
                blade.onGroupsAdded(blade.currentEntity);
            }

            function canSave() {
                return isDirty() && blade.hasUpdatePermission();
            }

            function isDirty() {
                return !angular.equals(blade.currentEntity, blade.origEntity);
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
            }

            $scope.$watchCollection('blade.currentEntity.tags', tags => {
                if (!tags) {
                    blade.assignedTags = [];
                    blade.availableTags = blade.tagsDictionary;
                }
                else {
                    blade.assignedTags = _.map(tags, tag => { return { value: tag }; });
                    blade.availableTags = _.filter(blade.tagsDictionary, tag => {
                        return _.all(tags, curr => curr !== tag);
                    });
                }
            });
        }]);
