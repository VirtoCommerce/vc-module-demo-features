using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using catalogCore = VirtoCommerce.CatalogModule.Core;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.Controllers.Api
{
    [Route("api/demo/catalog")]
    public class DemoCatalogController : Controller
    {
        private readonly IDemoProductPartSerarchService _partsSerarchService;
        private readonly IDemoProductPartService _partsService;

        public DemoCatalogController(
            IDemoProductPartService partsService
            , IDemoProductPartSerarchService partsSerarchService)
        {
            _partsSerarchService = partsSerarchService;
            _partsService = partsService;
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
            var result = await _partsSerarchService.SearchProductPartsAsync(criteria);
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
