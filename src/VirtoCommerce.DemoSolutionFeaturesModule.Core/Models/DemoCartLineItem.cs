using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.CartModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoCartLineItem: LineItem
    {
        public string ConfiguredProductId { get; set; }
    }
}
