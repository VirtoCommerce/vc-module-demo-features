using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services
{
    public interface IDemoUserNameResolver : IUserNameResolver
    {
        void SetCurrentUserName(string userName);
    }
}
