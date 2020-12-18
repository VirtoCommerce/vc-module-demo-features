using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models.Catalog
{
    public class DemoProductPartItemEntity
    {
        public string ConfiguredProductPartId { get; set; }

        public DemoProductPartEntity ConfiguredProductPart { get; set; }

        public string ItemId { get; set; }

        public DemoItemEntity Item { get; set; }
    }
}
