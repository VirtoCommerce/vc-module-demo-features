using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.CatalogModule.Core.Model.Search;
using VirtoCommerce.CatalogModule.Core.Search;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.SearchModule.Core.Model;
using catalogCore = VirtoCommerce.CatalogModule.Core;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.Controllers.Api
{
    [Route("api/demo/catalog")]
    public class DemoCatalogController : Controller
    {
        private readonly IDemoProductPartSearchService _partsSearchService;
        private readonly IDemoProductPartService _partsService;
        private readonly IProductIndexedSearchService _productIndexedSearchService;

        public DemoCatalogController(
            IDemoProductPartService partsService,
            IDemoProductPartSearchService partsSearchService,
            IProductIndexedSearchService productIndexedSearchService
            )
        {
            _partsSearchService = partsSearchService;
            _partsService = partsService;
            _productIndexedSearchService = productIndexedSearchService;
        }

        [HttpGet]
        [Route("product/parts/{id}")]
        [Authorize(catalogCore.ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<DemoProductPart>> GetProductPartById(string id)
        {
            var result = (await _partsService.GetByIdsAsync(new[] { id })).FirstOrDefault();
            return Ok(result);
        }

        [HttpPost]
        [Route("product/parts/search")]
        [Authorize(catalogCore.ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<DemoProductPartSearchResult>> Search([FromBody] DemoProductPartSearchCriteria criteria)
        {
            var result = await _partsSearchService.SearchProductPartsAsync(criteria);
            return Ok(result);
        }

        [HttpPost]
        [Route("product/parts/search/items")]
        [Authorize(catalogCore.ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<DemoProductPartItemSearchResult>> SearchPartItems([FromBody] DemoProductPartItemSearchCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            var result = new DemoProductPartItemSearchResult();

            var part = (await _partsSearchService.SearchProductPartsAsync(new DemoProductPartSearchCriteria { PartId = criteria.ConfiguredProductPartId }))
                .Results
                .FirstOrDefault();

            if (part != null && !part.PartItems.IsNullOrEmpty())
            {
                var partItems = part.PartItems;

                var products = (await _productIndexedSearchService.SearchAsync(new ProductIndexedSearchCriteria
                {
                    ObjectIds = partItems.Select(x => x.ItemId).ToArray(),
                    ObjectType = KnownDocumentTypes.Product,
                    Keyword = criteria.Keyword,
                    SearchPhrase = criteria.SearchPhrase,
                    Take = partItems.Length,
                    Skip = 0,
                }))
                    .Items;

                foreach (var catalogProduct in products)
                {
                    catalogProduct.Priority =
                        partItems.FirstOrDefault(x => x.ItemId.EqualsInvariant(catalogProduct.Id))?.Priority ??
                        catalogProduct.Priority;
                }

                result.Results = products
                    .AsQueryable()
                    .OrderBySortInfos(criteria.SortInfos)
                    .Skip(criteria.Skip)
                    .Take(criteria.Take)
                    .ToArray();

                result.TotalCount = part.PartItems.Length;
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("product/parts")]
        [Authorize(catalogCore.ModuleConstants.Security.Permissions.Update)]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<ActionResult> SaveProductPart([FromBody] DemoProductPart[] parts)
        {
            if (!parts.IsNullOrEmpty())
            {
                await _partsService.SaveChangesAsync(parts);
            }
            
            return Ok();
        }

        [HttpDelete]
        [Route("product/parts")]
        [Authorize(catalogCore.ModuleConstants.Security.Permissions.Delete)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteParts([FromQuery] string[] ids)
        {
            await _partsService.DeleteAsync(ids);
            return NoContent();
        }
    }
}
