using System.Threading.Tasks;
using DayCareApi.Models;
using System.Collections.Generic;

namespace DayCareApi.Repositories
{
    public interface IDayCareRepository
    {
        Task<int> AddChildAsync(DayCareReimbursement model, int initiatorEmpId);
        Task<int> UpdateChildAsync(DayCareReimbursement model);
        Task<int> DeleteChildAsync(int rid, int dcid);
        Task<int> UpdateDraftStatusAsync(int rid, int dcid, int choice);
        Task<int> GetQuarterAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<DayCareReimbursement>> GetByEmployeeIdAsync(int initiatorEmpId);
    }
}