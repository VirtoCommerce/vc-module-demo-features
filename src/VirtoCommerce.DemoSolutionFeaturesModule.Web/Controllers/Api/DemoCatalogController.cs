using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Catalog;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.Controllers.Api
{
    [Route("api/demo/catalog")]
    public class DemoCatalogController : Controller
    {
        private readonly IDemoProductPartSerarchService _partsSerarchService;
        private readonly IDemoProductPartService _partsService;
        private readonly IAuthorizationService _authorizationService;

        public DemoCatalogController(
            IDemoProductPartService partsService
            , IDemoProductPartSerarchService partsSerarchService
            , IAuthorizationService authorizationService)
        {
            _partsSerarchService = partsSerarchService;
            _partsService = partsService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Route("product/parts/{id}")]
        public async Task<ActionResult<DemoProductPart>> Search(string id)
        {
            var result = (await _partsService.GetByIdsAsync(new[] { id })).FirstOrDefault();
            return Ok(result);
        }

        [HttpPost]
        [Route("product/parts/search")]
        public async Task<ActionResult<DemoProductPartSearchResult>> Search(DemoProductPartSearchCriteria criteria)
        {
            var result = await _partsSerarchService.SearchProductPartsAsync(criteria);
            return Ok(result);
        }

        [HttpPost]
        [Route("product/parts")]
        public async Task<ActionResult> SaveProductPart([FromBody] DemoProductPart[] parts)
        {
            await _partsService.SaveChangesAsync(parts);
            return Ok();
        }

        [HttpDelete]
        [Route("product/parts")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteParts([FromQuery] string[] ids)
        {
            await _partsService.DeleteAsync(ids);
            return NoContent();
        }
    }
}
