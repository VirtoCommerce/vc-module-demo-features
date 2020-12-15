using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.Infrastructure
{
    public class DemoFeatureDefinitionProvider : IFeatureDefinitionProvider, IFeaturesStorage
    {
        private readonly ICollection<FeatureDefinition> _featureDefinitions;

        public DemoFeatureDefinitionProvider()
        {
            _featureDefinitions = new List<FeatureDefinition>();
        }

        public async Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName)
        {
            using (await AsyncLock.GetLockByKey(nameof(GetFeatureDefinitionAsync)).LockAsync())
            {
                var result = _featureDefinitions.FirstOrDefault(x => x.Name.EqualsInvariant(featureName));

                return result;
            }
        }

        public IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync()
        {
            return _featureDefinitions.ToAsyncEnumerable();
        }

        public void TryAddFeatureDefinition(FeatureDefinition featureDefinition)
        {
            if (featureDefinition == null)
            {
                throw new ArgumentNullException(nameof(featureDefinition));
            }

            if (_featureDefinitions.Any(x => x.Name.EqualsInvariant(featureDefinition.Name)))
            {
                return;
            }

            _featureDefinitions.Add(featureDefinition);
        }

        public void AddHighPriorityFeatureDefinition(IConfigurationSection configuration)
        {
            var featureConfigurations = configuration.GetChildren().ToList();

            foreach (var featureConfiguration in featureConfigurations)
            {
                var featureDefinition = new FeatureDefinition
                {
                    Name = featureConfiguration.Key,
                };
                if (!string.IsNullOrEmpty(featureConfiguration.Value) &&
                    bool.TryParse(featureConfiguration.Value, out var isEnabled))
                {
                    featureDefinition.EnabledFor = isEnabled ?
                        new[] { new FeatureFilterConfiguration { Name = "AlwaysOn", } } :
                        Array.Empty<FeatureFilterConfiguration>();
                }
                else
                {
                    featureDefinition.EnabledFor = new[] { new FeatureFilterConfiguration { Name = featureConfiguration.Value, } };
                }

                // Need to remove defined features from collection
                if (_featureDefinitions.Any(x => x.Name.EqualsInvariant(featureDefinition.Name)))
                {
                    var definedFeature = _featureDefinitions.First(x => x.Name.EqualsInvariant(featureDefinition.Name));

                    _featureDefinitions.Remove(definedFeature);
                }

                _featureDefinitions.Add(featureDefinition);
            }
        }
    }
}
