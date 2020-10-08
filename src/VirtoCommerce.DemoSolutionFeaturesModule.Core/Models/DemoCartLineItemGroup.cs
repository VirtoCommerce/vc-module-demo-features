using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoCartLineItemGroup: AuditableEntity
    {       
        public string ProductId { get; set; }
        public string ShoppingCartId { get; set; }

        public int Quantity { get; set; }

        public ICollection<string> ItemIds { get; set; }
    }
}
