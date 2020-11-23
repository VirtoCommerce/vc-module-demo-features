using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.DemoSolutionFeaturesModule.Core;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.CustomerSegment;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models.Search;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Web.Controllers.Api
{
    [Route("/api/demo/customersegments")]
    public class DemoCustomerSegmentController: Controller
    {
        private readonly IDemoCustomerSegmentService _customerSegmentService;
        private readonly IDemoCustomerSegmentSearchService _customerSegmentSearchService;
        private readonly IDemoCustomerSegmentConditionEvaluator _customerSegmentConditionEvaluator;

        public DemoCustomerSegmentController(IDemoCustomerSegmentService customerSegmentService,
            IDemoCustomerSegmentSearchService customerSegmentSearchService,
            IDemoCustomerSegmentConditionEvaluator customerSegmentConditionEvaluator)
        {
            _customerSegmentService = customerSegmentService;
            _customerSegmentSearchService = customerSegmentSearchService;
            _customerSegmentConditionEvaluator = customerSegmentConditionEvaluator;
        }

        /// <summary>
        /// Get new customer segment object 
        /// </summary>
        /// <remarks>Return a new customer segment object with populated dynamic expression tree</remarks>
        [HttpGet]
        [Route("new")]
        [Authorize(ModuleConstants.Security.Permissions.Create)]
        public ActionResult<DemoCustomerSegment> GetNewCustomerSegment()
        {
            var result = AbstractTypeFactory<DemoCustomerSegment>.TryCreateInstance();

            result.ExpressionTree = AbstractTypeFactory<DemoCustomerSegmentTree>.TryCreateInstance();
            result.ExpressionTree.MergeFromPrototype(AbstractTypeFactory<DemoCustomerSegmentTreePrototype>.TryCreateInstance());
            result.IsActive = true;

            return Ok(result);
        }

        /// <summary>
        /// Get customer segment by ID
        /// </summary>
        /// <param name="id">Customer segment ID</param>
        [HttpGet]
        [Route("{id}")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<DemoCustomerSegment>> GetCustomerSegmentById([FromRoute] string id)
        {
            var result = (await _customerSegmentService.GetByIdsAsync(new[] { id })).FirstOrDefault();

            result?.ExpressionTree?.MergeFromPrototype(AbstractTypeFactory<DemoCustomerSegmentTreePrototype>.TryCreateInstance());

            return Ok(result);
        }

        /// <summary>
        /// Create/Update customer segments.
        /// </summary>
        /// <param name="customerSegments">The customer segments.</param>
        [HttpPost]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.Update)]
        public async Task<ActionResult<DemoCustomerSegment[]>> SaveCustomerSegments([FromBody] DemoCustomerSegment[] customerSegments)
        {
            if (!customerSegments.IsNullOrEmpty())
            {
                await _customerSegmentService.SaveChangesAsync(customerSegments);
            }

            return Ok(customerSegments);
        }

        /// <summary>
        /// Deletes customer segments by IDs.
        /// </summary>
        /// <param name="ids">Customer segment IDs.</param>
        [HttpDelete]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.Delete)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteCustomerSegments([FromQuery] string[] ids)
        {
            await _customerSegmentService.DeleteAsync(ids);

            return NoContent();
        }

        /// <summary>
        /// Search customer segments by specified search criteria.
        /// </summary>
        /// <param name="criteria">Search criteria</param>
        /// <returns>Search result with total number of found customer segments and all found customer segments.</returns>
        [HttpPost]
        [Route("search")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<DemoCustomerSegmentSearchResult>> SearchCustomerSegments([FromBody] DemoCustomerSegmentSearchCriteria criteria)
        {
            var result = await _customerSegmentSearchService.SearchCustomerSegmentsAsync(criteria);

            return Ok(result);
        }

        /// <summary>
        /// Evaluate customer segment and return all IDs of all customers who meet criteria.
        /// </summary>
        /// <param name="conditionRequest">Request with customer segment criteria.</param>
        /// <returns>IDs of all customers who meet specified criteria.</returns>
        [HttpPost]
        [Route("preview")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public async Task<ActionResult<string[]>> PreviewCustomerSegment([FromBody] DemoCustomerSegmentConditionEvaluationRequest conditionRequest)
        {
            ValidateParameters(conditionRequest);

            var result = await _customerSegmentConditionEvaluator.EvaluateCustomerSegmentConditionAsync(conditionRequest);

            return Ok(result);
        }

        /// <summary>
        /// Get customer properties info
        /// </summary>        
        [HttpGet]
        [Route("customer/properties")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public ActionResult<ICollection<DemoCustomerProperty>> GetCustomerProperties()
        {
            var result = new List<DemoCustomerProperty>();

            result.Add(new DemoCustomerProperty("firstname"));
            result.Add(new DemoCustomerProperty("lastname"));
            result.Add(new DemoCustomerProperty("fullname"));
            result.Add(new DemoCustomerProperty("salutation"));
            result.Add(new DemoCustomerProperty("salutation"));
            result.Add(new DemoCustomerProperty("birthdate") { ValueType = "DateTime" });
            result.Add(new DemoCustomerProperty("emails"));
            result.Add(new DemoCustomerProperty("preferredcommunication"));
            result.Add(new DemoCustomerProperty("preferreddelivery"));
            result.Add(new DemoCustomerProperty("taxpayerid"));
                      
            return Ok(result);
        }

        private static void ValidateParameters(DemoCustomerSegmentConditionEvaluationRequest conditionRequest)
        {
            if (conditionRequest == null)
            {
                throw new ArgumentNullException(nameof(conditionRequest));
            }
        }
    }
}
