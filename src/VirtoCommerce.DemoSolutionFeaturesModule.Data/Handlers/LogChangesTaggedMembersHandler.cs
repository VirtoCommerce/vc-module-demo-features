using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Events.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Handlers
{
    public class LogChangesTaggedMembersHandler : IEventHandler<DemoTaggedMemberChangedEvent>
    {
        private readonly IChangeLogService _changeLogService;
        private readonly IDemoMemberInheritanceEvaluator _memberInheritanceEvaluator;

        public LogChangesTaggedMembersHandler(IChangeLogService changeLogService, IDemoMemberInheritanceEvaluator memberInheritanceEvaluator)
        {
            _changeLogService = changeLogService;
            _memberInheritanceEvaluator = memberInheritanceEvaluator;
        }

        public async Task Handle(DemoTaggedMemberChangedEvent message)
        {
            var logOperations = new List<OperationLog>();

            foreach (var changedEntry in message.ChangedEntries)
            {
                var logOperation = AbstractTypeFactory<OperationLog>.TryCreateInstance().FromChangedEntry(changedEntry);
                logOperations.Add(logOperation);

                var descendantIds = await _memberInheritanceEvaluator.GetAllDescendantIdsForMemberAsync(logOperation.ObjectId);

                if (!descendantIds.IsNullOrEmpty())
                {
                    foreach (var descendantId in descendantIds)
                    {
                        var descendantLogOperation = (OperationLog)logOperation.Clone();
                        descendantLogOperation.ObjectId = descendantId;
                        descendantLogOperation.OperationType = EntryState.Modified;
                        logOperations.Add(descendantLogOperation);
                    }

                }
            }

            BackgroundJob.Enqueue(() => LogEntityChangesInBackground(logOperations.ToArray()));
        }

        [DisableConcurrentExecution(10)]
        public void LogEntityChangesInBackground(OperationLog[] operationLogs)
        {
            _changeLogService.SaveChangesAsync(operationLogs).GetAwaiter().GetResult();
        }
    }
}
