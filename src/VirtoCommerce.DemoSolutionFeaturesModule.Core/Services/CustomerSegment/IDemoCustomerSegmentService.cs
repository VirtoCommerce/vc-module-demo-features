using System.Threading.Tasks;
using VirtoCommerce.DemoSolutionFeaturesModule.Core.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Core.Services
{
    public interface IDemoCustomerSegmentService
    {
        public Task<DemoCustomerSegment[]> GetByIdsAsync(string[] ids);

        public Task SaveChangesAsync(DemoCustomerSegment[] customerSegments);

        public Task DeleteAsync(string[] ids);
    }
}
