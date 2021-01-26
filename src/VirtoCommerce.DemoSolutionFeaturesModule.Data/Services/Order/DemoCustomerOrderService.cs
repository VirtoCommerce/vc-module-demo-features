using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CatalogModule.Core.Model;
using VirtoCommerce.CatalogModule.Core.Services;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.OrdersModule.Core.Services;
using VirtoCommerce.OrdersModule.Data.Repositories;
using VirtoCommerce.OrdersModule.Data.Services;
using VirtoCommerce.PaymentModule.Core.Services;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.ShippingModule.Core.Services;
using VirtoCommerce.StoreModule.Core.Services;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services.Order
{
    public class DemoCustomerOrderService : CustomerOrderService
    {        
        private readonly IItemService _itemService;
        public DemoCustomerOrderService(
            Func<IOrderRepository> orderRepositoryFactory, IUniqueNumberGenerator uniqueNumberGenerator
            , IStoreService storeService
            , IEventPublisher eventPublisher, ICustomerOrderTotalsCalculator totalsCalculator
            , IShippingMethodsSearchService shippingMethodsSearchService, IPaymentMethodsSearchService paymentMethodSearchService
            , IPlatformMemoryCache platformMemoryCache
            , IItemService itemService

            ) : base(orderRepositoryFactory, uniqueNumberGenerator
                , storeService
                , eventPublisher, totalsCalculator
                , shippingMethodsSearchService, paymentMethodSearchService
                , platformMemoryCache)
        {
            
            _itemService = itemService;
        }

        public override async Task<CustomerOrder[]> GetByIdsAsync(string[] orderIds, string responseGroup = null)
        {
            var orders =  (DemoCustomerOrder[])(await base.GetByIdsAsync(orderIds, responseGroup));

            await LoadProductsAsync(orders);

            return orders;
        }


        protected async Task LoadProductsAsync(params DemoCustomerOrder[] orders)
        {
            var productIds = orders.SelectMany(o => o.Items.Select(i => i.ProductId).Concat(o.ConfiguredGroups.Select(c => c.ProductId))).ToArray();
            var products = (await _itemService.GetByIdsAsync(productIds, ItemResponseGroup.None.ToString())).ToDictionary(x => x.Id, x => x);

            foreach (var lineItem in orders.SelectMany(o => o.Items))
            {
                 ((DemoOrderLineItem)lineItem).Product = products[lineItem.ProductId];
            }

            foreach (var group in orders.SelectMany(x => x.ConfiguredGroups))
            {
                group.Product = products[group.ProductId];
            }
        }
    }
}
