var mockData = [
    {
        id: 'guid1',
        imgSrc: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Content/Mock Data/case.svg',
        name: 'Case',
        priority: 1
    },
    {
        id: 'guid2',
        imgSrc: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Content/Mock Data/motherboard.svg',
        name: 'Motherboard',
        priority: 2
    },
    {
        id: 'guid3',
        imgSrc: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Content/Mock Data/processor.svg',
        name: 'Processor',
        priority: 3
    },
    {
        id: 'guid4',
        imgSrc: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Content/Mock Data/memory.svg',
        name: 'Memory',
        priority: 4
    },
    {
        id: 'guid5',
        imgSrc: 'Modules/$(VirtoCommerce.DemoSolutionFeaturesModule)/Content/Mock Data/graphics.svg',
        name: 'Graphics',
        priority: 5
    }
];

angular.module('virtoCommerce.DemoSolutionFeaturesModule')
.factory('virtoCommerce.DemoSolutionFeaturesModule.productPartsApi', ['$resource', '$q', '$timeout', function ($resource, $q, $timeout) {
    // Return mock data temporary
    return {
        search: (searchCriteria, callback) => {
            $timeout(
                callback({
                    totalCount: 21,
                    results: mockData
                }),
                1000);
        },
        update: (data, callback) => {
            $timeout(
                () => {
                    mockData = data;
                    callback(data);
                },
                1000);
        }
    }
    //return $resource('api/demo/catalog/product/:id/parts', {}, {
    //    update: { method: 'POST', isArray: true },
    //    delete: { method: 'DELETE' },
    //    search: { method: 'POST', url: 'api/demo/catalog/product/:id/parts/search' }
    //});
}]);
