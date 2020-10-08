using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.DemoSolutionFeaturesModule.Core;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;
using VirtoCommerce.OrdersModule.Data.Services;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Services;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Data.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web
{
    public class Module : IModule
    {
        public ManifestModuleInfo ModuleInfo { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            // database initialization
            var configuration = serviceCollection.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("VirtoCommerce.VirtoCommerceDemoSolutionFeaturesModule") ?? configuration.GetConnectionString("VirtoCommerce");
            serviceCollection.AddDbContext<CustomerDemoDbContext>(options => options.UseSqlServer(connectionString));
            
            serviceCollection.AddTransient<ICustomerRepository, CustomerDemoRepository>();
            serviceCollection.AddTransient<ICustomerOrderBuilder, DemoCustomerOrderBuilder>();
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            //Customer
            AbstractTypeFactory<Contact>.OverrideType<Contact, ContactDemo>().MapToType<ContactDemoEntity>();
            AbstractTypeFactory<Member>.OverrideType<Contact, ContactDemo>().MapToType<ContactDemoEntity>();
            AbstractTypeFactory<MemberEntity>.OverrideType<ContactEntity, ContactDemoEntity>();

            //Cart
            AbstractTypeFactory<LineItem>.OverrideType<LineItem, DemoCartLineItem>().MapToType<DemoCartLineItemEntity>();
            AbstractTypeFactory<LineItem>.OverrideType<LineItem, DemoCartConfiguredLineItem>().MapToType<DemoCartConfiguredLineItemEntity>();

            AbstractTypeFactory<LineItemEntity>.OverrideType<LineItemEntity, DemoCartLineItemEntity>();

            //AbstractTypeFactory<DemoCartLineItemGroup>.RegisterType<DemoCartLineItemGroup>();

            // register settings
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(ModuleConstants.Settings.AllSettings, ModuleInfo.Id);

            // register permissions
            var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsProvider.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions.Select(x =>
                new Permission()
                {
                    GroupName = "VirtoCommerceDemoSolutionFeaturesModule",
                    ModuleId = ModuleInfo.Id,
                    Name = x
                }).ToArray());

            // Ensure that any pending migrations are applied
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<CustomerDemoDbContext>())
                {
                    dbContext.Database.EnsureCreated();
                    dbContext.Database.Migrate();
                }
            }
        }

        public void Uninstall()
        {
            // do nothing in here
        }

    }

}
