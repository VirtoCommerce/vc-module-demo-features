using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.FeatureManagement;
using VirtoCommerce.Platform.Core;
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
        private readonly Func<IUserClaimsPrincipalFactory<ApplicationUser>> _userClaimsPrincipalFactoryFactory;

        public DevelopersFilter(
            IHttpContextAccessor httpContextAccessor,
            Func<IUserNameResolver> userNameResolverFactory,
            Func<UserManager<ApplicationUser>> userManagerFactory,
            Func<IUserClaimsPrincipalFactory<ApplicationUser>> userClaimsPrincipalFactoryFactory
            )
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _userNameResolverFactory = userNameResolverFactory;
            _userManagerFactory = userManagerFactory;
            _userClaimsPrincipalFactoryFactory = userClaimsPrincipalFactoryFactory;
        }

        public async Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            bool result;

            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.HasClaim(
                    PlatformConstants.Security.Claims.PermissionClaimType,
                    Demo.ModuleConstants.Security.Permissions.Developer
                    );
            }
            else
            {
                var userNameResolver = _userNameResolverFactory();
                using var userManager = _userManagerFactory();
                var userClaimsPrincipalFactory = _userClaimsPrincipalFactoryFactory();

                var currentUserName = userNameResolver.GetCurrentUserName();

                var currentUser = await userManager.FindByNameAsync(currentUserName);
                var claimsPrincipal = await userClaimsPrincipalFactory.CreateAsync(currentUser);

                result = claimsPrincipal.HasClaim(
                    PlatformConstants.Security.Claims.PermissionClaimType,
                    Demo.ModuleConstants.Security.Permissions.Developer
                    );
            }

            return result;
        }
    }
}
