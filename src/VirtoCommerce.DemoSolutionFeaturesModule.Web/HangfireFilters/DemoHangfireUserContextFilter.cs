using Hangfire.Server;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.HangfireFilters
{
    public class DemoHangfireUserContextFilter : IServerFilter
    {
        private readonly IDemoUserNameResolver _demoUserNameResolver;

        public DemoHangfireUserContextFilter(IDemoUserNameResolver demoUserNameResolver)
        {
            _demoUserNameResolver = demoUserNameResolver;
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            var userName = filterContext.GetJobParameter<string>("UserName");

            _demoUserNameResolver.SetCurrentUserName(userName);
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            // Pass
        }
    }
}
