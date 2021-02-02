using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Caching.Catalog
{
    public class DemoProductPartCacheRegion : CancellableCacheRegion<DemoProductPartCacheRegion>
    {
        public static IChangeToken CreateChangeToken(string[] customerSegmentIds)
        {
            if (customerSegmentIds == null)
            {
                throw new ArgumentNullException(nameof(customerSegmentIds));
            }

            var changeTokens = new List<IChangeToken> { CreateChangeToken() };

            changeTokens.AddRange(
                customerSegmentIds
                    .Select(CreateChangeTokenForKey)
            );

            return new CompositeChangeToken(changeTokens);
        }

        public static IChangeToken CreateChangeToken(DemoProductPart[] productPart)
        {
            if (productPart == null)
            {
                throw new ArgumentNullException(nameof(productPart));
            }

            return CreateChangeToken(productPart.Select(x => x.Id).ToArray());
        }

        public static void ExpireEntity(DemoProductPart productPart)
        {
            if (productPart == null)
            {
                throw new ArgumentNullException(nameof(productPart));
            }

            ExpireTokenForKey(productPart.Id);
        }
    }
}
