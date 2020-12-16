using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.Infrastructure
{
    public interface IFeaturesStorage
    {
        void TryAddFeatureDefinition(FeatureDefinition featureDefinition);

        /// <summary>
        /// Internal method. Should not be called
        /// </summary>
        /// <param name="configuration"></param>
        void AddHighPriorityFeatureDefinition(IConfigurationSection configuration);
    }
}
