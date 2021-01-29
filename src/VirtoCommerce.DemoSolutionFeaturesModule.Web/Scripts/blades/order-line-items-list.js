angular.module('virtoCommerce.DemoSolutionFeaturesModule')
    .controller('virtoCommerce.DemoSolutionFeaturesModule.orderLineItemsListController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.orderModule.catalogItems', 'virtoCommerce.pricingModule.prices', '$translate', 'platformWebApp.authService', function ($scope, bladeNavigationService, items, prices, $translate, authService) {
    let blade = $scope.blade;
    blade.updatePermission = 'order:update';
    blade.isVisiblePrices = authService.checkPermission('order:read_prices');
    $scope.showConfiguration = {};
    $scope.checkboxes = {};
    $scope.checkboxesCongurationItems = {};

    let selectedProducts = [];

    $translate('orders.blades.customerOrder-detail.title', { customer: blade.currentEntity.customerName }).then(result => {
        blade.title = 'orders.widgets.customerOrder-items.blade-title';
        blade.titleValues = { title: result };
        blade.subtitle = 'orders.widgets.customerOrder-items.blade-subtitle';
    });

    blade.toolbarCommands = [
        {
            name: "orders.commands.add-item", icon: 'fa fa-plus',
            executeMethod: () => {
                openAddEntityWizard();
            },
            canExecuteMethod: () => blade.currentEntity.operationType === 'CustomerOrder',
            permission: blade.updatePermission
        },
        {
            name: "platform.commands.remove", icon: 'fa fa-trash-o',
            executeMethod: () => {
                removeItems();
            },
            canExecuteMethod: () => _.any($scope.checkboxes, x => x) || isConfiguredLineItemSelected(),
            permission: blade.updatePermission
        }
    ];

    blade.refresh = function () {
        angular.forEach(blade.currentEntity.configuredGroups, product => {
            $scope.showConfiguration[product.id] = false;
        });
        $scope.configurationGroupIds = _.pluck(blade.currentEntity.configuredGroups, "id");
        blade.isLoading = false;
        blade.selectedAll = false;
    };

    $scope.openItemDynamicProperties = (item) => {
        const newBlade = {
            id: "dynamicPropertiesList",
            currentEntity: item,
            controller: 'platformWebApp.propertyValueListController',
            template: '$(Platform)/Scripts/app/dynamicProperties/blades/propertyValue-list.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.openItemDetail = (item) => {
        const newBlade = {
            id: "listItemDetail",
            itemId: item.productId,
            title: item.name,
            controller: 'virtoCommerce.catalogModule.itemDetailController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/item-detail.tpl.html'
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    };

    $scope.updateItemsInfo = (item) => {
        let index;

        if (item.hasOwnProperty('configuredGroupId')) {
            index = _.findIndex(blade.currentEntity.items, lineItem => lineItem.configuredGroupId === item.configuredGroupId && lineItem.id === item.id);
        } else {
            index = _.findIndex(blade.currentEntity.items, lineItem => lineItem.id === item.id);
        }

        blade.currentEntity.items[index] = item;
        blade.recalculateFn();
    };

    $scope.checkAll = (selected) => {
        angular.forEach(blade.currentEntity.usualItems, item => {
            $scope.checkboxes[item.id] = selected;
        });
        angular.forEach(blade.currentEntity.configuredGroups, item => {
            $scope.checkboxes[item.id] = selected;
        });
    };

    $scope.toggleConfiguration = (id) => {
        event.stopPropagation();
        $scope.showConfiguration[id] = !$scope.showConfiguration[id];
    };

    function removeConfiguration(configId) {
        blade.currentEntity.configuredGroups = _.filter(blade.currentEntity.configuredGroups, group => group.id !== configId);
        blade.currentEntity.items = _.filter(blade.currentEntity.items, item => item.configuredGroupId !== configId);
    }

    function filterConfigurationLineItemsCheckboxes() {
        const pairs = _.pairs($scope.checkboxesCongurationItems);
        angular.forEach(pairs, pair => {
            pair[1] = (_.filter(_.keys(pair[1]), key => pair[1][key]));
        });
        return pairs;
    }

    function isConfiguredLineItemSelected() {
        const pairs = filterConfigurationLineItemsCheckboxes();
        let selectedLineItems = [];
        angular.forEach(pairs, pair => {
            const configurationLineItems = pair[1];
            selectedLineItems.push(...configurationLineItems);
        });
        return selectedLineItems.length;
    }

    function removeItems() {
        const selectedIds = _.filter(_.keys($scope.checkboxes), key => $scope.checkboxes[key]);
        const configurationsToRemove = _.intersection(selectedIds, $scope.configurationGroupIds);
        const lineItemsToRemove = _.difference(selectedIds, configurationsToRemove);

        if (configurationsToRemove.length) {
            angular.forEach(configurationsToRemove, configId => {
                removeConfiguration(configId);
            });
        }

        const pairs = filterConfigurationLineItemsCheckboxes();
        angular.forEach(pairs, (pair) => {
            const [configId, configurationLineItems] = pair;
            const currentEntityGroup = _.findWhere(blade.currentEntity.configuredGroups, {id: configId});
            angular.forEach(configurationLineItems, lineItemId => {
                blade.currentEntity.items = _.filter(blade.currentEntity.items, item => item.id !== lineItemId || item.configuredGroupId !== configId);
                currentEntityGroup.items = _.filter(currentEntityGroup.items, item => item.id !== lineItemId);
            });
            if (!currentEntityGroup.items.length) {
                blade.currentEntity.configuredGroups = _.filter(blade.currentEntity.configuredGroups, group => group.id !== configId);
            }
        });

        angular.forEach(lineItemsToRemove, lineItemId => {
            blade.currentEntity.items = _.filter(blade.currentEntity.items, item => item.id !== lineItemId);
        });

        $scope.checkboxes = {};
        $scope.checkboxesCongurationItems = {};

        if (!blade.currentEntity.items.length) {
            blade.selectedAll = false;
        }

        blade.recalculateFn();
    }

    function addProductsToOrder(products) {
        angular.forEach(products, product => {
            items.get({ id: product.id }, data => {
                prices.getProductPrices({ id: product.id }, productPrices => {
                    const price = _.find(productPrices, x => { return x.currency === blade.currentEntity.currency });
                    const newLineItem = {
                        productId: data.id,
                        id: data.id,
                        catalogId: data.catalogId,
                        categoryId: data.categoryId,
                        name: data.name,
                        imageUrl: data.imgSrc,
                        sku: data.code,
                        quantity: 1,
                        price: price && price.list ? price.list : 0,
                        discountAmount: price && price.list && price.sale ? price.list - price.sale : 0,
                        currency: blade.currentEntity.currency
					};
                    blade.currentEntity.items.push(newLineItem);
                    blade.recalculateFn();
                    blade.refresh();
                });
            });
        });
    }

    function openAddEntityWizard() {
        const options = {
            checkItemFn: (listItem, isSelected) => {
                if (isSelected) {
                    if (_.all(selectedProducts, x => x.id !== listItem.id)) {
                        selectedProducts.push(listItem);
                    }
                }
                else {
                    selectedProducts = _.reject(selectedProducts, x => x.id === listItem.id);
                }
            }
        };
        const newBlade = {
            id: "CatalogItemsSelect",
            currentEntities: blade.currentEntity,
            title: "orders.blades.catalog-items-select.title",
            controller: 'virtoCommerce.catalogModule.catalogItemSelectController',
            template: 'Modules/$(VirtoCommerce.Catalog)/Scripts/blades/common/catalog-items-select.tpl.html',
            options: options,
            breadcrumbs: [],
            toolbarCommands: [
              {
                  name: "orders.commands.add-selected", icon: 'fa fa-plus',
                  executeMethod: (pickingBlade) => {
                      addProductsToOrder(selectedProducts);
                      selectedProducts.length = 0;
                      bladeNavigationService.closeBlade(pickingBlade);
                  },
                  canExecuteMethod: () => selectedProducts.length > 0
              }]
        };
        bladeNavigationService.showBlade(newBlade, $scope.blade);
    }

    blade.refresh();
}]);
