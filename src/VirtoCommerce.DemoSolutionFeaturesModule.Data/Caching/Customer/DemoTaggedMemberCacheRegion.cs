using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Caching.Customer
{
    public class DemoTaggedMemberCacheRegion : CancellableCacheRegion<DemoTaggedMemberCacheRegion>
    {
        public static IChangeToken CreateChangeToken(string[] ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            var changeTokens = new List<IChangeToken> { CreateChangeToken() };

            changeTokens.AddRange(
                ids
                    .Select(CreateChangeTokenForKey)
            );

            return new CompositeChangeToken(changeTokens);
        }

        public static IChangeToken CreateChangeToken(DemoTaggedMember[] taggedMember)
        {
            if (taggedMember == null)
            {
                throw new ArgumentNullException(nameof(taggedMember));
            }

            return CreateChangeToken(taggedMember.Select(x => x.Id).ToArray());
        }

        public static void ExpireEntity(DemoTaggedMember taggedMember)
        {
            if (taggedMember == null)
            {
                throw new ArgumentNullException(nameof(taggedMember));
            }

            ExpireTokenForKey(taggedMember.Id);
        }
    }
}
