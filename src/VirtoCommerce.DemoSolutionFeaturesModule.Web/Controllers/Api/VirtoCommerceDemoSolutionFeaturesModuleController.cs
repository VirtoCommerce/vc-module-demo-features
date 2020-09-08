using VirtoCommerce.DemoSolutionFeaturesModule.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.Controllers.Api
{
    [Route("api/VirtoCommerceDemoSolutionFeaturesModule")]
    public class VirtoCommerceDemoSolutionFeaturesModuleController : Controller
    {
        // GET: api/VirtoCommerceDemoSolutionFeaturesModule
        /// <summary>
        /// Get message
        /// </summary>
        /// <remarks>Return "Hello world!" message</remarks>
        [HttpGet]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public ActionResult<string> Get()
        {
            return Ok(new { result = "Hello world!" });
        }
    }
}
