using System;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CustomerModule.Core.Events;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Caching.Customer;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.CustomerModule.Data.Handlers
{
    public class ClearTaggedMemberCacheAtMemberChangedHandler : IEventHandler<MemberChangedEvent>
    {
        public Task Handle(MemberChangedEvent message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var memberIds = message.ChangedEntries.Select(x => x.OldEntry.Id).ToArray();

            foreach (var memberId in memberIds)
            {
                DemoTaggedMemberCacheRegion.ExpireEntity(memberId);
            }

            return Task.CompletedTask;
        }
    }
}
