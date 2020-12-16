using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.FeatureManagement;
using VirtoCommerce.Platform.Core;
using Demo = VirtoCommerce.DemoSolutionFeaturesModule.Core;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.Infrastructure
{
    [FilterAlias("Developers")]
    public class DevelopersFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DevelopersFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var result =
                _httpContextAccessor?.HttpContext?.User.HasClaim(
                    PlatformConstants.Security.Claims.PermissionClaimType,
                    Demo.ModuleConstants.Security.Permissions.Developer) ??
                false;

            return Task.FromResult(result);
        }
    }
}
