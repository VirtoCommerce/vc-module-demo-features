using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoCartLineItemConfiguredGroupEntity : Entity
    {
        public string GroupId { get; set; }

        public DemoCartConfiguredGroupEntity Group { get; set; }

        public string ItemId { get; set; }

        public DemoCartLineItemEntity Item { get; set; }
    }
}
