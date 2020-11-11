using System;
using Microsoft.AspNetCore.Mvc;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.Controllers.Api
{
    public class DemoSolutionFeaturesModuleController : Controller
    {
        /// <summary>
        /// Get store URL
        /// </summary>
        [HttpGet]
        [Route("/api/stores/url/{storeName}")]
        public ActionResult<string> Get([FromRoute] string storeName)
        {
            var storeUrl = Environment.GetEnvironmentVariable($"VC_STORE_URL_{storeName}");

            if (storeUrl != null)
            {
                return Ok(storeUrl);
            }

            return NotFound();
        }
    }
}
