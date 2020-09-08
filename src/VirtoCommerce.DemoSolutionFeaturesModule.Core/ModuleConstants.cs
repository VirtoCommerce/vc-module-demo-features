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

                public static string[] AllPermissions { get; } = { Read, Create, Access, Update, Delete };
            }
        }

        public static class Settings
        {
            public static class General
            {
                public static SettingDescriptor VirtoCommerceDemoSolutionFeaturesModuleEnabled { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerceDemoSolutionFeaturesModule.VirtoCommerceDemoSolutionFeaturesModuleEnabled",
                    GroupName = "VirtoCommerceDemoSolutionFeaturesModule|General",
                    ValueType = SettingValueType.Boolean,
                    DefaultValue = false
                };

                public static SettingDescriptor VirtoCommerceDemoSolutionFeaturesModulePassword { get; } = new SettingDescriptor
                {
                    Name = "VirtoCommerceDemoSolutionFeaturesModule.VirtoCommerceDemoSolutionFeaturesModulePassword",
                    GroupName = "VirtoCommerceDemoSolutionFeaturesModule|Advanced",
                    ValueType = SettingValueType.SecureString,
                    DefaultValue = "qwerty"
                };

                public static IEnumerable<SettingDescriptor> AllSettings
                {
                    get
                    {
                        yield return VirtoCommerceDemoSolutionFeaturesModuleEnabled;
                        yield return VirtoCommerceDemoSolutionFeaturesModulePassword;
                    }
                }
            }

            public static IEnumerable<SettingDescriptor> AllSettings
            {
                get
                {
                    return General.AllSettings;
                }
            }
        }
    }
}
