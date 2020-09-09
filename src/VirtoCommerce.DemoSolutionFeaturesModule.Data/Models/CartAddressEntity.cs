using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Data.Model;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Models
{
    public class CartAddressEntity: AddressEntity
    {
        public override Address ToModel(Address address)
        {
            base.ToModel(address);
            address.Key = Id;
            return address;
        }

        public override AddressEntity FromModel(Address address)
        {
            base.FromModel(address);
            Id = address.Key;
            return this;
        }
    }
}
