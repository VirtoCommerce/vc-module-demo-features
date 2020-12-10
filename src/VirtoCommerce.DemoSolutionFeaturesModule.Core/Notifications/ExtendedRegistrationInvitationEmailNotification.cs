using VirtoCommerce.NotificationsModule.Core.Types;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Notifications
{
    public class ExtendedRegistrationInvitationEmailNotification : RegistrationInvitationEmailNotification
    {
        public ExtendedRegistrationInvitationEmailNotification() : base(nameof(ExtendedRegistrationInvitationEmailNotification))
        {
            //for backward compatibility v.2
            Alias = "RegistrationInvitationNotification";
        }
    }
}
