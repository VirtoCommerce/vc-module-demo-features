// Call this to register your module to main application
var moduleName = "virtoCommerce.DemoSolutionFeaturesModule";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, []);
