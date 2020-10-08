using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class DemoCartLineItemEntity : LineItemEntity
    {
        //[StringLength(64)]
        //public string GroupId { get; set; }

        //public CartLineItemGroupDemoEntity Group { get; set; }

        
        public string ParentLineItemId { get; set; }

        public override LineItem ToModel(LineItem lineItem)
        {            
            base.ToModel(lineItem);

            if (lineItem is DemoCartLineItem item)
            {
                //item.GroupId = GroupId;
                item.ParentLineItemId = ParentLineItemId;
            }

            return lineItem;
        }

        public override LineItemEntity FromModel(LineItem lineItem, PrimaryKeyResolvingMap pkMap)
        {
            if (lineItem is DemoCartLineItem item)
            {
                //GroupId = item.GroupId;
                ParentLineItemId = item.ParentLineItemId;
            }

            return base.FromModel(lineItem, pkMap);
        }

        public override void Patch(LineItemEntity target)
        {
            if (target is DemoCartLineItemEntity item)
            {
                //item.GroupId = GroupId;
                item.ParentLineItemId = ParentLineItemId;
            }

            base.Patch(target);
        }
    }
}
