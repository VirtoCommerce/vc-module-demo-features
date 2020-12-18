using System;
using System.Collections.Generic;
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
        [Route("product/{productId}/parts/search")]
        public async Task<ActionResult<DemoProductPartSearchResult>> Search( [FromRoute] string productId, DemoProductPartSearchCriteria criteria)
        {
            if (criteria.ConfiguredProductId != productId)
            {
                return BadRequest();
            }

            var result = await _partsSerarchService.SearchCategoriesAsync(criteria);

            return Ok(result);
        }

        [HttpPost]
        [Route("product/{productId}/parts")]
        public async Task<ActionResult> SaveProductPart( [FromRoute] string productId, [FromBody] DemoProductPart[] parts)
        {

            if(parts.Any(x=>x.ConfiguredProductId !=  productId))
            {
                return BadRequest();
            }

            await _partsService.SaveChangesAsync(parts);

            return Ok();
        }


        [HttpDelete]
        [Route("product/{productId}/parts")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteParts([FromQuery] string[] ids)
        {
            await _partsService.DeleteAsync(ids);

            return NoContent();
        }
    }
}
