using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoCartConfiguredLineItemEntity : DemoCartLineItemEntity
    {
        public virtual ObservableCollection<DemoCartLineItemEntity> Items { get; set; } = new NullCollection<DemoCartLineItemEntity>();
    }
}
