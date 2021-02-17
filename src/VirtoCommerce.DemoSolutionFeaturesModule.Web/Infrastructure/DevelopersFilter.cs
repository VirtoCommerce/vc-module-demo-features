using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using Demo = VirtoCommerce.DemoSolutionFeaturesModule.Core;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.Infrastructure
{
    [FilterAlias("Developers")]
    public class DevelopersFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Func<IUserNameResolver> _userNameResolverFactory;
        private readonly Func<UserManager<ApplicationUser>> _userManagerFactory;
        private readonly MvcNewtonsoftJsonOptions _jsonOptions;

        public DevelopersFilter(
            IHttpContextAccessor httpContextAccessor,
            Func<IUserNameResolver> userNameResolverFactory,
            Func<UserManager<ApplicationUser>> userManagerFactory,
            IOptions<MvcNewtonsoftJsonOptions> jsonOptions
            )
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _userNameResolverFactory = userNameResolverFactory;
            _userManagerFactory = userManagerFactory;
            _jsonOptions = jsonOptions.Value;
        }

        public async Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            bool result;

            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.HasClaim(
                    PlatformConstants.Security.Claims.PermissionClaimType,
                    Demo.ModuleConstants.Security.Permissions.Developer);
            }
            else
            {
                var userNameResolver = _userNameResolverFactory();

                var currentUserName = userNameResolver.GetCurrentUserName();

                if (!currentUserName.EqualsInvariant("unknown"))
                {
                    using var userManager = _userManagerFactory();

                    var currentUser = await userManager.FindByNameAsync(currentUserName);

                    result = currentUser
                        .Roles
                        .SelectMany(x => x.Permissions.Select(p => p.ToClaim(_jsonOptions.SerializerSettings)))
                        .Any(x =>
                            x.Type.EqualsInvariant(PlatformConstants.Security.Claims.PermissionClaimType) &&
                            x.Value.EqualsInvariant(Demo.ModuleConstants.Security.Permissions.Developer));
                }
                else
                {
                    // Always true for cron scheduled jobs
                    result = true;
                }
            }

            return result;
        }
    }
}
