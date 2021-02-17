using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Events.Customer;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Handlers
{
    public class LogChangesTaggedMembersHandler : IEventHandler<DemoTaggedMemberChangedEvent>
    {
        private readonly IChangeLogService _changeLogService;

        public LogChangesTaggedMembersHandler(IChangeLogService changeLogService)
        {
            _changeLogService = changeLogService;
        }

        public Task Handle(DemoTaggedMemberChangedEvent message)
        {
            var logOperations = message.ChangedEntries.Select(x => AbstractTypeFactory<OperationLog>.TryCreateInstance().FromChangedEntry(x))
                .ToArray();

            BackgroundJob.Enqueue(() => LogEntityChangesInBackground(logOperations));

            return Task.CompletedTask;
        }

        [DisableConcurrentExecution(10)]
        public void LogEntityChangesInBackground(OperationLog[] operationLogs)
        {
            _changeLogService.SaveChangesAsync(operationLogs).GetAwaiter().GetResult();
        }
    }
}
