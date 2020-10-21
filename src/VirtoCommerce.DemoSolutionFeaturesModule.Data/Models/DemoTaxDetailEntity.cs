using System;
using System.Collections.Generic;
using System.Text;
using VirtoCommerce.CartModule.Data.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoTaxDetailEntity : TaxDetailEntity
    {
        #region Navigation properties

        public string ConfiguredGroupId { get; set; }

        public DemoCartConfiguredGroupEntity ConfiguredGroup { get; set; }

        #endregion
    }
}
