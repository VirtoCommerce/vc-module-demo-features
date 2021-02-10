using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        /// GET: api/demo/members/tagged/{id}
        /// </summary>
        [HttpGet]
        [Route("tagged/{id}")]
        [ProducesResponseType(typeof(DemoTaggedMember), StatusCodes.Status200OK)]
        public async Task<ActionResult<DemoTaggedMember>> GetDemoTaggedMember(string id)
        {
            var criteria = new DemoTaggedMemberSearchCriteria
            {
                MemberId = id,
                Take = 1
            };

            var taggedMember = (await _searchService.SearchTaggedMembersAsync(criteria)).Results.FirstOrDefault();

            return Ok(taggedMember);
        }


        /// <summary>
        /// PUT: api/demo/members/search/tagged
        /// </summary>
        [HttpPut]
        [Route("search/tagged")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> SaveTaggedMember([FromBody] DemoTaggedMember taggedMember)
        {
            try
            {
                await _taggedItemService.SaveChangesAsync(new[] { taggedMember });
            }
            // VP-4690: Handling concurrent update exception - e.g. adding 2 tagged for 1 entity
            catch (DbUpdateException e) when (e.InnerException is Microsoft.Data.SqlClient.SqlException sqlException && (sqlException.Number == 2601 || sqlException.Number == 2627))
            {
                throw new InvalidOperationException($"Tagged item for the member entity with id:\"{taggedMember.MemberId}\"  already exists." +
                                                    $" Please refresh the entity and execute the operation again.");
            }

            return NoContent();
        }


        /// <summary>
        /// POST: api/demo/members/search
        /// </summary>
        [HttpPost]
        [Route("tagged/search")]
        [ProducesResponseType(typeof(DemoTaggedMemberSearchResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<DemoTaggedMemberSearchResult>> Search([FromBody] DemoTaggedMemberSearchCriteria criteria)
        {
            var searchResult = await _searchService.SearchTaggedMembersAsync(criteria);
            return Ok(searchResult);
        }


    }
}
