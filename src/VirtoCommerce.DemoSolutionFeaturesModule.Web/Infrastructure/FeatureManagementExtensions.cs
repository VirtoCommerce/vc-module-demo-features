using System;
using Microsoft.FeatureManagement;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.Infrastructure
{
    public static class FeatureManagementExtensions
    {

        public static void TryAddFeature(this IFeaturesStorage featuresStorage, string featureName, string enabledFor)
        {
            featuresStorage.TryAddFeatureDefinition(new FeatureDefinition
            {
                Name = featureName,
                EnabledFor = new[] { new FeatureFilterConfiguration
                {
                    Name = enabledFor,
                } }
            });
        }

        public static void TryAddFeature(this IFeaturesStorage featuresStorage, string featureName, bool enabled)
        {
            featuresStorage.TryAddFeatureDefinition(new FeatureDefinition
            {
                Name = featureName,
                EnabledFor = enabled ?
                    new[] { new FeatureFilterConfiguration { Name = "AlwaysOn", } } :
                    Array.Empty<FeatureFilterConfiguration>()
            });
        }
    }
}
