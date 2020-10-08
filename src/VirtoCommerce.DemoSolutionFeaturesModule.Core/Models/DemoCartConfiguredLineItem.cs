using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.CartModule.Core.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Models
{
    public class DemoCartConfiguredLineItem : LineItem
    {
        public ICollection<DemoCartLineItem> Items { get; set; }
    }
}
