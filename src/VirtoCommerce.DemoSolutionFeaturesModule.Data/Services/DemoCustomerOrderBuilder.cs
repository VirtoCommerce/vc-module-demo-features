using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.OrdersModule.Core.Model;
using VirtoCommerce.OrdersModule.Core.Services;
using VirtoCommerce.OrdersModule.Data.Services;
using VirtoCommerce.Platform.Core.Common;
using LineItem = VirtoCommerce.OrdersModule.Core.Model.LineItem;
using Shipment = VirtoCommerce.OrdersModule.Core.Model.Shipment;
using ShipmentItem = VirtoCommerce.OrdersModule.Core.Model.ShipmentItem;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Services
{
    public class DemoCustomerOrderBuilder : CustomerOrderBuilder
    {
        public DemoCustomerOrderBuilder(ICustomerOrderService customerOrderService) : base(customerOrderService)
        {
        }

        protected override CustomerOrder ConvertCartToOrder(ShoppingCart cart)
        {
            var retVal = base.ConvertCartToOrder(cart);
            retVal.Status = "Unpaid";
            return retVal;
        }
    }
}
