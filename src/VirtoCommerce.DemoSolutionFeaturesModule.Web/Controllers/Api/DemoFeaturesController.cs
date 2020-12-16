using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.Controllers.Api
{
    [Route("api/demo/features")]
    public class DemoFeaturesController : Controller
    {
        private readonly IFeatureManager _featureManager;

        public DemoFeaturesController(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        [HttpGet]
        [Route("{featureName}")]
        public async Task<ActionResult<bool>> IsFeatureEnabled(string featureName)
        {
            return Ok(await _featureManager.IsEnabledAsync(featureName));
        }

        [HttpGet]
        public ActionResult<IAsyncEnumerable<string>> GetFeatureNames()
        {
            return Ok(_featureManager.GetFeatureNamesAsync());
        }
    }
}
