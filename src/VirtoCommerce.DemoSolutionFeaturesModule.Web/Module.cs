using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Core.Services;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Core;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.DemoSolutionFeaturesModule.Data;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Services;
using VirtoCommerce.OrdersModule.Data.Services;
using VirtoCommerce.PaymentModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

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
            serviceCollection.AddDbContext<DemoCartDbContext>(options => options.UseSqlServer(connectionString));

            serviceCollection.AddTransient<ICustomerRepository, CustomerDemoRepository>();
            serviceCollection.AddTransient<ICustomerOrderBuilder, DemoCustomerOrderBuilder>();
            serviceCollection.AddTransient<ICartRepository, DemoCartRepository>();
            serviceCollection.AddTransient<IShoppingCartTotalsCalculator, DemoShoppingCartTotalsCalculator>();
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            //Customer
            AbstractTypeFactory<Contact>.OverrideType<Contact, ContactDemo>().MapToType<ContactDemoEntity>();
            AbstractTypeFactory<Member>.OverrideType<Contact, ContactDemo>().MapToType<ContactDemoEntity>();
            AbstractTypeFactory<MemberEntity>.OverrideType<ContactEntity, ContactDemoEntity>();

            //Cart            
            AbstractTypeFactory<LineItem>.OverrideType<LineItem, DemoCartLineItem>().MapToType<DemoCartLineItemEntity>();
            AbstractTypeFactory<LineItemEntity>.OverrideType<LineItemEntity, DemoCartLineItemEntity>();

            AbstractTypeFactory<DemoCartConfiguredGroup>.RegisterType<DemoCartConfiguredGroup>().MapToType<DemoCartConfiguredGroupEntity>();
            AbstractTypeFactory<DemoCartConfiguredGroupEntity>.RegisterType<DemoCartConfiguredGroupEntity>();

            AbstractTypeFactory<ShoppingCart>.OverrideType<ShoppingCart, DemoShoppingCart>();
            AbstractTypeFactory<ShoppingCartEntity>.OverrideType<ShoppingCartEntity, DemoShoppingCartEntity>();

            // register settings
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(ModuleConstants.Settings.General.AllSettings, ModuleInfo.Id);

            // register invoicePaymentMethod
            var paymentMethodsRegistrar = appBuilder.ApplicationServices.GetRequiredService<IPaymentMethodsRegistrar>();
            paymentMethodsRegistrar.RegisterPaymentMethod<DemoInvoicePaymentMethod>();
            settingsRegistrar.RegisterSettingsForType(ModuleConstants.Settings.General.InvoicePaymentMethodSettings, nameof(DemoInvoicePaymentMethod));
            paymentMethodsRegistrar.RegisterPaymentMethod<DemoCreditCardPaymentMethod>();
            settingsRegistrar.RegisterSettingsForType(ModuleConstants.Settings.General.CreditCardPaymentMethodSettings, nameof(DemoCreditCardPaymentMethod));

            // register permissions
            var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsProvider.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions.Select(x =>
                new Permission
                {
                    GroupName = "VirtoCommerceDemoSolutionFeaturesModule",
                    ModuleId = ModuleInfo.Id,
                    Name = x,
                }).ToArray());

            // Ensure that any pending migrations are applied
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<CustomerDemoDbContext>())
                {
                    dbContext.Database.EnsureCreated();
                    dbContext.Database.Migrate();
                }
                using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<DemoCartDbContext>())
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
