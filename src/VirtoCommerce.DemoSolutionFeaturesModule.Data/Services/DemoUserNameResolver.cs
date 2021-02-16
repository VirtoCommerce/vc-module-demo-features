using System;
using System.Threading;
using Microsoft.AspNetCore.Http;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services
{
    public class DemoUserNameResolver : IDemoUserNameResolver
    {
        private static readonly AsyncLocal<string> _currentUserName = new AsyncLocal<string>();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DemoUserNameResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string GetCurrentUserName()
        {
            var result = _currentUserName.Value ?? "unknown";

            var context = _httpContextAccessor.HttpContext;

            if (context?.Request != null && context.User != null)
            {
                var identity = context.User.Identity;

                if (identity != null && identity.IsAuthenticated)
                {
                    result = context.Request.Headers["VirtoCommerce-User-Name"];

                    if (string.IsNullOrEmpty(result))
                    {
                        result = identity.Name;
                    }
                }
            }

            return result;
        }

        public void SetCurrentUserName(string userName)
        {
            _currentUserName.Value = userName;
        }
    }
}
