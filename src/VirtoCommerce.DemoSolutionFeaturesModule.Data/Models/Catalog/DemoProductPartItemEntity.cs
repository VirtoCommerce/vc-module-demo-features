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
