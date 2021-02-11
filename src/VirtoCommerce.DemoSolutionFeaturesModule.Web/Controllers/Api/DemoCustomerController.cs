using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Customer;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services.Customer;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.Controllers.Api
{
    [Route("api/demo/members")]
    [ApiController]
    public class DemoCustomerController : Controller
    {
        private readonly IDemoTaggedMemberService _taggedItemService;
        private readonly IDemoTaggedMemberSearchService _searchService;

        public DemoCustomerController(
            IDemoTaggedMemberService taggedItemService,
            IDemoTaggedMemberSearchService searchService
            )
        {
            _taggedItemService = taggedItemService;
            _searchService = searchService;
        }

        /// <summary>
        /// GET: api/demo/members/tagged/{memberId}
        /// </summary>
        [HttpGet]
        [Route("tagged/{memberId}")]
        [ProducesResponseType(typeof(DemoTaggedMember), StatusCodes.Status200OK)]
        public async Task<ActionResult<DemoTaggedMember>> GetDemoTaggedMember(string memberId)
        {
            var taggedMember = (await _taggedItemService.GetByIdsAsync( new [] { memberId })).FirstOrDefault();
            return Ok(taggedMember);
        }


        /// <summary>
        /// POST: api/demo/members/tagged
        /// </summary>
        [HttpPost]
        [Route("tagged")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SaveTaggedMember([FromBody] DemoTaggedMember taggedMember)
        {
            await _taggedItemService.SaveChangesAsync(new[] { taggedMember });
            return NoContent();
        }


        /// <summary>
        /// POST: api/demo/members/search
        /// </summary>
        [HttpPost]
        [Route("search/tagged")]
        [ProducesResponseType(typeof(DemoTaggedMemberSearchResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<DemoTaggedMemberSearchResult>> Search([FromBody] DemoTaggedMemberSearchCriteria criteria)
        {
            var searchResult = await _searchService.SearchTaggedMembersAsync(criteria);
            return Ok(searchResult);
        }


    }
}
