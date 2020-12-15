using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core
{
    public static class ModuleConstants
    {
        public static class Security
        {
            public static class Permissions
            {
                public const string Access = "virtoCommerceDemoSolutionFeaturesModule:access";
                public const string Create = "virtoCommerceDemoSolutionFeaturesModule:create";
                public const string Read = "virtoCommerceDemoSolutionFeaturesModule:read";
                public const string Update = "virtoCommerceDemoSolutionFeaturesModule:update";
                public const string Delete = "virtoCommerceDemoSolutionFeaturesModule:delete";
                public const string Developer = "demo:developer";

                public static string[] AllPermissions => new[] { Read, Create, Access, Update, Delete, Developer };
            }
        }

        public static class Settings
        {
            public static class General
            {
                public static readonly SettingDescriptor DemoInvoicePaymentMethodLogo = new SettingDescriptor
                {
                    Name = "VirtoCommerceDemoSolutionFeaturesModule.DemoInvoicePaymentMethod.Logo",
                    GroupName = "VirtoCommerceDemoSolutionFeaturesModule|DemoInvoicePaymentMethod",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = "",
                };

                public static readonly SettingDescriptor DemoCreditCardPaymentMethodLogo = new SettingDescriptor
                {
                    Name = "VirtoCommerceDemoSolutionFeaturesModule.DemoCreditCardPaymentMethod.Logo",
                    GroupName = "VirtoCommerceDemoSolutionFeaturesModule|DemoCreditCardPaymentMethod",
                    ValueType = SettingValueType.ShortText,
                    DefaultValue = "",
                };

                public static IEnumerable<SettingDescriptor> InvoicePaymentMethodSettings
                {
                    get
                    {
                        yield return DemoInvoicePaymentMethodLogo;
                    }
                }

                public static IEnumerable<SettingDescriptor> CreditCardPaymentMethodSettings
                {
                    get
                    {
                        yield return DemoCreditCardPaymentMethodLogo;
                    }
                }

                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return DemoInvoicePaymentMethodLogo;
                        yield return DemoCreditCardPaymentMethodLogo;
                    }
                }
            }
        }
    }
}
