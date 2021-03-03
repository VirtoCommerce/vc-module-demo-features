using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Core.Services;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Core.Events;
using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.CatalogModule.Data.Model;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogPersonalizationModule.Data.Search.Indexing;
using VirtoCommerce.CustomerModule.Core.Events;
using VirtoCommerce.CustomerModule.Core.Model;
using VirtoCommerce.CustomerModule.Core.Services;
using VirtoCommerce.CustomerModule.Data.Handlers;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Events.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Events.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Notifications;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Data;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Handlers;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Repositories.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Search.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Services;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Web.HangfireFilters;
using VirtoCommerce.DemoSolutionFeaturesModule.Web.Infrastructure;
using VirtoCommerce.NotificationsModule.Core.Services;
using VirtoCommerce.NotificationsModule.Core.Types;
using VirtoCommerce.NotificationsModule.TemplateLoader.FileSystem;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.OrdersModule.Core.Services;
using VirtoCommerce.OrdersModule.Data.Model;
using VirtoCommerce.OrdersModule.Data.Repositories;
using VirtoCommerce.OrdersModule.Data.Services;
using VirtoCommerce.PaymentModule.Core.Services;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.SearchModule.Core.Model;
using CartLineItem = VirtoCommerce.CartModule.Core.Model.LineItem;
using CartLineItemEntity = VirtoCommerce.CartModule.Data.Model.LineItemEntity;
using demoFeaturesCore = VirtoCommerce.DemoSolutionFeaturesModule.Core;
using OrderLineItem = VirtoCommerce.OrdersModule.Core.Model.LineItem;
using OrderLineItemEntity = VirtoCommerce.OrdersModule.Data.Model.LineItemEntity;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web
{
    public class Module : IModule
    {
        public ManifestModuleInfo ModuleInfo { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            // database initialization
            void ActionCallback(IServiceProvider provider, DbContextOptionsBuilder options)
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                options.UseSqlServer(configuration.GetConnectionString(ModuleInfo.Id) ?? configuration.GetConnectionString("VirtoCommerce"));
            }

            serviceCollection.AddDbContext<CustomerDemoDbContext>(ActionCallback);
            serviceCollection.AddDbContext<DemoCartDbContext>(ActionCallback);
            serviceCollection.AddDbContext<DemoOrderDbContext>(ActionCallback);
            serviceCollection.AddDbContext<DemoCatalogDbContext>(ActionCallback);

            // customer
            serviceCollection.AddTransient<ICustomerRepository, CustomerDemoRepository>();
            serviceCollection.AddTransient<IDemoTaggedMemberRepository, CustomerDemoRepository>();
            serviceCollection.AddTransient<Func<IDemoTaggedMemberRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IDemoTaggedMemberRepository>());
            serviceCollection.AddTransient<IDemoTaggedMemberService, DemoTaggedMemberService>();
            serviceCollection.AddTransient<IDemoTaggedMemberSearchService, DemoTaggedMemberSearchService>();
            serviceCollection.AddTransient<IMemberService, DemoMemberService>();
            serviceCollection.AddTransient<LogChangesTaggedMembersHandler>();
            serviceCollection.AddSingleton<DemoTaggedMemberIndexChangesProvider>();
            serviceCollection.AddSingleton<DemoTaggedMemberDocumentBuilder>();
            serviceCollection.AddTransient<IDemoMemberInheritanceEvaluator, DemoMemberInheritanceEvaluator>();
            serviceCollection.AddTransient<ClearTaggedMemberCacheAtMemberChangedHandler>();

            // cart
            serviceCollection.AddTransient<ICartRepository, DemoCartRepository>();
            serviceCollection.AddTransient<IShoppingCartTotalsCalculator, DemoShoppingCartTotalsCalculator>();
            // order
            serviceCollection.AddTransient<IOrderRepository, DemoOrderRepository>();
            serviceCollection.AddTransient<ICustomerOrderTotalsCalculator, DemoCustomerOrderTotalsCalculator>();
            serviceCollection.AddTransient<ICustomerOrderBuilder, DemoCustomerOrderBuilder>();

            serviceCollection.AddFeatureManagement().AddFeatureFilter<DevelopersFilter>();
            serviceCollection.AddSingleton<DemoFeatureDefinitionProvider>();
            serviceCollection.AddSingleton<IFeaturesStorage>(s => s.GetService<DemoFeatureDefinitionProvider>());
            serviceCollection.AddSingleton<IFeatureDefinitionProvider>(s => s.GetService<DemoFeatureDefinitionProvider>());

            // catalog
            serviceCollection.AddTransient<ICatalogRepository, DemoCatalogRepository>();
            serviceCollection.AddTransient<IDemoProductPartService, DemoProductPartService>();
            serviceCollection.AddTransient<IDemoProductPartSearchService, DemoProductPartSearchService>();

            serviceCollection.AddScoped<IDemoUserNameResolver, DemoUserNameResolver>();
            serviceCollection.AddScoped<IUserNameResolver, DemoUserNameResolver>();
            serviceCollection.AddSingleton<Func<IUserNameResolver>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IUserNameResolver>());

            serviceCollection.AddTransient<InvalidateProductPartsSearchCacheWhenProductIsDeletedHandler>();
            serviceCollection.AddTransient<LogChangesProductPartsHandler>();
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            // notifications
            var notificationRegistrar = appBuilder.ApplicationServices.GetService<INotificationRegistrar>();
            var moduleTemplatesPath = Path.Join(ModuleInfo.FullPhysicalPath, "Templates");

            notificationRegistrar.OverrideNotificationType<RegistrationInvitationEmailNotification, ExtendedRegistrationInvitationEmailNotification>()
                .WithTemplatesFromPath(Path.Join(moduleTemplatesPath, "Custom"), Path.Join(moduleTemplatesPath, "Default"));

            // customer
            AbstractTypeFactory<Contact>.OverrideType<Contact, ContactDemo>().MapToType<ContactDemoEntity>();
            AbstractTypeFactory<Member>.OverrideType<Contact, ContactDemo>().MapToType<ContactDemoEntity>();
            AbstractTypeFactory<MemberEntity>.OverrideType<ContactEntity, ContactDemoEntity>();

            AbstractTypeFactory<DemoTaggedMember>.RegisterType<DemoTaggedMember>().MapToType<DemoTaggedMemberEntity>();
            AbstractTypeFactory<DemoTaggedMemberEntity>.RegisterType<DemoTaggedMemberEntity>();

            // cart
            AbstractTypeFactory<CartLineItem>.OverrideType<CartLineItem, DemoCartLineItem>().MapToType<DemoCartLineItemEntity>();
            AbstractTypeFactory<CartLineItemEntity>.OverrideType<CartLineItemEntity, DemoCartLineItemEntity>();

            AbstractTypeFactory<DemoCartConfiguredGroup>.RegisterType<DemoCartConfiguredGroup>().MapToType<DemoCartConfiguredGroupEntity>();
            AbstractTypeFactory<DemoCartConfiguredGroupEntity>.RegisterType<DemoCartConfiguredGroupEntity>();

            AbstractTypeFactory<ShoppingCart>.OverrideType<ShoppingCart, DemoShoppingCart>().MapToType<DemoShoppingCartEntity>();
            AbstractTypeFactory<ShoppingCartEntity>.OverrideType<ShoppingCartEntity, DemoShoppingCartEntity>();

            // order
            AbstractTypeFactory<OrderLineItem>.OverrideType<OrderLineItem, DemoOrderLineItem>().MapToType<DemoOrderLineItemEntity>();
            AbstractTypeFactory<OrderLineItemEntity>.OverrideType<OrderLineItemEntity, DemoOrderLineItemEntity>();

            AbstractTypeFactory<DemoOrderConfiguredGroup>.RegisterType<DemoOrderConfiguredGroup>().MapToType<DemoOrderConfiguredGroupEntity>();
            AbstractTypeFactory<DemoOrderConfiguredGroupEntity>.RegisterType<DemoOrderConfiguredGroupEntity>();

            AbstractTypeFactory<CustomerOrder>.OverrideType<CustomerOrder, DemoCustomerOrder>()
                .MapToType<DemoCustomerOrderEntity>()
                .WithFactory(() => new DemoCustomerOrder { OperationType = "CustomerOrder" });
            AbstractTypeFactory<CustomerOrderEntity>.OverrideType<CustomerOrderEntity, DemoCustomerOrderEntity>();

            // catalog
            AbstractTypeFactory<ItemEntity>.OverrideType<ItemEntity, DemoItemEntity>();
            AbstractTypeFactory<CatalogProduct>.OverrideType<CatalogProduct, DemoProduct>();

            AbstractTypeFactory<DemoProductPart>.RegisterType<DemoProductPart>().MapToType<DemoProductPartEntity>();
            AbstractTypeFactory<DemoProductPartEntity>.RegisterType<DemoProductPartEntity>();

            // register settings
            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(demoFeaturesCore.ModuleConstants.Settings.General.AllSettings, ModuleInfo.Id);

            // register invoicePaymentMethod
            var paymentMethodsRegistrar = appBuilder.ApplicationServices.GetRequiredService<IPaymentMethodsRegistrar>();
            paymentMethodsRegistrar.RegisterPaymentMethod<DemoInvoicePaymentMethod>();
            settingsRegistrar.RegisterSettingsForType(demoFeaturesCore.ModuleConstants.Settings.General.InvoicePaymentMethodSettings, nameof(DemoInvoicePaymentMethod));
            paymentMethodsRegistrar.RegisterPaymentMethod<DemoCreditCardPaymentMethod>();
            settingsRegistrar.RegisterSettingsForType(demoFeaturesCore.ModuleConstants.Settings.General.CreditCardPaymentMethodSettings, nameof(DemoCreditCardPaymentMethod));

            // register permissions
            var permissionsProvider = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsProvider.RegisterPermissions(demoFeaturesCore.ModuleConstants.Security.Permissions.AllPermissions.Select(x =>
                new Permission
                {
                    GroupName = "VirtoCommerceDemoSolutionFeaturesModule",
                    ModuleId = ModuleInfo.Id,
                    Name = x,
                }).ToArray());

            var configuration = appBuilder.ApplicationServices.GetService<IConfiguration>();

            // features registration
            var featureStorage = appBuilder.ApplicationServices.GetService<IFeaturesStorage>();

            var demoFeaturesSection = configuration.GetSection("DemoFeatures");
            featureStorage.AddHighPriorityFeatureDefinition(demoFeaturesSection);

            featureStorage.TryAddFeature(demoFeaturesCore.ModuleConstants.Features.ConfigurableProduct, true);
            featureStorage.TryAddFeature(demoFeaturesCore.ModuleConstants.Features.UserGroupsInheritance, true);

            var inProcessBus = appBuilder.ApplicationServices.GetService<IHandlerRegistrar>();
            inProcessBus.RegisterHandler<ProductChangedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<InvalidateProductPartsSearchCacheWhenProductIsDeletedHandler>().Handle(message));
            inProcessBus.RegisterHandler<DemoProductPartChangedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<LogChangesProductPartsHandler>().Handle(message));
            inProcessBus.RegisterHandler<DemoTaggedMemberChangedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<LogChangesTaggedMembersHandler>().Handle(message));
            inProcessBus.RegisterHandler<MemberChangedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<ClearTaggedMemberCacheAtMemberChangedHandler>().Handle(message));

            #region Search

            var documentIndexingConfigurations = appBuilder.ApplicationServices.GetRequiredService<IEnumerable<IndexDocumentConfiguration>>();

            if (documentIndexingConfigurations != null)
            {
                //Member indexing
                var taggedItemProductDocumentSource = new IndexDocumentSource
                {
                    ChangesProvider = appBuilder.ApplicationServices.GetRequiredService<DemoTaggedMemberIndexChangesProvider>(),
                    DocumentBuilder = appBuilder.ApplicationServices.GetRequiredService<DemoTaggedMemberDocumentBuilder>()
                };
                foreach (var documentConfiguration in documentIndexingConfigurations.Where(c => c.DocumentType == KnownDocumentTypes.Member))
                {
                    documentConfiguration.RelatedSources ??= new List<IndexDocumentSource>();
                    documentConfiguration.RelatedSources.Add(taggedItemProductDocumentSource);
                }
            }

            #endregion

            // Ensure that any pending migrations are applied
            using var serviceScope = appBuilder.ApplicationServices.CreateScope();

            // customer
            using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<CustomerDemoDbContext>())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
            }
            // cart
            using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<DemoCartDbContext>())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
            }
            // order
            using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<DemoOrderDbContext>())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
            }
            // catalog
            using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<DemoCatalogDbContext>())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
            }

            // Add Hangfire filters/middlewares
            var demoUserNameResolver = serviceScope.ServiceProvider.GetRequiredService<IDemoUserNameResolver>();
            GlobalJobFilters.Filters.Add(new DemoHangfireUserContextFilter(demoUserNameResolver));
        }

        public void Uninstall()
        {
            // do nothing in here
        }
    }
}
